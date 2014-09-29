using UnityEngine;
using System.Collections;

public class PG_LeafController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		animateLeaf();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void animateLeaf(){
		showLeaf();
		animateRandomly();
	}
	

   private void animateRandomly(){
		float _randX = Random.Range(-5,5) ;
		//float _randY =Random.Range(-1,1);
		float _speed = Random.Range(1,8);
		Vector3 newPosition = new Vector3(transform.position.x+_randX, transform.position.y, -5F);
		iTween.MoveTo(gameObject,iTween.Hash("speed",_speed,   "position",newPosition, "easyType",iTween.EaseType.linear, "oncomplete","animateRandomly"));
	}
	
	
	public void showLeaf(){
		transform.renderer.enabled = true;
		}
	
	public void hideLeaf(){
		transform.renderer.enabled = false;
	}
}
