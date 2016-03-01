using UnityEngine;
using System.Collections;

namespace June.Payments {
	public class DummyPurchaseManager : PurchaseManager {
		#region implemented abstract members of PurchaseManager

		protected override void Init () {
			Util.Log("[DummyPurchaseManager] Int");
		}

		public override void RequestProductData (string[] productIdentifiers) {
			Util.Log("[DummyPurchaseManager] RequestProductData");
		}

		public override void PurchaseProduct (string productIdentifier, System.Action<PurchaseStatus, string> callback) {
			Util.Log("[DummyPurchaseManager] PurchaseProduct");
			callback (PurchaseStatus.Success, null);
		}

		public override void PurchaseProduct (string productIdentifier, string developerPayload, System.Action<PurchaseStatus, string> callback) {
			Util.Log("[DummyPurchaseManager] PurchaseProduct");
			callback (PurchaseStatus.Success, null);
		}

		public override void ConsumeProduct (string productIdentifier) {
			Util.Log("[DummyPurchaseManager] ConsumeProduct");
		}

		public override void RestorePurchases (System.Action<PurchaseStatus> restoreCompleteCallback, System.Action<PurchaseStatus, string, string> restoreItemCallback) {
			Util.Log("[DummyPurchaseManager] RestorePurchases");
		}

		public override bool CanMakePayments {
			get {
				return true;
			}
		}

		public override int ProductFetchedCount {
			get {
				return 0;
			}
		}

		#endregion
	}
}
