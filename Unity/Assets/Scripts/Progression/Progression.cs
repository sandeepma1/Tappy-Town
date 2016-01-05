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
		if (PlayerPrefs.GetInt ("Progression_DistanceCount") >= ObjectPlacer.m_instance.xpDistance) {
			LevelUp ();
		}
	}

	void LevelUp ()
	{
		PlayerPrefs.SetInt ("Progression_DistanceCount", 0);
		PlayerPrefs.SetInt ("PlayerXP", PlayerPrefs.GetInt ("PlayerXP") + 1);
		if (PlayerPrefs.GetInt ("PlayerXP") >= 50) {
			PlayerPrefs.SetInt ("PlayerXP", 50);
		}
	}
}
