using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class FileReader : MonoBehaviour {

	// Use this for initialization
	void Start () {
		string path = (Application.dataPath  + "/" + "myfile.txt");
		string text = System.IO.File.ReadAllText(path);
		string[] stringvalues = text.Split(',');

		List<string> doublevalues = new List<string>();
		foreach(string value in stringvalues)
		{
			doublevalues.Add(value);
		}
		foreach (var word in doublevalues) {
			print(word);
		}
	}
}
