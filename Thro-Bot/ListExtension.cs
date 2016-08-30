// ListExtension.cs

using System;
using System.Collections.Generic;
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
		public static T Random<T>(this List<T> list) {
			int count = list.Count;
			Random random = new Random();
			int i = random.Next (count);
			return list[i];
		}
	}
}
