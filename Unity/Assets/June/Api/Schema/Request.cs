using UnityEngine;
using System.Collections;

namespace June.Api.Schema {

	/// <summary>
	/// Request.
	/// </summary>
	public class Request {

		/// <summary>
		/// _id
		/// </summary>
		public const string Id = "_id";

		/// <summary>
		/// sid
		/// </summary>
		public const string SenderId = "sid";

		/// <summary>
		/// nm
		/// </summary>
		public const string SenderName = "nm";

		/// <summary>
		/// rid
		/// </summary>
		public const string ReceiverId = "rid";

		/// <summary>
		/// type
		/// </summary>
		public const string Type = "type";

		/// <summary>
		/// sav
		/// </summary>
		public const string SenderAvatarURL = "sav";

		/// <summary>
		/// seq
		/// </summary>
		public const string SenderEqupipped = "seq";

		/// <summary>
		/// data
		/// </summary>
		public const string Data = "data";

		/// <summary>
		/// ex
		/// </summary>
		public const string ExpiryTimestamp = "ets";

		/// <summary>
		/// push
		/// </summary>
		public const string IsPush = "push";

		/// <summary>
		/// The push message.
		/// </summary>
		public const string PushMessage = "message";

		/// <summary>
		/// Push message fields.
		/// </summary>
		public partial class PushMessageFields { 

			public const string Title = "ctitle";

			public const string Text = "ctext";

			public const string GroupKey = "gkey";

			public const string GroupDescription = "gdesc";
		}

		/// <summary>
		/// sg
		/// </summary>
		public const string Signature = "sg";

	}
}