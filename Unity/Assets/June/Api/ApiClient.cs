using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using Logger = June.DebugLogger;

namespace June.Api {

	/// <summary>
	/// API client.
	/// </summary>
	public partial class ApiClient {

		/// <summary>
		/// The SERVER NOT REACHABLE COUNT.
		/// </summary>
		private static int SERVER_NOT_REACHABLE_COUNT = 0;

		/// <summary>
		/// The June API Version
		/// </summary>
		public const string API_VERSION = "1.0";

		/// <summary>
		/// API Base URL
		/// </summary>
		#if RELEASE
				public const string BASE_URL = "http://prod.api.chhotabheemworld.com";
				public const string SECURED_BASE_URL = BASE_URL; //"https://prod.api.chhotabheemworld.com";
		#else
				public const string BASE_URL = "http://staging.api.chhotabheemworld.com";
				public const string SECURED_BASE_URL = "http://staging.api.chhotabheemworld.com";
		#endif

		/// <summary>
		/// The SECRET.
		/// </summary>
				private static readonly string SECRET  = "chh074bh33m123456abcdef";

		/// <summary>
		/// API Endpoints
		/// </summary>
		public partial class EndPoints {

			//Data Sunc
			public const string DataSync = BASE_URL + "/Data/Sync";

			//Sessions TODO: Implement session
			public const string SessionCreate = BASE_URL + "/Session/Create";
			public const string SessionCheck = BASE_URL + "/Session/Check";
			public const string SessionKill = BASE_URL + "/Session/Kill";

			//Player Endpoints
			public const string PlayerRegister = SECURED_BASE_URL + "/Player/Register";
			public const string PlayerUpdate = SECURED_BASE_URL + "/Player/Update";
			public const string PlayerCheckName = SECURED_BASE_URL + "/Player/CheckName";
			public const string PlayerGetPlayer = BASE_URL + "/Player/Get";
			public const string PlayerGetOpponents = BASE_URL + "/GameSession/GetOpponents";
			public const string PlayerGetPlayerByFacebookId = BASE_URL + "/Player/GetByFacebookId";
			public const string PlayerGetPlayerByEmail = BASE_URL + "/Player/GetByEmail";
			public const string PlayerSearch = BASE_URL + "/Player/Search";

			//Purchase
			public const string PurchaseMake = SECURED_BASE_URL + "/Purchase/Make";

			//Leaderboard
			public const string LeaderboardDaily = BASE_URL + "/Leaderboard/Daily";
			public const string LeaderboardWeekly = BASE_URL + "/Leaderboard/Weekly";
			public const string LeaderboardGlobal = BASE_URL + "/Leaderboard/Global";
			public const string LeaderboardFriends = BASE_URL + "/Leaderboard/Friends";
			public const string LeaderboardUpdateStats = BASE_URL + "/Leaderboard/UpdateStats";


			//Requests TODO:
			public const string RequestSend = BASE_URL + "/Request/Send";
			public const string RequestPending = BASE_URL + "/Request/Pending";
			public const string RequestRespond = BASE_URL + "/Request/Respond";

			//Friends TODO:
			public const string FriendGetAll = BASE_URL + "/Friend/GetAll";
			public const string FriendRemove = BASE_URL + "/Friend/Remove";

			//GameSettings
			public const string GameSettingsGet = BASE_URL + "/GameSettings/Get";
			public static string StatusVersion = BASE_URL + "/Status/Version";
		}

		#region EndPoints

		#region Data Sync

		/// <summary>
		/// Datas the sync.
		/// </summary>
		/// <returns>The sync.</returns>
		/// <param name="appId">App identifier.</param>
		/// <param name="appVer">App ver.</param>
		/// <param name="deviceId">Device identifier.</param>
		/// <param name="deviceType">Device type.</param>
		/// <param name="type">Type.</param>
		/// <param name="locale">Locale.</param>
		/// <param name="callback">Callback.</param>
		public static Job DataSync(string appId, string appVer, string deviceId, string deviceType, string playerId, string sessionId, string type, string locale, Action<APIResponse> callback) {

			var payload = new Dictionary<string, object>() {
				{ June.Api.Schema.APIRequest.ApplicationId, appId },
				{ June.Api.Schema.APIRequest.ApplicationVersion, appVer },
				{ June.Api.Schema.APIRequest.APIVersion, API_VERSION },
				{ June.Api.Schema.APIRequest.DeviceId, deviceId ?? string.Empty },
				{ June.Api.Schema.APIRequest.DeviceType, deviceType ?? string.Empty },
				{ June.Api.Schema.APIRequest.PlayerId, playerId },
				{ June.Api.Schema.APIRequest.SessionId, sessionId },
				{ "tp", type },
				{ "lc", locale }
			};
			return Job.Create(
				SendPOSTRequest(
					EndPoints.DataSync,
					payload,
					callback));
		}


