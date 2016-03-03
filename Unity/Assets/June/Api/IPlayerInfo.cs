using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace June.Api {

	/// <summary>
	/// Interface for player info.
	/// </summary>
	public partial interface IPlayerInfo {

		/// <summary>
		/// Gets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
		string Id { get; }

		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name.</value>
		string Name { get; }

		/// <summary>
		/// Gets the facebook identifier.
		/// </summary>
		/// <value>The facebook identifier.</value>
		string FacebookId { get; }

		/// <summary>
		/// Gets the avatar UR.
		/// </summary>
		/// <value>The avatar UR.</value>
		string AvatarURL { get; }

		/// <summary>
		/// Gets the game data document.
		/// </summary>
		/// <value>The game data document.</value>
		IDictionary<string, object> GameDataDoc { get; }

		/// <summary>
		/// Gets the items equipped document.
		/// </summary>
		/// <value>The items equipped document.</value>
		IDictionary<string, object> ItemsEquippedDoc { get; }
	}
}