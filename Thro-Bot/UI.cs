using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Thro_Bot
{
    class UI
    {

        //The UI texture for score
        public Texture2D m_Scoretexture;

        //The Position of the UI texture
        public Vector2 m_ScorePosition;

        //The origin of the score Texture
        private Vector2 m_ScoreOrigin;

        //The sprite font to draw the score
        public SpriteFont scoreFont;

        //The value of the score
        public int score;

        //The UI texture for the health bar
        public Texture2D m_HealthBarFrameTexture;

        //The position of the health bar
        public Vector2 m_HealthBarFramePosition;

        //The origin of the health Bar
        private Vector2 m_HealthBarFrameOrigin;

        //Font for the health bar
        public SpriteFont healthFont;

        //The texture of the health bar
        private Texture2D m_HealthBarTexture;

        //The position of the health bar
        private Vector2 m_PositionHealthBar;

        //The origin of the health bar
        private Vector2 m_OriginHealthBar;

        private Rectangle sourceRectangleHealthBar;

        //The initial width of the bar
        private int intitialFullWidth;

        public float playerHealth;

        //Font for the gameover
        public SpriteFont gameOverFont;

        //The stamina bar frame texture
        public Texture2D m_staminaBarFrame;

        //The position of the stamina bar frame
        public Vector2 m_staminaFramePosition;

        //The origin of the stamina frame
        public Vector2 m_staminaFrameOrigin;

        //The stamina bar texture
        public Texture2D m_staminaBar;

        //The position of the stamina bar
        public Vector2 m_staminaBarPosition;

        //The origin of the stamina bar
        public Vector2 m_staminaBarOrigin;

        //The rectangle for the stamina bar
        private Rectangle sourceRectangleStaminaBar;

        //The initial width of the stamina  bar
        private int initialWidthStaminaBar;

        //The amount of stamina the projectile has
        public float m_staminaAmount = 100;

        //The state of the stamina bar
        public bool m_rechargingStamina = false;

        //The Combo font
        //Font for the gameover
        public SpriteFont comboFont;


        public bool glitchScreen = false;

        //The glitch Backround texture
        public Texture2D m_GlitchTexture;

        //The position of the glitch texture
        public Vector2 m_GlitchPos;

        //The origin of the glitch image
        public Vector2 m_glitchOrigin;

        //The rectangle of the glitch screen
        public Rectangle sourceRectangleGlitch;

        //The alpha amount of the glitch
        private float m_glitchAlpha = 0.4f;

        //The min alpha amount of the glitch
        private float m_minGlitchAlpha = 0.0f;

        //The max alpha amount of the glitch
        private float m_maxGlitchAlpha = 0.4f;

        //The theta of the angle
        private float _theta = 0f;

        //The period of the signal
        private float _period = 0.05f;

        //The time span of the glitch
        private TimeSpan m_glitchDuration = TimeSpan.FromSeconds(0.8);

        //The current time
        private TimeSpan m_currentTime = TimeSpan.Zero;

        //Initializes the Score UI
        public void InitializeScore(Texture2D texture, Vector2 position, Vector2 origin)
        {
            //Set the score texuture
            m_Scoretexture = texture;

            //Set the starting position of the projectile around the middle of the screen and towards the bottom
            m_ScorePosition = position;

            //Set the origin of the projectile
            m_ScoreOrigin = new Vector2(texture.Width / 2, texture.Height / 2);

            //Set the value of the score at 0.
            score = 0;

        }

        //Initializes the Score UI
        public void InitializeHealth(Texture2D textureFrame, Vector2 position, Texture2D textureBar, Vector2 positionBar)
        {
            //Set the health texuture
            m_HealthBarFrameTexture = textureFrame;

            //Set the starting position of the projectile around the middle of the screen and towards the bottom
            m_HealthBarFramePosition = position;

            //Set the origin of the projectile
            m_HealthBarFrameOrigin = new Vector2(textureFrame.Width / 2, textureFrame.Height / 2);

            //Set the health bar
            m_HealthBarTexture = textureBar;

            //Set the position of the bar
            m_PositionHealthBar = positionBar;

            //Set the origin of the bar
            m_OriginHealthBar = new Vector2(textureFrame.Width/2, textureBar.Height / 2);


            //Temporary values
            playerHealth = 100f;

            //Set the rectangle
            sourceRectangleHealthBar = new Rectangle(0, 0, 365, m_HealthBarTexture.Height - 10);

            //Get the full width of the bar
            intitialFullWidth = sourceRectangleHealthBar.Width;

        }


        public void InitializeStamina(Texture2D textureFrame, Vector2 positionFrame, Texture2D textureBar, Vector2 positionBar)
        {
            //Set the stamina frame texture
            m_staminaBarFrame = textureFrame;

            //Set the position of the frame
            m_staminaFramePosition = positionFrame;

            //Set the origin of the stamina frame
            m_staminaFrameOrigin = new Vector2(m_staminaBarFrame.Width / 2, m_staminaBarFrame.Height / 2);

            //Set the texture of the stamina
            m_staminaBar = textureBar;

            //Set the position of the stamina bar
            m_staminaBarPosition = positionBar;

            //Set the origin of the stamina bar
            m_staminaBarOrigin = new Vector2(m_staminaBar.Width / 2, m_staminaBar.Height / 2);

            //Set the rectangle of the stamnina bar
            sourceRectangleStaminaBar = new Rectangle(0, 0, m_staminaBar.Width, m_staminaBar.Height);

            //Set the initial width of the stamina bar
            initialWidthStaminaBar = sourceRectangleStaminaBar.Width;

        }

        public void InitializeGlitchScreen(Texture2D glitchScreen, Vector2 position)
        {
            //Set the texture of the glitch screen
            m_GlitchTexture = glitchScreen;

            //Set the position of the glitch screen
            m_GlitchPos = position;

            //Set the origin of the glitch texture
            m_glitchOrigin = new Vector2(m_GlitchTexture.Width/2,m_GlitchTexture.Height/2);

            //Set the rectangle
            sourceRectangleGlitch = new Rectangle(0, 0, m_GlitchTexture.Width, m_GlitchTexture.Height);
        }

        //Ui updated the health bar
        public void Update(GameTime gameTime) {

            
            sourceRectangleHealthBar.Width = (int)Math.Round((playerHealth / 100f) * intitialFullWidth);


            if (m_rechargingStamina)
            {
                //Maybe add lerp
                if (m_staminaAmount < 100)
                {
                    m_staminaAmount += 0.25f;
                }
                else {
                    m_rechargingStamina = false;
                }
            }
           


            sourceRectangleStaminaBar.Width = (int)Math.Round((m_staminaAmount / 100f) * initialWidthStaminaBar);


            //Flash the glitch screen
            if(glitchScreen)
            flashGlitch(gameTime);

        }

        public void flashGlitch(GameTime gameTime)
        {

            m_currentTime += gameTime.ElapsedGameTime;



            // dtheta = 1 / framerate / period
            float dTheta = 1f / 60f / _period;

            // Increment theta, constraint to 0-2PI
            _theta = (_theta + dTheta) % (2f * (float)Math.PI);


            float midpt = (m_maxGlitchAlpha - m_minGlitchAlpha) / 2f;


            m_glitchAlpha = m_minGlitchAlpha + midpt + (m_maxGlitchAlpha - m_minGlitchAlpha) * (float)Math.Sin(_theta);

            if (m_currentTime > m_glitchDuration)
            {
                glitchScreen = false;
                m_currentTime = TimeSpan.Zero;
            }

        }



        public void Draw(SpriteBatch spriteBatch)
        {

            if (glitchScreen)
            {
                //Draw the glitch texture
                spriteBatch.Draw(m_GlitchTexture, m_GlitchPos, sourceRectangleGlitch, Color.White * m_glitchAlpha, 0f, m_glitchOrigin, 1.4f, SpriteEffects.None, 0f);
            }

            float angle = (float)Math.PI;  // 180 degrees
            //Check the value of the player health
            if (playerHealth >= 50)
            {
              
                //Draw the Health Bar UI element
                spriteBatch.Draw(m_HealthBarTexture, m_PositionHealthBar, sourceRectangleHealthBar, Color.CornflowerBlue, angle, m_OriginHealthBar, 0.7f, SpriteEffects.None, 0f);
            }
            else if (playerHealth >= 20)
            {

                //Draw the Health Bar UI element
                spriteBatch.Draw(m_HealthBarTexture, m_PositionHealthBar, sourceRectangleHealthBar, Color.Tan, angle, m_OriginHealthBar, 0.7f, SpriteEffects.None, 0f);
            }
            else {
                //Draw the Health Bar UI element
                spriteBatch.Draw(m_HealthBarTexture, m_PositionHealthBar, sourceRectangleHealthBar, Color.Red, angle, m_OriginHealthBar, 0.7f, SpriteEffects.None, 0f);
            }

            //Draw the Score UI element
            Rectangle sourceRectangleScore = new Rectangle(0,0,m_Scoretexture.Width,m_Scoretexture.Height);
            spriteBatch.Draw(m_Scoretexture, m_ScorePosition, sourceRectangleScore, Color.White, 0f, m_ScoreOrigin, .45f, SpriteEffects.None, 0f);

            //Draw the Health Frame UI element
            Rectangle sourceRectangleHealthBarFrame = new Rectangle(0, 0, m_HealthBarFrameTexture.Width, m_HealthBarFrameTexture.Height);
            spriteBatch.Draw(m_HealthBarFrameTexture,m_HealthBarFramePosition, sourceRectangleHealthBarFrame, Color.White, 0f, m_HealthBarFrameOrigin, .60f, SpriteEffects.None, 0f);

            //Draw the stamina bar
            spriteBatch.Draw(m_staminaBar, m_staminaBarPosition, sourceRectangleStaminaBar, Color.OrangeRed,angle, m_staminaBarOrigin, 0.27f, SpriteEffects.None, 0f);


            //Draw the stamina Frame UI element
            Rectangle sourceRectangleStaminaBarFrame = new Rectangle(0, 0, m_staminaBarFrame.Width, m_staminaBarFrame.Height);
            spriteBatch.Draw(m_staminaBarFrame, m_staminaFramePosition, sourceRectangleStaminaBarFrame,Color.White, 0f, m_staminaFrameOrigin, 0.27f, SpriteEffects.None, 0f);

        }



    }
}
