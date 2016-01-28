using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SampleCharacter : MonoBehaviour
{
	public GameObject charMesh;
	public string charName;
	public string charDesc;
	public string charID;
	public string currType;
	public string currValue;
	public string currID;
	public bool isUnlocked;
	public bool isSelected;

	public SampleCharacter (GameObject mesh, string name, string desc, string id, string cType, string cValue, string cID, bool isUnlck, bool isSelect)
	{
		charMesh = mesh;
		charName = name;
		charDesc = desc;
		charID = id;
		currType = cType;
		currValue = cValue;
		currID = cID;
		isUnlocked = isUnlck;
		isSelected = isSelect;
	}
}