using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Gui : MonoBehaviour {

	public GameObject textPrefab;
	public GameObject colorPallete;
	private int GET_TEXT_DIALOG_BOX_WINDOW_ID = 1;
	private bool toggleForGetTextDialg = false;
	private string getText = "";




	void OnGUI(){
		autoResize ();
		if (GUI.Button (new Rect (10, 10, 50, 50), "T")) {
			toggleForGetTextDialg = !toggleForGetTextDialg;
		}

		autoResize ();
		colorPallete.SetActive(toggleForGetTextDialg);
		if (toggleForGetTextDialg) {
			GUI.Window (GET_TEXT_DIALOG_BOX_WINDOW_ID, new Rect (Screen.width / 2 - 50, Screen.height / 2 - 25, 100, 50), showGetTextDialogBox,"");
		}
	}

	private void showGetTextDialogBox(int windowId){
	
		autoResize ();
		getText = GUI.TextField (new Rect (5, 5, 90, 20),getText);
		autoResize ();
		if (GUI.Button (new Rect (5, 26, 50, 25), "Done")) {
			toggleForGetTextDialg = false;
			Instantiate(textPrefab);
			textPrefab.SetActive(true);
			TextMesh textMesh = textPrefab.GetComponent<TextMesh>();
			textMesh.text = getText;
			BoxCollider2D coll = textPrefab.GetComponent<BoxCollider2D>();
			Bounds bounds =  textMesh.renderer.renderer.bounds;
			coll.size = new Vector2(bounds.size.x, bounds.size.y);



		}else if(GUI.Button (new Rect (51, 26, 50, 25),"Cancel")){
			toggleForGetTextDialg = false;
		}
	}


	void autoResize(){
		Vector2 resizeRation = new Vector2((float)Screen.width/1280 ,(float)Screen.height/ 800);
		GUI.matrix = Matrix4x4.TRS (Vector3.zero, Quaternion.identity, new Vector3(resizeRation.x,resizeRation.y,1.0f));
	}

   }
