using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class SaveStringArray
{
	public static void AddCharIDtoUnlock (string id)
	{
		string[] tempID = new string[June.LocalStore.Instance.GetStringArray ("unlockedCharacters").Length];
		tempID = June.LocalStore.Instance.GetStringArray ("unlockedCharacters");
		if (!IsCharacterUnlocked (id)) {
			for (int i = 0; i < June.LocalStore.Instance.GetStringArray ("unlockedCharacters").Length; i++) {
				if (June.LocalStore.Instance.GetStringArray ("unlockedCharacters") [i] == "") {
					tempID [i] = id;				
					June.LocalStore.Instance.SetStringArray ("unlockedCharacters", tempID);
					break;
				}
			}
		}
	}

	public static bool IsCharacterUnlocked (string id)
	{
		for (int i = 0; i < June.LocalStore.Instance.GetStringArray ("unlockedCharacters").Length; i++) {
			if (June.LocalStore.Instance.GetStringArray ("unlockedCharacters") [i] == id) {
				return true;
			}
		}
		return false;
	}

	public static void AddCharTokenIDtoUnlock (string id)
	{	
		if (!isCharacterTokenFull (id)) {
			June.LocalStore.Instance.SetInt (id, June.LocalStore.Instance.GetInt (id) + 1);
		}
	}

	public static string GetCharacterTokenCount (string id)
	{				
		return June.LocalStore.Instance.GetInt (id).ToString ();	
	}

	public static bool isCharacterTokenFull (string id)
	{		
		for (int i = 0; i < CharacterManager.AllCharacters.Count; i++) {
			if (CharacterManager.AllCharacters [i].CurrencyCollectibleId == id) {
//				UnityEngine.MonoBehaviour.print (June.LocalStore.Instance.GetInt (id) + " " + CharacterManager.AllCharacters [i].CurrencyValue);
				if (June.LocalStore.Instance.GetInt (id) >= CharacterManager.AllCharacters [i].CurrencyValue) {
					return true;
				}
			}
		}
		return false;
	}

	public static bool CheckIfIsSelected (string id)
	{		
		if (CharacterManager.CurrentCharacterSelected.Id == id) {
			return true;
		}
		return false;
	}


}
