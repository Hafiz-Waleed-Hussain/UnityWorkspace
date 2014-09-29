#region AGNITUS 2013
/* JungleWorld- Naming Fruits Game
 * Developer- Asema Hassan
 * Unity3D*/
#endregion

using UnityEngine;
using System.Collections;

public class NF_DragTrail : MonoBehaviour 
{
public TrailRenderer trailRendererPrefab;
TrailRenderer trailRenderer;
Ray ray;
RaycastHit hit;
public int dragFingerIndex = -1;
// Use this for initialization
void Start(){
	trailRenderer = Instantiate(trailRendererPrefab, transform.position, transform.rotation ) as TrailRenderer;
	trailRenderer.transform.parent = this.transform;
	trailRenderer.enabled = false;
}

void Awake(){
}

void OnEnable () {
	FingerGestures.OnFingerDragBegin += FingureGestures_OnDragBegin;
	FingerGestures.OnFingerDragMove += FingureGestures_OnDragMove;
	FingerGestures.OnFingerDragEnd += FingureGestures_OnDragEnd;
}

 // Convert from screen-space coordinates to world-space coordinates on the Z = 0 plane
public static Vector3 GetWorldPos( Vector2 screenPos )
{
    Ray ray = Camera.main.ScreenPointToRay( screenPos );
    // we solve for intersection with z = 0 plane
    float t = -ray.origin.z / ray.direction.z;

    return ray.GetPoint( t );
}
	
#region FingerGestures events
	
public void enableDragTrail(){
	trailRenderer.enabled=true;
	}
void FingureGestures_OnDragBegin(int fingerIndex, Vector2 fingerPos, Vector2 delta){
		enableDragTrail();
		if(dragFingerIndex != -1) return;
		dragFingerIndex = fingerIndex;
}

void FingureGestures_OnDragMove( int fingerIndex, Vector2 fingerPos, Vector2 delta ){
	Vector3 _newPos = GetWorldPos(fingerPos);
	if( fingerIndex == dragFingerIndex ){	
		if(trailRenderer.enabled){
				trailRenderer.transform.position = _newPos;
			}
		}
}
public void cancelDragTrail(){
            dragFingerIndex = -1;
			trailRenderer.enabled=false;
	}
void FingureGestures_OnDragEnd( int fingerIndex, Vector2 fingerPos ){
	if(fingerIndex == dragFingerIndex){
		cancelDragTrail();
	}
}
void OnDisable (){
	FingerGestures.OnFingerDragBegin -= FingureGestures_OnDragBegin;
	FingerGestures.OnFingerDragMove -= FingureGestures_OnDragMove;
	FingerGestures.OnFingerDragEnd -= FingureGestures_OnDragEnd;
}
#endregion

}
