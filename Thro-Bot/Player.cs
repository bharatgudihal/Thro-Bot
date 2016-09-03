using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Thro_Bot
{
    class Player
    {

        //Animation representing the player
        public Texture2D m_PlayerTexture;

        //Position of the PLayer relative to the upper left side of the screen
        public Vector2 m_Position;

        //state of the Player
        public bool m_bAlive;

        //Amount of hit points that player has
        public float m_iHealth;

        //The rotation angle of the player
        public float m_fRotation;
        private float rotationSpeed;

        //The origin of the player
        public Vector2 m_PlayerOrigin;

        //The combo multiplier
        public int m_iComboMultiplier;

        //If a combo is active
        public bool m_bComboActive;

        //The time the combo started
        public TimeSpan m_CurrentComboTime = TimeSpan.Zero;

        //The amount of time till the combo resets
        public TimeSpan m_ComboCoolDown = TimeSpan.FromSeconds(3);


        //Get the width of the player ship sprite
        public int m_iSpriteWidth
        {
           get { return m_PlayerTexture.Width; }
        }

        public int m_iSpriteHeight
        {
            get { return m_PlayerTexture.Height; }
        }




        public void Initialize(Texture2D texture,Vector2 position)
        {
            m_PlayerTexture = texture;
            
            //Set the starting position of the player around the middle of the screen and towards the bottom
            m_Position = position;

            //Set the player to be active
            m_bAlive = true;

            //Set the player health
            m_iHealth = 100;

            //Set the angle to 0
            m_fRotation = 0f;

            //Set the Player origin
            //m_PlayerOrigin = Vector2.Zero;
            m_PlayerOrigin = new Vector2(m_PlayerTexture.Width * 0.5f, m_PlayerTexture.Height * 0.5f);

            // Set rotation speed
            rotationSpeed = 0.05f;

            //Set the combo multiplier
            m_iComboMultiplier = 0;

            //Set the state of the combo
            m_bComboActive = false;

            
        }

        public void Update()
        {
            m_fRotation -= rotationSpeed;                       
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //The Rectangle to render the texture
            Rectangle sourceRectangle = new Rectangle(0, 0, m_PlayerTexture.Width, m_PlayerTexture.Height);
            
            spriteBatch.Draw(m_PlayerTexture,m_Position,sourceRectangle,Color.White,m_fRotation, m_PlayerOrigin, .1f,SpriteEffects.None,0f);

        }

        public void Reset() {
            m_iHealth = 100;

        }

    }
}
