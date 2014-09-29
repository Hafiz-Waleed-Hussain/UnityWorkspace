using UnityEngine;
using System.Collections;

public class AGTopMenu : MonoBehaviour {
	
	public GUISkin customSkin = null;
	public AGWorldIndex currentWorldIndex = AGWorldIndex.k_None;
	public bool shouldShowStarCount = false;
	
	public GameObject homeButton = null;
	public GameObject worldButton = null;
	
	public GameObject childImage = null;
	
	// Use this for initialization
	void Awake(){
		
	}
	
	void OnDisable () {
		FingerGestures.OnFingerLongPress -= FingerGestures_OnFingerLongPress;
		FingerGestures.OnFingerTap -= FingerGestures_OnFingerTap;
	}
	
	void Start () {
		
		//FingerGestures.Defaults.Fingers[0].LongPress.Duration
		FingerGestures.Defaults.Fingers[0].LongPress.Duration = 1.0f;
		FingerGestures.OnFingerTap += FingerGestures_OnFingerTap;
		FingerGestures.OnFingerLongPress += FingerGestures_OnFingerLongPress;
		
		//float DEVICE_WIDTH = Screen.currentResolution.width;
		float DEVICE_HEIGHT = Screen.currentResolution.height;
		float yPos = 0;
		
		//DEVICE_HEIGHT = 552;
		//Debug.Log(DEVICE_HEIGHT);
		
		if(DEVICE_HEIGHT == 752 || DEVICE_HEIGHT == 1504){
			Camera.mainCamera.orthographicSize = 376;
			Camera.mainCamera.transform.position = new Vector3(Camera.mainCamera.transform.position.x,-24,Camera.mainCamera.transform.position.z);
		}
		if(DEVICE_HEIGHT == 736) {
			Camera.mainCamera.orthographicSize = 368;
			Camera.mainCamera.transform.position = new Vector3(Camera.mainCamera.transform.position.x,-32,Camera.mainCamera.transform.position.z);
		}
		if(DEVICE_HEIGHT == 552) {
			Camera.mainCamera.orthographicSize = 368;
			Camera.mainCamera.transform.position = new Vector3(Camera.mainCamera.transform.position.x,-32,Camera.mainCamera.transform.position.z);
		}
		
		if(homeButton){
			
			float xPos = 598.0f;
			if(DEVICE_HEIGHT==1200) { // targetting 1920x 1200
				yPos = homeButton.transform.position.y;
			} else if(DEVICE_HEIGHT == 752 || DEVICE_HEIGHT == 1504){
				yPos = homeButton.transform.position.y - 48;
			} else if(DEVICE_HEIGHT == 736){
				yPos = homeButton.transform.position.y - 64;
			} else if(DEVICE_HEIGHT == 552){
				xPos = 640.0f;
				yPos = homeButton.transform.position.y - 64;
			} else {
				yPos=358;
			}
			
			homeButton.transform.localScale = new Vector3(84.0f, 84.0f, 1.0f);
			homeButton.transform.position = new Vector3(xPos, yPos, homeButton.transform.position.z);
		}
		if(worldButton){
			
			float xPos = -598.0f;
			//if the world button is present that means that its ingame and should show child image
			childImage = Instantiate(Resources.Load("HomeScreen/Prefabs/StarCountChildImage")) as GameObject;
			childImage.transform.position = new Vector3(490F, 365F, -500F);
			
			if(DEVICE_HEIGHT==1200) {// targetting 1920x 1200
				yPos = worldButton.transform.position.y;
			} else if(DEVICE_HEIGHT == 752 || DEVICE_HEIGHT == 1504){
				yPos = worldButton.transform.position.y - 48;
			} else if(DEVICE_HEIGHT == 736){
				yPos = worldButton.transform.position.y - 64;
			} else if(DEVICE_HEIGHT == 552){
				yPos = worldButton.transform.position.y - 64;
				xPos = -640.0f;
			} else {
				yPos=358;
			}
			
			worldButton.transform.localScale = new Vector3(84.0f, 84.0f, 1.0f);
			worldButton.transform.position = new Vector3(xPos, yPos, worldButton.transform.position.z);
			
			childImage.transform.position = new Vector3(-490, yPos+7.0f, childImage.transform.position.z);
		}
		
	}
	
	public void hideHomeButtons(bool hide) {
		
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
	
		if(homeButton != null && selection.name.Equals(homeButton.name)) {
			AGGameState.gotoHomeScreen();
		}
		
		if(worldButton != null && selection.name.Equals(worldButton.name)) {
			AGGameState.switchWorlds(currentWorldIndex);
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
	
	/*
	void OnGUI() {
		
		if(customSkin != null) {
			GUI.skin = customSkin;
		}
        
		float offset = Screen.currentResolution.width / 1280.0f;
		
		if (GUI.Button(new Rect(0, 0, 170 * offset, 170 * offset), worldButton)){
			Application.LoadLevel(0);
		}
		
		if(GUI.Button(new Rect(Screen.currentResolution.width - 170 * offset, 0, 170 * offset, 170 * offset), homeButton)) {
			Application.LoadLevel(2);
		}
    }
    */
	
	void Update() {
		//back button
		if(Application.platform == RuntimePlatform.Android) {
			if(Input.GetKey(KeyCode.Escape)) {
				AGGameState.gotoHomeScreen();
			}
		}
	}
}
