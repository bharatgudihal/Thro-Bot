using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

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

        bool gamePaused = false;

        //Represents the player
        Player player;

        //represents the projectile
        Projectile projectile;
        Vector2 projectilePosition;

        //represents the previous projectile position
        Vector2 previousProjectilePosition;

        //texture of the projectile
        Texture2D projectileTexture;
        Texture2D spinningProjectileTexture;
        Texture2D projectileTrailTexture;

        SoundEffect spinLoopSnd;
        SoundEffectInstance spinLoopInstance;

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

        SoundEffect wallBoundSnd;

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

        //Power Up list
        List<PowerUp> powerUpsList;
        Texture2D[] powerUpTextures;
		Texture2D healthPickupEffect;
		SoundEffect pickupHealthSnd;

        // Enemy death particle list
        List<Texture2D> enemyPiecesList;
        SoundEffect enemyDeathSnd;

        // Particle system list
        ParticleSystemBase enemyDeathPS;
        ParticleSystemBase bouncePS;
		ParticleSystemBase pickupHealthPS;
		ParticleSystemBase pickupSpawnPS;
        List<ParticleSystemBase> activeParticleSystems;

        // Random
        Random random;

        TimeSpan collisionTime = TimeSpan.FromSeconds(0.1);
        TimeSpan previousCollisionTime = TimeSpan.Zero;

        // Screen resolution
        const int WIDTH = 750;
        const int HEIGHT = 1000;

        // Spawn interval
        const float SPAWN_INTERVAL = 1.5f;
        TimeSpan spawnTimeSpan;

        //Power ups spawn interval
        float POWERUP_INTERVAL = 10f;
        TimeSpan powerUpTimeSpan;

        // Current game time
        TimeSpan currentPowerUpTime;


        // Current game time
        TimeSpan currentTime;

        //The damage flash time
        TimeSpan damageFlashTime = TimeSpan.FromSeconds(0.4);
        // Current damage falshgame time
        TimeSpan currentDamagFlashTime = TimeSpan.Zero;

        //The timespan for a single tap
        TimeSpan currentpaceBarTap = TimeSpan.Zero;

        //the timespan for two tap
        TimeSpan doubleTap = TimeSpan.FromSeconds(0.2);
        // Sound effects
        private SoundEffect enemyDeath;
        private SoundEffect playerDeath;
        private SoundEffect spinLoop;
        private SoundEffect wallBounce;
        SoundEffect discHitEnemySnd;
        SoundEffect discHitShieldSnd;
        SoundEffect throwDiscSnd;
        SoundEffect recallDiscSnd;
        SoundEffect playerHurtSnd;
        //the last killed enemy
        EnemyBase lastKilledEnemy = null;

        // Boss variables
        EnemyBase boss;
        TimeSpan lastBossTime = TimeSpan.Zero;
        TimeSpan bossSpawnDelay = TimeSpan.FromMinutes(1);
        private Texture2D bossTexture;
        private Texture2D bossShieldTexture;
        private bool bossIsSpawned = false;
        //private BossShield bossShield;
        private BossShield[] bossShields;
        private bool bossAnimationStarted = false;
        private BossCore bossCore;
        private Lazer lazer;
		SoundEffect bossIdleSnd;
		SoundEffect laserChargeSnd;
		SoundEffect laserShootSnd;
		SoundEffectInstance bossIdleLoop;
		SoundEffectInstance laserCharge;
		SoundEffectInstance laserShootLoop;
        private Texture2D bossCoreTexture;
        private Texture2D lazerTexture;
        private TimeSpan currentCoreTime = TimeSpan.Zero;
        private TimeSpan coreAnimationTime = TimeSpan.FromSeconds(1);
        private TimeSpan previousBossCollision = TimeSpan.Zero;
        private TimeSpan bossCollisionTime = TimeSpan.FromMilliseconds(50);
        private TimeSpan previousBossAnimationTime = TimeSpan.Zero;
        private TimeSpan bossAnimationTime = TimeSpan.FromSeconds(15);

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
            powerUpsList = new List<PowerUp>();

            enemyDeathPS = new ParticleSystemBase(0f, 1f, 4,
                0.5f, 1.5f,
                0.05f, 0.25f,
                new Vector2(-4f, -4f), new Vector2(4f, 4f),
                0.02f, 0.1f);

            bouncePS = new ParticleSystemBase(0f, 0.5f, 5,
                0.3f, 1f,
                0.05f, 0.15f,
                new Vector2(-3f, -3f), new Vector2(3f, 3f),
                0.02f, 0.1f);

			pickupHealthPS = new ParticleSystemBase (0f, 1f, 8,
				0.8f, 1.6f,
				0.25f, 0.5f,
				new Vector2 (-2f, -2f), new Vector2 (2f, 2f),
				0f, 0f, true, false);

			pickupSpawnPS = new ParticleSystemBase (0f, 1f, 6,
				0.8f, 1.6f,
				0.2f, 0.4f,
				new Vector2 (-4f, -4f), new Vector2 (4f, 4f),
				0f, 0f, true, false);

            activeParticleSystems = new List<ParticleSystemBase>() {
                enemyDeathPS,
                bouncePS,
				pickupHealthPS,
				pickupSpawnPS
            };


            random = new Random();
            currentTime = TimeSpan.Zero;
            spawnTimeSpan = TimeSpan.FromSeconds(SPAWN_INTERVAL);
            powerUpTimeSpan = TimeSpan.FromSeconds(POWERUP_INTERVAL);
            currentPowerUpTime = TimeSpan.Zero;
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
            player.DeathAnimation(Content.Load<Texture2D>("Graphics/DeathShake"), 100, 100, 8, 90, false);


            //Load the projectile texture
            projectilePosition = new Vector2(playerPosition.X + 10f, playerPosition.Y);
            projectileTexture = Content.Load<Texture2D>("Graphics/Discv2");
            projectileTrailTexture = Content.Load<Texture2D>("Graphics/Discv2");
            spinningProjectileTexture = Content.Load<Texture2D>("Graphics/DiscFinal_spin");
            projectile.Initialize(projectileTexture, projectilePosition, Vector2.Zero);
            projectile.InitializeTrail(new List<Texture2D>() { projectileTrailTexture });
            activeParticleSystems.Add(projectile.m_Trail);
            spinLoopSnd = Content.Load<SoundEffect>("Sounds/SpinLoop");
            discHitEnemySnd = Content.Load<SoundEffect>("Sounds/DiscHitEnemy");
            discHitShieldSnd = Content.Load<SoundEffect>("Sounds/DiscHitShield");
            recallDiscSnd = Content.Load<SoundEffect>("Sounds/RecallDisc");
            throwDiscSnd = Content.Load<SoundEffect>("Sounds/ThrowDisc");

            //Load the background 
            backgroundTexture = Content.Load<Texture2D>("Graphics/BackgroundDark");
			wallBoundSnd = Content.Load<SoundEffect>("Sounds/WallBounce");

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

            //Load the powerUp textures
            powerUpTextures = new Texture2D[1];
            powerUpTextures[0] = Content.Load<Texture2D>("Graphics/HealthPowerUp");
			healthPickupEffect = Content.Load<Texture2D>("Graphics/HealthEffect");
			pickupHealthSnd = Content.Load<SoundEffect>("Sounds/PickupHealth");

            // Load enemy piece textures
            enemyPiecesList = new List<Texture2D>() {
                Content.Load<Texture2D>("Graphics/Piece_01"),
                Content.Load<Texture2D>("Graphics/Piece_02"),
                Content.Load<Texture2D>("Graphics/Piece_03"),
                Content.Load<Texture2D>("Graphics/Piece_04"),
                Content.Load<Texture2D>("Graphics/SmallPiece")
            };

            enemyDeathSnd = Content.Load<SoundEffect>("Sounds/EnemyDeath");

            //Load the player damage texture
            playerDamageTexture = Content.Load<Texture2D>("Graphics/EdgeFadeV2");
            playerHurtSnd = Content.Load<SoundEffect>("Sounds/PlayerHurt");

            //Load the score texture
            Vector2 scorePosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + (GraphicsDevice.Viewport.Width * 0.22f), GraphicsDevice.Viewport.TitleSafeArea.Y + (GraphicsDevice.Viewport.Height * 0.040f));
            ui.InitializeScore(Content.Load<Texture2D>("Graphics/ScoreUI"), scorePosition, Vector2.Zero);

            //Load the health textures
            Vector2 healthPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + (GraphicsDevice.Viewport.Width * 0.73f), GraphicsDevice.Viewport.TitleSafeArea.Y + (GraphicsDevice.Viewport.Height * 0.030f));
            Vector2 healthBarPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + (GraphicsDevice.Viewport.Width * 0.58f), GraphicsDevice.Viewport.TitleSafeArea.Y + (GraphicsDevice.Viewport.Height * 0.030f));
            ui.InitializeHealth(Content.Load<Texture2D>("Graphics/HealthUI"), healthPosition, Content.Load<Texture2D>("Graphics/HealthBarUI"), healthBarPosition);

            //Load the stamina textures
            Vector2 staminaFramePosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + (GraphicsDevice.Viewport.Width * 0.805f), GraphicsDevice.Viewport.TitleSafeArea.Y + (GraphicsDevice.Viewport.Height * 0.064f));
            Vector2 staminaBarPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + (GraphicsDevice.Viewport.Width * 0.82f), GraphicsDevice.Viewport.TitleSafeArea.Y + (GraphicsDevice.Viewport.Height * 0.064f));
            ui.InitializeStamina(Content.Load<Texture2D>("Graphics/StaminaFrameUI"), staminaFramePosition, Content.Load<Texture2D>("Graphics/StaminaBarUI"), staminaBarPosition);

            //Load the glitch texture
            Vector2 glitchPosition = new Vector2(GraphicsDevice.Viewport.Width /2, GraphicsDevice.Viewport.Height/2);
            ui.InitializeGlitchScreen(Content.Load<Texture2D>("Graphics/Glitch"), glitchPosition);


            //Load the score font
            ui.scoreFont = Content.Load<SpriteFont>("Fonts/Score");

            //Load the health font
            ui.healthFont = Content.Load<SpriteFont>("Fonts/Health");

            //Load the game over font
            ui.gameOverFont = Content.Load<SpriteFont>("Fonts/GameOver");

            //Load the combo font
            ui.comboFont = Content.Load<SpriteFont>("Fonts/Combo");

            //Loading sounds
            enemyDeath = Content.Load<SoundEffect>("Sounds/EnemyDeath");
            playerDeath = Content.Load<SoundEffect>("Sounds/player_death");
            spinLoop = Content.Load<SoundEffect>("Sounds/SpinLoop");
            wallBounce = Content.Load<SoundEffect>("Sounds/WallBounce");

            // Loading boss texture
            bossTexture = Content.Load<Texture2D>("Graphics/Boss");
            bossShieldTexture = Content.Load<Texture2D>("Graphics/Boss_Shield");
            bossCoreTexture = Content.Load<Texture2D>("Graphics/Boss_core");
            lazerTexture = Content.Load<Texture2D>("Graphics/Lazer");
			bossIdleSnd = Content.Load<SoundEffect>("Sounds/BossIdle");
			laserChargeSnd = Content.Load<SoundEffect>("Sounds/LaserCharge");
			laserShootSnd = Content.Load<SoundEffect>("Sounds/LaserShootLoop");
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
            if (!gamePaused)
            {
                // Spawn Boss
                if (gameTime.TotalGameTime - lastBossTime > bossSpawnDelay)
                {
                    if (enemiesList.Count == 0)
                    {
                        lastBossTime = gameTime.TotalGameTime;
                        SpawnBoss(gameTime);
                        previousBossAnimationTime = gameTime.TotalGameTime;
                    }
                } // Update Boss if spawned
                else
                if (bossIsSpawned)
                {
                    UpdateBoss(gameTime);
                }
                else
                {
                    // Spawn enemies
                    SpawnEnemies(gameTime);
                }
                //Spawn a power up
                SpawnPowerUps(gameTime);
            }
            //Update the player
            UpdatePlayer(gameTime);
            if (!gamePaused)
            {
                //Update the projectile
                UpdateProjectile(gameTime);

                // Update enemy
                if (!bossIsSpawned)
                {
                    UpdateEnemies(gameTime);
                }
                //Update powerUps
                UpdatePowerUps(gameTime);
            }
            //Update the UI
            ui.Update(gameTime);

            //Update the combo
            UpdateCombo(gameTime);

            // Update all particle systems
            UpdateParticleSystems();

            UpdateDamageFlash(gameTime);


            if (player.finishedAnimation)
            {

                //Freeze the game
                gameOver = true;
                gamePaused = true;
            }

            base.Update(gameTime);
        }

        private void UpdateBoss(GameTime gameTime)
        {
            if(gameTime.TotalGameTime - previousBossAnimationTime > bossAnimationTime && !bossAnimationStarted)
            {
                previousBossAnimationTime = gameTime.TotalGameTime;
                bossAnimationStarted = true;
                bossCore = new BossCore();
                bossCore.Initialize(bossCoreTexture, boss.m_Position);
            }
            if (!bossAnimationStarted)
            {
                bossCore = null;
                lazer = null;
                foreach (EnemyBase enemy in enemiesList)
                {
                    enemy.Update(gameTime);
                    CheckBossCollisions(enemy, gameTime);                    
                }
                // If boss is killed, clear the list
                if (!enemiesList[0].m_Active)
                {
                    enemiesList.Clear();
                    lastBossTime = gameTime.TotalGameTime;
                }
            }// If the boss core has not blinked
            else if (bossCore.GetOpactity() < 1f)
            {
                // Lerp opacity towards 1
                bossCore.SetOpacity(bossCore.GetOpactity() + 1f / 60f);
				if (laserCharge == null) {
					laserCharge = laserChargeSnd.CreateInstance();

				}
				if (laserCharge.State != SoundState.Playing) {
					laserCharge.Play();
				}
            }
            else
            {
                // Fire Lazers!!!
                if (null == lazer)
                {
                    lazer = new Lazer();
					lazer.Initialize(lazerTexture, new Vector2(boss.m_Position.X - lazerTexture.Width / 2, boss.m_Position.Y + 30f));
					if (laserShootLoop == null) {
						laserShootLoop = laserShootSnd.CreateInstance();
						laserShootLoop.IsLooped = true;
					}
					laserCharge.Stop();
					laserShootLoop.Play();
					playerHurtSnd.Play(1f, random.RandomFloat(-0.1f, 0.1f), 0f);
                }
                else
                {
                    currentCoreTime += gameTime.ElapsedGameTime;
                    if (currentCoreTime < coreAnimationTime)
                    {
                        player.m_iHealth -= 0.5f;
                        
                        flashDamage = true;
                        ui.glitchScreen = true;
                        //Cap the maximum health to lose
                        if (player.m_iHealth >= 0f)
                        {
                            ui.playerHealth = (int)player.m_iHealth;
                        }
                    }
                    else
                    {
                        // Stop animation and reset boss state
                        currentCoreTime = TimeSpan.Zero;
                        bossAnimationStarted = false;     
						laserShootLoop.Pause();                   
                    }
                }
            }
        }

        private void CheckBossCollisions(EnemyBase enemy, GameTime gameTime)
        {
            if (projectile.m_bActive)
            {
                Rectangle enemyRectangle;
                if (enemy.m_Type == EnemyBase.Type.Boss)
                {
                    enemyRectangle = new Rectangle((int)enemy.m_Position.X - enemy.Texture.Width / 2, (int)enemy.m_Position.Y - enemy.Texture.Height / 2, enemy.Texture.Width, enemy.Texture.Height);
                }
                else
                {
                    enemyRectangle = new Rectangle((int)enemy.m_Position.X, (int)enemy.m_Position.Y, enemy.Texture.Width * 7 / 10, enemy.Texture.Height * 6 / 10);
                }
                Rectangle projectileRectangle = new Rectangle((int)projectile.m_Position.X - projectile.m_ProjectileTexture.Width / 2, (int)projectile.m_Position.Y - projectile.m_ProjectileTexture.Height / 2, projectile.m_ProjectileTexture.Width, projectile.m_ProjectileTexture.Height);
                if (enemyRectangle.Intersects(projectileRectangle) && pixelCollision(enemy, projectile.m_ProjectileTexture, projectile.m_Position, Rectangle.Intersect(projectileRectangle, enemyRectangle)) && gameTime.TotalGameTime - previousBossCollision > bossCollisionTime)
                {
                    previousBossCollision = gameTime.TotalGameTime;
                    if (enemy.m_Type == EnemyBase.Type.Boss)
                    {
						ShowEnemyDeath (enemy);
						discHitEnemySnd.Play (1f, random.RandomFloat (-0.1f, 0.1f), 0f);
                        ((Boss)enemy).Health -= 10;
                        ((Boss)enemy).SetColor(Color.White);
                        activeParticleSystems.Remove(projectile.m_Trail);
                        projectile = new Projectile();
                        projectile.Initialize(projectileTexture, projectilePosition, Vector2.Zero);
                        projectile.InitializeTrail(new List<Texture2D>() { projectileTrailTexture });
                        if (((Boss)enemy).Health == 0)
                        {
                            //Add Boss points
                            ui.score += boss.m_PointValue;

                            for (int i = 0; i < enemiesList.Count; i++)
                            {
                                enemiesList[i].m_Active = false;
                            }
                            bossIsSpawned = false;
                        }
                    }
                    else
                    {
						// boss shield?
						ShowBounce (projectile.m_Position + projectile.m_ProjectileOrigin, enemy.m_Color);
						discHitShieldSnd.Play (1f, random.RandomFloat (-0.1f, 0.1f), 0f);

                        if (Math.Abs((previousProjectilePosition.X - projectile.m_Position.X)) > Math.Abs((previousProjectilePosition.Y - projectile.m_Position.Y)))
                        {
                            projectile.m_fProjectileSpeedX = -projectile.m_fProjectileSpeedX;
                        }
                        else
                        {
                            projectile.m_fProjectileSpeedY = -projectile.m_fProjectileSpeedY;
                        }
                    }
                }
                else
                {
                    if (enemy.m_Type == EnemyBase.Type.Boss)
                    {
                        if (((Boss)enemy).Health <= 90 && ((Boss)enemy).Health > 60)
                        {
                            ((Boss)enemy).SetColor(Color.Purple);
                        }
                        else if (((Boss)enemy).Health <= 60 && ((Boss)enemy).Health > 30)
                        {
                            ((Boss)enemy).SetColor(Color.YellowGreen);
                        }
                        else if (((Boss)enemy).Health <= 30)
                        {
                            ((Boss)enemy).SetColor(Color.Red);
                        }
                        else
                        {
                            ((Boss)enemy).SetColor(Color.Orange);
                        }
                    }
                }
            }
        }

        private void SpawnBoss(GameTime gameTime)
        {
            boss = new Boss(new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Width / 2, GraphicsDevice.Viewport.TitleSafeArea.Height / 2));
            boss.Initialize(bossTexture, new Vector2(GraphicsDevice.Viewport.Width / 2, 0));
            BossShield[] shields = new BossShield[8];
            for(int i=0;i<8; i++)
            {
                shields[i] = new BossShield(ref boss);
                shields[i].Initialize(enemyTextures[3],
                    Vector2.Zero, (float)(i * Math.PI / 4));
                enemiesList.Add(shields[i]);
            }
            //bossShield = new BossShield(ref boss);
            //bossShield.Initialize(bossShieldTexture, boss.m_Position, 0);
            ((Boss)boss).SetBossShield(ref shields);
            //enemiesList.Add(bossShield);
            enemiesList.Add(boss);
            bossIsSpawned = true;
            previousBossAnimationTime = gameTime.TotalGameTime;
			if (bossIdleLoop == null) {
				bossIdleLoop = bossIdleSnd.CreateInstance();
				bossIdleLoop.IsLooped = true;
				bossIdleLoop.Volume = 0.5f;
			}

			bossIdleLoop.Play();
        }

        private void UpdateEnemies(GameTime gameTime)
        {
            for (int i = 0; i < enemiesList.Count; i++)
            {
                EnemyBase enemy = enemiesList[i];
                if (enemy.m_Active)
                {
                    enemy.Update(gameTime);
                    if (CheckCollisionWithProjectile(enemy, gameTime))
                    {
                        ShowBounce(projectile.m_Position + projectile.m_ProjectileOrigin, enemy.m_Color);

                        if (enemy.GetType() != typeof(Shield))
                        {
                            if ((enemy.m_Type == EnemyBase.Type.SquigglyTriangle))
                            {

                                if (!projectile.selfRotate)
                                {
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
                                    //Update combo
                                    player.m_CurrentComboTime = TimeSpan.Zero;
                                    player.m_bComboActive = true;

                                    if(player.m_iComboMultiplier < 10)
                                    player.m_iComboMultiplier += 1;

                                    ((HexagonEnemy)enemy).shield1.m_Active = false;
                                    ((HexagonEnemy)enemy).shield2.m_Active = false;
                                }
                            }
                        }
                        // Bounce off the shield
                        else
                        {

                            discHitShieldSnd.Play(0.7f, random.RandomFloat(-0.1f, 0.1f), 0f);

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
                        enemy.m_Active = false;
                        if (enemy.m_Type != EnemyBase.Type.Shield)
                        {
                            player.m_iHealth -= 10;                            
                            playerHurtSnd.Play(1f, random.RandomFloat(-0.1f, 0.1f), 0f);
                            flashDamage = true;
                            //Cap the maximum health to lose
                            if (player.m_iHealth >= 0f)
                            {
                                ui.playerHealth = player.m_iHealth;
                            }

                            //Glitch the screen
                            ui.glitchScreen = true;
                            

                        }
                    }
                }
                else
                {
                    enemy.Kill();
                    lastKilledEnemy = enemiesList[i];
                    enemiesList.RemoveAt(i);
                    enemyDeath.Play();
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

        private bool CheckCollisionWithProjectile(EnemyBase enemy, GameTime gameTime)
        {
            bool collision = false;
            // Only check collision if projectile is released and is active
            if (!projectile.m_bInOrbitToPlayer && projectile.m_bActive)
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
                EnemyBase shield1 = null;
                EnemyBase shield2 = null;
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
                        shield1 = new Shield(ref enemy);
                        shield1.Initialize(enemyTextures[3], Vector2.Zero, 0);
                        shield2 = new Shield(ref enemy);
                        shield2.Initialize(enemyTextures[3], Vector2.Zero, (float)Math.PI);
                        ((HexagonEnemy)enemy).setShield1(ref shield1);
                        ((HexagonEnemy)enemy).setShield2(ref shield2);
                        break;
                    default:
                        break;
                }
                if (null != enemy)
                {
                    enemy.onDeath += new EnemyBase.EnemyEventHandler(ShowEnemyDeath);
                    enemiesList.Add(enemy);
                }
                if (null != shield1)
                {
                    enemiesList.Add(shield1);
                }
                if (null != shield2)
                {
                    enemiesList.Add(shield2);
                }
            }
        }


        private void SpawnPowerUps(GameTime gameTime)
        {

            if (gameTime.TotalGameTime - currentPowerUpTime > powerUpTimeSpan)
            {
                

                PowerUp powerUp = new HealthPowerUp();
                if (!bossIsSpawned) {
                    powerUp.Initialize(powerUpTextures[0], new Vector2(random.Next(powerUpTextures[0].Width, WIDTH - powerUpTextures[0].Width), random.Next(200, HEIGHT - 200)));
                    currentPowerUpTime = gameTime.TotalGameTime;
                    powerUpsList.Add(powerUp);
                    ShowPickupSpawn(powerUp.m_Position + powerUp.m_Origin);
                }
                else
                {
                    //The boss's radius
                    float bossRadius = boss.Texture.Width / 2 + 40;

                    Vector2 powerUpPos = new Vector2(random.Next(powerUpTextures[0].Width, WIDTH - powerUpTextures[0].Width), random.Next(200, HEIGHT - 200));

                    if(Vector2.Distance(powerUpPos, new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Width / 2, GraphicsDevice.Viewport.TitleSafeArea.Height / 2)) > bossRadius)
                    {
                        powerUp.Initialize(powerUpTextures[0], powerUpPos);
                        currentPowerUpTime = gameTime.TotalGameTime;
                        powerUpsList.Add(powerUp);
                        ShowPickupSpawn(powerUp.m_Position + powerUp.m_Origin);
                    }
                }
            }
        }


        private void UpdatePowerUps(GameTime gameTime)
        {

            for (int i = 0; i < powerUpsList.Count; i++)
            {

                powerUpsList[i].Update(gameTime);

                //if the projectile collides with the player
                if (CheckPowerUpWithPlayer(powerUpsList[i], gameTime))
                {

                    if (player.m_iHealth < 100)
                    {
                        player.m_iHealth += 10;

                        if (player.m_iHealth > 100)
                        {
                            player.m_iHealth = 100;
                        }

                    }
                    ui.playerHealth = (int)player.m_iHealth;
                    powerUpsList[i].m_Active = false;
					pickupHealthSnd.Play(1f, random.RandomFloat(-0.1f, 0.1f), 0f);
					ShowPickupHealth (powerUpsList[i].m_Position + powerUpsList[i].m_Origin);
                }

                if (!powerUpsList[i].m_Active)
                {
                    powerUpsList.RemoveAt(i);
                }

            }
        }

        private bool CheckPowerUpWithPlayer(PowerUp powerUp, GameTime gameTime)
        {
            bool collision = false;
            // Only check collision if projectile is released
            if (!projectile.m_bInOrbitToPlayer && projectile.m_bActive)
            {
                Rectangle powerUpRectangle;
                powerUpRectangle = new Rectangle((int)powerUp.m_Position.X, (int)powerUp.m_Position.Y, powerUp.Texture.Width * 7 / 10, powerUp.Texture.Height * 6 / 10);
                Rectangle projectileRectangle = new Rectangle((int)projectile.m_Position.X - projectile.m_ProjectileTexture.Width / 2, (int)projectile.m_Position.Y - projectile.m_ProjectileTexture.Height / 2, projectile.m_ProjectileTexture.Width, projectile.m_ProjectileTexture.Height);
                if (powerUpRectangle.Intersects(projectileRectangle))
                {
                    collision = true;
                }

                else
                    collision = false;

            }
            return collision;
        }



        private void UpdatePlayer(GameTime gameTime)
        {
            if (!gamePaused)
            {
                player.Update(gameTime);

                //Check the case where the space bar is pressed
                if (previousKeyboardState.IsKeyUp(Keys.Space) && currentKeyboardState.IsKeyDown(Keys.Space))
                {
                    //Launch the projectile
                    projectile.m_bInOrbitToPlayer = false;
                    throwDiscSnd.Play(1f, random.RandomFloat(-0.1f, 0.1f), 0f);
                }
            }

            //Check if the player pressed Yor N and the game over context is on
            if (gameOver)
            {
                if (currentKeyboardState.IsKeyDown(Keys.N))
                {
                    Exit();
                }
                else if (currentKeyboardState.IsKeyDown(Keys.Y))
                {

                    ResetGame(gameTime);
                    gamePaused = false;

                }



            }
        }

        protected void UpdateProjectile(GameTime gameTime)
        {
            if (projectile.m_bActive)
            {
                previousProjectilePosition = projectile.m_Position;
                if (CheckCornerCollision()) {
                    projectile.m_fProjectileSpeedY = -projectile.m_fProjectileSpeedY;
                    projectile.m_fProjectileSpeedX = -projectile.m_fProjectileSpeedX;
                    wallBoundSnd.Play(0.8f, random.RandomFloat(-0.1f, 0.1f), 0f);
                    edge = edge_hit;
                    ShowBounce(projectile.m_Position, projectile.selfRotate ? Color.Red : Color.White);
                }
                else if (projectile.m_Position.X <= 10f || projectile.m_Position.X >= GraphicsDevice.Viewport.TitleSafeArea.Width - 10f)
                {
                    wallBoundSnd.Play(0.8f, random.RandomFloat(-0.1f, 0.1f), 0f);
                    projectile.m_fProjectileSpeedX = -projectile.m_fProjectileSpeedX;
                    edge = edge_hit;
                    ShowBounce(projectile.m_Position + projectile.m_ProjectileOrigin, projectile.selfRotate ? Color.Red : Color.White);
                }
                else if (projectile.m_Position.Y <= 10f || projectile.m_Position.Y >= GraphicsDevice.Viewport.TitleSafeArea.Height - 10f)
                {
                    wallBoundSnd.Play(0.8f, random.RandomFloat(-0.1f, 0.1f), 0f);
                    projectile.m_fProjectileSpeedY = -projectile.m_fProjectileSpeedY;
                    edge = edge_hit;
                    ShowBounce(projectile.m_Position + projectile.m_ProjectileOrigin, projectile.selfRotate ? Color.Red : Color.White);
                }
                else
                {
                    edge = edge_normal;
                }

            //Check is projectile has been launched, rotate it around its center


                if (currentKeyboardState.IsKeyDown(Keys.Space))
                {


                    if (ui.m_staminaAmount > 0f)
                    {
                        ui.m_rechargingStamina = false;
                        projectile.selfRotate = true;
                        ui.m_staminaAmount -= 1f;

                        projectile.m_ProjectileTexture = spinningProjectileTexture;
                        projectile.m_Trail.SetAllTint(Color.Red);
                        if (spinLoopInstance == null)
                        {
                            spinLoopInstance = spinLoopSnd.CreateInstance();
                            spinLoopInstance.IsLooped = true;
                        }
                        spinLoopInstance.Play();

                    }
                    else
                    {

                        projectile.selfRotate = false;
                    }

                }


                else
                {
                    ui.m_rechargingStamina = true;
                    projectile.selfRotate = false;
                    projectile.m_ProjectileTexture = projectileTexture;
                    projectile.m_Trail.SetAllTint(Color.White);
                    if (spinLoopInstance != null) spinLoopInstance.Pause();
                }

                if (currentKeyboardState.IsKeyDown(Keys.R) && !projectile.m_bInOrbitToPlayer)
                {
                    // Make projectile inactive to negate collisions
                    projectile.m_bActive = false;
                    recallDiscSnd.Play(1f, random.RandomFloat(-0.1f, 0.1f), 0f);
                    projectile.m_ProjectileColor = Color.Gray;
                    projectile.m_Trail.SetAllTint(Color.Gray);
                }
                projectile.Update(player.m_Position, gameTime);
            }// Update lerp position
            else if (projectile.m_Position != player.m_Position)
            {
                projectile.ReturnProjectile(player.m_Position, gameTime);
                projectile.m_ProjectileColor = Color.Gray;
            }// Create new projectile
            else
            {
                activeParticleSystems.Remove(projectile.m_Trail);
                projectile = new Projectile();
                projectile.Initialize(projectileTexture, projectilePosition, Vector2.Zero);
                projectile.InitializeTrail(new List<Texture2D>() { projectileTrailTexture });
                projectile.m_Trail.SetAllTint(Color.White);
            }
        }

        private bool CheckCornerCollision()
        {
            return projectile.m_Position == new Vector2(0, 0) || projectile.m_Position == new Vector2(0, GraphicsDevice.Viewport.Height) || projectile.m_Position == new Vector2(GraphicsDevice.Viewport.Width, 0) || projectile.m_Position == new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
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
                    if (player.m_iComboMultiplier < 10)
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
                        if (player.m_iComboMultiplier < 10)
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

                        discHitEnemySnd.Play(1f, random.RandomFloat(-0.1f, 0.1f), 0f);
                        ShowBounce(projectile.m_Position + projectile.m_ProjectileOrigin, enemy.m_Color);
                    }

                }



            }//end of check collision time
        }

        void UpdateDamageFlash(GameTime gameTime)
        {

            currentDamagFlashTime += gameTime.ElapsedGameTime;

            if (currentDamagFlashTime >= damageFlashTime)
            {

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

            //Draw the power ups
            DrawPowerUps(spriteBatch);

            //Draw enemies
            DrawEnemies(spriteBatch);
            // Draw boss shield
            if (bossIsSpawned)
            {
                //bossShield.Draw(spriteBatch);
                if (bossCore != null)
                {
                    bossCore.Draw(spriteBatch);
                }
                if (lazer != null)
                {
                    lazer.Draw(spriteBatch);
                }
            }
            DrawParticleSystems(spriteBatch);

            if (player.m_bComboActive && player.m_iComboMultiplier > 1)
            {
                //Draw the combo indicator
                spriteBatch.DrawString(ui.comboFont, "Combo: x" + player.m_iComboMultiplier.ToString(), new Vector2(250, 490), Color.White);

                //Draw the multiplier 
                spriteBatch.DrawString(ui.scoreFont, "x" + player.m_iComboMultiplier.ToString(), lastKilledEnemy.m_Position, Color.White);
            }

            //Draw ui
            ui.Draw(spriteBatch);

            //Draw the score
            spriteBatch.DrawString(ui.scoreFont, ui.score.ToString(), new Vector2(78, 40), Color.White);




            //Draw the player health
            spriteBatch.DrawString(ui.healthFont, ui.playerHealth.ToString() + "%", new Vector2(680, 35), Color.White);

            //Draw the damage rectangle
            Rectangle damageRectangle = new Rectangle(0, 0, playerDamageTexture.Width, playerDamageTexture.Height);
            Vector2 origin = new Vector2(playerDamageTexture.Width / 2, playerDamageTexture.Height / 2);
            Vector2 position = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height - 200);

            //Draw the flash damage
            if (flashDamage)
            {
                spriteBatch.Draw(playerDamageTexture, position, damageRectangle, Color.Red, 0f, origin, 1f, SpriteEffects.None, 0f);
            }

            //Draw the game over screen
            if (player.finishedAnimation)
            {

                //Draw the combo indicator
                spriteBatch.DrawString(ui.gameOverFont, "Replay Y/N?", new Vector2(GraphicsDevice.Viewport.Width / 2 - 200, GraphicsDevice.Viewport.Height / 2 - 20), Color.White);
                player.m_bComboActive = false;

                //playerDeath.Play();

            }

            //Stop drawing
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void ResetGame(GameTime gameTime)
        {
            player.Reset();
            activeParticleSystems.Remove(projectile.m_Trail);
            projectile = new Projectile();
            projectile.Initialize(projectileTexture, projectilePosition, Vector2.Zero);
            projectile.InitializeTrail(new List<Texture2D>() { projectileTrailTexture });
            enemiesList.Clear();
            ui.playerHealth = 100;
            gameOver = false;
            gamePaused = false;
            ui.score = 0;
            powerUpsList.Clear();
            bossIsSpawned = false;
            lazer = null;
            bossCore = null;
            lastBossTime = gameTime.TotalGameTime;
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
            enemyDeathPS.m_Position = enemy.m_Position + 
				(enemy.m_Type == EnemyBase.Type.Boss ? Vector2.Zero : enemy.m_Center);
            enemyDeathPS.SetTint(enemy.m_Color);
            enemyDeathPS.Emit(8);

            enemyDeathSnd.Play(0.8f, random.RandomFloat(-0.1f, 0.1f), 0f);
        }

        void ShowBounce(Vector2 pos, Color color, float windX = 0f, float windY = 0f)
        {
            if (bouncePS.m_Sprites == null)
                bouncePS.m_Sprites = new List<Texture2D>() { enemyPiecesList[4] };

            bouncePS.SetWind(new Vector2(windX, windY));
            bouncePS.m_Position = pos;
            bouncePS.SetTint(color);
            bouncePS.Emit();
        }

		void ShowPickupSpawn (Vector2 pos) {
			if (pickupSpawnPS.m_Sprites == null)
				pickupSpawnPS.m_Sprites = new List<Texture2D>() { enemyPiecesList[4] };

			pickupSpawnPS.m_Position = pos;
			pickupSpawnPS.Emit();
		}

		void ShowPickupHealth (Vector2 pos) {
			if (pickupHealthPS.m_Sprites == null)
				pickupHealthPS.m_Sprites = new List<Texture2D>() { healthPickupEffect, enemyPiecesList[4] };

			pickupHealthPS.m_Position = pos;
			pickupHealthPS.Emit();
		}

        private void DrawPowerUps(SpriteBatch spriteBatch)
        {
            foreach (PowerUp powerUp in powerUpsList)
            {
                if (!powerUp.m_Active) continue;
                powerUp.Draw(spriteBatch);
            }
        }

    }

}