		#endregion

		#region Purchase

		/// <summary>
		/// Purchases the make.
		/// </summary>
		/// <returns>The make.</returns>
		/// <param name="appId">App identifier.</param>
		/// <param name="appVer">App ver.</param>
		/// <param name="deviceId">Device identifier.</param>
		/// <param name="deviceType">Device type.</param>
		/// <param name="playerId">Player identifier.</param>
		/// <param name="itemId">Item identifier.</param>
		/// <param name="productId">Product identifier.</param>
		/// <param name="currency">Currency.</param>
		/// <param name="cost">Cost.</param>
		/// <param name="receipt">Receipt.</param>
		/// <param name="callback">Callback.</param>
		public static Job PurchaseMake(string appId, string appVer, string deviceId, string deviceType, string playerId, 
		                               string itemId, string productId, string currency, float cost, string receipt, 
		                               Action<APIResponse> callback) {
			var payload = new Dictionary<string, object>() {
				{ June.Api.Schema.APIRequest.ApplicationId, appId },
				{ June.Api.Schema.APIRequest.ApplicationVersion, appVer },
				{ June.Api.Schema.APIRequest.APIVersion, API_VERSION },
				{ June.Api.Schema.APIRequest.DeviceId, deviceId ?? string.Empty },
				{ June.Api.Schema.APIRequest.DeviceType, deviceType ?? string.Empty },
//				{ June.Api.Schema.APIRequest.PlayerId, playerId },
				{ "pid", playerId },

				{ "tp", "" },	// inapp / ingame

				{ June.Api.Schema.APIRequest.ItemId, itemId },
				{ June.Api.Schema.APIRequest.ProductId, productId },
				{ June.Api.Schema.APIRequest.Currency, currency },
				{ June.Api.Schema.APIRequest.Cost, cost },
				{ June.Api.Schema.APIRequest.ReceiptData, receipt }
			};
			return Job.Create(
				SendPOSTRequest(
					EndPoints.PurchaseMake,
					payload,
					callback));
		}

		#endregion

		#region Player
		/// <summary>
		/// Rregisters the player.
		/// </summary>
		/// <returns>The register.</returns>
		/// <param name="appId">App identifier.</param>
		/// <param name="appVer">App ver.</param>
		/// <param name="apiVer">API ver.</param>
		/// <param name="deviceId">Device identifier.</param>
		/// <param name="deviceType">Device type.</param>
		/// <param name="email">Email.</param>
		/// <param name="fbId">Fb identifier.</param>
		/// <param name="fbTk">Fb tk.</param>
		/// <param name="phoneNumbers">Phone numbers.</param>
		public static Job PlayerRegister(string appId, string appVer, string deviceId, string deviceType, string deviceToken, string email, string fbId, string fbTk, string[] phoneNumbers, Action<APIResponse> callback) {
			var payload = new Dictionary<string, object>() {
				{ June.Api.Schema.APIRequest.ApplicationId, appId },
				{ June.Api.Schema.APIRequest.ApplicationVersion, appVer },
				{ June.Api.Schema.APIRequest.APIVersion, API_VERSION },
				{ June.Api.Schema.APIRequest.DeviceId, deviceId ?? string.Empty },
				{ June.Api.Schema.APIRequest.DeviceType, deviceType ?? string.Empty },
				{ June.Api.Schema.APIRequest.DeviceToken, deviceToken ?? string.Empty },
				{ June.Api.Schema.APIRequest.Email, email ?? string.Empty },
				{ June.Api.Schema.APIRequest.FacebookId, fbId ?? string.Empty },
				{ June.Api.Schema.APIRequest.FacebookToken, fbTk ?? string.Empty },
				{ June.Api.Schema.APIRequest.PhoneNumbers, phoneNumbers ?? new string[] { } }/*,

				// HACK: This data needs to be received from the Game, right now resetting the players data to defaults.
				{ June.Api.Schema.APIRequest.GameData, June.Api.GameData.GetDefaultObject() }*/
			};
			return Job.Create(
				SendPOSTRequest(
					EndPoints.PlayerRegister,
					payload,
					callback));
		}

