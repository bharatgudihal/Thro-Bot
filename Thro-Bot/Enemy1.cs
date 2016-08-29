using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Thro_Bot
{
    class Enemy1
    {

        // Position
        public Vector2 Position;

        // Texture
        public Texture2D Texture;

        // Rectangle
        public Rectangle rectangle;

        // Origin
        private Vector2 origin;

        // Active flag
        public bool Active = true;

        // Descend speed
        private const float speed = 1f;

        public void Initialize(Texture2D texture, Vector2 position)
        {
            Texture = texture;
            Position = position;
            rectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
            origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
        }

        public void Update()
        {
            Position.Y += speed;            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, rectangle, Color.White, 0f, origin, 0.5f, SpriteEffects.None, 0f);
        }
    }
        
}
