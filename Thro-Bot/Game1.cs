using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Diagnostics;

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
        Vector2 projectilePosition;

        //texture of the projectile
        Texture2D projectileTexture;

        //Represents the UI score board
        UI ui;


        //Keyboard sates used to determine key presses
        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;


        //The texture of the background
        Texture2D backgroundTexture;
        Texture2D edge;
        Texture2D edge_normal;
        Texture2D edge_hit;

        //Ring+line texture
        Texture2D ringLineTexture;
        Vector2 ringLinePosition;
        Vector2 ringLineOrigin;

        // Enemy list
        List<EnemyBase> enemiesList;
        Texture2D enemyTexture;

        // Random
        Random random;

        // Screen resolution
        const int WIDTH = 750;
        const int HEIGHT = 1000;

        // Spawn interval
        const float SPAWN_INTERVAL = 2f;
        TimeSpan spawnTimeSpan;

        // Current game time
        TimeSpan currentTime;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //Change the size of the window
            graphics.PreferredBackBufferWidth = WIDTH; //set the value to the desired width
            graphics.PreferredBackBufferHeight = HEIGHT; //set the value to the desired height
            //graphics.IsFullScreen = true;
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
            ringLinePosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + (GraphicsDevice.Viewport.Width * 0.5f), GraphicsDevice.Viewport.TitleSafeArea.Y + (GraphicsDevice.Viewport.Height * 0.92f));
            enemiesList = new List<EnemyBase>();
            random = new Random();
            currentTime = TimeSpan.Zero;
            spawnTimeSpan = TimeSpan.FromSeconds(SPAWN_INTERVAL);
            ui = new UI();

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
            Vector2 playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + (GraphicsDevice.Viewport.Width * 0.5f), GraphicsDevice.Viewport.TitleSafeArea.Y + (GraphicsDevice.Viewport.Height * 0.92f));
            player.Initialize(Content.Load<Texture2D>("Graphics/Player"), playerPosition);          

            //Load the projectile texture
            projectilePosition = new Vector2(playerPosition.X + 10f, playerPosition.Y);
            projectileTexture = Content.Load<Texture2D>("Graphics/Discv2");
            projectile.Initialize(projectileTexture, projectilePosition, Vector2.Zero);


            //Load the background 
            backgroundTexture = Content.Load<Texture2D>("Graphics/Background");

            // Load ring line
            ringLineTexture = Content.Load<Texture2D>("Graphics/Ring_Line");
            ringLineOrigin = new Vector2(ringLineTexture.Width / 2, ringLineTexture.Height / 2);

            // Load edge textures
            edge_normal = Content.Load<Texture2D>("Graphics/Edge_normal");
            edge_hit = Content.Load<Texture2D>("Graphics/Edge_Hit");
            edge = edge_normal;

            // Load enemy texture
            enemyTexture = Content.Load<Texture2D>("Graphics/E1");

            //Load the score texture
            Vector2 scorePosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + (GraphicsDevice.Viewport.Width * 0.45f), GraphicsDevice.Viewport.TitleSafeArea.Y + (GraphicsDevice.Viewport.Height * 0.067f));
            ui.Initialize(Content.Load<Texture2D>("Graphics/ScoreUI"),scorePosition, Vector2.Zero);

            //Load the score font
            ui.scoreFont = Content.Load<SpriteFont>("Fonts/Score");
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

            // Spawn enemies
            SpawnEnemies(gameTime);

            // Save the previous state of the keyboard 
            previousKeyboardState = currentKeyboardState;

            //Read the current state
            currentKeyboardState = Keyboard.GetState();

            //Update the player
            UpdatePlayer(gameTime);

            //Update the projectile
            UpdateProjectile(gameTime);

            // Update enemy
            UpdateEnemies();

            base.Update(gameTime);
        }

        private void UpdateEnemies()
        {
            for (int i=0;i<enemiesList.Count;i++)
            {
                EnemyBase enemy = enemiesList[i];
                if (enemy.m_Active)
                {
                    enemy.Update();
                    if (enemy.m_Position.Y > GraphicsDevice.Viewport.Height || CheckCollision(enemy))
                    {
                        enemy.Active = false;

                        //Cause damage to the player
                        if (!CheckCollision(enemy))
                        {
                            player.m_iHealth -= 10;
                            ui.playerHealth = player.m_iHealth;
                        }
                        //Add points to the player score
                        else
                        {
                            ui.score += 100;
                        }
                    }
                }

                
                else
                {
                    enemiesList.RemoveAt(i);
                }            
            }
        }

        private bool CheckCollision(Enemy1 enemy)
        {
            bool collision = false;            
            Rectangle enemyRectangle = new Rectangle((int)enemy.Position.X, (int)enemy.Position.Y, enemy.Texture.Width-35, enemy.Texture.Height-50);
            Rectangle projectileRectangle = new Rectangle((int)projectile.m_Position.X-projectile.m_ProjectileTexture.Width/2, (int)projectile.m_Position.Y-projectile.m_ProjectileTexture.Height/2, projectile.m_ProjectileTexture.Width, projectile.m_ProjectileTexture.Height);
            if (enemyRectangle.Intersects(projectileRectangle)){                
                collision = pixelCollision(enemy, projectile, Rectangle.Intersect(projectileRectangle,enemyRectangle));
            }
            return collision;
        }

        private bool pixelCollision(Enemy1 enemy, Projectile projectile, Rectangle rectangle)
        {            
            Color[] color1 = new Color[enemy.Texture.Width * enemy.Texture.Height];
            Color[] color2 = new Color[projectile.m_ProjectileTexture.Width * projectile.m_ProjectileTexture.Height];
            enemy.Texture.GetData(color1);
            projectile.m_ProjectileTexture.GetData(color2);
            projectile.m_ProjectileTexture.GetData(color2);
            int x1 = rectangle.X;
            int x2 = rectangle.X+rectangle.Width;
            int y1 = rectangle.Y;
            int y2 = rectangle.Y+rectangle.Height;
            for (int y= y1;y< y2; y++)
            {
                for(int x=x1; x < x2; x++)
                {
                    Color a = color1[Math.Abs((x-(int)enemy.Position.X)) + Math.Abs((y-(int)enemy.Position.Y)) * enemy.Texture.Width];
                    Color b = color2[Math.Abs((x - (int)projectile.m_Position.X)) + Math.Abs((y - (int)projectile.m_Position.Y)) * projectile.m_ProjectileTexture.Width];
                    if(a.A !=0 && b.A != 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void SpawnEnemies(GameTime gameTime)
        {
            if (gameTime.TotalGameTime - currentTime > spawnTimeSpan)
            {
                currentTime = gameTime.TotalGameTime;
                EnemyBase enemy = random.Next(0,2) == 0 ? (EnemyBase)new LinearTriangleEnemy() : new SquigglyTriangleEnemy();
                enemy.Initialize(enemyTexture, new Vector2(random.Next(enemyTexture.Width, WIDTH - enemyTexture.Width),0));
                enemiesList.Add(enemy);
            }
        }

        private void UpdatePlayer(GameTime gameTime)
        {
            player.Update();

            //Check the case where the space bar is pressed
            if (previousKeyboardState.IsKeyUp(Keys.Space) && currentKeyboardState.IsKeyDown(Keys.Space))
            {
                //Launch the projectile
                projectile.m_bInOrbitToPlayer = false;                
            }
        }

        protected void UpdateProjectile(GameTime gameTime) {

            if (projectile.m_Position.X <= 10f || projectile.m_Position.X >= GraphicsDevice.Viewport.TitleSafeArea.Width - 10f)
            {
                projectile.m_fProjectileSpeedX = -projectile.m_fProjectileSpeedX;
                projectile.m_iBounces++;
                edge = edge_hit;
            }else if (projectile.m_Position.Y <= 10f || projectile.m_Position.Y >= GraphicsDevice.Viewport.TitleSafeArea.Height - 10f)
            {
                projectile.m_fProjectileSpeedY = -projectile.m_fProjectileSpeedY;
                projectile.m_iBounces++;
                edge = edge_hit;
            }else
            {
                edge = edge_normal;
            }

            if (projectile.m_iBounces > 3) {
                projectile = new Projectile();
                projectile.Initialize(projectileTexture, projectilePosition, Vector2.Zero);
            }


            //Check is projectile has been launched, rotate it around its center
            if (currentKeyboardState.IsKeyDown(Keys.Space)){                
                projectile.selfRotate = true;
            }else
            {
                projectile.selfRotate = false;
            }            

            projectile.Update(player.m_Position, gameTime);
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

            // Draw edge
            Rectangle edgeRectangle = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            spriteBatch.Draw(edge, edgeRectangle, Color.White);

            // Draw ring line
            Rectangle ringLineRectangle = new Rectangle(0, 0, ringLineTexture.Width, ringLineTexture.Height);
            spriteBatch.Draw(ringLineTexture, ringLinePosition, ringLineRectangle, Color.White, 0f, ringLineOrigin, 0.5f, SpriteEffects.None, 0f);

            //Draw the Player
            player.Draw(spriteBatch);

            //Draw the projectile
            projectile.Draw(spriteBatch);

            //Draw enemies
            DrawEnemies(spriteBatch);

            //Draw ui
            ui.Draw(spriteBatch);

            //Draw the score
            spriteBatch.DrawString(ui.scoreFont, ui.score.ToString(), new Vector2(150,80), Color.White);

            //Draw the player health
            spriteBatch.DrawString(ui.scoreFont, "Health: " + ui.playerHealth.ToString(), new Vector2(300, 80), Color.White);

            //Stop drawing
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawEnemies(SpriteBatch spriteBatch)
        {
            foreach (EnemyBase enemy in enemiesList)
            {
				if (!enemy.m_Active) continue;
                enemy.Draw(spriteBatch);
            }
        }
    }
}
