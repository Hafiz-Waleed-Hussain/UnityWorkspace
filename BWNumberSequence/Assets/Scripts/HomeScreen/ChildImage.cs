#define MAC_PLATFORM

using UnityEngine;
using System.Collections;

public class ChildImage : MonoBehaviour {
	
	public GameObject childImage = null;
	public TextMesh starCount = null;
	public TextMesh childName = null;
	public Texture2D defaultChildImage;
	// Use this for initialization
	void Start () {
#if (!MAC_PLATFORM)
		updateChild();
#endif
	}
	
	// Update is called once per frame
	void Update () {
		starCount.text = string.Format("{0}", AGGameState.starCount);
	}
	
	void updateChild () {
		Texture2D _childTexture = new Texture2D(1024,768);
		byte [] data = AGGameState.getChildImage();
		
		if(data != null && data.Length > 0) {
			_childTexture.LoadImage(data);
		} else {
			_childTexture = defaultChildImage;
		}
		
		childImage.renderer.material.mainTexture = _childTexture;
		
		starCount.text = string.Format("{0}", AGGameState.getStarCount());
		string chName = AGGameState.getChildName();
		
		if(chName == null || chName.Equals("")) {
			chName = "Child";
		}
		childName.text = chName;
		
	}
}
