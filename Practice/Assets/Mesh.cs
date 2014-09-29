using UnityEngine;
using System.Collections.Generic;

public class Mesh : MonoBehaviour {

	private List<Vector3> vertices = new List<Vector3>();
	private List<int> triangles = new List<int>();

	private MeshFilter meshFilter;

	void Start () {

		meshFilter = GetComponent<MeshFilter> ();

		float x = transform.position.x;
		float y = transform.position.y;
		float z = transform.position.z;

		vertices.Add (new Vector3 (x, y, z));
		vertices.Add (new Vector3 (x, y+2, z));
		vertices.Add (new Vector3 (x-2, y+2, z));


		triangles.Add (0);
		triangles.Add (1);
		triangles.Add (2);


		UnityEngine.Mesh mesh = new UnityEngine.Mesh ();
		meshFilter.mesh = mesh;

		mesh.vertices = vertices.ToArray();
		mesh.triangles = triangles.ToArray();


	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