				/// <summary>
		/// Players the update.
		/// </summary>
		/// <returns>The update.</returns>
		/// <param name="appId">App identifier.</param>
		/// <param name="appVer">App ver.</param>
		/// <param name="deviceId">Device identifier.</param>
		/// <param name="deviceType">Device type.</param>
		/// <param name="deviceToken">Device token.</param>
		/// <param name="playerId">Player Id</param>
		/// <param name="email">Email.</param>
		/// <param name="fbId">Fb identifier.</param>
		/// <param name="fbTk">Fb tk.</param>
		/// <param name="phoneNumbers">Phone numbers.</param>
		/// <param name="walletValues">Wallet values.</param>
		/// <param name="gameData">Game data.</param>
		/// <param name="itemsOwned">Items owned.</param>
		/// <param name="itemsEquipped">Items equipped.</param>
		/// <param name="callback">Callback.</param>
		public static Job PlayerUpdate(string appId, string appVer, string deviceId, string deviceType, string deviceToken, string playerId, string email, string fbId, string fbTk, string[] phoneNumbers
		                               ,string playerName
		                               ,IDictionary<string, object> walletValues
		                               ,IDictionary<string, object> gameData
		                               ,SimpleJson.JsonArray itemsOwned
		                               ,IDictionary<string, object> itemsEquipped
		                               ,Action<APIResponse> callback) {
			var payload = new Dictionary<string, object>() {
				{ June.Api.Schema.APIRequest.ApplicationId, appId },
				{ June.Api.Schema.APIRequest.ApplicationVersion, appVer },
				{ June.Api.Schema.APIRequest.APIVersion, API_VERSION },
				{ June.Api.Schema.APIRequest.DeviceId, deviceId ?? string.Empty },
				{ June.Api.Schema.APIRequest.DeviceType, deviceType ?? string.Empty },
				{ June.Api.Schema.APIRequest.DeviceToken, deviceToken ?? string.Empty },
				{ June.Api.Schema.APIRequest.Email, email ?? string.Empty },
				{ June.Api.Schema.APIRequest.FacebookId, fbId ?? string.Empty },
				{ June.Api.Schema.APIRequest.FacebookToken, fbTk ?? string.Empty },
				{ June.Api.Schema.APIRequest.PhoneNumbers, phoneNumbers ?? new string[] { } },
				{ June.Api.Schema.APIRequest.PlayerId, playerId },
				{ June.Api.Schema.APIRequest.PlayerName, playerName },
				{ June.Api.Schema.APIRequest.Wallet, walletValues },
				{ June.Api.Schema.APIRequest.GameData, gameData },
				{ June.Api.Schema.APIRequest.ItemsOwned, itemsOwned },
				{ June.Api.Schema.APIRequest.ItemsEquipped, itemsEquipped }
			};
			return Job.Create(
				SendPOSTRequest(
					EndPoints.PlayerUpdate,
					payload,
					callback));
		}

				/// <summary>
		/// Updates the players name.
		/// </summary>
		/// <returns>The update name.</returns>
		/// <param name="appId">App identifier.</param>
		/// <param name="appVer">App ver.</param>
		/// <param name="deviceId">Device identifier.</param>
		/// <param name="deviceType">Device type.</param>
		/// <param name="deviceToken">Device token.</param>
		/// <param name="playerId">Player identifier.</param>
		/// <param name="playerName">Player name.</param>
		public static Job PlayerCheckName(string appId, string appVer, string deviceId, string deviceType, string deviceToken, string playerId, string playerName, int playerNameSuffix, Action<APIResponse> callback) {
			var payload = new Dictionary<string, object>() {
				{ June.Api.Schema.APIRequest.ApplicationId, appId },
				{ June.Api.Schema.APIRequest.ApplicationVersion, appVer },
				{ June.Api.Schema.APIRequest.APIVersion, API_VERSION },
				{ June.Api.Schema.APIRequest.DeviceId, deviceId ?? string.Empty },
				{ June.Api.Schema.APIRequest.DeviceType, deviceType ?? string.Empty },
				{ June.Api.Schema.APIRequest.DeviceToken, deviceToken ?? string.Empty },
				{ June.Api.Schema.APIRequest.PlayerId, playerId },
				{ June.Api.Schema.APIRequest.PlayerName, playerName },
				{ June.Api.Schema.APIRequest.PlayerNameSuffix, playerNameSuffix }
			};
			return Job.Create(
				SendPOSTRequest(
					EndPoints.PlayerCheckName,
					payload,
					callback));
		}

		/// <summary>
		/// Updates the players name.
		/// </summary>
		/// <returns>The update name.</returns>
		/// <param name="appId">App identifier.</param>
		/// <param name="appVer">App ver.</param>
		/// <param name="deviceId">Device identifier.</param>
		/// <param name="deviceType">Device type.</param>
		/// <param name="deviceToken">Device token.</param>
		/// <param name="playerId">Player identifier.</param>
		/// <param name="playerName">Player name.</param>
		public static Job PlayerUpdateName(string appId, string appVer, string deviceId, string deviceType, string deviceToken, string playerId, string playerName, Action<APIResponse> callback) {
			var payload = new Dictionary<string, object>() {
				{ June.Api.Schema.APIRequest.ApplicationId, appId },
				{ June.Api.Schema.APIRequest.ApplicationVersion, appVer },
				{ June.Api.Schema.APIRequest.APIVersion, API_VERSION },
				{ June.Api.Schema.APIRequest.DeviceId, deviceId ?? string.Empty },
				{ June.Api.Schema.APIRequest.DeviceType, deviceType ?? string.Empty },
				{ June.Api.Schema.APIRequest.DeviceToken, deviceToken ?? string.Empty },
				{ June.Api.Schema.APIRequest.PlayerId, playerId },
				{ June.Api.Schema.APIRequest.PlayerName, playerName }
			};
			return Job.Create(
				SendPOSTRequest(
					EndPoints.PlayerUpdate,
					payload,
					callback));
		}

