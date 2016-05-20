using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UITextureScroll : MonoBehaviour
{
	//these variables will be visible in the Inspector
	public float xScrollSpeed;
	public float yScrollSpeed;
	public RawImage source;
	RawImage rawImage;
	public Texture[] imageBank;
	float uvSizeWidth, uvSizeHeight = 0;
	//these variables will hold the offset calculations
	private float xOffset;
	private float yOffset;
	public static UITextureScroll m_instance = null;

	void Start ()
	{
		m_instance = this;
		rawImage = source.gameObject.GetComponent<RawImage> ();
		uvSizeWidth = rawImage.uvRect.width;
		uvSizeHeight = rawImage.uvRect.height;
		rawImage.texture = (Texture)imageBank [0]; //default Image as cone image
	}

	public void ChangeScrollImage (int index)
	{
		rawImage.texture = (Texture)imageBank [index];
	}

	/*	void ScrollTexture (RawImage source)
	{
		rawImage = source.gameObject.GetComponent<RawImage> ();
		uvSizeWidth = rawImage.uvRect.width;
		uvSizeHeight = rawImage.uvRect.height;
	}*/

	void Update ()
	{
		xOffset = Time.time * xScrollSpeed;
		yOffset = Time.time * yScrollSpeed;	
		rawImage.uvRect = new Rect (xOffset, yOffset, uvSizeWidth, uvSizeHeight);
	}
}
