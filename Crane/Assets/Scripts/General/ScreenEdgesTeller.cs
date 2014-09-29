using UnityEngine;
using System.Collections;

public class ScreenEdgesTeller : MonoBehaviour {


	public enum ScreenEdges {LEFT, RIGHT, TOP, BOTTOM};
	public ScreenEdges screenEdges;
	public float xOffset ;
	public float yOffset ;
	

	void Start () {

		float xPosition = 0f;
		float yPosition = 0f;

		Camera camera = Camera.main;

		switch (screenEdges) {
		
		case ScreenEdges.RIGHT:
			xPosition = camera.orthographicSize * camera.aspect + xOffset; 
			yPosition = yOffset;
			break;
		case ScreenEdges.TOP:
			xPosition = xOffset;
			yPosition = camera.orthographicSize+yOffset;
			break;

		case ScreenEdges.BOTTOM:
			break;
		case ScreenEdges.LEFT:
			break;
		}

		Vector3 position = gameObject.transform.position;
		gameObject.transform.position = new Vector3 (xPosition, yPosition, position.z); 
	}

	void Update(){
		Start ();
	}
}
