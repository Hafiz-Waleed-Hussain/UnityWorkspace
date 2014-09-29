using UnityEngine;
using System.Collections;
public class BWCloudsLayer : MonoBehaviour {
	
	public GameObject cloudPrefab;
	private ArrayList clouds = null;
	public Material [] materials;
	// Use this for initialization
	void Start () {
		clouds = new ArrayList(8);
		
		for(int i = 0; i<8; i++) {
			GameObject cloud = Instantiate (cloudPrefab) as GameObject;
			cloud.transform.parent = this.transform;
			//int mat = (int)(Random.value * (materials.Length - 1));
			
			cloud.renderer.material = materials[i];
			clouds.Add(cloud);
			cloud.transform.localPosition = new Vector3 (500 + Random.value*1000,Random.value*200,0);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(clouds != null && clouds.Count > 0) {
			foreach (GameObject cloud in clouds) {
				Vector3 newPos = cloud.transform.position;
				newPos.x -= Random.value*50.0f;
				cloud.transform.position = Vector3.Lerp(cloud.transform.position, newPos, Time.deltaTime);
			}
		}
	}
}
