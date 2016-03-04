using UnityEngine;
using System.Collections;
using System.Reflection;

public class tteest : MonoBehaviour
{
	public string test = "a";
	public int a = 1, b = 10;
	public int newVar = 0;

	// Use this for initialization
	void Start ()
	{	

		//print (test.Substring (0, test.Length - 2));
		//print (test.Substring (test.Length - 2));
		/*newVar = (int)this.GetType ().GetField ("test").GetValue (this);
		print (newVar + b);*/
	}
	

}
