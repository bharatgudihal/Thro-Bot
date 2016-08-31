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


        //Ui updated the health bar
        public void Update() {

            sourceRectangleHealthBar.Width = (int)Math.Round((playerHealth / 100f) * intitialFullWidth);
            
        }



        public void Draw(SpriteBatch spriteBatch)
        {
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
                spriteBatch.Draw(m_HealthBarTexture, m_PositionHealthBar, sourceRectangleHealthBar, Color.DarkRed, angle, m_OriginHealthBar, 0.7f, SpriteEffects.None, 0f);
            }

            //Draw the Score UI element
            Rectangle sourceRectangleScore = new Rectangle(0,0,m_Scoretexture.Width,m_Scoretexture.Height);
            spriteBatch.Draw(m_Scoretexture, m_ScorePosition, sourceRectangleScore, Color.White, 0f, m_ScoreOrigin, .45f, SpriteEffects.None, 0f);

            //Draw the Health Frame UI element
            Rectangle sourceRectangleHealthBarFrame = new Rectangle(0, 0, m_HealthBarFrameTexture.Width, m_HealthBarFrameTexture.Height);
            spriteBatch.Draw(m_HealthBarFrameTexture,m_HealthBarFramePosition, sourceRectangleHealthBarFrame, Color.White, 0f, m_HealthBarFrameOrigin, .60f, SpriteEffects.None, 0f);

           

        }



    }
}
