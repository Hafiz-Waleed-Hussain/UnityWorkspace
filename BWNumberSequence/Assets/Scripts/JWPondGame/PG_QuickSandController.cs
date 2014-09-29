using UnityEngine;
using System.Collections;

public class PG_QuickSandController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		hideQuickSand();
	}
	
	// Update is called once per frame
	void Update () {}
	
	public void hideQuickSand(){
		transform.renderer.enabled = false;
	}
	
	public void showQuickSand(int _sand){
		transform.renderer.enabled = true;
		renderer.material.mainTexture = Resources.Load(PG_Constants._quickSand+string.Format("Sand0{0}",_sand)) as Texture2D;
	}
	
	
}
