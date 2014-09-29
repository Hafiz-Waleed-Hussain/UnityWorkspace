//#define MAC_PLATFORM

using UnityEngine;
using System.Collections;

public class HomeScreen : MonoBehaviour {
	
	
	public GameObject homeButton;
	public GameObject bwButton;
	public GameObject jwButton;
	public GameObject fwButton;
	public GameObject swButton;
	public GameObject cwButton;
	public GameObject iwButton;
	
	public GameObject childSwitchButton;
	
	public GameObject tvButton;
	
	public Transform topMenu;
	
	public Texture2D defaultChildImage;
	
	
	public GameObject childImage = null;
	public TextMesh starCount = null;
	public TextMesh childName = null;
	// Use this for initialization
	
	void OnDisable () {
		FingerGestures.OnFingerTap -= FingerGestures_OnFingerTap;
		FingerGestures.OnFingerLongPress -= FingerGestures_OnFingerLongPress;
	}
	
	void Start () {
		FingerGestures.Defaults.Fingers[0].LongPress.Duration = 1.0f;
		FingerGestures.OnFingerTap += FingerGestures_OnFingerTap;
		FingerGestures.OnFingerLongPress += FingerGestures_OnFingerLongPress;
		
		
		if(topMenu != null) {
			
			float DEVICE_HEIGHT = Screen.currentResolution.height;
			float yPos = topMenu.position.y;
			float xPos = topMenu.position.x;
			
			if(DEVICE_HEIGHT == 752 || DEVICE_HEIGHT == 1504){
				yPos = topMenu.position.y - 48;
			} else if(DEVICE_HEIGHT == 736 || DEVICE_HEIGHT == 552){
				yPos = topMenu.position.y - 64;
			}
			
			if(DEVICE_HEIGHT == 552) {
				xPos = 0.0f;
				
				Vector3 hPos = homeButton.transform.position;
				hPos.x = 590;
				homeButton.transform.position = hPos;
			}
			
			topMenu.position = new Vector3(xPos, yPos, topMenu.position.z);
		}
		
#if (!MAC_PLATFORM)
		updateChild();
		
#endif
	}
	
	void FingerGestures_OnFingerLongPress( int fingerIndex, Vector2 fingerPos )
    {
        GameObject selection = PickObject(fingerPos);
		if(selection == null) return;
		
		if(homeButton != null && selection.name.Equals(homeButton.name)) {
			Debug.Log(selection);
			AGGameState.gotoDashboard();
		}
    }
	
	void FingerGestures_OnFingerTap( int fingerIndex, Vector2 fingerPos, int tapCount )
    {
		GameObject selection = PickObject(fingerPos);
		if(selection == null) return;
	
		if(bwButton != null && selection.name.Equals(bwButton.name)) {
			AGGameState.switchWorlds(AGWorldIndex.k_BeeWorld);
		}
		
		if(jwButton != null && selection.name.Equals(jwButton.name)) {
			AGGameState.switchWorlds(AGWorldIndex.k_JungleWorld);
		}
		
		if(fwButton != null && selection.name.Equals(fwButton.name)) {
			AGGameState.switchWorlds(AGWorldIndex.k_FishWorld);
		}
		
		if(iwButton != null && selection.name.Equals(iwButton.name)) {
			AGGameState.switchWorlds(AGWorldIndex.k_IckyWorld);
		}
		
		if(swButton != null && selection.name.Equals(swButton.name)) {
			AGGameState.switchWorlds(AGWorldIndex.k_ShapeWorld);
		}
		
		if(cwButton != null && selection.name.Equals(cwButton.name)) {
			AGGameState.switchWorlds(AGWorldIndex.k_CircusWorld);
		}
		
		if(tvButton != null && selection.name.Equals(tvButton.name)) {
			AGGameState.switchWorlds(AGWorldIndex.k_VideoWorld);
		}
		
		if(childSwitchButton != null && selection.name.Equals(childSwitchButton.name)) {
			Debug.Log("switching child");
			AGGameState.switchChild();
		}
    }
	
	void OnApplicationPause(bool pause) {
		if(pause) {
			
		} else {
			updateChild();
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
		
		if(chName != null && !chName.Equals(childName.text)) {
			
			double timeSpent = (double) (Time.time - AGGameState.startingTime);
			AndroidJavaObject persistanceManager = AGGameState.getPersistanceManager();
			persistanceManager.Call("addChildSessionTimeLog", (double) timeSpent);
		
			
			AGGameState.startingTime = Time.time;
		}
		if(chName == null || chName.Equals("")) {
			chName = "Child";
		}
		childName.text = chName;
		
		if(AGGameState.getChildCount() > 1) {
			childSwitchButton.SetActive(true);
		} else {
			childSwitchButton.SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
