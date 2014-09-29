using UnityEngine;
using System.Collections;

//[ExecuteInEditMode]
public class ColorPalleteGUI : MonoBehaviour {

	private bool isClicked = false;
	private TextMesh mTextMesh;

	public void showPallete(){
		isClicked = true;
	}

	public void hidePallete(){
		isClicked = false;
		mTextMesh = null;
	}

	public void setTextMesh(TextMesh textMesh){
		mTextMesh = textMesh;
	}

	private static int boxWidth = 336;
	private static int boxHeight = 200;
	

	void OnGUI() {

		if (isClicked) {
			GUI.matrix = GenericGUIHandler.getTRS ();
//			GUI.Window (0, new Rect (Screen.width / 2 - 118, Screen.height / 2 - 100, 336, 140), showGetTextDialogBox, "Choose Color");
			GUI.Window (0, new Rect (GenericGUIHandler.baseWidth/2 - boxWidth/2  , GenericGUIHandler.baseHeight/2 - boxHeight/2 , boxWidth, boxHeight), showGetTextDialogBox,"Choose Color");
		} 


	}


			




	private void showGetTextDialogBox(int windowId){

		GUI.matrix = GenericGUIHandler.getTRS ();
		generateRowOfButtons (5, 25, 50, 50);

	}


	private void generateRowOfButtons(int x, int y,int buttonWidth, int buttonHeight){

		int orignalXPosition = x;

		setButtonProperties ();
		if (GUI.Button(new Rect(x,y,buttonWidth,buttonHeight),"Red")) {
			mTextMesh.color = Color.red;
			hidePallete();
		}

		x += buttonWidth + 5;
		setButtonProperties ();

		GUI.matrix = GenericGUIHandler.getTRS ();
		if (GUI.Button(new Rect(x,y,buttonWidth,buttonHeight),"Yellow")) {
			mTextMesh.color = Color.yellow;
			hidePallete();
		}

		x += buttonWidth + 5;
		setButtonProperties ();
		if (GUI.Button(new Rect(x,y,buttonWidth,buttonHeight),"Green")) {
			mTextMesh.color = Color.green;
			hidePallete();

		}

		x += buttonWidth + 5;
		
		setButtonProperties ();
		if (GUI.Button(new Rect(x,y,buttonWidth,buttonHeight),"Blue")) {
			mTextMesh.color = Color.blue;
			hidePallete();

		}

		x += buttonWidth + 5;

		setButtonProperties ();
		if (GUI.Button(new Rect(x,y,buttonWidth,buttonHeight),"White")) {
			mTextMesh.color = Color.white;
			hidePallete();
		}

		x += buttonWidth + 5;
		
		setButtonProperties ();
		if (GUI.Button(new Rect(x,y,buttonWidth,buttonHeight),"Black")) {
			mTextMesh.color = Color.black;
			hidePallete();
		}

		// Second Row
		x = orignalXPosition;
		y += 55;

		setButtonProperties ();
		if (GUI.Button(new Rect(x,y,buttonWidth,buttonHeight),"Gray")) {
			mTextMesh.color = Color.gray;
			hidePallete();
		}
		
		x += buttonWidth + 5;
		
		setButtonProperties ();
		if (GUI.Button(new Rect(x,y,buttonWidth,buttonHeight),"Cyan")) {
			mTextMesh.color = Color.cyan;
			hidePallete();
			
		}
		
		x += buttonWidth + 5;
		setButtonProperties ();

		if (GUI.Button(new Rect(x,y,buttonWidth,buttonHeight),"Magenta")) {
			hidePallete();
			mTextMesh.color = Color.magenta;
			
		}
		
		x += buttonWidth + 5;
		setButtonProperties ();

		if (GUI.Button(new Rect(x,y,buttonWidth,buttonHeight),"Temp")) {
			mTextMesh.color = new Color(.5f,.1f,.3f);
			hidePallete();

		}
		
		x += buttonWidth + 5;
		setButtonProperties ();

		if (GUI.Button(new Rect(x,y,buttonWidth,buttonHeight),"White")) {

			mTextMesh.color = Color.white;
			hidePallete();
		}
		
		x += buttonWidth + 5;
		setButtonProperties ();

		if (GUI.Button(new Rect(x,y,buttonWidth,buttonHeight),"Black")) {
			mTextMesh.color = Color.black;
			hidePallete();

		}

		// Third Row Animations
		x = orignalXPosition;
		y += 55;
		
		setButtonProperties ();
		if (GUI.Button(new Rect(x,y,buttonWidth,buttonHeight),"Zoom")) {
			Animator anim = mTextMesh.gameObject.GetComponent<Animator>();
			anim.enabled = true;
			if(anim.GetBool("half_rotate")){
				anim.SetBool("half_rotate",false);
			}
			anim.SetBool("start_zoom",true);

			hidePallete();
		}
		
		x += buttonWidth + 5;
		
		setButtonProperties ();
		if (GUI.Button(new Rect(x,y,buttonWidth,buttonHeight),"Rotat")) {
			Animator anim = mTextMesh.gameObject.GetComponent<Animator>();
			anim.enabled = true;
			if(anim.GetBool("start_zoom")){
				anim.SetBool("start_zoom",false);
			}
			anim.SetBool("half_rotate",true);
			hidePallete();
			
		}

		x += buttonWidth + 5;
		
		setButtonProperties ();
		if (GUI.Button(new Rect(x,y,buttonWidth,buttonHeight),"Reset")) {

			Animator anim = mTextMesh.gameObject.GetComponent<Animator>();
			anim.enabled = true;
			if(anim.GetBool("start_zoom")){
				anim.SetBool("start_zoom",false);
			}
			if(anim.GetBool("half_rotate")){
				anim.SetBool("half_rotate",false);
			}
			anim.enabled = false;


			hidePallete();
			
		}

		x += buttonWidth + 5;
		
		setButtonProperties ();
		if (GUI.Button(new Rect(x,y,buttonWidth,buttonHeight),"Delete")) {
			SingleTouchScript singleTouchScript = mTextMesh.GetComponent<SingleTouchScript>();
			singleTouchScript.removeText();
			hidePallete();
			
		}
		
	}

	private void setButtonProperties(){
		GUI.matrix = GenericGUIHandler.getTRS ();
	}

	
}
