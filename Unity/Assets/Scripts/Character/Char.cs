using UnityEngine;
using System.Collections;

public class Char
{
	public int Id;
	public int SortId;
	public string PrefabName;
	public string Name;
	public string CharacterName;
	public string Description;
	public int UIBackgroundScrollID;
	public string WorldName;
	public bool NightMode;
	public CharCurrencyType CurrencyType;
	public int Value;
	public int CollectibleId;

	public Char (int charSortId, int charId, string charPrefabName, string charName, string charCharacterName, string charDescription, int charUIBackgroundScrollID, string charWorldName, 
	             bool charNightMode, CharCurrencyType charCurrencyType, int charValue, int charCollectibleId)
	{
		 
		SortId = charSortId;
		Id = charId;
		PrefabName = charPrefabName;
		Name = charName;
		CharacterName = charCharacterName;
		Description = charDescription;
		UIBackgroundScrollID = charUIBackgroundScrollID;
		WorldName = charWorldName;
		NightMode = charNightMode;
		CurrencyType = charCurrencyType;
		Value = charValue;
		CollectibleId = charCollectibleId;
	}

	public enum CharCurrencyType
	{
		Coins,
		Tokens,
		Collectibles
	}
}