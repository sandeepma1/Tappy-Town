using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class SaveStringArray
{
	public static void AddCharIDtoUnlock (string id)
	{
		string[] tempID = new string[June.LocalStore.Instance.GetStringArray ("unlockedCharacters").Length];
		tempID = June.LocalStore.Instance.GetStringArray ("unlockedCharacters");

		for (int i = 0; i < June.LocalStore.Instance.GetStringArray ("unlockedCharacters").Length; i++) {
			if (June.LocalStore.Instance.GetStringArray ("unlockedCharacters") [i] == "") {
				tempID [i] = id;				
				June.LocalStore.Instance.SetStringArray ("unlockedCharacters", tempID);
				//DisplayAllIds ();
				break;
			}
		}
	}

	/*public static void DisplayAllIds ()
	{
		for (int i = 0; i < June.LocalStore.Instance.GetStringArray ("unlockedCharacters").Length; i++) {
			UnityEngine.MonoBehaviour.print (June.LocalStore.Instance.GetStringArray ("unlockedCharacters") [i]);
		}
	}
*/
	public static bool CheckIfIDContains (string id)
	{
		for (int i = 0; i < June.LocalStore.Instance.GetStringArray ("unlockedCharacters").Length; i++) {
			if (June.LocalStore.Instance.GetStringArray ("unlockedCharacters") [i] == id) {
				return true;
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
