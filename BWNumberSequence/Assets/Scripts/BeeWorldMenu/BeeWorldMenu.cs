using UnityEngine;
using System.Collections;

public class BeeWorldMenu : MonoBehaviour {
	
	void OnDisable () {
		FingerGestures.OnFingerTap -= FingerGestures_OnFingerTap;
	}
	// Use this for initialization
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
		
		if(selection.name.StartsWith("Icon")) {
			BeeWorldMenuIcon icon = selection.GetComponent<BeeWorldMenuIcon>();
			AGGameState.startGame(icon.gameIndex);
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
	
	// Convert from screen-space coordinates to world-space coordinates on the Z = 0 plane
    private Vector3 GetWorldPos( Vector2 screenPos )
    {
        Ray ray = Camera.main.ScreenPointToRay( screenPos );

        // we solve for intersection with z = 0 plane
        float t = -ray.origin.z / ray.direction.z;

        return ray.GetPoint( t );
    }
}
