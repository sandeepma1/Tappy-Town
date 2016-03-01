using UnityEngine;
using System.Collections;
using June.Core;
using System.Collections.Generic;
using System;

namespace June.Api {

	/// <summary>
	/// Friend Model.
	/// </summary>
	public partial class Friend : PlayerInfo {

		/// <summary>
		/// Initializes a new instance of the <see cref="June.Api.Friend"/> class.
		/// </summary>
		/// <param name="doc">Document.</param>
		public Friend(IDictionary<string, object> doc) : base(doc) { }

		/// <summary>
		/// Gets the friends from array.
		/// </summary>
		/// <returns>The friends from array.</returns>
		/// <param name="array">Array.</param>
		public static List<Friend> GetFriendsFromArray(SimpleJson.JsonArray array) {
			return BaseList<Friend>.GetListFromArray(array, doc => new Friend(doc));
		}
	}
}