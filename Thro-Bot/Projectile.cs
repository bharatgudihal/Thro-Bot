using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Thro_Bot
{
    class Projectile
    {
        //The player the projectile is parented to



        //The texture that represents the projectile
        public Texture2D m_ProjectileTexture;

        //Position of the projectile relative to the upper left side of the screen
        public Vector2 m_Position;
        
        //state of the projectile
        public bool m_bActive;

        //The rotation of the projectile
        public float m_fProjectileRotation;
        public float m_fProjectileRotation_fixed;
        private float rotationSpeed;

        public bool selfRotate;

        public float m_fProjectileSpeedX;
        public float m_fProjectileSpeedY;

        public bool m_bInOrbitToPlayer;

        //The amount of times the projectile has hit the screen sides to return to the player
        public int m_iBounces;

        public Vector2 selfOrigin;

        //The origin of the projectile
        public Vector2 m_ProjectileOrigin;

        //Get the width of the player ship sprite
        public int m_iSpriteWidth
        {
            get { return m_ProjectileTexture.Width; }
        }

        public int m_iSpriteHeight
        {
            get { return m_ProjectileTexture.Height; }
        }


        public void Initialize(Texture2D texture, Vector2 position,Vector2 origin,Vector2 dummyOrigin)
        {
            m_ProjectileTexture = texture;

            //Set the starting position of the projectile around the middle of the screen and towards the bottom
            m_Position = position;

            //Set the origin of the projectile
            m_ProjectileOrigin = origin;

            //Set the projectile's rotation
            m_fProjectileRotation = 0f;
            
            //Set the player to be active
            m_bActive = true;

            //Start with the projectile in orbit with the player
            m_bInOrbitToPlayer = true;

            //Set the angle to 0
            m_fProjectileRotation = 0f;
            m_fProjectileRotation_fixed = 0f;

            m_fProjectileSpeedX = 10f;
            m_fProjectileSpeedY = 10f;

            rotationSpeed = 0.05f;

            //Set the number of bounces to 4
            m_iBounces = 4;

            selfRotate = false;

            selfOrigin.X = m_ProjectileTexture.Width / 2;
            selfOrigin.Y = m_ProjectileTexture.Height / 2;

        }

        public void Update()
        {
            if (m_bInOrbitToPlayer)
            {
                m_fProjectileRotation -= rotationSpeed;
                m_fProjectileRotation_fixed = m_fProjectileRotation;
            }
            else {                
                m_Position.X += (float)(m_fProjectileSpeedX * Math.Cos((double)m_fProjectileRotation_fixed));
                m_Position.Y += (float)(m_fProjectileSpeedY * Math.Sin((double)m_fProjectileRotation_fixed));
                if (selfRotate)
                {
                    m_fProjectileRotation -= rotationSpeed;
                }
                else
                {
                    m_ProjectileOrigin = Vector2.Zero;                   
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //The Rectangle to render the texture
            Rectangle sourceRectangle = new Rectangle(0, 0, m_ProjectileTexture.Width, m_ProjectileTexture.Height);
            spriteBatch.Draw(m_ProjectileTexture, m_Position, sourceRectangle,  Color.White, m_fProjectileRotation, m_ProjectileOrigin, .5f, SpriteEffects.None, 0f);

        }
    }
}
