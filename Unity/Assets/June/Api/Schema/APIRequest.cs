using UnityEngine;
using System.Collections;

namespace June.Api.Schema {

	/// <summary>
	/// API request.
	/// </summary>
	public partial class APIRequest {

		/// <summary>
		/// _aid
		/// </summary>
		public const string ApplicationId = "aid";

		/// <summary>
		/// _av
		/// </summary>
		public const string ApplicationVersion = "av";

		/// <summary>
		/// _api
		/// </summary>
		public const string APIVersion = "api";

		/// <summary>
		/// _si
		/// </summary>
		public const string SessionId = "si";

		/// <summary>
		/// _pid
		/// </summary>
		public const string PlayerId = "_id";

		/// <summary>
		/// _di
		/// </summary>
		public const string DeviceId = "di";

		/// <summary>
		/// _dt
		/// </summary>
		public const string DeviceType = "dt";

		/// <summary>
		/// dtk
		/// </summary>
		public const string DeviceToken = "dtk";

		/// <summary>
		/// em
		/// </summary>
		public const string Email = "em";

		/// <summary>
		/// pwd
		/// </summary>
		public const string Password = "pwd";

		/// <summary>
		/// fbid
		/// </summary>
		public const string FacebookId = "fbid";

		/// <summary>
		/// fbtk
		/// </summary>
		public const string FacebookToken = "fbtk";

		/// <summary>
		/// ph
		/// </summary>
		public const string PhoneNumbers = "ph";

		/// <summary>
		/// nm
		/// </summary>
		public const string PlayerName = "nm";

		/// <summary>
		/// nms
		/// </summary>
		public const string PlayerNameSuffix = "nms";

		/// <summary>
		/// wlt
		/// </summary>
		public const string Wallet = "wlt";

		public partial class WalletFields {
			public const string Coins = "coins";
			public const string diamonds = "diamonds";
		}

		/// <summary>
		/// gd
		/// </summary>
		public const string GameData = "gd";

		public partial class GameDataFields {
			public const string TotalRaces = "tr";
			public const string TotalWins = "win";
			public const string TotalHits = "hit";
			public const string TotalDeaths = "dth";
			public const string TotalTrophies = "tr";
		}

		/// <summary>
		/// io
		/// </summary>
		public const string ItemsOwned = "io";

		public partial class ItemsOwnedFields {
			public const string Id = "id";
			public const string Type = "tp";
			public const string ExpiryTimeStamp = "ets";
		}

		public const string ItemsUnlocked = "iu";

		/// <summary>
		/// eq
		/// </summary>
		public const string ItemsEquipped = "eq";

		/// <summary>
		/// it
		/// </summary>
		public const string ItemId = "it";

		/// <summary>
		/// pi
		/// </summary>
		public const string ProductId = "pi";

		public const string Currency = "cu";

		public const string Cost = "co";

		public const string ReceiptData = "rd";

		public const string PageNumber = "pg";

		public const string RecordsPerPage = "rp";

		public const string RequestType = "tp";

		public const string SenderId = "sid";

		public const string SenderEquipped = "seq";

		public const string SenderAvatarURL = "sav";

		public const string ReceiverId = "rid";

		public const string Data = "data";

		public const string Signature = "sg";

		public const string IsPush = "psuh";

		public const string PushMessage = "message";

		public const string ExpiryTime = "ets";

		public const string Action = "action";

		public const string FriendId = "fid";

		/// <summary>
		/// Push message fields.
		/// </summary>
		public partial class PushMessageFields { 

			public const string Title = "ctitle";

			public const string Text = "ctext";

			public const string GroupKey = "gkey";

			public const string GroupDescription = "gdesc";
		}
	}
}