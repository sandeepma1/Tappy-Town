using UnityEngine;
using System.Collections;
using June;

public class StoreScreenScript : MonoBehaviour {


	StoreItem storeItem;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PurchaseButtonOnTap ()
	{

		Util.Log ("PurchaseButtonOnTap");


		storeItem = Store.GetStoreItemByIdentifier (GameEventManager.InAppProductIds.Package_1);

		if (Application.internetReachability == NetworkReachability.NotReachable) {
				Etcetera.ShowAlert ("Purchase", "Please check the internet connection and try again.", "Ok");
			return;
		}

		if (null != storeItem) {

			June.MessageBroker.Publish (June.Messages.PurchaseBuyTap);
			Store.Purchase (storeItem, PurchaseSuccessfull);
		} else {
				Etcetera.ShowAlert ("Purchase", "Please check the internet connection and try again.", "Ok");
				Etcetera.HideProgressDialog ();

		}
	}

	void PurchaseSuccessfull (PurchaseStatus status, string error)
	{
		Util.Log (" PurchaseSuccessfull Start ");

			Etcetera.HideProgressDialog ();
		if (status == PurchaseStatus.Success) {

			June.MessageBroker.Publish (June.Messages.PurchaseSuccessful);
			CoinCalculation.m_instance.AddCoins (storeItem.Quantity);

				Etcetera.ShowAlert ("Purchase", "Purchase was successful!", "Awesome");
		} else if (status == PurchaseStatus.NoAccount) {
				Etcetera.HideProgressDialog ();
				Etcetera.ShowAlert ("Purchase", "Cannot purchase. Please login to Google Play Account via Phone Settings!", "Ok");
		} else {  //Cancelled, Failure

			June.MessageBroker.Publish (June.Messages.PurchaseFailed);
				Etcetera.ShowAlert ("Purchase", "Purchase was unsuccessful!", "Ok", "", clicked => {
				if (clicked.Contains ("Ok")) {
				} 
			});
		}

		Util.Log ("PurchaseSuccessfull End ");

	}





}
