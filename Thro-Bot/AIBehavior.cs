// AIBehavior.cs

using Microsoft.Xna.Framework;

namespace Thro_Bot {

	/// <summary>
	/// Base class for all AI behaviors. 
	/// </summary>
	public abstract class AIBehavior {

		/// <summary>
		/// Current progress through this behavior (0-1).
		/// </summary>
		protected float _progress;

		/// <summary>
		/// Time (in seconds) to complete a cycle of this behavior.
		/// </summary>
		protected float _behaviorTime;

		/// <summary>
		/// Updates the behavior.
		/// </summary>
		/// <param name="dt">Delta time.</param>
		public virtual void Update (float dt) {
			_progress = (_progress + dt / _behaviorTime) % 1f;
		}

		public abstract Vector2 Position {
	}

	public class LinearMovementBehavior : AIBehavior {
		protected float _direction;
		protected float _speed;

		public float Direction {
			get { return _direction; }
			set { _direction = value; }
		}
		
		public float Speed {
			get { return _speed; }
			set { _speed = value; }
		}

		public Vector2 UpdatePosition () {

		}
	}

	public class SquigglyMovementBehavior : LinearMovementBehavior {
		protected float _squigglyWidth;


	}
}