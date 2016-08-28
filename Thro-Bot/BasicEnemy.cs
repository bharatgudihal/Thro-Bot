using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Thro_Bot {

	/// <summary>
	/// Class for basic enemies with linear movement.
	/// </summary>
	public class BasicEnemy {

		#region Fields

		public static Texture2D Texture;
		public static float Scale = 0.5f;
		public Vector2 m_Pivot;
		public Vector2 m_Position;
		public float m_Rotation;
		public bool m_Active;

		public float m_MoveDirection = (float)Math.PI / 2f;
		public const float MoveSpeed = 0.25f;

		#endregion
		#region Monogame Callbacks

		public void Initialize (Texture2D texture, Vector2 position) {
			Texture = texture;

			m_Position = position;
			m_Rotation = (float)(Math.PI);

			m_Pivot = new Vector2 ((float)(Texture.Width/2), (float)(Texture.Height/2));

			m_Active = true;

			
		}

		public void Update (GameTime gameTime) {
			m_Position += new Vector2 (
				(float)Math.Cos (m_MoveDirection), 
				(float)Math.Sin (m_MoveDirection)
			) * MoveSpeed;

			//Debug.WriteLine (m_Position);
		}

		public void Draw (SpriteBatch spriteBatch) {
			 //The Rectangle to render the texture
            Rectangle sourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
            
            spriteBatch.Draw(Texture,m_Position,sourceRectangle,Color.White,m_Rotation, m_Pivot, Scale,SpriteEffects.None,0f);
		}

		#endregion
	}
}