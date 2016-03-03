using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using June.Core;

namespace June.Api {

	/// <summary>
	/// Item owned.
	/// </summary>
	public class ItemOwned : BaseModel {

		/// <summary>
		/// Gets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
		public string Id {
			get {
				return GetString(Schema.Player.ItemsOwnedFields.Id);
			}
		}

		/// <summary>
		/// Gets the type.
		/// </summary>
		/// <value>The type.</value>
		public string Type {
			get {
				return GetString(Schema.Player.ItemsOwnedFields.Type);
			}
		}

		/// <summary>
		/// Gets the expiry time stamp.
		/// </summary>
		/// <value>The expiry time stamp.</value>
		public int ExpiryTimeStamp {
			get {
				return GetInt(Schema.Player.ItemsOwnedFields.ExpiryTimeStamp);
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance is expired.
		/// </summary>
		/// <value><c>true</c> if this instance is expired; otherwise, <c>false</c>.</value>
		public bool IsExpired {
			get {
				return (ExpiryTimeStamp > 0) ? Util.CurrentUTCTimestamp > ExpiryTimeStamp : false;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="June.Api.ItemOwned"/> class.
		/// </summary>
		/// <param name="doc">Document.</param>
		public ItemOwned(IDictionary<string, object> doc) : base(doc) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="June.Api.ItemOwned"/> class.
		/// </summary>
		/// <param name="id">Identifier.</param>
		/// <param name="type">Type.</param>
		/// <param name="exipryTimestamp">Exipry timestamp.</param>
		public ItemOwned(string id, string type, int exipryTimestamp) : this(
			new Dictionary<string, object>() {
				{ Schema.Player.ItemsOwnedFields.Id, id },
				{ Schema.Player.ItemsOwnedFields.Type, type },
				{ Schema.Player.ItemsOwnedFields.ExpiryTimeStamp, exipryTimestamp }
			}) { }

		public static IDictionary<string, object> GetItemOwnedDoc(string id, string type, int exipryTimestamp) {
			return new Dictionary<string, object> () {
				{ Schema.Player.ItemsOwnedFields.Id, id },
				{ Schema.Player.ItemsOwnedFields.Type, type },
				{ Schema.Player.ItemsOwnedFields.ExpiryTimeStamp, exipryTimestamp }
			};
		}
	}
}