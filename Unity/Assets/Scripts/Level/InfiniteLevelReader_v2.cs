using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System.Reflection;

public class InfiniteLevelReader_v2 : MonoBehaviour
{
	public GameObject[] a, b, c, d, f, p, h, cs, lo;
	TextAsset worldData;
	string[] chars;
	string[] lines;
	int countOfBlank = 0;

	void Start ()
	{
		ReadGamePlayElementsFromWorldCSV ();
	}

	void ReadGamePlayElementsFromWorldCSV ()
	{
		worldData = Resources.Load ("Worlds/" + CharacterManager.CurrentCharacterSelected.WorldName) as TextAsset;
		lines = Regex.Split (worldData.text, "\n");
		for (int i = 0; i < lines.Length - 1; i++) {
			if (lines [i] != "") {
				chars = Regex.Split (lines [i], ",");
				switch (chars [0]) {
				case "a":
					FillArray (chars, ref a);
					break;
				case "b":					
					FillArray (chars, ref b);
					break;
				case "c":					
					FillArray (chars, ref c);
					break;
				case "d":					
					FillArray (chars, ref d);
					break;
				case "f":					
					FillArray (chars, ref f);
					break;
				case "p":					
					FillArray (chars, ref p);
					break;
				case "h":					
					FillArray (chars, ref h);
					break;
				case "cs":					
					FillArray (chars, ref cs);
					break;
				case "lo":					
					FillArray (chars, ref lo);
					break;
				default:
					break;
				}
			}
		}
	}

	void FillArray (string[] array, ref GameObject[] elements)
	{
		countOfBlank = 0;
		foreach (var s in array) {
			if (s != "") {
				countOfBlank++;
			}
		}
		elements = new GameObject[countOfBlank - 2];
		for (int x = 0; x < elements.Length; x++) {
			elements [x] = Resources.Load ("Prefabs/" + array [x + 1]) as GameObject;
		}
	}
	// ******************************************************************************End Reads the game play elements from CSV file provided.

}
