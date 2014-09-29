using UnityEngine;
using System.Collections.Generic;

public class PolygonGenerator : MonoBehaviour {


	public List<Vector3> newVertices = new List<Vector3>();

	public List<int> newTriangles = new List<int>();

	public List<Vector2> newUV = new List<Vector2>();

	private Mesh mesh;

	void Start () {



		mesh = GetComponent<MeshFilter> ().mesh;

		float x = transform.position.x;
		float y = transform.position.y;
		float z = transform.position.z;

		newVertices.Add (new Vector3 (0, 0, 0));
		newVertices.Add (new Vector3 (1, 1, 0));
		newVertices.Add (new Vector3 (0, 1, 0));
//		newVertices.Add (new Vector3 (x, y-1, z));

		newTriangles.Add (0);
		newTriangles.Add (1);
		newTriangles.Add (2);
//		newTriangles.Add (1);
//		newTriangles.Add (2);
//		newTriangles.Add (3);

		mesh.Clear ();
		mesh.vertices = newVertices.ToArray ();
		mesh.triangles = newTriangles.ToArray ();
		mesh.Optimize ();
		mesh.RecalculateNormals ();

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
