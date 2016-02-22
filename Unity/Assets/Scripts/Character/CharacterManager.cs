using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterManager : BaseConfig<CharacterManager,Character>
{
	public override System.Func<IDictionary<string, object>, Character> ItemConverter {
		get {
			return (doc) => new Character (doc);
		}
	}

	/// <summary>
	/// Gets the name of the resource.
	/// </summary>
	/// <value>The name of the resource.</value>
	public override string ResourceName {
		get {
			return CharacterJSONFields.JSONFileName;
		}
	}

	/// <summary>
	/// Gets the root key.
	/// </summary>
	/// <value>The root key.</value>
	public override string RootKey {
		get {
			return CharacterJSONFields.RootKey;
		}
	}

	protected override void LoadItems ()
	{
		base.LoadItems ();
		Items.Sort ((i1, i2) => i1.SortId.CompareTo (i2.SortId));
	}

	public static List<Character> AllCharacters {		
		get {
			return Instance.Items;
		}
	}

	public static Character CurrentCharacterSelected {
		get { 
			string charName = PlayerPrefs.GetString ("currentCharacterSelectedID", "chr_mailman");			
			Character character = Util.FirstOrDefault (AllCharacters, c => c.PrefabName == charName);
			return character ?? DefaultCharacter;
		} 
	}

	public static Character GetCharacterWithId (string id)
	{
		return Util.FirstOrDefault (AllCharacters, c => c.Id == id);
	}

	/*public static Character GetCharacterWithPrefabna]= (string id)
	{
		return Util.FirstOrDefault (AllCharacters, c => c.Id == id);
	}*/

	public static Character DefaultCharacter {
		get {
			return GetCharacterWithId ("IGC-000");
		}
	}

	public static void ChangeCharacter (string characterPrefabName)
	{
		PlayerPrefs.SetString ("currentCharacterSelectedID", characterPrefabName);
	}
}

public class Character : BaseModel
{
	public string Id {
		get {
			return GetString (CharacterJSONFields.Id);
		}
	}

	public int SortId {
		get {
			return 	GetInt (CharacterJSONFields.SortId);
		}
	}

	private IDictionary<string,object> _EnvironmentDetails = null;

	public IDictionary<string,object> Environment {
		get {
			if (_EnvironmentDetails == null) {
				_EnvironmentDetails = Get<IDictionary<string,object>> (CharacterJSONFields.Environment);
			}
			return _EnvironmentDetails;
		}
	}

	public string WorldName {
		get {
			return Environment [CharacterJSONFields.EnvironmentFields.WorldName] as string;
		}
	}

	public bool IsNightModeOn {
		get {
			return (bool)Environment [CharacterJSONFields.EnvironmentFields.NightMode];
		}
	}

	/*public string StartItemPrefabName {
		get {
			return Environment [CharacterJSONFields.EnvironmentFields.StartItemPrefabName] as string;
		}
	}*/

	public string PrefabName {
		get {
			return GetString (CharacterJSONFields.PrefabName);
		}
	}

	public string Name {
		get {
			return GetString (CharacterJSONFields.Name);
		}
	}

	public string Description {
		get {
			return GetString (CharacterJSONFields.Description);
		}
	}

	private IDictionary<string,object> _Currency = null;

	public IDictionary<string,object> Currency { 
		get { 
			if (_Currency == null) { 
				_Currency = Get<IDictionary<string,object>> (CharacterJSONFields.Currency); 
			} 
			return _Currency; 
		} 
	}

	public string CurrencyType {
		get { 
			if (Currency.ContainsKey (CharacterJSONFields.CurrencyFields.CurrencyType))
				return Currency [CharacterJSONFields.CurrencyFields.CurrencyType] as string;
			return string.Empty;
		} 
	}

	public float CurrencyValue {
		get { 
			if (Currency.ContainsKey (CharacterJSONFields.CurrencyFields.Value))
				return float.Parse (Currency [CharacterJSONFields.CurrencyFields.Value].ToString ());
			return 0.0f;
		} 
	}

	public string CurrencyCollectibleId {
		get { 
			if (Currency.ContainsKey (CharacterJSONFields.CurrencyFields.CollectibleId) && CurrencyType.Equals ("collectibles"))
				return Currency [CharacterJSONFields.CurrencyFields.CollectibleId] as string;
			return string.Empty;
		} 
	}

	public Character (IDictionary<string,object> doc) : base (doc)
	{
	}
}

public class CharacterJSONFields
{
	public const string JSONFileName = "JSONs/Characters";
	public const string RootKey = "Characters";
	public const string Id = "id";
	public const string SortId = "si";
	public const string PrefabName = "pr";
	public const string Name = "nm";
	public const string Description = "ds";

	public const string Environment = "env";

	public class EnvironmentFields
	{
		public const string WorldName = "wn";
		public const string NightMode = "nm";
	}

	public const string Currency = "cr";

	public class CurrencyFields
	{
		public const string CurrencyType = "ct";
		public const string Value = "val";
		public const string CollectibleId = "id";
	}
}