		/// <summary>
		/// Games the session get opponents.
		/// </summary>
		/// <returns>The session get opponents.</returns>
		/// <param name="appId">App identifier.</param>
		/// <param name="appVer">App ver.</param>
		/// <param name="deviceId">Device identifier.</param>
		/// <param name="deviceType">Device type.</param>
		/// <param name="playerId">Player identifier.</param>
		/// <param name="count">Count.</param>
		/// <param name="callback">Callback.</param>
		public static Job GameSessionGetOpponents(string appId, string appVer, string deviceId, string deviceType, string playerId, int count, Action<APIResponse> callback) {
			var payload = new Dictionary<string, object>() {
				{ June.Api.Schema.APIRequest.ApplicationId, appId },
				{ June.Api.Schema.APIRequest.ApplicationVersion, appVer },
				{ June.Api.Schema.APIRequest.APIVersion, API_VERSION },
				{ June.Api.Schema.APIRequest.DeviceId, deviceId },
				{ June.Api.Schema.APIRequest.DeviceType, deviceId },
				{ June.Api.Schema.APIRequest.PlayerId, playerId },
				{ "cnt", count }
			};
			return Job.Create(
				SendPOSTRequest(
				EndPoints.PlayerGetOpponents,
				payload,
				callback));
		}


		/// <summary>
		/// Players the get player.
		/// </summary>
		/// <returns>The get player.</returns>
		/// <param name="appId">App identifier.</param>
		/// <param name="appVer">App ver.</param>
		/// <param name="deviceId">Device identifier.</param>
		/// <param name="deviceType">Device type.</param>
		/// <param name="playerId">Player identifier.</param>
		/// <param name="callback">Callback.</param>
		public static Job PlayerGetPlayer(string appId, string appVer, string deviceId, string deviceType, string playerId, Action<APIResponse> callback) {
			var payload = new Dictionary<string, object>() {
				{ June.Api.Schema.APIRequest.ApplicationId, appId },
				{ June.Api.Schema.APIRequest.ApplicationVersion, appVer },
				{ June.Api.Schema.APIRequest.APIVersion, API_VERSION },
				{ June.Api.Schema.APIRequest.DeviceId, deviceId },
				{ June.Api.Schema.APIRequest.DeviceType, deviceType },
				{ June.Api.Schema.APIRequest.PlayerId, playerId }
			};
			return Job.Create(
				SendPOSTRequest(
					EndPoints.PlayerGetPlayer,
					payload,
					callback));
		}


		public static Job PlayerGetPlayerByFacebookId(string appId, string appVer, string deviceId, string deviceType, string playerId, string sessionId, string facebookId, Action<APIResponse> callback) {
			var payload = new Dictionary<string, object>() {
				{ June.Api.Schema.APIRequest.ApplicationId, appId },
				{ June.Api.Schema.APIRequest.ApplicationVersion, appVer },
				{ June.Api.Schema.APIRequest.APIVersion, API_VERSION },
				{ June.Api.Schema.APIRequest.DeviceId, deviceId },
				{ June.Api.Schema.APIRequest.DeviceType, deviceType },
				{ June.Api.Schema.APIRequest.PlayerId, playerId },
				{ June.Api.Schema.APIRequest.FacebookId, facebookId },
				{ June.Api.Schema.APIRequest.SessionId, sessionId }
			};
			return Job.Create(
				SendPOSTRequest(
					EndPoints.PlayerGetPlayerByFacebookId,
					payload,
					callback));
		}

		public static Job PlayerSearch(string appId, string appVer, string deviceId, string deviceType, string playerId, string sessionId, string searchTerm, int pageNo, int countPerPage, Action<APIResponse> callback) {
			var payload = new Dictionary<string, object>() {
				{ June.Api.Schema.APIRequest.ApplicationId, appId },
				{ June.Api.Schema.APIRequest.ApplicationVersion, appVer },
				{ June.Api.Schema.APIRequest.APIVersion, API_VERSION },
				{ June.Api.Schema.APIRequest.DeviceId, deviceId },
				{ June.Api.Schema.APIRequest.DeviceType, deviceType },
				{ June.Api.Schema.APIRequest.PlayerId, playerId },
				{ June.Api.Schema.APIRequest.SessionId, sessionId },
				{ "str", searchTerm },
				{ "pg", pageNo },
				{ "rp", countPerPage }
			};
			return Job.Create(
				SendPOSTRequest(
					EndPoints.PlayerSearch,
					payload,
					callback));
		}

