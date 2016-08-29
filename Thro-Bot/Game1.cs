using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// Testing commit sync
namespace Thro_Bot
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Represents the player
        Player player;

        //represents the projectile
        Projectile projectile;


        //texture of the projectile
        Texture2D projectileTexture;

        //Keyboard sates used to determine key presses
        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;


        //The texture of the background
        Texture2D backgroundTexture;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //Change the size of the window
            graphics.PreferredBackBufferWidth = 600; //set the value to the desired width
            graphics.PreferredBackBufferHeight = 800; //set the value to the desired height
            graphics.ApplyChanges();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

            player = new Player();

            projectile = new Projectile();

            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Load the player resources
            //Vector2 playerPosition = Vector2.Zero;
            Vector2 playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + (GraphicsDevice.Viewport.Width * 0.5f), GraphicsDevice.Viewport.TitleSafeArea.Y + (GraphicsDevice.Viewport.Height * 0.81f));
            player.Initialize(Content.Load<Texture2D>("Graphics/PlayerTestv2"), playerPosition);          

            //Load the projectile texture
            Vector2 projectilePosition = new Vector2(playerPosition.X + 10f, playerPosition.Y);
            projectileTexture = Content.Load<Texture2D>("Graphics/Discv2");
            projectile.Initialize(projectileTexture, projectilePosition, Vector2.Zero);


            //Load the background 
            backgroundTexture = Content.Load<Texture2D>("Graphics/Background");

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            // Save the previous state of the keyboard 
            previousKeyboardState = currentKeyboardState;

            //Read the current state
            currentKeyboardState = Keyboard.GetState();

            //Update the player
            UpdatePlayer(gameTime);

            //Add the projectile
            UpdateProjectile();

            base.Update(gameTime);
        }


        private void UpdatePlayer(GameTime gameTime)
        {

            player.Update();

            //Check the case where the space bar is pressed
            if (currentKeyboardState.IsKeyDown(Keys.Space))
            {
                //Launch the projectile
                projectile.m_bInOrbitToPlayer = false;                
            }




        }

        protected void UpdateProjectile() {

            if (projectile.m_Position.X <= 10f || projectile.m_Position.X >= GraphicsDevice.Viewport.TitleSafeArea.Width - 10f)
            {
                projectile.m_fProjectileSpeedX = -projectile.m_fProjectileSpeedX;
                projectile.m_iBounces++;
            }

            if (projectile.m_Position.Y <= 10f || projectile.m_Position.Y >= GraphicsDevice.Viewport.TitleSafeArea.Height - 10f)
            {
                projectile.m_fProjectileSpeedY = -projectile.m_fProjectileSpeedY;
                projectile.m_iBounces++;
            }

            if (projectile.m_iBounces > 4) {

                projectile.m_bInOrbitToPlayer = true;

                //Reset the bounces
                projectile.m_iBounces = 0;
            }


            //Check is projectile has been launched, rotate it around its center
            if (currentKeyboardState.IsKeyDown(Keys.Space)){                
                projectile.selfRotate = true;
            }else
            {
                projectile.selfRotate = false;
            }            

            projectile.Update(player.m_Position);
        }



        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);



            //Start drawing
            spriteBatch.Begin();


            //Draw the background
            Rectangle sourceRectangle = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            spriteBatch.Draw(backgroundTexture, sourceRectangle,Color.White);

            //Draw the Player
            player.Draw(spriteBatch);

            //Draw the projectile
            projectile.Draw(spriteBatch);

            

            //Stop drawing
            spriteBatch.End();


            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
