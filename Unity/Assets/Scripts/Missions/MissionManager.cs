using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using June;
using June.Core;

public class MissionManager : BaseConfig<MissionManager,Mission>
{
	public override System.Converter<IDictionary<string, object>, Mission> ItemConverter {
		get {
			return (doc) => new Mission (doc);
		}
	}

	/// <summary>
	/// Gets the name of the resource.
	/// </summary>
	/// <value>The name of the resource.</value>
	public override string ResourceName {
		get {
			return MissionJSONFields.JSONFileName;
		}
	}

	/// <summary>
	/// Gets the root key.
	/// </summary>
	/// <value>The root key.</value>
	public override string RootKey {
		get {
			return MissionJSONFields.RootKey;
		}
	}

	protected override void LoadItems ()
	{
		base.LoadItems ();
		Items.Sort ((i1, i2) => i1.SortId.CompareTo (i2.SortId));
	}

	public static List<Mission> AllMissions {		
		get {
			return Instance.Items;
		}
	}
}

public class Mission : BaseModel
{
	public int SortId {
		get {
			return 	GetInt (MissionJSONFields.SortId);
		}
	}

	public string Description {
		get {
			return GetString (MissionJSONFields.Description);
		}
	}

	public int Reward {
		get {
			return GetInt (MissionJSONFields.Reward);
		}
	}

	public bool SingleRun {
		get {
			return GetBool (MissionJSONFields.SingleRun);
		}
	}

	public int Value {
		get {
			return GetInt (MissionJSONFields.Value);
		}
	}

	public string PlayerPrefs {
		get {
			return GetString (MissionJSONFields.PlayerPrefs);
		}
	}

	public string RewardType {
		get {
			return GetString (MissionJSONFields.RewardType);
		}
	}

	public Mission (IDictionary<string,object> doc) : base (doc)
	{
	}
}

public class MissionJSONFields
{
	public const string JSONFileName = "JSONs/Missions";
	public const string RootKey = "Missions";
	public const string SortId = "si";
	public const string Description = "ds";
	public const string Reward = "rw";
	public const string SingleRun = "sr";
	public const string Value = "va";
	public const string PlayerPrefs = "pp";
	public const string RewardType = "rt";
}