﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System.Collections.Generic;

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

		List<BasicEnemy> enemies;


        //texture of the projectile
        Texture2D projectileTexture;

        //Keyboard sates used to determine key presses
        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;




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

			enemies = new List<BasicEnemy>() {
				new BasicEnemy()
			};

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
            Vector2 playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + (GraphicsDevice.Viewport.Width * 0.5f), GraphicsDevice.Viewport.TitleSafeArea.Y + (GraphicsDevice.Viewport.Height * 0.8f));
            player.Initialize(Content.Load<Texture2D>("Graphics/PlayerTestv2"), playerPosition);

            

            //Load the projectile texture
            Vector2 projectilePosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + (GraphicsDevice.Viewport.Width * 0.5f)+10f, GraphicsDevice.Viewport.TitleSafeArea.Y + (GraphicsDevice.Viewport.Height * 0.8f));
            projectileTexture = Content.Load<Texture2D>("Graphics/Discv2");
            projectile.Initialize(projectileTexture, projectilePosition, Vector2.Zero,playerPosition);
            //projectile.m_DummyPosition.X += 10f;   
			
			// Load the basic enemy resources
			BasicEnemy.Texture = Content.Load<Texture2D>("Graphics/enemy1");
			foreach (BasicEnemy enemy in enemies) {
				enemy.Initialize (BasicEnemy.Texture, new Vector2 (
					GraphicsDevice.Viewport.TitleSafeArea.X + GraphicsDevice.Viewport.Width/2, 
					GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.Height/2)
				);
			}
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

            //The space bar releases the projectile



            // Save the previous state of the keyboard 
            previousKeyboardState = currentKeyboardState;

            //Read the current state
            currentKeyboardState = Keyboard.GetState();

            //Update the player
            UpdatePlayer(gameTime);

            //Add the projectile
            UpdateProjectile();

			foreach (BasicEnemy enemy in enemies) {
				enemy.Update(gameTime);
			}

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

            if (projectile.m_Position.X <= 0 || projectile.m_Position.X >= GraphicsDevice.Viewport.Width - projectile.m_iSpriteWidth/2)
            {
                projectile.m_fProjectileSpeedX = -projectile.m_fProjectileSpeedX;
            }

            if (projectile.m_Position.Y <= 0 || projectile.m_Position.Y >= GraphicsDevice.Viewport.Height - projectile.m_iSpriteHeight)
            {
                projectile.m_fProjectileSpeedY = -projectile.m_fProjectileSpeedY;
            }
            if (currentKeyboardState.IsKeyDown(Keys.R)){
                projectile.m_ProjectileOrigin = projectile.selfOrigin;
                projectile.selfRotate = true;
            }else
            {
                projectile.m_ProjectileOrigin = Vector2.Zero;
                projectile.selfRotate = false;
            }            

            projectile.Update();
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

            //Draw the Player
            player.Draw(spriteBatch);

            //Draw the projectile
            projectile.Draw(spriteBatch);

			foreach (BasicEnemy enemy in enemies)
				enemy.Draw (spriteBatch);

            //Stop drawing
            spriteBatch.End();


            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
