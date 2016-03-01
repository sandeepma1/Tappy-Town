using UnityEngine;
using System.Collections;

namespace June.Api.Schema {

	/// <summary>
	/// API response.
	/// </summary>
	public partial class APIResponse {
		public const string Error = "error";

		public const string Message = "msg";

		public const string Result = "result";

		public partial class ResultFields {
			public const string Created = "created";

			public const string Updated = "updated";

			public const string Success = "success";

			public const string Fraud = "fraud";

			public const string Player = "player";

			public const string Items = "items";

			public const string Page = "pg";

			public const string ResultsPerPage = "rp";

			public const string Leaderboard = "lb";

			public const string Data = "data";

			public const string Available = "available";

			public const string Banned = "banned";

			public const string Name = "nm";

			public const string NameSuffix = "nms";

			public const string SessionId = "si";
		}
	}
}