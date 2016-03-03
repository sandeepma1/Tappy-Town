using System;
using System.Collections;
using System.Collections.Generic;
using June.Core;

namespace June.Api {
	/// <summary>
	/// API response.
	/// </summary>
	public class APIResponse : BaseModel {

		/// <summary>
		/// Gets a value indicating whether this instance is error.
		/// </summary>
		/// <value><c>true</c> if this instance is error; otherwise, <c>false</c>.</value>
		public bool IsError {
			get {
				return GetBool(Schema.APIResponse.Error);
			}
		}

		/// <summary>
		/// Gets the message.
		/// </summary>
		/// <value>The message.</value>
		public string Message {
			get {
				return GetString(Schema.APIResponse.Message);
			}
		}

		/// <summary>
		/// Gets the result.
		/// </summary>
		/// <value>The result.</value>
		public IDictionary<string, object> Result {
			get {
				return Get<IDictionary<string, object>>(Schema.APIResponse.Result);
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance's result is valid.
		/// </summary>
		/// <value><c>true</c> if this instance is result valid; otherwise, <c>false</c>.</value>
		public bool IsResultValid {
			get {
				return null != Result;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="APIResponse"/> class.
		/// </summary>
		/// <param name="doc">Document.</param>
		public APIResponse(IDictionary<string, object> doc) : base(doc) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="APIResponse"/> class.
		/// </summary>
		/// <param name="isError">If set to <c>true</c> is error.</param>
		/// <param name="message">Message.</param>
		public APIResponse(bool isError, string message) 
			: this(new Dictionary<string, object>() {
				{ Schema.APIResponse.Error, isError },
				{ Schema.APIResponse.Message, message }
			}) { }

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="APIResponse"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="APIResponse"/>.</returns>
		public override string ToString () {
			return string.Format ("[APIResponse: IsError={0}, Message={1}, Result={2}, IsResultValid={3}]", IsError, Message, Result, IsResultValid);
		}
	}
}