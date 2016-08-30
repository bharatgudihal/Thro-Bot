// RandomExtension.cs

using System;

namespace Thro_Bot {

	/// <summary>
	/// Extension functions for C# random class.
	/// </summary>
	public static class RandomExtension {

		/// <summary>
		/// Returns a random float between the specified floats.
		/// </summary>
		/// <param name="a">Minimum value.</param>
		/// <param name="b">Maximum value.</param>
		public static float RandomFloat (this Random random, float a, float b) {
			return a + (float)random.NextDouble() * (b - a);
		}
	}
}
