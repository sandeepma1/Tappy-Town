using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class DataMaster : MonoBehaviour
{
	DateTime currentDate;
	TimeSpan difference;

	void Start ()
	{
		InvokeRepeating ("CalculateTime", 1, 1);
	}

	void CalculateTime ()
	{
		currentDate = System.DateTime.Now;
		DateTime oldDate1 = DateTime.FromBinary (Convert.ToInt64 (June.LocalStore.Instance.GetString ("addedDate")));
		difference = oldDate1.Subtract (currentDate);
	}
}