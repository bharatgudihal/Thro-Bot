// Particle.cs

using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Thro_Bot {
	public class Particle {

		/// <summary>
		/// Is this particle active?
		/// </summary>
		public bool m_Active;

		/// <summary>
		/// 2D position of this particle.
		/// </summary>
		public Vector2 m_Position;

		/// <summary>
		/// Rotation of this particle (radians).
		/// </summary>
		public float m_Rotation;

		/// <summary>
		/// Scale of this particle.
		/// </summary>
		public float m_Scale;

		/// <summary>
		/// Texture used by this particle.
		/// </summary>
		public Texture2D m_Texture;

		/// <summary>
		/// Rectangle in which to draw this particle's sprite.
		/// </summary>
		public Rectangle m_Rect;

		/// <summary>
		/// Location of this particle's origin (pivot point).
		/// </summary>
		public Vector2 m_Origin;

		/// <summary>
		/// Color tint to apply to this particle.
		/// </summary>
		public Color m_Color;

		protected float m_MaxLifetime;

		/// <summary>
		/// Current lifetime of this particle (seconds).
		/// </summary>
		protected float m_Lifetime;

		/// <summary>
		/// Current velocity of this particle (units/step).
		/// </summary>
		public Vector2 m_Velocity;

		/// <summary>
		/// Angular velocity of this particle (radians/step).
		/// </summary>
		public float m_AngularVelocity;

		public bool Dead { get { return !m_Active; } }

		public event ParticleDeathHandler OnDeath;
		public delegate void ParticleDeathHandler (Particle p);

		public void Initialize (Texture2D texture, Vector2 position, 
			float rotation, float scale, Color color, float lifetime, 
			Vector2 velocity, float angularVelocity)
		{
			m_Active = true;
			m_Texture = texture;
			m_Position = position;
			m_Rotation = rotation;
			m_Rect = new Rectangle (0, 0, m_Texture.Width, m_Texture.Height);
			m_Origin = new Vector2 (m_Texture.Width/2, m_Texture.Height/2);
			m_Scale = scale;
			m_Color = color;
			m_MaxLifetime = lifetime;
			m_Lifetime = lifetime;
			m_Velocity = velocity;
			m_AngularVelocity = angularVelocity;
		}

		public void Update () {
			// Decrement lifetime
			m_Lifetime -= 1f/ 60f;
			if (m_Lifetime <= 0f) Kill();
			else {
				m_Position += m_Velocity;
				m_Rotation += m_AngularVelocity;

				m_Color.A = (byte)(m_Lifetime / m_MaxLifetime);
			}
		}

		public void Draw (SpriteBatch spriteBatch) {
			spriteBatch.Draw (m_Texture, m_Position, m_Rect, m_Color, m_Rotation, m_Origin, m_Scale, SpriteEffects.None, 0f);
		}

		public void Kill () {
			if (OnDeath != null) OnDeath (this);

			m_Active = false;
		}
	}
}
