using UnityEngine;
using System.Collections;

namespace June.Api.Schema {

	/// <summary>
	/// Player.
	/// </summary>
	public partial class Player {
		public const string DeviceId = "di";
		
		public const string DeviceType = "dt";
		
		public const string DeviceToken = "dtk";
		
		public const string Email = "em";
		
		public const string FacebookId = "fbid";
		
		public const string FacebookToken = "fbtk";
		
		public const string PhoneNumbers = "ph";
		
		public const string PlayerId = "_id";
		
		public const string PlayerName = "nm";

		public const string PlayerNameSuffix = "nms";
		
		public const string Wallet = "wlt";

		public const string GameData = "gd";

		public const string ItemsOwned = "io";
		
		public partial class ItemsOwnedFields {
			public const string Id = "id";
			public const string Type = "tp";
			public const string ExpiryTimeStamp = "ets";
		}
		
		public const string ItemsEquipped = "eq";

		public const string ItemsUnlocked = "iu";
	}
}