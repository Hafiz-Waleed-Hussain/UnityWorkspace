using UnityEngine;
using System.Collections;

public class DestroyerScript : MonoBehaviour
{

		public void OnTriggerEnter2D (Collider2D other)
		{

				if (other.tag == "Player") {
						Debug.Break ();
						return;
				}

				if (other.gameObject.transform.parent) {
						Destroy (other.gameObject.transform.parent.gameObject);		
				} else if (other.gameObject) {
						Destroy (other.gameObject);		
				}
			
		}
}
