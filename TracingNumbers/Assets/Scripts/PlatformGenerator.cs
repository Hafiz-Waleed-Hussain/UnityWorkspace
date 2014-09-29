using UnityEngine;
using System.Collections.Generic;

public class PlatformGenerator : MonoBehaviour {

	public Transform tile;
//	private List<Transform> platform;
	void Start () {
	
//		platform = new List<Transform> ();
		for (int y = -1; y < 4; y++) {
						for (int x = 0; x < 5; x++) {
				tile.renderer.material.color = Color.blue;
				Instantiate (tile, new Vector3 (x+ x*.1f, y+y*.1f, 0), Quaternion.identity);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
