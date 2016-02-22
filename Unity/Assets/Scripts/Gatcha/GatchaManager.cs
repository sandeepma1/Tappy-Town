using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GatchaManager : BaseConfig<GatchaManager,Gatcha>
{
	public override System.Func<IDictionary<string, object>, Gatcha> ItemConverter {
		get {
			return (doc) => new Gatcha (doc);
		}
	}

	public override string ResourceName {
		get {
			return GatchaJSONFields.JSONFileName;
		}
	}

	public override string RootKey {
		get {
			return GatchaJSONFields.RootKey;
		}
	}

	public static List<Gatcha> AllGatchas {		
		get {
			return Instance.Items;
		}
	}

	public static Gatcha GetGatchaWithId (string id)
	{
		return Util.FirstOrDefault (AllGatchas, c => c.Id == id);
	}

}

public class Gatcha : BaseModel
{
	public string Id {
		get {
			return GetString (GatchaJSONFields.number);
		}
	}

	public string Number {
		get {
			return GetString (GatchaJSONFields.number) as string;
		}
	}

	public string ID {
		get {
			return GetString (GatchaJSONFields.ID);
		}
	}

	public string Quantity {
		get {
			return GetString (GatchaJSONFields.Quantity);
		}
	}

	public string Probability {
		get {
			return GetString (GatchaJSONFields.Probability);
		}
	}

	public Gatcha (IDictionary<string,object> doc) : base (doc)
	{
	}
}

public class GatchaJSONFields
{
	public const string JSONFileName = "JSONs/Gatcha";
	public const string RootKey = "Gatcha";
	public const string number = "no";
	public const string ID = "id";
	public const string Quantity = "qu";
	public const string Probability = "pr";
}     