		/// <summary>
		/// Gets the latest version of the application
		/// </summary>
		/// <returns>The version.</returns>
		public static Job StatusVersion(string deviceType, string applicationId, string publishedId, Action<APIResponse> callback) {
			string url = string.Format("{0}?dt={1}&appid={2}&pub={3}", EndPoints.StatusVersion, deviceType, applicationId, publishedId);
			return SendGETRequest(url, callback);
		}

		#endregion

		#region Session

		public static Job SessionCreate(string appId, string appVer, string deviceId, string deviceType, string playerId, Action<APIResponse> callback) {
			var payload = new Dictionary<string, object>() {
				{ June.Api.Schema.APIRequest.ApplicationId, appId },
				{ June.Api.Schema.APIRequest.ApplicationVersion, appVer },
				{ June.Api.Schema.APIRequest.APIVersion, API_VERSION },
				{ June.Api.Schema.APIRequest.DeviceId, deviceId ?? string.Empty },
				{ June.Api.Schema.APIRequest.PlayerId, playerId }
			};
			return Job.Create(
				SendPOSTRequest(
					EndPoints.SessionCreate,
					payload,
					callback));
		}

		#endregion

		#region Requests

		public static Job RequestSend(string appId, string appVer, string deviceId, string deviceType, string playerId, string sessionId,
			string requestType, IDictionary<string, object> currentEquipped, string senderAvatarURL, 
			string receiverId, IDictionary<string, object> data, 
			string signature, bool isPush, IDictionary<string, object> pushMessage, int expiryInSeconds,
			Action<APIResponse> callback) {

			var payload = new Dictionary<string, object>() {
				{ June.Api.Schema.APIRequest.ApplicationId, appId },
				{ June.Api.Schema.APIRequest.ApplicationVersion, appVer },
				{ June.Api.Schema.APIRequest.APIVersion, API_VERSION },
				{ June.Api.Schema.APIRequest.DeviceId, deviceId ?? string.Empty },
				{ June.Api.Schema.APIRequest.DeviceType, deviceType },
				{ June.Api.Schema.APIRequest.PlayerId, playerId },
				{ June.Api.Schema.APIRequest.SessionId, sessionId },

				{ June.Api.Schema.APIRequest.RequestType, requestType },
				{ June.Api.Schema.APIRequest.SenderId, playerId },
				{ June.Api.Schema.APIRequest.SenderEquipped, currentEquipped },
				{ June.Api.Schema.APIRequest.SenderAvatarURL, senderAvatarURL },
				{ June.Api.Schema.APIRequest.ReceiverId, receiverId },
				{ June.Api.Schema.APIRequest.Data, data },
				{ June.Api.Schema.APIRequest.Signature, signature },
				{ June.Api.Schema.APIRequest.IsPush, isPush },
				{ June.Api.Schema.APIRequest.PushMessage, pushMessage },
				{ June.Api.Schema.APIRequest.ExpiryTime, expiryInSeconds }
			};
			return Job.Create(
				SendPOSTRequest(
					EndPoints.RequestSend,
					payload,
					callback));
		}

		public static Job RequestPending(string appId, string appVer, string deviceId, string deviceType, string sessionId,
			string playerId, string requestType, Action<APIResponse> callback) {
			var payload = new Dictionary<string, object>() {
				{ June.Api.Schema.APIRequest.ApplicationId, appId },
				{ June.Api.Schema.APIRequest.ApplicationVersion, appVer },
				{ June.Api.Schema.APIRequest.APIVersion, API_VERSION },
				{ June.Api.Schema.APIRequest.DeviceId, deviceId ?? string.Empty },
				{ June.Api.Schema.APIRequest.DeviceType, deviceType },
				{ June.Api.Schema.APIRequest.PlayerId, playerId },
				{ June.Api.Schema.APIRequest.SessionId, sessionId },

				{ June.Api.Schema.APIRequest.RequestType, requestType }
			};
			return Job.Create(
				SendPOSTRequest(
					EndPoints.RequestPending,
					payload,
					callback));
		}

		public static Job RequestRespond(string appId, string appVer, string deviceId, string deviceType, string sessionId,
			string playerId, string requestType, string signature, string action, Action<APIResponse> callback) {
			var payload = new Dictionary<string, object>() {
				{ June.Api.Schema.APIRequest.ApplicationId, appId },
				{ June.Api.Schema.APIRequest.ApplicationVersion, appVer },
				{ June.Api.Schema.APIRequest.APIVersion, API_VERSION },
				{ June.Api.Schema.APIRequest.DeviceId, deviceId ?? string.Empty },
				{ June.Api.Schema.APIRequest.DeviceType, deviceType },
				{ June.Api.Schema.APIRequest.PlayerId, playerId },
				{ June.Api.Schema.APIRequest.SessionId, sessionId },

				{ June.Api.Schema.APIRequest.RequestType, requestType },
				{ June.Api.Schema.APIRequest.Action, action }
			};
			return Job.Create(
				SendPOSTRequest(
					EndPoints.RequestRespond,
					payload,
					callback));
		}

