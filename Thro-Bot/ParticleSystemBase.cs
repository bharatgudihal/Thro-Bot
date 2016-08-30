// ParticleSystemBase.cs

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Thro_Bot {

	public class ParticleSystemBase {

		/// <summary>
		/// 2D position of this particle system.
		/// </summary>
		public Vector2 m_Position;

		/// <summary>
		/// List of possible sprites to use for particles.
		/// </summary>
		public List<Texture2D> m_Sprites;

		/// <summary>
		/// All currently active and drawing particles.
		/// </summary>
		protected List<Particle> _activeParticles;

		/// <summary>
		/// Particles that will be removed.
		/// </summary>
		protected List<Particle> _particlesToRemove;

		/// <summary>
		/// List of dead particles to be reused.
		/// </summary>
		protected List<Particle> _deadParticles;

		/// <summary>
		/// Rate (seconds) of emissions. A value of 0 or lower means this particle
		/// system will not automatically emit.
		/// </summary>
		public float m_EmissionRate;

		/// <summary>
		/// Timer (seconds) until next emission.
		/// </summary>
		protected float m_EmissionTimer;

		/// <summary>
		/// Radius within which particles can be emitted.
		/// </summary>
		public float m_EmissionRadius;

		/// <summary>
		/// Tint to apply to particles emitted in the future.
		/// </summary>
		protected Color m_ColorTint;

		/// <summary>
		/// Lifetime for newly-created particles.
		/// </summary>
		protected float m_Lifetime;

		/// <summary>
		/// Particle scale range.
		/// </summary>
		protected float m_MinScale, m_MaxScale;

		/// <summary>
		/// Particle initial velocity range.
		/// </summary>
		protected Vector2 m_MinVelocity, m_MaxVelocity;

		/// <summary>
		/// Particle initial angular velocity range.
		/// </summary>
		protected float m_MinAngularVelocity, m_MaxAngularVelocity;

		/// <summary>
		/// Random number generator for this particle system.
		/// </summary>
		protected Random RNG;

		public ParticleSystemBase () {
			// Init lists
			_activeParticles = new List<Particle>();
			_particlesToRemove = new List<Particle>();
			_deadParticles = new List<Particle>();

			// Init RNG
			RNG = new Random();
		}

		public ParticleSystemBase (float emissionRate, float emissionRadius,
			float lifetime, float minScale, float maxScale, Vector2 minVelocity, Vector2 maxVelocity,
			float minAngularVelocity, float maxAngularVelocity) : 
			this() 
		{
			m_EmissionRate = emissionRate;
			if (m_EmissionRate > 0f) m_EmissionTimer = m_EmissionRate;

			m_EmissionRadius = emissionRadius;
			m_Lifetime = lifetime;
			m_MinScale = minScale;
			m_MaxScale = maxScale;
			m_MinVelocity = minVelocity;
			m_MaxVelocity = maxVelocity;
			m_MinAngularVelocity = minAngularVelocity;
			m_MaxAngularVelocity = maxAngularVelocity;
		}

		public void Update () {
			

			// Update each particle
			for (int i = 0; i < _activeParticles.Count; i++)
				_activeParticles[i].Update();
			//foreach (Particle particle in _activeParticles)
				//particle.Update();

			// Flush dead particles
			//foreach (Particle deadParticle in _particlesToRemove)
			//	_deadParticles.Add (deadParticle);
		
			//_deadParticles.Clear();
		}

		public void Draw (SpriteBatch spriteBatch) {

			foreach (Particle particle in _activeParticles)
				particle.Draw(spriteBatch);
		}

		/// <summary>
		/// Emits the specified number of particles.
		/// </summary>
		/// <param name="numParticles">Number of particles to emit.</param>
		public void Emit (int numParticles) {
			if (m_Sprites == null)
				throw new NullReferenceException ("No sprites set!");

			for (int i = 0; i < numParticles; i++) {

				Particle particle;
				Texture2D sprite = m_Sprites.Random();

				// Either create new particle or recycle old one
				if (_deadParticles.Count > 0) {
					particle = _deadParticles[0];
					_deadParticles.RemoveAt(0);
				} else {
					particle = new Particle();
					particle.OnDeath += new Particle.ParticleDeathHandler (Deactivate);
				}

				// Calculate random position (polar)
				float a = RNG.RandomFloat(0f, 2f * (float)Math.PI);
				Vector2 pos = new Vector2 ((float)Math.Cos (a), (float)Math.Sin (a)) 
					* RNG.RandomFloat (0f, m_EmissionRadius) + m_Position;

				// Randomize starting rotation
				float r = RNG.RandomFloat(0f, 2f * (float)Math.PI);

				// Randomize starting scale
				float scale = RNG.RandomFloat (m_MinScale, m_MaxScale);

				// Randomize starting velocity
				Vector2 v = new Vector2 (
					RNG.RandomFloat (m_MinVelocity.X, m_MaxVelocity.X),
					RNG.RandomFloat (m_MinVelocity.Y, m_MaxVelocity.Y)
				);

				// Randomize starting angular velocity
				float av = RNG.RandomFloat (m_MinAngularVelocity, m_MaxAngularVelocity);

				// Init particle
				particle.Initialize (sprite, pos, r, scale, m_ColorTint, m_Lifetime, v, av);

				_activeParticles.Add (particle);
			}
		}

		void Deactivate (Particle particle) {
			_activeParticles.Remove (particle);
			_deadParticles.Add (particle);
		}

		/// <summary>
		/// Sets the tint of all future particles.
		/// </summary>
		/// <param name="color">New color to use.</param>
		public void SetTint (Color color) {
			m_ColorTint = color;
		}
	}
}
