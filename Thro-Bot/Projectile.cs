using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Thro_Bot
{
    class Projectile
    {
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

        //The scale of the projectile 
        public float m_fProjectileScale;

        //The max scale of the projectile
        private const float MAX_PROJECTILE_SCALE = 1.65f;

        //The min scale of the projectile
        private const float MIN_PROJECTILE_SCALE = 1.15f;

        //The direction of scaling the projectile
        private bool m_bScaleUp;

        //The elapsed time
        private TimeSpan elapsedTime = TimeSpan.Zero;

        //The time until max scale
        private TimeSpan scaleTime = TimeSpan.FromSeconds(0.1);

        //The duration of time
        private const int duration = 5000;

        private Color m_projectileColor = Color.White;

        //The origin of the projectile
        public Vector2 m_ProjectileOrigin;

        // Projectile rotation radius
        public float rotationRadius = 55f;

        //Get the width of the player ship sprite
        public int m_iSpriteWidth
        {
            get { return m_ProjectileTexture.Width; }
        }

        public int m_iSpriteHeight
        {
            get { return m_ProjectileTexture.Height; }
        }

        // Projectile Rectangle
        public Rectangle sourceRectangle;

        public void Initialize(Texture2D texture, Vector2 position,Vector2 origin)
        {
            m_ProjectileTexture = texture;

            //Set the starting position of the projectile around the middle of the screen and towards the bottom
            m_Position = position;

            //Set the origin of the projectile
            m_ProjectileOrigin = new Vector2(texture.Width/2,texture.Height/2);

            //Set the projectile's rotation
            m_fProjectileRotation = 0f;
            
            //Set the player to be active
            m_bActive = true;

            //Start with the projectile in orbit with the player
            m_bInOrbitToPlayer = true;

            //Set the angle to 0
            m_fProjectileRotation = 0f;
            m_fProjectileRotation_fixed = 0f;

            m_fProjectileSpeedX = 15f;
            m_fProjectileSpeedY = 15f;

            rotationSpeed = 0.05f;

            //Set the number of bounces to 4
            m_iBounces = 0;

            selfRotate = false;

            //Set the initial scale of the projectile
            m_fProjectileScale = 1f;

            //Set the value to scale up every time
            m_bScaleUp = true;

            //Set the color of the disc to white
            m_projectileColor = Color.White;


            //The Rectangle to render the texture
            sourceRectangle = new Rectangle(0, 0, m_ProjectileTexture.Width, m_ProjectileTexture.Height);

        }

        public void Update(Vector2 origin, GameTime gameTime)
        {
            if (m_bInOrbitToPlayer)
            {
                m_fProjectileRotation -= rotationSpeed*2f;
                //The equation of a circle
                m_Position.X = origin.X + (float)(rotationRadius * Math.Cos((double)m_fProjectileRotation / 2));
                m_Position.Y = origin.Y + (float)(rotationRadius * Math.Sin((double)m_fProjectileRotation / 2));
                m_fProjectileRotation_fixed = m_fProjectileRotation / 2;
            }else {
                m_Position.X += (float)(m_fProjectileSpeedX * Math.Sin(m_fProjectileRotation_fixed));
                m_Position.Y -= (float)(m_fProjectileSpeedY * Math.Cos(m_fProjectileRotation_fixed));


                //Spin the projectile
                if (selfRotate)
                {
                    m_fProjectileRotation -= rotationSpeed * 10f;
                    AlterProjectileScale(gameTime);
                    m_projectileColor = Color.Red;
                    
                }

                //Return the projectile to its original scale
                else {
                    m_fProjectileScale = 1f;
                    m_projectileColor = Color.White;
                }
               
            }
        }


        private void AlterProjectileScale(GameTime gameTime) {

            if (m_bScaleUp)
            {
                m_fProjectileScale = ScaleProjectile(m_fProjectileScale, MAX_PROJECTILE_SCALE, gameTime);

                if (m_fProjectileScale == MAX_PROJECTILE_SCALE)
                {
                    m_bScaleUp = false;
                    elapsedTime = TimeSpan.Zero;
                }

            }
            else {
                m_fProjectileScale = ScaleProjectile(m_fProjectileScale, MIN_PROJECTILE_SCALE, gameTime);

                if (m_fProjectileScale == MIN_PROJECTILE_SCALE)
                {
                    m_bScaleUp = true;
                    elapsedTime = TimeSpan.Zero;
                }
            }
        }


        private float ScaleProjectile(float initialScale, float targetScale, GameTime gameTime) {

            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime >= scaleTime)
                elapsedTime = scaleTime;

            float amount = (float)elapsedTime.Ticks / scaleTime.Ticks;
            initialScale = MathHelper.Lerp(initialScale, targetScale, amount);


            return initialScale;
        }

        
        public void Draw(SpriteBatch spriteBatch)
        {                        
            spriteBatch.Draw(m_ProjectileTexture, m_Position, sourceRectangle, m_projectileColor, m_fProjectileRotation, m_ProjectileOrigin, m_fProjectileScale, SpriteEffects.None, 0f);
        }
    }
}