		#endregion

		#region Friend

		public static Job FriendGetAll(string appId, string appVer, string deviceId, string deviceType, string playerId, string sessionId, Action<APIResponse> callback) {
			var payload = new Dictionary<string, object>() {
				{ June.Api.Schema.APIRequest.ApplicationId, appId },
				{ June.Api.Schema.APIRequest.ApplicationVersion, appVer },
				{ June.Api.Schema.APIRequest.APIVersion, API_VERSION },
				{ June.Api.Schema.APIRequest.DeviceId, deviceId ?? string.Empty },
				{ June.Api.Schema.APIRequest.DeviceType, deviceType },
				{ June.Api.Schema.APIRequest.PlayerId, playerId },
				{ June.Api.Schema.APIRequest.SessionId, sessionId }
			};
			return Job.Create(
				SendPOSTRequest(
					EndPoints.FriendGetAll,
					payload,
					callback));
		}

		public static Job FriendRemove(string appId, string appVer, string deviceId, string deviceType, string playerId, string sessionId, string friendId, Action<APIResponse> callback) {
			var payload = new Dictionary<string, object>() {
				{ June.Api.Schema.APIRequest.ApplicationId, appId },
				{ June.Api.Schema.APIRequest.ApplicationVersion, appVer },
				{ June.Api.Schema.APIRequest.APIVersion, API_VERSION },
				{ June.Api.Schema.APIRequest.DeviceId, deviceId ?? string.Empty },
				{ June.Api.Schema.APIRequest.DeviceType, deviceType },
				{ June.Api.Schema.APIRequest.PlayerId, playerId },
				{ June.Api.Schema.APIRequest.SessionId, sessionId },
				{ June.Api.Schema.APIRequest.FriendId, friendId }
			};
			return Job.Create(
				SendPOSTRequest(
					EndPoints.FriendRemove,
					payload,
					callback));
		}

		#endregion

		#region Leaderboard

		public static Job LeaderboardGlobal(string appId, string appVer, string deviceId, string playerId, int pageNo, int resultsPerPage, Action<APIResponse> callback) {
			var payload = new Dictionary<string, object>() {
				{ June.Api.Schema.APIRequest.ApplicationId, appId },
				{ June.Api.Schema.APIRequest.ApplicationVersion, appVer },
				{ June.Api.Schema.APIRequest.APIVersion, API_VERSION },
				{ June.Api.Schema.APIRequest.DeviceId, deviceId ?? string.Empty },
				{ June.Api.Schema.APIRequest.PlayerId, playerId },
				{ June.Api.Schema.APIRequest.PageNumber, pageNo },
				{ June.Api.Schema.APIRequest.RecordsPerPage, resultsPerPage }
			};
			return Job.Create(
				SendPOSTRequest(
					EndPoints.LeaderboardGlobal,
					payload,
					callback));			
		}

		public static Job LeaderboardDaily(string appId, string appVer, string deviceId, string playerId, int pageNo, int resultsPerPage, Action<APIResponse> callback) {
			var payload = new Dictionary<string, object>() {
				{ June.Api.Schema.APIRequest.ApplicationId, appId },
				{ June.Api.Schema.APIRequest.ApplicationVersion, appVer },
				{ June.Api.Schema.APIRequest.APIVersion, API_VERSION },
				{ June.Api.Schema.APIRequest.DeviceId, deviceId ?? string.Empty },
				{ June.Api.Schema.APIRequest.PlayerId, playerId },
				{ June.Api.Schema.APIRequest.PageNumber, pageNo },
				{ June.Api.Schema.APIRequest.RecordsPerPage, resultsPerPage }
			};
			return Job.Create(
				SendPOSTRequest(
					EndPoints.LeaderboardDaily,
					payload,
					callback));			
		}

		public static Job LeaderboardWeekly(string appId, string appVer, string deviceId, string playerId, int pageNo, int resultsPerPage, Action<APIResponse> callback) {
			var payload = new Dictionary<string, object>() {
				{ June.Api.Schema.APIRequest.ApplicationId, appId },
				{ June.Api.Schema.APIRequest.ApplicationVersion, appVer },
				{ June.Api.Schema.APIRequest.APIVersion, API_VERSION },
				{ June.Api.Schema.APIRequest.DeviceId, deviceId ?? string.Empty },
				{ June.Api.Schema.APIRequest.PlayerId, playerId },
				{ June.Api.Schema.APIRequest.PageNumber, pageNo },
				{ June.Api.Schema.APIRequest.RecordsPerPage, resultsPerPage }
			};
			return Job.Create(
				SendPOSTRequest(
					EndPoints.LeaderboardWeekly,
					payload,
					callback));			
		}

