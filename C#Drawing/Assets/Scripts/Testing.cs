using UnityEngine;
using System.Collections;

public class Testing : MonoBehaviour {

	


//	private Texture2D mTexture2D;

//	void Start () {
	

//		mTexture2D = Instantiate(renderer.material.mainTexture) as Texture2D;
//		transform.localScale = new Vector3 (Screen.width,Screen.width,1);
//		renderer.material.mainTexture = mTexture2D;

//		mPen = new Pen (System.Drawing.Color.Black);
//	}


	public Material mat;
	private Vector3 startVertex;
	private Vector3 mousePos;
	void Update() {
		mousePos = Input.mousePosition;
		Debug.Log (mousePos);
		if (Input.GetKeyDown(KeyCode.Space))
			startVertex = new Vector3(mousePos.x / Screen.width, mousePos.y / Screen.height, 0);
		
	}
	void OnPostRender() {
		if (!mat) {
			Debug.LogError("Please Assign a material on the inspector");
			return;
		}
		Debug.Log ("Call");
		GL.PushMatrix();
		mat.SetPass(0);
		GL.LoadOrtho();
		GL.Begin(GL.LINES);
		GL.Color(Color.red);

		GL.Vertex(startVertex);
		GL.Vertex(new Vector3(mousePos.x / Screen.width, mousePos.y / Screen.height, 0));
		GL.End();
		GL.PopMatrix();
	}
	void Example() {
		startVertex = new Vector3(0, 0, 0);
	}
}
