using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Thro_Bot
{
    public abstract class PowerUp
    {

        //The power up is active
        public bool m_Active;

        //The position of the power up
        public Vector2 m_Position;

        //The rotation of the power up
        public float m_Rotation = 0f;

        //The random time generator
        public Random random = new Random();

        //The scale of the power up
        protected float m_Scale;

        protected abstract float m_MinScale { get; }

        protected abstract float m_MaxScale { get; }

        //The range of spawn times
        protected abstract int m_SpawnTimeRange{ get; }

        //The lower limit of the random
        protected abstract int m_Lower { get; }

        //The upper limit of the random
        protected abstract int m_Upper { get; }

        //The theta of the angle
        private float _theta = 0f;

        //The amplitude of the angle
        private float _amplitude;

        //The period of the signal
        private float _period = 0.15f;

        //The speed of the signal
        private float _speed;

        //The origin of the power up
        public Vector2 m_Origin;

        //The file path of the texture
        protected abstract string m_TexturePath { get; }

        //The texture of the power up
        protected Texture2D m_Texture;

        public Texture2D Texture {

            get { return m_Texture; }
        }

        //The rectangle to display the power up
        protected Rectangle m_Rect;

        //The value of the power up
        public abstract int m_PointValue { get; }

        public TimeSpan m_lifeTime = TimeSpan.FromSeconds(5);
        public TimeSpan m_currentTime = TimeSpan.Zero;

        public virtual void Initialize(Texture2D texture, Vector2 position) {
            m_Active = true;
            m_Position = position;
            m_Rotation = 0f;
            m_Texture = texture;
            m_Rect = new Rectangle(0, 0, m_Texture.Width, m_Texture.Height);
            m_Origin = new Vector2(m_Texture.Width/2, m_Texture.Height / 2);
        }

        public virtual void Update(GameTime gameTime) {

            if (m_Active) {

                Pulse();

                m_currentTime += gameTime.ElapsedGameTime;

                if (m_currentTime > m_lifeTime)
                {
                    m_currentTime = TimeSpan.Zero;
                    Deactivate();
                }
            }

        }

        public virtual void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(m_Texture, m_Position, m_Rect, Color.White, m_Rotation, m_Origin, m_Scale,SpriteEffects.None, 0f);

        }

        public void Pulse() {

            // dtheta = 1 / framerate / period
            float dTheta = 1f / 60f / _period;

            // Increment theta, constraint to 0-2PI
            _theta = (_theta + dTheta) % (2f * (float)Math.PI);

            float midpt = (m_MaxScale - m_MinScale) / 2f;


            m_Scale = m_MinScale + midpt + (m_MaxScale - m_MinScale) * (float)Math.Sin(_theta);
        }
     


        public void Deactivate() {

            m_Active = false;
        }

    }
}
