using UnityEngine;
using System.Collections;

public class TextGeneratorButton : MonoBehaviour {
	
	private string text = "";
	private bool isClicked = false;

	private static int count = 0;

	private static int boxWidth = 400;
	private static int boxHeight = 130;

	private static int buttonWidth = boxWidth / 2 - 5;
	private static int buttonHeight = 50;

	private PenPalleteGUI mPenPalleteGUI;
	public PaitingBoard paitingBoard;

	void Start(){
		mPenPalleteGUI = GetComponent<PenPalleteGUI> ();
	}

	void OnGUI()
	{

		int y = 2;

		GUI.matrix = GenericGUIHandler.getTRS ();
		if(GUI.Button(new Rect(2,y,50,50),"T")){
			isClicked = true;
		}

		if (isClicked == true) {
				
			GUI.matrix = GenericGUIHandler.getTRS ();
			GUI.Window (0, new Rect (GenericGUIHandler.baseWidth/2 - boxWidth/2  , GenericGUIHandler.baseHeight/2 - boxHeight/2 , boxWidth, boxHeight), showGetTextDialogBox,"Enter Text");
		}

		y += 58;
		GUI.matrix = GenericGUIHandler.getTRS ();
		if(GUI.Button(new Rect(2,y,50,50),"C")){
			GUI.matrix = GenericGUIHandler.getTRS ();
			mPenPalleteGUI.showPallete (paitingBoard);

		}

		y += 58;
		GUI.matrix = GenericGUIHandler.getTRS ();
		if(GUI.Button(new Rect(2,y,50,50),"+")){
			if(paitingBoard.radius < 40)
				paitingBoard.radius+=5;
		}

		y += 58;
		GUI.matrix = GenericGUIHandler.getTRS ();
		if(GUI.Button(new Rect(2,y,50,50),"-")){
			if(paitingBoard.radius > 5)
				paitingBoard.radius-=5;
		}

		y += 58;
		GUI.matrix = GenericGUIHandler.getTRS ();
		if(GUI.Button(new Rect(2,y,50,50),"2")){
			paitingBoard.twoLines = !paitingBoard.twoLines;
		}

		y += 58;
		GUI.matrix = GenericGUIHandler.getTRS ();
		if(GUI.Button(new Rect(2,y,50,50),"R")){
			paitingBoard.reset();
		}

		y += 58;
		GUI.matrix = GenericGUIHandler.getTRS ();
		if(GUI.Button(new Rect(2,y,50,50),"P")){
			paitingBoard.hardness = 1;
			paitingBoard.isPaint = true;
		}

		y += 58;
		GUI.matrix = GenericGUIHandler.getTRS ();
		if(GUI.Button(new Rect(2,y,50,50),"B")){
//			paitingBoard.hardness = 1000;
			paitingBoard.isPaint = false;
		}

		y += 58;
		GUI.matrix = GenericGUIHandler.getTRS ();
		if(GUI.Button(new Rect(2,y,100,50),"Back W")){
			paitingBoard.loadTexture("");
		}

		y += 58;
		GUI.matrix = GenericGUIHandler.getTRS ();
		if(GUI.Button(new Rect(2,y,100,50),"Back R")){
			paitingBoard.loadTexture("_red");

		}

		y += 58;
		GUI.matrix = GenericGUIHandler.getTRS ();
		if(GUI.Button(new Rect(2,y,100,50),"Back G")){
			paitingBoard.loadTexture("_green");

		}

	}
		

	private void showGetTextDialogBox(int windowId){

		GUI.matrix = GenericGUIHandler.getTRS ();
		text = GUI.TextField (new Rect (5, 20, boxWidth-10, 40),text);
		GUI.matrix = GenericGUIHandler.getTRS ();

		if (GUI.Button (new Rect (5, 70, buttonWidth, buttonHeight), "Done")) {
			isClicked = false;
			GameObject textPrefab = Instantiate(Resources.Load("Prefabs/Text", typeof(GameObject)),Vector3.zero,Quaternion.identity) as GameObject;
			textPrefab.name = textPrefab.name+count;
			TextMesh textMesh = textPrefab.GetComponent<TextMesh>();
			textMesh.text = text;
			BoxCollider2D coll = textPrefab.GetComponent<BoxCollider2D>();
			Bounds bounds =  textMesh.renderer.renderer.bounds;
			coll.size = new Vector2(bounds.size.x, bounds.size.y);
			Animator anim = textPrefab.GetComponent<Animator>();
			anim.enabled = false;
			count++;

		}
		if(GUI.Button (new Rect (boxWidth/2, 70, buttonWidth, buttonHeight),"Cancel")){
			isClicked = false;
		}
	}
		
}
