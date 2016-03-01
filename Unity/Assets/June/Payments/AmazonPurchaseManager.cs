#if UNITY_AMAZON
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AmazonPurchaseManager : PurchaseManager
{
	public GameObject AmazonPurchaseManagerObject;
	public List<AmazonItem> m_ProductListAmazon;

	#region implemented abstract members of PurchaseManager
	/// <summary>
	/// Gets a value indicating whether this instance can make payments.
	/// </summary>
	/// <value>true</value>
	/// <c>false</c>
	public override bool CanMakePayments {
		get {
			return true;
		}
	}

	/// <summary>
	/// Gets the product fetched count.
	/// </summary>
	/// <value>The product fetched count.</value>
	public override int ProductFetchedCount {
		get {
			return (null != m_ProductListAmazon ? m_ProductListAmazon.Count : 0);
		}
	}



	/// <summary>
	/// Initializes this instance.
	/// </summary>
	protected override void Init () {
		AmazonPurchaseManagerObject = new GameObject("AmazonPurchaseManager", typeof(AmazonIAPManager));
		
		//m_sStatus = "Amazon";
		AmazonIAPManager.itemDataRequestFailedEvent += AmazonItemDataRequestFailed;
		AmazonIAPManager.itemDataRequestFinishedEvent += AmazonItemDataRequestSuccessful;
		AmazonIAPManager.purchaseFailedEvent += AmazonPurchaseFailed;
		AmazonIAPManager.purchaseSuccessfulEvent += AmazonPurchaseSuccessful;
		AmazonIAPManager.onGetUserIdResponseEvent += AmazonUserIdResponse;
		AmazonIAPManager.purchaseUpdatesRequestSuccessfulEvent += AmazonPurchaseUpdateSuccessful;
		AmazonIAPManager.purchaseUpdatesRequestFailedEvent += AmazonPurchaseUpdateFailed;

		AmazonIAP.initiatePurchaseUpdates();
		AmazonIAP.initiateItemDataRequest(ProductIdentifiers.ToArray());
		AmazonIAP.initiateGetUserIdRequest();
	}


	public void SetupPendingPurchases(){
		AmazonPurchaseUpdateSuccessful(null, new List<AmazonReceipt>());
	}

	/// <summary>
	/// Requests the product data from the respective stores.
	/// </summary>
	/// <param name="productIdentifiers">Product identifiers.</param>
	public override void RequestProductData (string[] productIdentifiers) {
		AmazonIAP.initiateItemDataRequest(productIdentifiers);
	}

	/// <summary>
	/// Purchases the product.
	/// </summary>
	/// <param name="productIdentifier">Product identifier.</param>
	/// <param name="callback">Callback.</param>
	/// 

	public override void PurchaseProduct (string productIdentifier, System.Action<PurchaseStatus,string> callback) {
		Util.Log ("[AmazonPurchaseManager] Purchasing - " + productIdentifier);
		_OnPurchaseCallback = callback;

		AmazonIAP.initiatePurchaseRequest(productIdentifier);
	}

	public override void PurchaseProduct (string productIdentifier, string developerPayload, Action<PurchaseStatus, string> callback)
	{
		PurchaseProduct(productIdentifier,callback);
	}


	/// <summary>
	/// Consumes the product.
	/// </summary>
	/// <param name="productIdentifier">Product identifier.</param>
	public override void ConsumeProduct (string productIdentifier) {
		//throw new System.NotImplementedException ();
	}

	/// <summary>
	/// Restores the purchases.
	/// </summary>
	/// <param name="itemCallback">Item callback.</param>
	/// <param name="restoreCallback">Restore callback.</param>
	public override void RestorePurchases (Action<PurchaseStatus> restoreCompleteCallback1, Action<PurchaseStatus, string, string> restoreItemCallback1) {
		//throw new System.NotImplementedException ();
		_OnRestoreCompleteCallback= restoreCompleteCallback1;
		_OnRestoreItemCallback = restoreItemCallback1;
	}

	#endregion

	#region Event Handlers
	/// <summary>
	/// Amazon Item Data Request Failed
	/// </summary>
	void AmazonItemDataRequestFailed () {
		// If you get this response, either retry or stop purchase attempt.
		Util.Log("[AmazonPurchaseManager] AmazonItemDataRequestFailed");
	}
	
	/// <summary>
	/// Amazons item data request successful.
	/// </summary>
	/// <param name="unavailableSkus">Unavailable skus.</param>
	/// <param name="availableItems">Available items.</param>
	void AmazonItemDataRequestSuccessful (List<string> unavailableSkus, List<AmazonItem> availableItems) {
		// Do not continue with the purchase attempt if the your SKU is in the list of unavailable SKUs!
		Util.Log("[AmazonPurchaseManager] AmazonItemDataRequestSuccessful. Unavailable skus: " +
		         (null != unavailableSkus ? unavailableSkus.Count : 0) + 
		         ", Avaiable skus: " + (null != availableItems ? availableItems.Count : 0));

		m_ProductListAmazon = availableItems;

		Store.UpdateStoreItems(availableItems);
	}
	/// <summary>
	/// Amazons purchase successful.
	/// </summary>
	/// <param name="receipt">Receipt.</param>
	void AmazonPurchaseSuccessful (AmazonReceipt receipt) {
		// On successful purchase, you may entitle content
		Util.Log("[AmazonPurchaseManager] AmazonPurchaseSuccessful - " + receipt.ToString());
		
		
		if(null != _OnPurchaseCallback ) {
			Util.Log(" ORIGINAL JSON JSON RECIEPT "+receipt.originalJson);
			string jsonString = string.Format("{{ \"userid\":\"{0}\", \"receipt\":{1} }}", LocalStorage.Instance.GetString(LocalStorageKeys.AMAZON_USER_ID), receipt.originalJson??"null");
			Util.Log("JSON RECIEPT "+jsonString);
			_OnPurchaseCallback(PurchaseStatus.Success,receipt.originalJson);//"SKIP_INAPP_VERIFICATION_PUHLEESE!!!");//
		}
	}
	/// <summary>
	/// Amazons purchase failed.
	/// </summary>
	/// <param name="reason">Reason.</param>
	void AmazonPurchaseFailed (string reason) {
		Util.Log("[AmazonPurchaseManager] AmazonPurchaseFailed - " + reason);
		LocalStorage.Instance.Increment(LocalStorageKeys.GEMS_PURCHASE_CANCEL_COUNT);
		if(null != _OnPurchaseCallback) {
			_OnPurchaseCallback(PurchaseStatus.Failure,reason);
		}
	}

	/// <summary>
	/// 
	/// 
	/// </summary>
	/// <param name="userId">UserId</param>
	void AmazonUserIdResponse (string userId)
	{
		LocalStorage.Instance.SetString(LocalStorageKeys.AMAZON_USER_ID,userId);
		Util.Log("[ AmazonUserIdResponse ]USER ID RESPONSE"+userId);
	}
	/// <summary>
	/// Amazons the purchase update successful.
	/// </summary>
	/// <param name="arg1">Arg1.</param>
	/// <param name="history">History.</param>
	void AmazonPurchaseUpdateSuccessful (List<string> arg1, List<AmazonReceipt> history)
	{
		List<AmazonReceipt> list =GetAllPendingReciepts();
		if(list != null && list.Count > 0)
			history.AddRange(list);
		Util.Log("[ AmazonPurchaseUpdateSuccessful ]history "+history.Count);
		foreach( AmazonReceipt reciept in history ){
			Util.Log("[ AmazonPurchaseUpdateSuccessful ]RESPONSE"+reciept.originalJson);
			EquipPurchase(reciept);
		}
		
	}
	/// <summary>
	/// Amazons the purchase update failed.
	/// </summary>
	void AmazonPurchaseUpdateFailed ()
	{
		Util.Log("[ AmazonPurchaseUpdateFailed ]USER ID RESPONSE");
	}
	#endregion
	#region Pending purchases helper methods
	/// <summary>
	/// Equips the purchase.
	/// </summary>
	/// <param name="productidentifier">Productidentifier.</param>
	/// <param name="product">Product.</param>
	public void EquipPurchase(AmazonReceipt product) {
		if(null == product) {
			UnityEngine.Debug.Log ("[AmazonPurchaseManager] EquipPurchase - NULL");
			return;
		}
		
		UnityEngine.Debug.Log ("[AmazonPurchaseManager] EquipPurchase - " + product.ToString());
		StoreItem item = Store.GetStoreItemByIdentifier(product.sku);
		UnityEngine.Debug.Log ("[AmazonPurchaseManager] EquipPurchase StoreItem - " + (null != item ? item.ToString() : "<null>"));
		if(null != item && null != product ) {
			UnityEngine.Debug.Log ("[AmazonPurchaseManager] EquipPurchase MakePurchase");
			NinjumpAPI.Purchase(
				item.Id,
				product.sku,
				item.CurrencyCode,
				Convert.ToSingle(item.CurrentPrice),
				product.originalJson,//"SKIP_INAPP_VERIFICATION_PUHLEESE!!!",//
				item.Type == StoreItemType.Offer,
				null,
				(items, cards) => {
				UnityEngine.Debug.Log("[AmazonPurchaseManager] EquipPurchase Success: " + string.Join(",", items));
				RemovePendingReciept(product);
				NinjumpAPI.GetPlayer(null);
			},
			(error) => {
				UnityEngine.Debug.Log("[AmazonPurchaseManager] EquipPurchase ERROR: " + error);
				SetFailedEquipReciepts(product);
			},
			(items) => {
				UnityEngine.Debug.Log("[AmazonPurchaseManager] EquipPurchase Partial Success: " + string.Join(",", items));
			}
			);
		}	
	}
	private const string FAILED_PURCHASED_KEYS = "Failed_Purchased_key";
	/// <summary>
	/// Sets the failed equip reciepts.
	/// </summary>
	/// <param name="reciept">Reciept.</param>
	public void SetFailedEquipReciepts(AmazonReceipt reciept){
		List<AmazonReceipt> receipts =  GetAllPendingReciepts();
		if(receipts == null)
			receipts = new List<AmazonReceipt>();
		bool isFound = false;
		for(int i =0 ;i<receipts.Count;i++){
			if(receipts[i].token == reciept.token){
				return;
			}
		}
		receipts.Add(reciept);
		Hashtable tab = new Hashtable();
		tab.Add("receipts",AmazonReceipt.toListOfHashtable(receipts));
		
		LocalStorage.Instance.SetString(FAILED_PURCHASED_KEYS,tab.toJson());
	}
	/// <summary>
	/// Gets all pending reciepts.
	/// </summary>
	/// <returns>The all pending reciepts.</returns>
	public List<AmazonReceipt> GetAllPendingReciepts(){
		try{
			Util.Log (" [PENDING RECEIPTS ] GetAllPendingReciepts "+LocalStorage.Instance.GetString(FAILED_PURCHASED_KEYS));
			if(!String.IsNullOrEmpty( LocalStorage.Instance.GetString(FAILED_PURCHASED_KEYS))){
				return  AmazonReceipt.fromArrayList(LocalStorage.Instance.GetString(FAILED_PURCHASED_KEYS).hashtableFromJson()["receipts"] as ArrayList);
			}
		}catch(Exception ex){
			Util.Log(ex.StackTrace.ToString());
		}
		return null;
		
	}
	/// <summary>
	/// Removes the pending reciept.
	/// </summary>
	/// <param name="receipt">Receipt.</param>
	public void RemovePendingReciept(AmazonReceipt receipt){
		List<AmazonReceipt> receipts =  GetAllPendingReciepts();
		if(receipts == null)
			return;
		bool isFound = false;
		int i = 0;
		for(i =0 ;i<receipts.Count;i++){
			if(receipts[i].token == receipt.token){
				break;
			}
		}
		receipts.RemoveAt(i);
		Hashtable tab = new Hashtable();
		tab.Add("receipts",AmazonReceipt.toListOfHashtable(receipts));
		LocalStorage.Instance.SetString(FAILED_PURCHASED_KEYS,tab.toJson());
		
	}
	#endregion
}
#endif
