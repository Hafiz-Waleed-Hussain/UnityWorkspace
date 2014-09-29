#region AGNITUS 2013
/* JungleWorld- Naming Fruits Game
 * Developer- Asema Hassan
 * Unity3D*/
#endregion
using UnityEngine;
using System.Collections;

public class NF_AnimateRope : MonoBehaviour {

public float speed;
Vector3 target1;
Vector3 target2;
private float direction=1.0f;
private float currentPosition=0.0f;
// Use this for initialization
void Start () {
	
	target1 = new Vector3(this.transform.position.x+40,this.transform.position.y,this.transform.position.z);
	target2 = new Vector3(this.transform.position.x-80,this.transform.position.y,this.transform.position.z);
}

 void Update (){
    currentPosition = Mathf.Clamp01(currentPosition + speed * Time.deltaTime * direction);
    if(direction == 1.0f && currentPosition > 0.99f)
		direction=-1.0f;
    if(direction ==-1.0f && currentPosition < 0.01f) 
		direction= 1.0f;
    transform.position = Vector3.Lerp(target1, target2, currentPosition);

	}
}
