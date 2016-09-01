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

        //represents the previous projectile position
        Vector2 previousProjectilePosition;

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

        //The texture of the player damage
        Texture2D playerDamageTexture;
        //Flash the damage texture
        bool flashDamage;

        //Check if the game over context is active
        bool gameOver;

        //Ring+line texture
        Texture2D ringLineTexture;
        Vector2 ringLinePosition;
        Vector2 ringLineOrigin;
        Rectangle ringLineRectangle;

        // Enemy list
        List<EnemyBase> enemiesList;
        Texture2D[] enemyTextures;

        // Enemy death particle list
        List<Texture2D> enemyPiecesList;

        // Particle system list
        ParticleSystemBase enemyDeathPS;
        List<ParticleSystemBase> activeParticleSystems;

        // Random
        Random random;

        TimeSpan collisionTime = TimeSpan.FromSeconds(0.1);
        TimeSpan previousCollisionTime = TimeSpan.Zero;

        // Screen resolution
        const int WIDTH = 750;
        const int HEIGHT = 1000;

        // Spawn interval
        const float SPAWN_INTERVAL = 2f;
        TimeSpan spawnTimeSpan;

        // Current game time
        TimeSpan currentTime;

        //The damage flash time
        TimeSpan damageFlashTime = TimeSpan.FromSeconds(0.4);
        // Current damage falshgame time
        TimeSpan currentDamagFlashTime = TimeSpan.Zero;

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

            enemyDeathPS = new ParticleSystemBase(0f, 1f, 0.5f, 1.5f,
                0.1f, 0.25f,
                new Vector2(-4f, -4f), new Vector2(4f, 4f),
                0.02f, 0.1f);

            activeParticleSystems = new List<ParticleSystemBase>() {
                enemyDeathPS
            };


            random = new Random();
            currentTime = TimeSpan.Zero;
            spawnTimeSpan = TimeSpan.FromSeconds(SPAWN_INTERVAL);
            ui = new UI();
            gameOver = false;

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
            ringLineRectangle = new Rectangle(0, 0, ringLineTexture.Width, ringLineTexture.Height);

            // Load edge textures
            edge_normal = Content.Load<Texture2D>("Graphics/Edge_normal");
            edge_hit = Content.Load<Texture2D>("Graphics/Edge_Hit");
            edge = edge_normal;

            // Load enemy texture
            enemyTextures = new Texture2D[4];
            enemyTextures[0] = Content.Load<Texture2D>("Graphics/E1");
            enemyTextures[1] = Content.Load<Texture2D>("Graphics/E2");
            enemyTextures[2] = Content.Load<Texture2D>("Graphics/E3");
            enemyTextures[3] = Content.Load<Texture2D>("Graphics/E3_Shield");

            // Load enemy piece textures
            enemyPiecesList = new List<Texture2D>() {
                Content.Load<Texture2D>("Graphics/Piece_01"),
                Content.Load<Texture2D>("Graphics/Piece_02"),
                Content.Load<Texture2D>("Graphics/Piece_03"),
                Content.Load<Texture2D>("Graphics/Piece_04")
            };

            //Load the player damage texture
            playerDamageTexture = Content.Load<Texture2D>("Graphics/EdgeFadeV2");


            //Load the score texture
            Vector2 scorePosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + (GraphicsDevice.Viewport.Width * 0.22f), GraphicsDevice.Viewport.TitleSafeArea.Y + (GraphicsDevice.Viewport.Height * 0.040f));
            ui.InitializeScore(Content.Load<Texture2D>("Graphics/ScoreUI"), scorePosition, Vector2.Zero);

            Vector2 healthPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + (GraphicsDevice.Viewport.Width * 0.73f), GraphicsDevice.Viewport.TitleSafeArea.Y + (GraphicsDevice.Viewport.Height * 0.030f));
            Vector2 healthBarPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + (GraphicsDevice.Viewport.Width * 0.58f), GraphicsDevice.Viewport.TitleSafeArea.Y + (GraphicsDevice.Viewport.Height * 0.030f));
            ui.InitializeHealth(Content.Load<Texture2D>("Graphics/HealthUI"), healthPosition, Content.Load<Texture2D>("Graphics/HealthBarUI"), healthBarPosition);

            //Load the score font
            ui.scoreFont = Content.Load<SpriteFont>("Fonts/Score");

            //Load the health font
            ui.healthFont = Content.Load<SpriteFont>("Fonts/Health");

            //Load the game over font
            ui.gameOverFont = Content.Load<SpriteFont>("Fonts/GameOver");
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
            UpdateEnemies(gameTime);

            //Update the UI
            ui.Update();

            //Update the combo
            UpdateCombo(gameTime);

            // Update all particle systems
            UpdateParticleSystems();

            UpdateDamageFlash(gameTime);

            base.Update(gameTime);
        }

        private void UpdateEnemies(GameTime gameTime)
        {
            for (int i = 0; i < enemiesList.Count; i++)
            {
                EnemyBase enemy = enemiesList[i];
                if (enemy.m_Active)
                {
                    enemy.Update();
                    if (CheckCollisionWithPlayer(enemy, gameTime))
                    {
                        if (enemy.GetType() != typeof(Shield))
                        {
                            if ((enemy.m_Type == EnemyBase.Type.SquigglyTriangle))
                            {

                                if (!projectile.selfRotate)
                                {
                                    // Destroy enemy
                                    enemy.m_Active = true;

                                }
                                else
                                {
                                    enemy.m_Active = false;
                                    //Add points to the player score
                                    ui.score += 100 * player.m_iComboMultiplier;
                                }
                            }
                            else
                            {
                                enemy.m_Active = false;
                                //Add points to the player score
                                ui.score += 100 * player.m_iComboMultiplier;
                                if (enemy.GetType() == typeof(HexagonEnemy))
                                {
                                    ((HexagonEnemy)enemy).shield.m_Active = false;
                                }
                            }
                        }
                        // Bounce off the shield
                        else
                        {


                            if (0 < Math.Abs(enemy.m_Rotation) && Math.Abs(enemy.m_Rotation) <= Math.PI / 3)
                            {
                                projectile.m_fProjectileSpeedX = -projectile.m_fProjectileSpeedX;
                            }
                            else if (Math.PI / 3 < Math.Abs(enemy.m_Rotation) && Math.Abs(enemy.m_Rotation) <= 2 * Math.PI / 3)
                            {
                                projectile.m_fProjectileSpeedY = -projectile.m_fProjectileSpeedY;
                            }
                            else if (2 * Math.PI / 3 < Math.Abs(enemy.m_Rotation) && Math.Abs(enemy.m_Rotation) <= Math.PI)
                            {
                                projectile.m_fProjectileSpeedX = -projectile.m_fProjectileSpeedX;
                            }
                            else if (Math.PI < Math.Abs(enemy.m_Rotation) && Math.Abs(enemy.m_Rotation) <= 4 * Math.PI / 3)
                            {
                                projectile.m_fProjectileSpeedX = -projectile.m_fProjectileSpeedX;
                            }
                            else if (4 * Math.PI / 3 < Math.Abs(enemy.m_Rotation) && Math.Abs(enemy.m_Rotation) <= 5 * Math.PI / 3)
                            {
                                projectile.m_fProjectileSpeedY = -projectile.m_fProjectileSpeedY;
                            }
                            else
                            {
                                projectile.m_fProjectileSpeedX = -projectile.m_fProjectileSpeedX;
                            }
                        }


                    }
                    else if (CheckCollisionWithPlayerShield(enemy))
                    {
                        player.m_iHealth -= 10;
                        enemy.m_Active = false;
                        flashDamage = true;
                        //Cap the maximum health to lose
                        if (player.m_iHealth >= 0f)
                        {
                            ui.playerHealth = player.m_iHealth;
                        }
                    }
                }
                else
                {
                    enemy.Kill();
                    enemiesList.RemoveAt(i);
                }

            }
        }

        private bool CheckCollisionWithPlayerShield(EnemyBase enemy)
        {
            bool collision = false;
            Rectangle enemyRectangle;
            enemyRectangle = GetEnemyRectangle(enemy);
            Rectangle playerShieldRectangle = new Rectangle(0, 910, ringLineTexture.Width / 2, 2);
            if (enemyRectangle.Intersects(playerShieldRectangle))
            {
                collision = true;
            }
            else
            {
                float distance = Vector2.Distance(player.m_Position, new Vector2(enemy.m_Position.X + enemy.Texture.Width / 2, enemy.m_Position.Y + enemy.Texture.Height / 2));
                if (distance <= projectile.rotationRadius + projectile.m_iSpriteWidth / 2)
                {
                    collision = true;
                }
            }
            return collision;
        }

        private static Rectangle GetEnemyRectangle(EnemyBase enemy)
        {
            Rectangle enemyRectangle;
            if (enemy.GetType() != typeof(Shield))
            {
                enemyRectangle = new Rectangle((int)enemy.m_Position.X, (int)enemy.m_Position.Y, enemy.Texture.Width * 7 / 10, enemy.Texture.Height * 6 / 10);
            }
            else
            {
                enemyRectangle = new Rectangle((int)enemy.m_Position.X - enemy.Texture.Width / 2, (int)enemy.m_Position.Y - enemy.Texture.Height / 2, enemy.Texture.Width, enemy.Texture.Height);
            }

            return enemyRectangle;
        }

        private bool CheckCollisionWithPlayer(EnemyBase enemy, GameTime gameTime)
        {
            bool collision = false;
            // Only check collision if projectile is released
            if (!projectile.m_bInOrbitToPlayer)
            {
                Rectangle enemyRectangle;
                enemyRectangle = GetEnemyRectangle(enemy);
                Rectangle projectileRectangle = new Rectangle((int)projectile.m_Position.X - projectile.m_ProjectileTexture.Width / 2, (int)projectile.m_Position.Y - projectile.m_ProjectileTexture.Height / 2, projectile.m_ProjectileTexture.Width, projectile.m_ProjectileTexture.Height);
                if (enemyRectangle.Intersects(projectileRectangle))
                {
                    collision = pixelCollision(enemy, projectile.m_ProjectileTexture, projectile.m_Position, Rectangle.Intersect(projectileRectangle, enemyRectangle));
                }
                //check the enemy type if collision happened
                if (collision)
                {
                    CheckEnemyType(enemy, enemyRectangle, gameTime);
                }
            }
            return collision;
        }

        private bool pixelCollision(EnemyBase enemy, Texture2D texture, Vector2 position, Rectangle rectangle)
        {
            Color[] color1 = new Color[enemy.Texture.Width * enemy.Texture.Height];
            Color[] color2 = new Color[texture.Width * texture.Height];
            enemy.Texture.GetData(color1);
            texture.GetData(color2);
            int x1 = rectangle.X;
            int x2 = rectangle.X + rectangle.Width;
            int y1 = rectangle.Y;
            int y2 = rectangle.Y + rectangle.Height;
            for (int y = y1; y < y2; y++)
            {
                for (int x = x1; x < x2; x++)
                {
                    Color a = color1[Math.Abs((x - (int)enemy.m_Position.X)) + Math.Abs((y - (int)enemy.m_Position.Y)) * enemy.Texture.Width];
                    Color b = color2[Math.Abs((x - (int)position.X)) + Math.Abs((y - (int)position.Y)) * texture.Width];
                    if (a.A != 0 && b.A != 0)
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
                int spawn = random.Next(0, 3);
                EnemyBase enemy = null;
                EnemyBase shield = null;
                switch (spawn)
                {
                    case 0:
                        enemy = new LinearTriangleEnemy();
                        enemy.Initialize(enemyTextures[0], new Vector2(random.Next(enemyTextures[0].Width, WIDTH - enemyTextures[0].Width), 0));
                        break;
                    case 1:
                        enemy = new SquigglyTriangleEnemy();
                        enemy.Initialize(enemyTextures[1], new Vector2(random.Next(enemyTextures[1].Width, WIDTH - enemyTextures[1].Width), 0));
                        break;
                    case 2:
                        enemy = new HexagonEnemy();
                        enemy.Initialize(enemyTextures[2], new Vector2(random.Next(enemyTextures[2].Width, WIDTH - enemyTextures[2].Width), 0));
                        shield = new Shield(ref enemy);
                        shield.Initialize(enemyTextures[3], Vector2.Zero);
                        ((HexagonEnemy)enemy).setShield(ref shield);
                        break;
                    default:
                        break;
                }
                if (null != enemy)
                {
                    enemy.onDeath += new EnemyBase.EnemyEventHandler(ShowEnemyDeath);
                    enemiesList.Add(enemy);
                }
                if (null != shield)
                    enemiesList.Add(shield);
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

            //Check if the player pressed Yor N and the game over context is on
            if (gameOver)
            {
                if (currentKeyboardState.IsKeyDown(Keys.N))
                {
                    Exit();
                }
                else if(currentKeyboardState.IsKeyDown(Keys.Y)){

                    ResetGame();

                }



            }
        }

        protected void UpdateProjectile(GameTime gameTime)
        {

            previousProjectilePosition = projectile.m_Position;

            if (projectile.m_Position.X <= 10f || projectile.m_Position.X >= GraphicsDevice.Viewport.TitleSafeArea.Width - 10f)
            {
                projectile.m_fProjectileSpeedX = -projectile.m_fProjectileSpeedX;
                projectile.m_iBounces++;
                edge = edge_hit;
            }
            else if (projectile.m_Position.Y <= 10f || projectile.m_Position.Y >= GraphicsDevice.Viewport.TitleSafeArea.Height - 10f)
            {
                projectile.m_fProjectileSpeedY = -projectile.m_fProjectileSpeedY;
                projectile.m_iBounces++;
                edge = edge_hit;
            }
            else
            {
                edge = edge_normal;
            }

            if (projectile.m_iBounces > 3)
            {
                projectile = new Projectile();
                projectile.Initialize(projectileTexture, projectilePosition, Vector2.Zero);
            }


            //Check is projectile has been launched, rotate it around its center
            if (currentKeyboardState.IsKeyDown(Keys.Space))
            {
                projectile.selfRotate = true;
            }
            else
            {
                projectile.selfRotate = false;
            }

            projectile.Update(player.m_Position, gameTime);
        }


        private void UpdateCombo(GameTime gameTime)
        {

            if (player.m_bComboActive)
            {

                player.m_CurrentComboTime += gameTime.ElapsedGameTime;


                if (player.m_CurrentComboTime >= player.m_ComboCoolDown)
                {
                    player.m_iComboMultiplier = 0;
                    player.m_bComboActive = false;
                    player.m_CurrentComboTime = TimeSpan.Zero;
                }

            }
        }

        private void CheckEnemyType(EnemyBase enemy, Rectangle enemyRectangle, GameTime gameTime)
        {

            //Check the enemy type
            if (gameTime.TotalGameTime - previousCollisionTime > collisionTime)
            {
                previousCollisionTime = gameTime.TotalGameTime;

                //Compare to yellow triangle
                if (enemy.m_Type == EnemyBase.Type.LinearTriangle)
                {
                    player.m_CurrentComboTime = TimeSpan.Zero;
                    player.m_bComboActive = true;
                    player.m_iComboMultiplier += 1;

                    //Check if the projectile is not spinning
                    if (!projectile.selfRotate)
                    {

                        if (previousProjectilePosition.Y < enemy.m_Position.Y + 20f)
                        {

                            //Bounce it off enemy on the y component
                            projectile.m_fProjectileSpeedY = -projectile.m_fProjectileSpeedY;
                        }
                        else
                        {
                            //Bounce it off enemy on the x component
                            projectile.m_fProjectileSpeedX = -projectile.m_fProjectileSpeedX;
                        }
                    }

                }

                //Compare to purple triangle
                if (enemy.m_Type == EnemyBase.Type.SquigglyTriangle)
                {

                    //Check if the projectile is spinning
                    if (projectile.selfRotate)
                    {
                        player.m_CurrentComboTime = TimeSpan.Zero;
                        player.m_bComboActive = true;
                        player.m_iComboMultiplier += 1;
                    }

                    else
                    {

                        if (previousProjectilePosition.Y < enemy.m_Position.Y + 20f)
                        {

                            //Bounce it off enemy on the y component
                            projectile.m_fProjectileSpeedY = -projectile.m_fProjectileSpeedY;
                        }
                        else
                        {
                            //Bounce it off enemy on the x component
                            projectile.m_fProjectileSpeedX = -projectile.m_fProjectileSpeedX;
                        }
                    }

                }

                //Compare to orange hexagon
                if (enemy.m_Type == EnemyBase.Type.Hexagon)
                {
                    player.m_CurrentComboTime = TimeSpan.Zero;
                    player.m_bComboActive = true;
                    player.m_iComboMultiplier += 1;

                }



            }//end of check collision time
        }

        void UpdateDamageFlash(GameTime gameTime) {

            currentDamagFlashTime += gameTime.ElapsedGameTime;

            if (currentDamagFlashTime >= damageFlashTime) {

                flashDamage = false;
                currentDamagFlashTime = TimeSpan.Zero;
            }

        }


        void UpdateParticleSystems()
        {
            foreach (ParticleSystemBase ps in activeParticleSystems)
                ps.Update();
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
            spriteBatch.Draw(backgroundTexture, sourceRectangle, Color.White);

            // Draw ring line            
            spriteBatch.Draw(ringLineTexture, ringLinePosition, ringLineRectangle, Color.White, 0f, ringLineOrigin, 0.5f, SpriteEffects.None, 0f);

            // Draw edge
            Rectangle edgeRectangle = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            spriteBatch.Draw(edge, edgeRectangle, Color.White);

            //Draw the Player
            player.Draw(spriteBatch);

            //Draw the projectile
            projectile.Draw(spriteBatch);

            //Draw enemies
            DrawEnemies(spriteBatch);

            DrawParticleSystems(spriteBatch);

            //Draw ui
            ui.Draw(spriteBatch);

            //Draw the score
            spriteBatch.DrawString(ui.scoreFont, ui.score.ToString(), new Vector2(78, 40), Color.White);


            if (player.m_bComboActive && player.m_iComboMultiplier > 1)
            {
                //Draw the combo indicator
                spriteBatch.DrawString(ui.scoreFont, "Combo: x" + player.m_iComboMultiplier.ToString(), new Vector2(150, 80), Color.White);
            }

            //Draw the player health
            spriteBatch.DrawString(ui.healthFont, ui.playerHealth.ToString() + "%", new Vector2(680, 35), Color.White);

            //Draw the damage rectangle
            Rectangle damageRectangle = new Rectangle(0, 0, playerDamageTexture.Width, playerDamageTexture.Height);
            Vector2 origin = new Vector2(playerDamageTexture.Width/2,playerDamageTexture.Height/2);
            Vector2 position = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height - 200);

            //Draw the flash damage
            if (flashDamage) {
                spriteBatch.Draw(playerDamageTexture, position,damageRectangle, Color.Red,0f, origin, 1f, SpriteEffects.None, 0f);
            }

            //Draw the game over screen
            if (ui.playerHealth <= 0) {
                //Draw the combo indicator
                spriteBatch.DrawString(ui.gameOverFont, "Replay Y/N?", new Vector2(GraphicsDevice.Viewport.Width/2 - 200, GraphicsDevice.Viewport.Height/2 - 20), Color.White);
                gameOver = true;
            }

            //Stop drawing
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void ResetGame() {
            player.Reset();
            projectile = new Projectile();
            projectile.Initialize(projectileTexture, projectilePosition, Vector2.Zero);
            enemiesList.Clear();
            ui.playerHealth = 100;
            gameOver = false;
        }


        private void DrawEnemies(SpriteBatch spriteBatch)
        {
            foreach (EnemyBase enemy in enemiesList)
            {
                if (!enemy.m_Active) continue;
                enemy.Draw(spriteBatch);
            }
        }

        void DrawParticleSystems(SpriteBatch spriteBatch)
        {
            foreach (ParticleSystemBase ps in activeParticleSystems)
                ps.Draw(spriteBatch);
        }

        void ShowEnemyDeath(EnemyBase enemy)
        {
            if (enemyDeathPS.m_Sprites == null)
                enemyDeathPS.m_Sprites = enemyPiecesList;

            enemyDeathPS.SetWind(new Vector2(
                (float)(projectile.m_fProjectileSpeedX * Math.Sin(projectile.m_fProjectileRotation_fixed)),
                -(float)(projectile.m_fProjectileSpeedY * Math.Cos(projectile.m_fProjectileRotation_fixed))
            ) * 0.6f);
            enemyDeathPS.m_Position = enemy.m_Position + enemy.m_Center;
            enemyDeathPS.SetTint(enemy.m_Color);
            enemyDeathPS.Emit(8);
        }
    }

}
