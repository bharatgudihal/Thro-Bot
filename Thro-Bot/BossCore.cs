using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Thro_Bot
{
    class BossCore
    {

        private Texture2D Texture;
        private Vector2 Position;
        private float Opacity;
        private Rectangle SourceRectangle;
        private Vector2 Center;
        public void Initialize(Texture2D texture, Vector2 position)
        {
            Texture = texture;
            Position = position;
            Opacity = 0;
            SourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
            Center = new Vector2(Texture.Width/2,Texture.Height/2);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture,Position, SourceRectangle,Color.Red * Opacity, 0f,Center,1f,SpriteEffects.None,0);
        }

        public float GetOpactity()
        {
            return Opacity;
        }

        public void SetOpacity(float opacity)
        {
            Opacity = opacity;
        }
    }
}
