using UnityEngine;
using System.Collections;

public class TextureScroll : MonoBehaviour
{
		//these variables will be visible in the Inspector
		public float xScrollSpeed;
		public float yScrollSpeed;
		public Renderer rend;
		//these variables will hold the offset calculations
		private float xOffset;
		private float yOffset;
		void Start ()
		{
				rend = GetComponent<Renderer> ();
				rend.enabled = true;
		}
		// Update is called once per frame
		void Update ()
		{
				//this section calculates the speed and applies it the 
				//offset of the material every frame
				xOffset = Time.time * xScrollSpeed;
				yOffset = Time.time * yScrollSpeed;
				rend.material.mainTextureOffset = new Vector2 (xOffset, yOffset);
		}
}
