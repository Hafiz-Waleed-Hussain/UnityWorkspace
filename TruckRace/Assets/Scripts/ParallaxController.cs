using UnityEngine;
using System.Collections;

public class ParallaxController : MonoBehaviour {

	public GameObject [] cloudLayer;
	public GameObject [] backgroundMountainLayer;
	public GameObject [] middleMountainLayer;
	public GameObject [] frontMountainLayer;

	public float cloudLayerSpeed;
	public float backgroundMountainLayerSpeed;
	public float middleMountainLayerSpeed;
	public float frontMountainLayerSpeed;

	public Camera camera;

	private Vector3 lastCameraPosition;

	void Start () {
		lastCameraPosition = camera.transform.position;
	}
	
	void Update () {
	
		Vector3 currentCameraPosition = camera.transform.position;
		float xCameraPositionDifference = lastCameraPosition.x - currentCameraPosition.x;

		adjustParallaxPositionForArray (cloudLayer,cloudLayerSpeed,xCameraPositionDifference);
		adjustParallaxPositionForArray (backgroundMountainLayer, backgroundMountainLayerSpeed, xCameraPositionDifference);
		adjustParallaxPositionForArray (middleMountainLayer, middleMountainLayerSpeed, xCameraPositionDifference);
		adjustParallaxPositionForArray (frontMountainLayer, frontMountainLayerSpeed, xCameraPositionDifference);
		lastCameraPosition = camera.transform.position;
	}

	void adjustParallaxPositionForArray(GameObject[] layerArray, float layerSpeed, float xCameraPositionDifference){

		for (int i=0; i <layerArray.Length; i++) {
			Vector3 objPosition = layerArray[i].transform.position;
			objPosition.x += xCameraPositionDifference * layerSpeed;
			layerArray[i].transform.position = objPosition;
		}
	}
}
