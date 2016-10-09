using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

public class Progression : MonoBehaviour
{
	public static Progression m_instance = null;

	void Awake ()
	{
		m_instance = this;
	}

	public	void UpdatePlayerXP ()
	{
		if (Bronz.LocalStore.Instance.GetInt ("Progression_DistanceCount") >= ObjectPlacer.m_instance.xpDistance) {
			LevelUp ();
		}
	}

	void LevelUp ()
	{
		Bronz.LocalStore.Instance.SetInt ("Progression_DistanceCount", 0);
		Bronz.LocalStore.Instance.SetInt ("PlayerXP", Bronz.LocalStore.Instance.GetInt ("PlayerXP") + 1);
		if (Bronz.LocalStore.Instance.GetInt ("PlayerXP") >= 50) {
			Bronz.LocalStore.Instance.SetInt ("PlayerXP", 50);
		}
	}
}
