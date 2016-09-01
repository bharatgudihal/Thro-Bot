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

		protected int m_ParticlesPerEmission;

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
		protected float m_MinLifetime, m_MaxLifetime;

		/// <summary>
		/// Particle scale range.
		/// </summary>
		protected float m_MinScale, m_MaxScale;

		/// <summary>
		/// Particle initial velocity range.
		/// </summary>
		protected Vector2 m_MinVelocity, m_MaxVelocity;

		protected Vector2 m_Wind;

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

			m_ColorTint = Color.White;
		}

		public ParticleSystemBase (float emissionRate, float emissionRadius, int particlesPerEmission,
			float minLifetime, float maxLifetime, float minScale, float maxScale, Vector2 minVelocity, Vector2 maxVelocity,
			float minAngularVelocity, float maxAngularVelocity) : 
			this() 
		{
			m_EmissionRate = emissionRate;
			if (m_EmissionRate > 0f) m_EmissionTimer = m_EmissionRate;

			m_EmissionRadius = emissionRadius;
			m_ParticlesPerEmission = particlesPerEmission;
			m_MinLifetime = minLifetime;
			m_MaxLifetime = maxLifetime;
			m_MinScale = minScale;
			m_MaxScale = maxScale;
			m_MinVelocity = minVelocity;
			m_MaxVelocity = maxVelocity;
			m_MinAngularVelocity = minAngularVelocity;
			m_MaxAngularVelocity = maxAngularVelocity;
		}

		public void Update () {
			if (m_EmissionRate > 0f) {
				m_EmissionTimer -= 1f / 60f;
				if (m_EmissionTimer <= 0f) {
					Emit ();
					m_EmissionTimer = m_EmissionRate;
				}
			}

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

		public void Emit () {
			//Debug.WriteLine ("emit");
			Emit (m_ParticlesPerEmission);
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
				
				Texture2D sprite = m_Sprites.Random(RNG);

				// Either create new particle or recycle old one
				if (_deadParticles.Count > 0) {
					particle = _deadParticles[0];
					_deadParticles.RemoveAt(0);
				} else {
					particle = new Particle();
					particle.OnDeath += new Particle.ParticleDeathHandler (Deactivate);
				}

				// Randomize lifetime
				float l = RNG.RandomFloat (m_MinLifetime, m_MaxLifetime);

				Vector2 pos;
				if (m_EmissionRadius == 0f) {
					pos = m_Position;
				} else {
					// Calculate random position (polar)
					float a = RNG.RandomFloat(0f, 2f * (float)Math.PI);
					pos = new Vector2 ((float)Math.Cos (a), (float)Math.Sin (a)) 
						* RNG.RandomFloat (0f, m_EmissionRadius) + m_Position;
				}

				// Randomize starting rotation
				float r = RNG.RandomFloat(0f, 2f * (float)Math.PI);

				// Randomize starting scale
				float scale = RNG.RandomFloat (m_MinScale, m_MaxScale);

				// Randomize starting velocity
				Vector2 v = new Vector2 (
					RNG.RandomFloat (m_MinVelocity.X, m_MaxVelocity.X),
					RNG.RandomFloat (m_MinVelocity.Y, m_MaxVelocity.Y)
				) + m_Wind;

				// Randomize starting angular velocity
				float av = RNG.RandomFloat (m_MinAngularVelocity, m_MaxAngularVelocity);

				// Init particle
				particle.Initialize (sprite, pos, r, scale, m_ColorTint, l, v, av);

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

		public void SetAllTint (Color color) {
			m_ColorTint = color;
			foreach (Particle particle in _activeParticles) {
				particle.m_InitialColor = color;
			}
		}

		public void SetWind (Vector2 wind) {
			m_Wind = wind;
		}
	}
}
