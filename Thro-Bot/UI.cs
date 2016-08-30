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

        //Temporary values for debugging you can erase them latter on
        public SpriteFont tempHealth;

        public int playerHealth;


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
        public void InitializeHealth(Texture2D texture, Vector2 position, Vector2 origin)
        {
            //Set the health texuture
            m_HealthBarFrameTexture = texture;

            //Set the starting position of the projectile around the middle of the screen and towards the bottom
            m_HealthBarFramePosition = position;

            //Set the origin of the projectile
            m_HealthBarFrameOrigin = new Vector2(texture.Width / 2, texture.Height / 2);

            //Temporary values
            playerHealth = 100;

        }


        public void Draw(SpriteBatch spriteBatch)
        {

            //Draw the Score UI element
            Rectangle sourceRectangleScore = new Rectangle(0,0,m_Scoretexture.Width,m_Scoretexture.Height);
            spriteBatch.Draw(m_Scoretexture, m_ScorePosition, sourceRectangleScore, Color.White, 0f, m_ScoreOrigin, .75f, SpriteEffects.None, 0f);

            //Draw the Health Frame UI element
            Rectangle sourceRectangleHealthBarFrame = new Rectangle(0, 0, m_HealthBarFrameTexture.Width, m_HealthBarFrameTexture.Height);
            spriteBatch.Draw(m_HealthBarFrameTexture,m_HealthBarFramePosition, sourceRectangleHealthBarFrame, Color.White, 0f, m_HealthBarFrameOrigin, .75f, SpriteEffects.None, 0f);

        }



    }
}
