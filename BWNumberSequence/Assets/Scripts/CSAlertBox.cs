using UnityEngine;
using System.Collections;

public class CSAlertBox : MonoBehaviour {
	
	public GameObject storeImage = null;
	
	void OnDisable () {
		FingerGestures.OnFingerTap -= FingerGestures_OnFingerTap;
	}
	
	void Start () {
		FingerGestures.OnFingerTap += FingerGestures_OnFingerTap;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void FingerGestures_OnFingerTap( int fingerIndex, Vector2 fingerPos, int tapCount )
    {
		GameObject selection = PickObject(fingerPos);
		//Debug.Log(selection.name);
		if(selection == null) return;
		
		if(selection == this.gameObject) {
			Destroy(gameObject);
		}
    }
	
	private GameObject PickObject( Vector2 screenPos )
    {
        Ray ray = Camera.main.ScreenPointToRay( screenPos );
        RaycastHit hit;

        if( Physics.Raycast( ray, out hit ) )
            return hit.collider.gameObject;

        return null;
    }
	
	public void setAlertWithImage (string imageName) {
		string fullName = string.Format("ComingSoonImages/Sprites/{0}", imageName);
		Texture2D imgText = (Texture2D) Resources.Load(fullName);
		storeImage.renderer.material.mainTexture = imgText;
	}
}
