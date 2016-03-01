using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using June.Core;

namespace June.Api {

	/// <summary>
	/// API Player.
	/// </summary>
	public partial class Player : BaseModel, IPlayerInfo {

		/// <summary>
		/// Gets the player identifier.
		/// </summary>
		/// <value>The player identifier.</value>
		public string PlayerId {
			get {
				return GetString(Schema.Player.PlayerId);
			}
		}

		/// <summary>
		/// Gets the device identifier.
		/// </summary>
		/// <value>The device identifier.</value>
		public string DeviceId {
			get {
				return GetString(Schema.Player.DeviceId);
			}
		}

		/// <summary>
		/// Gets the type of the device.
		/// </summary>
		/// <value>The type of the device.</value>
		public string DeviceType {
			get {
				return GetString(Schema.Player.DeviceType);
			}
		}

		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name {
			get {
				return PlayerName;
			}
		}

		/// <summary>
		/// Gets the name of the player.
		/// </summary>
		/// <value>The name of the player.</value>
		public string PlayerName {
			get {
				return GetString(Schema.Player.PlayerName);
			}
			set {
				Set (Schema.Player.PlayerName, value);
			}
		}

		/// <summary>
		/// Gets the player name suffix.
		/// </summary>
		/// <value>The player name suffix.</value>
		public int PlayerNameSuffix {
			get {
				return GetInt(Schema.Player.PlayerNameSuffix);
			}
		}

		/// <summary>
		/// Gets the avatar URL.
		/// </summary>
		/// <value>The avatar UR.</value>
		public string AvatarURL {
			get {
				return string.Format("https://graph.facebook.com/{0}/picture", this.FacebookId);
			}
		}

		/// <summary>
		/// Gets the email.
		/// </summary>
		/// <value>The email.</value>
		public string Email {
			get {
				return GetString(Schema.Player.Email);
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
		/// Gets the facebook token.
		/// </summary>
		/// <value>The facebook token.</value>
		public string FacebookToken {
			get {
				return GetString(Schema.Player.FacebookToken);
			}
		}

		/// <summary>
		/// Gets the phone numbers.
		/// </summary>
		/// <value>The phone numbers.</value>
		public List<string> PhoneNumbers {
			get {
				return GetStringList(Schema.Player.PhoneNumbers);
			}
		}

		/// <summary>
		/// Gets the wallet document.
		/// </summary>
		/// <value>The wallet document.</value>
		public IDictionary<string, object> WalletDoc {
			get {
				return Get<IDictionary<string, object>>(Schema.Player.Wallet);
			}
		}

		/// <summary>
		/// Gets the game data doc.
		/// </summary>
		/// <value>The game data.</value>
		public IDictionary<string, object> GameDataDoc {
			get {
				return Get<IDictionary<string, object>>(Schema.Player.GameData);
			}
		}

		private BaseCollection<ItemOwned> _ItemsOwned;
		/// <summary>
		/// Gets the items owned.
		/// </summary>
		/// <value>The items owned.</value>
		public BaseCollection<ItemOwned> ItemsOwned {
						get {
				if(null == _ItemsOwned) {
					_ItemsOwned = new BaseCollection<ItemOwned>(
						Get<SimpleJson.JsonArray>(Schema.Player.ItemsOwned),
						doc => new ItemOwned(doc));
				}
				return _ItemsOwned;
			}
		}

		/// <summary>
		/// Gets the equipped.
		/// </summary>
		/// <value>The equipped.</value>
		public IDictionary<string, object> ItemsEquippedDoc {
			get {
				return Get<IDictionary<string, object>>(Schema.Player.ItemsEquipped);
			}
		}

		/// <summary>
		/// Gets the items unlocked.
		/// </summary>
		/// <value>The items unlocked.</value>
		public List<string> ItemsUnlocked {
			get {
				return GetStringList(Schema.Player.ItemsUnlocked);
			}
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="June.Api.Player"/>.
		/// </summary>
		public override string ToString () {
			return "";//string.Format ("[Player: Wallet={0}, GameData={1}, Equipped={2}, PlayerId={3}, DeviceId={4}, DeviceType={5}, PlayerName={6}, Email={7}, FacebookId={8}, FacebookToken={9}, PhoneNumbers={10}, WalletDoc={11}, GameDataDoc={12}, ItemsOwned={13}, ItemsEquipped={14}]", Wallet, GameData, Equipped, PlayerId, DeviceId, DeviceType, PlayerName, Email, FacebookId, FacebookToken, PhoneNumbers, WalletDoc, GameDataDoc, ItemsOwned, ItemsEquippedDoc);
		}


		/// <summary>
		/// Initializes a new instance of the <see cref="June.Api.Player"/> class.
		/// </summary>
		/// <param name="doc">Document.</param>
		public Player(IDictionary<string, object> doc) : base(doc) { }
	}
}