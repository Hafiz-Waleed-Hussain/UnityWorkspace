/// <summary>
/// Camera follow.
/// this script use for control main camera to follow target(character)
/// </summary>

using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	
	[HideInInspector]
	public Transform target; //Target to follow
	public float angle = 15; //Angle camera
	public float distance = -4; //Distance target
	public float height = 2.5f; // Height camera
	
	
	//Private variable field
	private Vector3 posCamera;
	private Vector3 angleCam;
	private bool shake;
	public static CameraFollow instace;
	
	void Start(){
		instace = this;	
	}
	
	void LateUpdate(){
		if(target != null){
			if(target.position.z >= 0){
				if(shake == false){
					posCamera.x = Mathf.Lerp(posCamera.x, target.position.x, 5 * Time.deltaTime);
					posCamera.y = Mathf.Lerp(posCamera.y, target.position.y + height, 5 * Time.deltaTime);
					posCamera.z = Mathf.Lerp(posCamera.z, target.position.z + distance, GameAttribute.gameAttribute.speed); //* Time.deltaTime);
					transform.position = posCamera;
					angleCam.x = angle;
					angleCam.y = Mathf.Lerp(angleCam.y, 0, 1 * Time.deltaTime);
					angleCam.z = transform.eulerAngles.z;
					transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, angleCam, 1 * Time.deltaTime);
				}
			}else{
				if(PatternSystem.instance.loadingComplete == true){
					Vector3 dummy = Vector3.zero;
					posCamera.x = Mathf.Lerp(posCamera.x, 0, 5 * Time.deltaTime);
					posCamera.y = Mathf.Lerp(posCamera.y, dummy.y + height, 5 * Time.deltaTime);
					posCamera.z = dummy.z + distance;
					transform.position = posCamera;
					angleCam.x = angle;
					angleCam.y = transform.eulerAngles.y;
					angleCam.z = transform.eulerAngles.z;
					transform.eulerAngles = angleCam;
				}
			}
		}
	}
	
	
	//Reset camera when charater die
	public void Reset(){
		shake = false;
		Vector3 dummy = Vector3.zero;
		posCamera.x = 0;
		posCamera.y = dummy.y + height;
		posCamera.z = dummy.z + distance;
		transform.position = posCamera;
		angleCam.x = angle;
		angleCam.y = transform.eulerAngles.y;
		angleCam.z = transform.eulerAngles.z;
		transform.eulerAngles = angleCam;
	}
	
	//Shake camera
	public void ActiveShake(){
		shake = true;
		StartCoroutine(ShakeCamera());	
	}
	
	IEnumerator ShakeCamera(){
		float count = 0;
		Vector3 pos = Vector3.zero;;
		while(count <= 0.2f){
			count += 1 * Time.smoothDeltaTime;	
			pos.x = transform.position.x + Random.Range(-0.05f,0.05f);
			pos.y = target.position.y+ height;// + Random.Range(-0.05f,0.05f);
			pos.z = target.position.z + distance;
			transform.position = pos;
			yield return 0;
		}
		transform.position = posCamera;
		posCamera.x = transform.position.x;
		posCamera.y = target.position.y + height;
		posCamera.z = target.position.z + distance;
		transform.position = posCamera;
		angleCam.x = angle;
		angleCam.y = transform.eulerAngles.y;
		angleCam.z = transform.eulerAngles.z;
		transform.eulerAngles = angleCam;
		shake = false;
	}
	
}
