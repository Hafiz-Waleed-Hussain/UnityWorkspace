using UnityEngine;
using System.Collections;

//[ExecuteInEditMode]
public class PenPalleteGUI : MonoBehaviour {

	private bool isClicked = false;

	private PaitingBoard mPaintingBoard;
	public void showPallete(PaitingBoard paitingBoard){
		isClicked = true;
		mPaintingBoard = paitingBoard;
	}

	public void hidePallete(Color c){
		isClicked = false;
		mPaintingBoard.isEraserColor = c; 
	}

	public void setTextMesh(TextMesh textMesh){
	}

	private static int boxWidth = 336;
	private static int boxHeight = 140;
	

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
			
			hidePallete(Color.red);
		}

		x += buttonWidth + 5;
		setButtonProperties ();

		GUI.matrix = GenericGUIHandler.getTRS ();
		if (GUI.Button(new Rect(x,y,buttonWidth,buttonHeight),"Yellow")) {
			hidePallete(Color.yellow);
		}

		x += buttonWidth + 5;
		setButtonProperties ();
		if (GUI.Button(new Rect(x,y,buttonWidth,buttonHeight),"Green")) {
			hidePallete(Color.green);

		}

		x += buttonWidth + 5;
		
		setButtonProperties ();
		if (GUI.Button(new Rect(x,y,buttonWidth,buttonHeight),"Blue")) {
			hidePallete(Color.blue);

		}

		x += buttonWidth + 5;

		setButtonProperties ();
		if (GUI.Button(new Rect(x,y,buttonWidth,buttonHeight),"White")) {
			hidePallete(Color.white);
		}

		x += buttonWidth + 5;
		
		setButtonProperties ();
		if (GUI.Button(new Rect(x,y,buttonWidth,buttonHeight),"Black")) {
			hidePallete(Color.black);
		}

		// Second Row
		x = orignalXPosition;
		y += 55;

		setButtonProperties ();
		if (GUI.Button(new Rect(x,y,buttonWidth,buttonHeight),"Gray")) {
			hidePallete(Color.gray);
		}
		
		x += buttonWidth + 5;
		
		setButtonProperties ();
		if (GUI.Button(new Rect(x,y,buttonWidth,buttonHeight),"Cyan")) {
			hidePallete(Color.cyan);
			
		}
		
		x += buttonWidth + 5;
		setButtonProperties ();

		if (GUI.Button(new Rect(x,y,buttonWidth,buttonHeight),"Magenta")) {
			hidePallete(Color.magenta);

		}
		
		x += buttonWidth + 5;
		setButtonProperties ();

		if (GUI.Button(new Rect(x,y,buttonWidth,buttonHeight),"Temp")) {
			hidePallete(new Color(.5f,.1f,.3f));

		}
		
		x += buttonWidth + 5;
		setButtonProperties ();

		if (GUI.Button(new Rect(x,y,buttonWidth,buttonHeight),"White")) {

			hidePallete(Color.white);
		}
		
		x += buttonWidth + 5;
		setButtonProperties ();

		if (GUI.Button(new Rect(x,y,buttonWidth,buttonHeight),"Black")) {
			hidePallete(Color.black);

		}

	}

	private void setButtonProperties(){
		GUI.matrix = GenericGUIHandler.getTRS ();
	}
}
