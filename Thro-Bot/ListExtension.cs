// ListExtension.cs

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thro_Bot {

	/// <summary>
	/// Extension functions for C# lists.
	/// </summary>
	public static class ListExtension {

		/// <summary>
		/// Returns a random element from the list.
		/// </summary>
		public static T Random<T>(this List<T> list, Random random) {
			int count = list.Count;
			int i = random.Next (0, count);
			Debug.WriteLine (i);
			return list[i];
		}
	}
}
