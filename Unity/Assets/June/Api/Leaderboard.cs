using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using June.Core;

namespace June.Api {

	/// <summary>
	/// Leaderboard.
	/// </summary>
	public partial class Leaderboard : BaseModel {

		/// <summary>
		/// Gets the page no.
		/// </summary>
		/// <value>The page no.</value>
		public int PageNo {
			get {
				return GetInt(Schema.Leaderboard.PageNo);
			}
		}

		/// <summary>
		/// Gets the results per page.
		/// </summary>
		/// <value>The results per page.</value>
		public int ResultsPerPage {
			get {
				return GetInt(Schema.Leaderboard.ResultsPerPage);
			}
		}

		private LeaderboardItemCollection _Items;
		/// <summary>
		/// Gets the items.
		/// </summary>
		/// <value>The items.</value>
		public LeaderboardItemCollection Items {
			get {
				if(null == _Items) {
					_Items = new LeaderboardItemCollection(Get<SimpleJson.JsonArray>(Schema.Leaderboard.LeaderboardItems));
				}
				return _Items;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="June.Api.Leaderboard"/> class.
		/// </summary>
		/// <param name="doc">Document.</param>
		public Leaderboard(IDictionary<string, object> doc) : base(doc) { } 
	}

	/// <summary>
	/// Leaderboard item collection.
	/// </summary>
	public partial class LeaderboardItemCollection : BaseCollection<LeaderboardItem> {

		/// <summary>
		/// Gets the array from dict.
		/// </summary>
		/// <returns>The array from dict.</returns>
		/// <param name="doc">Document.</param>
		private static SimpleJson.JsonArray GetArrayFromDict(SimpleJson.JsonArray doc) {

			if (null != doc) {
				for (int i=0; i<doc.Count; i++) {
					((IDictionary<string, object>)doc[i]).Add("rank", i + 1);
				}
			}

			return doc;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="June.Api.LeaderboardItemCollection"/> class.
		/// </summary>
		/// <param name="doc">Document.</param>
		public LeaderboardItemCollection(SimpleJson.JsonArray doc) 
		: base(GetArrayFromDict(doc), d => new LeaderboardItem(d)) { }
	}


	/// <summary>
	/// Leaderboard.
	/// </summary>
	public partial class LeaderboardItem : BaseModel {

		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name.</value>
		public string PlayerName {
			get {
				return GetString(Schema.Player.PlayerName);
			}
		}

		/// <summary>
		/// Gets the name suffix.
		/// </summary>
		/// <value>The name suffix.</value>
		public int PlayerNameSuffix {
			get {
				return GetInt(Schema.Player.PlayerNameSuffix);
			}
		}

		/// <summary>
		/// Gets the facebook identifier.
		/// </summary>
		/// <value>The facebook identifier.</value>
		public string FacebookId {
			get {
				return GetString(Schema.Player.FacebookId);
			}
		}

		/// <summary>
		/// Gets the game data document.
		/// </summary>
		/// <value>The game data document.</value>
		public IDictionary<string, object> GameDataDoc {
			get {
				return Get<IDictionary<string, object>>(Schema.Player.GameData);
			}
		}

		/// <summary>
		/// Gets the items equipped.
		/// </summary>
		/// <value>The items equipped.</value>
		public IDictionary<string, object> ItemsEquipped {
			get {
				return Get<IDictionary<string, object>>(Schema.Player.ItemsEquipped);
			}
		}

		

		/// <summary>
		/// Initializes a new instance of the <see cref="June.Api.LeaderboardItem"/> class.
		/// </summary>
		/// <param name="doc">Document.</param>
		public LeaderboardItem(IDictionary<string, object> doc) : base(doc) {
				}

	
		public override string ToString ()
		{
			return string.Format ("[LeaderboardItem: Name={0}, GameDataDoc={1}, ItemsEquipped={2}]", PlayerName, GameDataDoc, ItemsEquipped);
		}
	}
}