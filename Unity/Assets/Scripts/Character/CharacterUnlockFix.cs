using UnityEngine;
using System.Collections;

public class CharacterUnlockFix : MonoBehaviour
{
	public GameObject charMenu;
	public static CharacterUnlockFix m_instance = null;
	// Use this for initialization
	void Start ()
	{
		m_instance = this;
	}

	public void FixIssue ()
	{
		charMenu.SetActive (false);
		charMenu.SetActive (true);
	}
}
