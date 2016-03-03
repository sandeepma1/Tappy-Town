using UnityEngine;
using System.Collections;
using June.Core;
using System.Collections.Generic;
using System;

namespace June.Api {
	
	/// <summary>
	/// Request.
	/// </summary>
	public partial class Request : BaseModel {

		/// <summary>
		/// Gets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
		public string Id {
			get {
				return GetString(Schema.Request.Id);
			}
		}

		/// <summary>
		/// Gets the sender identifier.
		/// </summary>
		/// <value>The sender identifier.</value>
		public string SenderId {
			get {
				return GetString(Schema.Request.SenderId);
			}
		}

		/// <summary>
		/// Gets the name of the sender.
		/// </summary>
		/// <value>The name of the sender.</value>
		public string SenderName {
			get {
				return GetString(Schema.Request.SenderName);
			}
		}

		/// <summary>
		/// Gets the receiver identifier.
		/// </summary>
		/// <value>The receiver identifier.</value>
		public string ReceiverId {
			get {
				return GetString(Schema.Request.ReceiverId);
			}
		}

		/// <summary>
		/// Gets the type.
		/// </summary>
		/// <value>The type.</value>
		public string Type {
			get {
				return GetString(Schema.Request.Type);
			}
		}

		/// <summary>
		/// Gets the sender avatar UR.
		/// </summary>
		/// <value>The sender avatar UR.</value>
		public string SenderAvatarURL {
			get {
				return GetString(Schema.Request.SenderAvatarURL);
			}
		}

		/// <summary>
		/// Gets the expiry timestamp.
		/// </summary>
		/// <value>The expiry timestamp.</value>
		public int ExpiryTimestamp {
			get {
				return GetInt(Schema.Request.ExpiryTimestamp);
			}
		}

		/// <summary>
		/// Gets the sender equipped document.
		/// </summary>
		/// <value>The sender equipped document.</value>
		public IDictionary<string, object> SenderEquippedDoc {
			get {
				return Get<IDictionary<string, object>>(Schema.Request.SenderEqupipped);
			}
		}

		/// <summary>
		/// Gets the data document.
		/// </summary>
		/// <value>The data document.</value>
		public IDictionary<string, object> DataDoc {
			get {
				return Get<IDictionary<string, object>>(Schema.Request.Data);
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance is push.
		/// </summary>
		/// <value><c>true</c> if this instance is push; otherwise, <c>false</c>.</value>
		public bool IsPush {
			get {
				return GetBool(Schema.Request.IsPush);
			}
		}

		protected PushMessage _PushMessage;
		/// <summary>
		/// Gets the push message.
		/// </summary>
		/// <value>The push message document.</value>
		public PushMessage PushMessage {
			get {
				if(null == _PushMessage) {
					_PushMessage = GetModel<PushMessage>(Schema.Request.PushMessage, doc => new PushMessage(doc));
				}
				return _PushMessage;
			}
		}

		/// <summary>
		/// Gets the signature.
		/// </summary>
		/// <value>The signature.</value>
		public string Signature {
			get {
				return GetString(Schema.Request.Signature);
			}
		}

		/// <summary>
		/// Responds to friend request.
		/// </summary>
		/// <param name="isAccepted">If set to <c>true</c> friend request is accepted.</param>
		/// <param name="callback">Callback.</param>
		public void RespondToFriendRequest(bool isAccepted, Action<bool> callback) {
//			_IsDismissed = true;
//
//			NinjumpAPI.UpdateFriendRequest(this.SenderUsername, isAccepted, status => {
//				if(status) {
//					GameManager.Instance.RequestManagerInstance.RemoveRequestBySignature(this.Signature);
//					if(null != NinjumpAPI.Friends && isAccepted) {
//						NinjumpAPI.Friends.AddFriend(this.SenderUsername, this.SenderName, this.SenderCurrentCharacter, this.SenderCurrentCharacterColor, false);
//					}
//				}
//
//				if(null != callback) {
//					callback(status);
//				}
//			});
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="June.Api.Request"/> class.
		/// </summary>
		/// <param name="doc">Document.</param>
		public Request(IDictionary<string, object> doc) : base(doc) { }
	}

	/// <summary>
	/// Push message.
	/// </summary>
	public partial class PushMessage : BaseModel {

		/// <summary>
		/// Gets the title.
		/// </summary>
		/// <value>The title.</value>
		public string Title {
			get {
				return GetString(Schema.Request.PushMessageFields.Title);
			}
		}

		/// <summary>
		/// Gets the text.
		/// </summary>
		/// <value>The text.</value>
		public string Text {
			get {
				return GetString(Schema.Request.PushMessageFields.Text);
			}
		}

		/// <summary>
		/// Gets the group key.
		/// </summary>
		/// <value>The group key.</value>
		public string GroupKey {
			get {
				return GetString(Schema.Request.PushMessageFields.GroupKey);
			}
		}

		/// <summary>
		/// Gets the group description.
		/// </summary>
		/// <value>The group description.</value>
		public string GroupDescription {
			get {
				return GetString(Schema.Request.PushMessageFields.GroupDescription);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="June.Api.PushMessage"/> class.
		/// </summary>
		/// <param name="doc">Document.</param>
		public PushMessage(IDictionary<string, object> doc) : base(doc) { }
	}
}