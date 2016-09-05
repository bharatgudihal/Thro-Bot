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


        //The image representing the player's death
        Texture2D deathSpriteStrip;

        //The time since the last update
        int elapsedTime;

        //The time we display a frame until the next one
        int frameTime;

        //The number of frames that the animation contains
        int frameCount;

        //The index of the current frame we are displaying
        int currentFrame;

        //The area of the rectangle to display the death animation
        Rectangle sourceRectDeath = new Rectangle();

        //The width of a given frame
        int FrameWidth;

        //The height of a given frame
        int FrameHeight;

        //Determines if the animation is looping
        public bool Looping;

        //Activate the death animation
        public bool deathAnimationActivate;

        public bool finishedAnimation = false;

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

        public void Update(GameTime gametime)
        {


            if (m_iHealth <= 0)
            {
                m_fRotation = 0;
                deathAnimationActivate = true;
                PlayDeathAnimation(gametime);

            }
            else {
                m_fRotation -= rotationSpeed;

            }
                               
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //The Rectangle to render the texture
            Rectangle sourceRectangle = new Rectangle(0, 0, m_PlayerTexture.Width, m_PlayerTexture.Height);

            if (!deathAnimationActivate)
            {
                spriteBatch.Draw(m_PlayerTexture, m_Position, sourceRectangle, Color.White, m_fRotation, m_PlayerOrigin, .1f, SpriteEffects.None, 0f);
            }
            else {
                spriteBatch.Draw(deathSpriteStrip, new Vector2(m_Position.X - 50, m_Position.Y - 50), sourceRectDeath, Color.White);
            }

        }

        public void DeathAnimation(Texture2D deathSprite, int frameWidth, int frameHeight, int frameCount, int frametime, bool looping )
        {

            this.FrameWidth = frameWidth;

            this.FrameHeight = frameHeight;

            this.frameCount = frameCount;

            this.frameTime = frametime;

            Looping = looping;

            deathSpriteStrip = deathSprite;

            //Set the time to 
            elapsedTime = 0;

            currentFrame = 0;

            deathAnimationActivate = false;

            finishedAnimation = false;
        }


        public void PlayDeathAnimation(GameTime gameTime) {

            if (!deathAnimationActivate)
            {
                return;
            }

            elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;


            if (elapsedTime > frameTime) {

                currentFrame++;

                if (currentFrame == frameCount)
                {
                
                        deathAnimationActivate = false;
                        finishedAnimation = true;

                }

                //Reset the the elapsed time to zero
                elapsedTime = 0; 

            }

            sourceRectDeath = new Rectangle(currentFrame * FrameWidth, 0, FrameWidth, FrameHeight);

            //Grab the correct frame in the image strip by multiplying the current frame index by the frame width
          

        }



        public void Reset() {
            m_iHealth = 100;
            finishedAnimation = false;
            deathAnimationActivate = false;
            currentFrame = 0;
        }

    }
}
