using UnityEngine;
using System.Collections;

public class MAAnimal : MonoBehaviour {
	
	public GameObject animalBody;
	public GameObject animalEyes;
	public GameObject particles = null;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void setAnimal(Hashtable animalData) {
		string name = (string) animalData["name"];
		float scaleX = MAConstants.GetAsFloat(animalData["x"]);
		float scaleY = MAConstants.GetAsFloat(animalData["y"]);
		
		scaleX = 325F * 0.7f;
		scaleY = 250F * 0.7f;
		
		gameObject.name = name;
		
		string imageName = string.Format("JWMemoryAnimals/Sprites/{0}", name);
		animalBody.renderer.material.mainTexture = (Texture2D)Resources.Load(imageName);
		animalBody.transform.localScale = new Vector3(scaleX, scaleY, 1);
		
		onlyBody();
	}
	
	public void onlyEyes () {
		animalBody.SetActive(false);
		animalEyes.SetActive(true);
		particles.SetActive(false);
	}
	
	public void onlyBody () {
		animalBody.SetActive(true);
		animalEyes.SetActive(false);
		particles.SetActive(true);
	}
	
	public void yesAnimation() {
		animalBody.SetActive(true);
		animalEyes.SetActive(false);
		particles.SetActive(true);
		//iTween.MoveTo(gameObject, iTween.Hash("position", transform.position, "time", 1.0, "onComplete", "yesAnimationFinished"));
	}
	
	public void yesAnimationFinished() {
		Destroy(gameObject);
	}
	
	public void noAnimation() {
		animalBody.SetActive(true);
		animalEyes.SetActive(false);
		particles.SetActive(false);
		//iTween.MoveTo(gameObject, iTween.Hash("position", transform.position, "time", 0.5, "onComplete", "noAnimationFinished"));
	}
	
	public void noAnimationFinished() {
		onlyEyes();
	}
}
