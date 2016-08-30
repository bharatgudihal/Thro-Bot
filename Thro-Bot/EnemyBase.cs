// MovementBehaviorBase.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Thro_Bot
{
	/// <summary>
	/// Base class for all movement behaviors.
	/// </summary>
	public abstract class MovementBehaviorBase
	{
		/// <summary>
		/// Returns a new position.
		/// </summary>
		/// <param name="position">Initial position.</param>
		/// <param name="rotation">Current rotation.</param>
		/// <param name="speed">Movement speed.</param>
		public abstract Vector2 Move (Vector2 position, float rotation, float speed);
	}

	/// <summary>
	/// Movement behavior that moves the enemy in a straight line.
	/// </summary>
	public class LinearMovementBehavior : MovementBehaviorBase
	{
		public override Vector2 Move(Vector2 position, float rotation, float speed)
		{
			return position + new Vector2 (
				(float)Math.Cos ((double)rotation),
				(float)Math.Sin ((double)rotation)
			) * speed;
		}
	}

	public class SquigglyMovementBehavior : MovementBehaviorBase
	{
		/// <summary>
		/// Current position in oscillation (0-2PI).
		/// </summary>
		private float _theta;

		/// <summary>
		/// Amplitude of oscillation (units).
		/// </summary>
		private float _amplitude;

		/// <summary>
		/// Time (seconds) to complete an oscillation.
		/// </summary>
		private float _period;

		public SquigglyMovementBehavior (float period, float amplitude) 
		{
			_theta = 0f;
			_period = period;
			_amplitude = amplitude;
		}

		public override Vector2 Move(Vector2 position, float rotation, float speed) 
		{
			// dtheta = 1 / framerate / period
			float dTheta = 1f / 60f / _period;

			// Increment theta, constraint to 0-2PI
			_theta = (_theta + dTheta) % (2f * (float)Math.PI);

			// Add sine offset
			Vector2 offset = new Vector2 (
				speed,
				_amplitude * (float)Math.Sin (_theta)
			);

			float sin = (float)Math.Sin (rotation);
			float cos = (float)Math.Cos (rotation);

			// Apply rotation
			Vector2 rotatedOffset = new Vector2 (
				(offset.X * cos) - (offset.Y * sin),
				(offset.X * sin) + (offset.Y * cos)
			);

			return position + rotatedOffset;
		}
	}

	/// <summary>
	/// Base class for all rotation behaviors.
	/// </summary>
	public abstract class RotationBehaviorBase {

		public abstract float Rotate (float rotation);
	}

    public abstract class EnemyBase
    {
		/// <summary>
		/// True if this enemy is currently active.
		/// </summary>
		public bool m_Active;

		/// <summary>
		/// Current 2D position of this enemy.
		/// </summary>
		public Vector2 m_Position;

		/// <summary>
		/// Current rotation of this enemy (radians).
		/// </summary>
		protected float m_Rotation;

		/// <summary>
		/// Scale of this enemy;
		/// </summary>
		protected abstract float m_Scale { get; }

		/// <summary>
		/// Origin (pivot point) of this enemy.
		/// </summary>
		protected Vector2 m_Origin;

		/// <summary>
		/// Movement speed of this enemy (units per step).
		/// </summary>
		protected abstract float m_MovementSpeed { get; }

		protected abstract string m_TexturePath { get; }

		/// <summary>
		/// Sprite to use for this enemy.
		/// </summary>
		protected Texture2D m_Texture;

		public Texture2D Texture {
			get {
				return m_Texture;
			}
		}

		/// <summary>
		/// Rectangle in which to display this enemy's sprite.
		/// </summary>
		protected Rectangle m_Rect;

		/// <summary>
		/// Color tint for this enemy.
		/// </summary>
		public abstract Color m_Color { get; }

		/// <summary>
		/// The number of health points taken from the player when this enemy
		/// crosses the boundary line.
		/// </summary>
		public abstract int m_HurtValue { get; }

		/// <summary>
		/// The number of points given when this enemy is killed.
		/// </summary>
		public abstract int m_PointValue { get; }

		/// <summary>
		/// Movement behavior to use for this enemy type.
		/// </summary>
		protected MovementBehaviorBase _movementBehavior;

		/// <summary>
		/// Rotation behavior to use for this enemy type.
		/// </summary>
		protected RotationBehaviorBase _rotationBehavior;
		
		/// <summary>
		/// Initializes this instance's movement and rotation behaviors.
		/// </summary>
		public abstract void InitializeBehaviors ();

		public virtual void Initialize (Texture2D texture, Vector2 position) {
			m_Active = true;
			m_Position = position;
			m_Rotation = (float)Math.PI / 2f;
			m_Texture = texture;
			m_Rect = new Rectangle (0, 0, m_Texture.Width, m_Texture.Height);
            //m_Origin = new Vector2 (m_Texture.Width/2, m_Texture.Height/2);
            m_Origin = Vector2.Zero;

			// Init movement/rotation behaviors
			InitializeBehaviors();
		}

		public virtual void Update () {
			if (_movementBehavior != null)
				m_Position = _movementBehavior.Move (m_Position, m_Rotation, m_MovementSpeed);

			if (_rotationBehavior != null)
				m_Rotation = _rotationBehavior.Rotate (m_Rotation);
		}

		public virtual void Draw (SpriteBatch spriteBatch) {
			spriteBatch.Draw (m_Texture, m_Position, m_Rect, m_Color, m_Rotation - ((float)Math.PI/2f), m_Origin, m_Scale, SpriteEffects.None, 0f);
		}

		public void Kill () {
			m_Active = false;
		}
    }
}