		public static Job LeaderboardFriends(string appId, string appVer, string deviceId, string deviceType, string playerId, string sessionId, int pageNo, int resultsPerPage, Action<APIResponse> callback) {
			var payload = new Dictionary<string, object>() {
				{ June.Api.Schema.APIRequest.ApplicationId, appId },
				{ June.Api.Schema.APIRequest.ApplicationVersion, appVer },
				{ June.Api.Schema.APIRequest.APIVersion, API_VERSION },
				{ June.Api.Schema.APIRequest.DeviceId, deviceId ?? string.Empty },
				{ June.Api.Schema.APIRequest.DeviceType, deviceType },
				{ June.Api.Schema.APIRequest.PlayerId, playerId },
				{ June.Api.Schema.APIRequest.SessionId, sessionId },
				{ June.Api.Schema.APIRequest.PageNumber, pageNo },
				{ June.Api.Schema.APIRequest.RecordsPerPage, resultsPerPage }
			};
			return Job.Create(
				SendPOSTRequest(
					EndPoints.LeaderboardFriends,
					payload,
					callback));			
		}

		public static Job LeaderboardUpdateStats(string appId, string appVer, string deviceId, string playerId, IDictionary<string,  object> gameData, Action<APIResponse> callback) {
			var payload = new Dictionary<string, object>() {
				{ June.Api.Schema.APIRequest.ApplicationId, appId },
				{ June.Api.Schema.APIRequest.ApplicationVersion, appVer },
				{ June.Api.Schema.APIRequest.APIVersion, API_VERSION },
				{ June.Api.Schema.APIRequest.DeviceId, deviceId ?? string.Empty },
				{ June.Api.Schema.APIRequest.PlayerId, playerId },
				{ "gd", gameData }
			};
			return Job.Create(
				SendPOSTRequest(
					EndPoints.LeaderboardUpdateStats,
					payload,
					callback));			
		}

		#endregion

		#endregion

		#region Util Methods
		
		/// <summary>
		/// Sends the POST request.
		/// </summary>
		/// <returns>The POST request.</returns>
		/// <param name="url">URL.</param>
		/// <param name="payload">Payload.</param>
		/// <param name="callback">Callback.</param>
		public static IEnumerator SendPOSTRequest(string url, IDictionary<string, object> payload, Action<APIResponse> callback) {
			return SendPOSTRequest(
				url, 
				GetPayloadFromDictionary(payload),
				responseStr => {
				if(null != callback) {
					callback(GetObjectFromResponse(responseStr));
				}
			});
		}
		
		/// <summary>
		/// Sends the POST request.
		/// </summary>
		/// <returns>The POST request.</returns>
		/// <param name="url">URL.</param>
		/// <param name="payload">Payload.</param>
		/// <param name="responseCallabck">Response callabck.</param>
		public static IEnumerator SendPOSTRequest(string url, string payload, Action<string> responseCallabck) {

			yield return new WaitForSeconds (0f);
			/*string encodedPayload = WWW.EscapeURL(payload);
			
			// Generate the hash-key for this payload
			var key = GetHashForPayload(encodedPayload);
			
			// Add the key and payload to the final data object to be sent across
			string jsonData = string.Format("{{ \"key\":\"{0}\", \"payload\":\"{1}\" }}", 
			                                key, encodedPayload);
			
			Logger.Log("[JuneAPI] POST Request - " + url + Environment.NewLine + jsonData);
			
			yield return Job.Create(Util.ExecutePostCommand(url, jsonData, 
			                                                www => {
				string responseStr = null != www
					&& true == www.isDone
						&& true == string.IsNullOrEmpty(www.error) 
						&& null != www.bytes 
						? System.Text.Encoding.UTF8.GetString(www.bytes)
						: null;
				string error = (null != www && www.isDone && false == string.IsNullOrEmpty(www.error)) 
					? www.error 
						: null;
				
				string headerResponse = (null != www
				                         && www.isDone && null != www.responseHeaders
				                         && www.responseHeaders.ContainsKey("X-ANDROID-RESPONSE-SOURCE"))
					? www.responseHeaders["X-ANDROID-RESPONSE-SOURCE"]
					: null;
				
				Logger.Log("[JuneAPI] POST ["+url+"] Response - " + responseStr + "HEADER RESPONSE "+headerResponse);
				
				// 400 - Bad Request, format is incorrect.
				// 401 - Unauthorized, key did not match.
				
				// 403 - Forbidden, session is invalid.					
				if(!string.IsNullOrEmpty(error) && error.Contains("403")) {
					//ChhotaBheemApi.RaiseSessionExpiredEvent();
				}
				
				// 409 - Conflict, player has logged in from another device.
				if (((!string.IsNullOrEmpty(headerResponse) && headerResponse.Contains("409")) || (!string.IsNullOrEmpty(error) && error.Contains("409")))
				    && true == PlayerProfile.StartSessionConflictCheck) {
					ChhotaBheemApi.RaiseOnSessionConflict();
				}
				
				// error ->
				//
				// * Could not resolve host (Could not contact DNS servers)
				//		Probably no connection to the internet
				//
				// * Resolving host timed out
				//		Connection is very poor
				//
				// * Could not resolve host (Domain name not found)
				//		The server trying to be contacted doesn't exist.
				//
				// * couldn't connect to host
				//		When the server takes very long to respond. It's probably down.
				
				if (!string.IsNullOrEmpty(error)) {
					Logger.Log("[JuneAPI] ERROR (" + url + ") " + error);
				}
				
				//Check if internet is there
				if(!string.IsNullOrEmpty(error)) {
					SERVER_NOT_REACHABLE_COUNT++;
					Util.IsInternetReachable = false;
					if(SERVER_NOT_REACHABLE_COUNT > 5) {
						//NinjumpAPI.RaiseServerUnavailable("The server is currently not reachable");
					}
				}
				else {
					Util.IsInternetReachable = true;
					SERVER_NOT_REACHABLE_COUNT = 0;
				}
				
				if(null != responseCallabck) {
					responseCallabck(responseStr);
				}
			}));*/
		}
		
