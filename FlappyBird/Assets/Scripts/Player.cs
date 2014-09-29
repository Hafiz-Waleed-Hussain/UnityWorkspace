using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

		void Start ()
		{
	
		}
	
		void Update ()
		{
	
				Vector3 position = gameObject.transform.position;
						if (Input.GetKey (KeyCode.UpArrow)) {
								position.y += .1f;		
								gameObject.transform.position = new Vector3 (position.x, position.y, position.z);
						} else if (Input.GetKey (KeyCode.DownArrow)) {
								position.y -= .1f;		
								gameObject.transform.position = new Vector3 (position.x, position.y, position.z);

				}
		}

	void OnCollisionEnter2D(Collision2D info){

		if (info.gameObject.tag == "boundaries") {
			Debug.Log ("Enter");		
		}else if(info.gameObject.tag == "enemy"){
			FireEffect.Instance.Explosion(gameObject.transform.position);
			SoundEffect.Instance.makeExplosionSound();
			Destroy(gameObject);
		}

	}


	void OnDestroy()
	{
//		Application.LoadLevel ("Menu");
		transform.parent.gameObject.AddComponent<GameOverScript>();
	}

}
