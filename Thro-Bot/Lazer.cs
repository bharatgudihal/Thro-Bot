using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Thro_Bot
{
    public class Lazer
    {
        private Texture2D Texture;
        private Vector2 Position;        
        private Rectangle SourceRectangle;
        public void Initialize(Texture2D texture, Vector2 position)
        {
            Texture = texture;
            Position = position;            
            SourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height/2-33);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position,SourceRectangle, Color.White);
        }

        public Rectangle GetSourceRectangle()
        {
            return SourceRectangle;
        }
    }
}