		/// <summary>
		/// Gets the hash for payload.
		/// </summary>
		/// <returns>The hash for payload.</returns>
		/// <param name="encodedPayload">Encoded payload.</param>
		private static string GetHashForPayload(string encodedPayload) {
			return Util.SHA1Encode(string.Concat(encodedPayload, "&", SECRET));
		}
		
		/// <summary>
		/// Gets the payload from dictionary.
		/// </summary>
		/// <returns>The payload from dictionary.</returns>
		/// <param name="dictionary">Dictionary.</param>
		private static string GetPayloadFromDictionary(IDictionary<string, object> dictionary) {
			return SimpleJson.SimpleJson.SerializeObject(dictionary);
		}
		
		/// <summary>
		/// Gets the object from response.
		/// </summary>
		/// <returns>The object from response.</returns>
		/// <param name="response">Response.</param>
		private static APIResponse GetObjectFromResponse(string response) {
			APIResponse apiResponse = new APIResponse (isError: true, message: "Error in deserializing JSON");
			if (!string.IsNullOrEmpty (response)) {
				object obj;
				if (SimpleJson.SimpleJson.TryDeserializeObject (response, out obj) && obj is IDictionary<string, object>) {
					IDictionary<string, object> doc = obj as IDictionary<string, object>;
					apiResponse = new APIResponse (doc);
				}
			}
			
			return apiResponse;
		}
		
		/// <summary>
		/// Sends the GET request.
		/// </summary>
		/// <returns>The GET request.</returns>
		/// <param name="url">URL.</param>
		/// <param name="responseCallback">Response callback.</param>
		public static Job SendGETRequest(string url, Action<string> responseCallback) {
			Logger.Log("[JuneAPI] GET Request - " + url);
			return Job.Create(
				Util.ExecuteGetCommand(url,
					www => {
						string response = null;
						if(null != www && www.isDone) {
							Logger.Log("[JuneAPI] GET Response - " + url);
							response = (null != www.bytes ? System.Text.Encoding.UTF8.GetString(www.bytes) : null);
						}
						if(null != responseCallback) {
							responseCallback(response);
						}
					}));
		}
		
		/// <summary>
		/// Sends the GET request.
		/// </summary>
		/// <returns>The GET request.</returns>
		/// <param name="url">URL.</param>
		/// <param name="responseCallback">Response callback.</param>
		public static Job SendGETRequest(string url, Action<APIResponse> responseCallback) {
			return SendGETRequest(url,
			                      (string responseStr) => {
				if(null != responseCallback) {
					responseCallback(GetObjectFromResponse(responseStr));
				}
			});
		}
		
		/// <summary>
		/// Sends the GET request custom URL.
		/// </summary>
		/// <returns>The GET request custom URL.</returns>
		/// <param name="url">URL.</param>
		/// <param name="responseCallback">Response callback.</param>
		public static IEnumerator SendGETRequestCustomUrl(string url, Action<string> responseCallback) {
			url = Uri.EscapeUriString(url);
			Logger.Log("[JuneAPI] GET Request - " + url);
			yield return Job.Create(
				Util.ExecuteGetCommand(url,
					www => {
					string response = null;
					if(null != www && www.isDone) {
						Logger.Log("[JuneAPI] GET Response - " + url);
						response = (null != www.bytes ? System.Text.Encoding.UTF8.GetString(www.bytes) : null);
					}
					if(null != responseCallback) {
						responseCallback(response);
					}
				}));
		}
		#endregion

	}
}