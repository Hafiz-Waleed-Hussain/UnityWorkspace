using UnityEngine;
using System.Collections;

public class SizeScript : MonoBehaviour {

	public float speed = 1;
	public Light light;
	private Color color;
	private Vector3 newPosition;

	void Awake(){
		float quadHeight = Camera.main.orthographicSize * 2;
		float quadWidth = Camera.main.aspect * quadHeight;
		color = light.color;
//		transform.localScale = new Vector3 (quadWidth, quadHeight, 0);

	}

	void Update(){

		light.color = Color.Lerp(light.color, color, Time.deltaTime * speed);
		newPosition.z = -2;
		light.transform.position = Vector3.Lerp (light.transform.position, newPosition, Time.deltaTime * speed);

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast(ray,out hit,1000)){

			Debug.DrawLine(ray.origin, hit.point);
					if (Input.GetButton ("Fire1")) {
					
				if( hit.collider.name == "white"){
					color=Color.white;
					newPosition = hit.collider.transform.position;
				}
				else if( hit.collider.name == "red"){
					color=Color.red;
					newPosition = hit.collider.transform.position;
				}
				else if( hit.collider.name == "green"){
					color=Color.green;
					newPosition = hit.collider.transform.position;
				}
					else if( hit.collider.name == "blue"){
					color=Color.blue;
					newPosition = hit.collider.transform.position;
				}
					
				}

		}
	}

}
