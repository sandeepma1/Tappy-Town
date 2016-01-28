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
		if (June.LocalStore.Instance.GetInt ("Progression_DistanceCount") >= ObjectPlacer.m_instance.xpDistance) {
			LevelUp ();
		}
	}

	void LevelUp ()
	{
		June.LocalStore.Instance.SetInt ("Progression_DistanceCount", 0);
		June.LocalStore.Instance.SetInt ("PlayerXP", June.LocalStore.Instance.GetInt ("PlayerXP") + 1);
		if (June.LocalStore.Instance.GetInt ("PlayerXP") >= 50) {
			June.LocalStore.Instance.SetInt ("PlayerXP", 50);
		}
	}
}
