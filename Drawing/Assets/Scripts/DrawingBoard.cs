using UnityEngine;
using System.Collections;

public class DrawingBoard : MonoBehaviour {

	public Texture2D texture;

	void Awake(){

		Color[] colors = texture.GetPixels();
		Debug.Log (colors.Length);

		for(int i = 0 ; i < colors.Length; i++){

			if(i%2==0)
			colors[i] = Color.black;
		}

		texture.SetPixels (colors);
		texture.Apply ();
	}

	void Start () {


	}
	
	void Update () {
	
	}
}
