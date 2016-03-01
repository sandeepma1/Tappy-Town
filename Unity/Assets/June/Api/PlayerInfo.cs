using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using June.Core;

namespace June.Api {

	/// <summary>
	/// Player item.
	/// </summary>
	public partial class PlayerInfo : BaseModel, IPlayerInfo {
		
		/// <summary>
		/// Gets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
		public new string Id {
			get {
				return GetString(Schema.PlayerInfo.Id);
			}
		}

		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name {
			get {
				return GetString(Schema.PlayerInfo.PlayerName);
			}
		}

		/// <summary>
		/// Gets the facebook identifier.
		/// </summary>
		/// <value>The facebook identifier.</value>
		public string FacebookId { 
			get {
				return GetString(Schema.PlayerInfo.FacebookId);
			}
		}

		/// <summary>
		/// Gets the avataar URL.
		/// </summary>
		/// <value>The avataar UR.</value>
		public string AvatarURL {
			get {
				string url = GetString(Schema.PlayerInfo.AvatarURL);
				if(string.IsNullOrEmpty(url) && !string.IsNullOrEmpty(this.FacebookId)) {
					url = string.Format("https://graph.facebook.com/{0}/picture", this.FacebookId);
				}
				return url;
			}
		}

		/// <summary>
		/// Gets the game data doc.
		/// </summary>
		/// <value>The game data.</value>
		public IDictionary<string, object> GameDataDoc {
			get {
				return Get<IDictionary<string, object>>(Schema.PlayerInfo.GameData);
			}
		}

		/// <summary>
		/// Gets the equipped.
		/// </summary>
		/// <value>The equipped.</value>
		public IDictionary<string, object> ItemsEquippedDoc {
			get {
				return Get<IDictionary<string, object>>(Schema.PlayerInfo.ItemsEquipped);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="June.Api.Friend"/> class.
		/// </summary>
		/// <param name="doc">Document.</param>
		public PlayerInfo(IDictionary<string, object> doc) : base(doc) { }

		/// <summary>
		/// Gets the player items from array.
		/// </summary>
		/// <returns>The player items from array.</returns>
		/// <param name="array">Array.</param>
		public static List<PlayerInfo> GetPlayerItemsFromArray(SimpleJson.JsonArray array) {
			return BaseList<PlayerInfo>.GetListFromArray(array, doc => new PlayerInfo(doc));
		}
	}
}