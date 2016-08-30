using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Thro_Bot
{
    class UI
    {

        //The UI texture
        public Texture2D m_Scoretexture;

        //The Position of the UI texture
        public Vector2 m_ScorePosition;

        //The origin of the score Texture
        private Vector2 m_ScoreOrigin;

        //The sprite font to draw the score
        public SpriteFont scoreFont;

        //The value of the score
        public int score;


        //Temporary values for debugging you can erase them latter on
        public SpriteFont tempHealth;

        public int playerHealth;


        //Initializes the UI
        public void Initialize(Texture2D texture, Vector2 position, Vector2 origin)
        {
            m_Scoretexture = texture;

            //Set the starting position of the projectile around the middle of the screen and towards the bottom
            m_ScorePosition = position;

            //Set the origin of the projectile
            m_ScoreOrigin = new Vector2(texture.Width / 2, texture.Height / 2);

            //Set the value of the score at 0.
            score = 0;

            //Temporary values
            playerHealth = 100;

        }


        public void Draw(SpriteBatch spriteBatch)
        {

            Rectangle sourceRectangle = new Rectangle(0,0,m_Scoretexture.Width,m_Scoretexture.Height);
            spriteBatch.Draw(m_Scoretexture, m_ScorePosition, sourceRectangle, Color.White, 0f, m_ScoreOrigin, .75f, SpriteEffects.None, 0f);
        }



    }
}
