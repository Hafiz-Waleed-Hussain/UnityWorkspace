/// <summary>
/// GUI manager
/// this script use to control all GUI in game
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class GUIManager : MonoBehaviour {
	
	public enum ButtonType{
		pause,resume,exit,restart,sumRestart,sumExit
	}
	
	
	[System.Serializable]
	public class RectGroup{
		public string name;
		public float x, y, sizeX, sizeY;
	}
	
	//GUI Item state(ex double jump state, multiply state)
	[System.Serializable]
	public class GroupGUITexture{
		public string name;
		public Item.TypeItem itemType;
		public GUITexture guiTexture;
		public GUIText guiText;
		public Color colorText;
		public float x, y, sizeX, sizeY;
	}
	
	//GUI Buton(ex. pause button , resume button)
	[System.Serializable]
	public class GroupGUIButton{
		public string name;
		public ButtonType buttonType;
		public GUITexture guiTexture;
		public Texture2D buttonNormal;
		public Texture2D buttonActive;
		public float x, y, sizeX, sizeY;
	}
	
	
	//GUI Score(ex. distance score,coin)
	[System.Serializable]
	public class GroupGUIScore{
		public string name;
		public GUITexture guiTexture;
		public float x, y, sizeX, sizeY;
	}
	

	
	public bool isPreview; //set to preview GUI
	public GUIText[] guiTextArr; 
	public List<RectGroup> positionSet = new List<RectGroup>();
	public List<GroupGUITexture> itemStateSet = new List<GroupGUITexture>();
	public List<GroupGUIButton> menuButtonSet = new List<GroupGUIButton>();
	public List<GroupGUIScore> scoreSet = new List<GroupGUIScore>();
	private CalOnGUI calOnGUI;
	private bool createGUI;

	[HideInInspector] public bool showSumGUI;

	public static GUIManager instance;
	
	void Start(){
		calOnGUI = GetComponent<CalOnGUI> ();
		instance = this;
	}
	
	void Update(){
		
		//Set to preview GUI in editor
		if(Application.isPlaying == false){
			if(calOnGUI == null){
				calOnGUI = GetComponent<CalOnGUI> ();
			}
			for(int i = 0; i < itemStateSet.Count; i++){
				if(isPreview){
					itemStateSet[i].guiTexture.enabled = true;
					itemStateSet[i].guiText.enabled = true;
					if(itemStateSet[i].itemType == Item.TypeItem.ItemJump){
						ShowGUI(i, 1);
					}	
					
					if(itemStateSet[i].itemType == Item.TypeItem.ItemMagnet){
						ShowGUI(i, 1);
					}
					
					if(itemStateSet[i].itemType == Item.TypeItem.ItemMultiply){
						ShowGUI(i, 1);
					}
					
					if(itemStateSet[i].itemType == Item.TypeItem.ItemSprint){
						ShowGUI(i, 1);
					}
					
				}else{
					itemStateSet[i].guiTexture.enabled = false;
					itemStateSet[i].guiText.enabled = false;	
				}
			}
			
			for(int i = 0; i < menuButtonSet.Count; i++){
				if(isPreview){
					menuButtonSet[i].guiTexture.enabled = true;
					ShowGUIButton(i);	
					
				}else{	
					menuButtonSet[i].guiTexture.enabled = false;
				}
			}
			
			for(int i = 0; i < scoreSet.Count; i++)
			{
				if(isPreview){
					scoreSet[i].guiTexture.enabled = true;
					ShowGUIScore(i);
				}else{	
					scoreSet[i].guiTexture.enabled = false;
				}
			}
			
		}else{
			if(PatternSystem.instance.loadingComplete == true){
				for(int i = 0; i < guiTextArr.Length; i++){
					//guiText[i].pixelOffset = new Vector2(calOnGUI.SetGUI(positionSet[i].x, positionSet[i].y, positionSet[i].sizeX).x
					//									, calOnGUI.SetGUI(positionSet[i].x, positionSet[i].y, positionSet[i].sizeX).y);
					
					guiTextArr[i].enabled = true;
					guiTextArr[i].pixelOffset = new Vector2(GUI_Calculate.RectWithScrren_WidthAndHeight_Sizeheight(new Vector2(positionSet[i].x, positionSet[i].y),
																												new Vector2(positionSet[i].sizeX, positionSet[i].sizeY)).x,
														GUI_Calculate.RectWithScrren_WidthAndHeight_Sizeheight(new Vector2(positionSet[i].x, positionSet[i].y),
																												new Vector2(positionSet[i].sizeX, positionSet[i].sizeY)).y);
					guiTextArr[i].fontSize = GUI_Calculate.FontSize((int)positionSet[i].sizeX);
					if(positionSet[i].name == "Distance"){
						guiTextArr[i].text = ""+(int)GameAttribute.gameAttribute.distance;
					}else if(positionSet[i].name == "Coin"){
						guiTextArr[i].text = ""+(int)GameAttribute.gameAttribute.coin;
					}
				}
				
				//RectWithScrren_WidthAndHeight_Sizeheight
				
				for(int i = 0; i < scoreSet.Count; i++)
				{
					scoreSet[i].guiTexture.enabled = true;
					ShowGUIScore(i);

				}

				if(Controller.instace != null){
				for(int i = 0; i < itemStateSet.Count; i++){
					if(itemStateSet[i].itemType == Item.TypeItem.ItemJump){
						if(Controller.instace.timeJump > 0){
							itemStateSet[i].guiTexture.enabled = true;
							itemStateSet[i].guiText.enabled = true;
							ShowGUI(i, Controller.instace.timeJump);
						}else{
							itemStateSet[i].guiTexture.enabled = false;
							itemStateSet[i].guiText.enabled = false;
						}
					}	
					
					if(itemStateSet[i].itemType == Item.TypeItem.ItemMagnet){
						if(Controller.instace.timeMagnet > 0){
							itemStateSet[i].guiTexture.enabled = true;
							itemStateSet[i].guiText.enabled = true;
							ShowGUI(i, Controller.instace.timeMagnet);
						}else{
							itemStateSet[i].guiTexture.enabled = false;
							itemStateSet[i].guiText.enabled = false;
						}
					}
					
					if(itemStateSet[i].itemType == Item.TypeItem.ItemMultiply){
						if(Controller.instace.timeMultiply > 0){
							itemStateSet[i].guiTexture.enabled = true;
							itemStateSet[i].guiText.enabled = true;
							ShowGUI(i, Controller.instace.timeMultiply);
						}else{
							itemStateSet[i].guiTexture.enabled = false;
							itemStateSet[i].guiText.enabled = false;
						}
					}
					
					if(itemStateSet[i].itemType == Item.TypeItem.ItemSprint){
						if(Controller.instace.timeSprint > 0){
							itemStateSet[i].guiTexture.enabled = true;
							itemStateSet[i].guiText.enabled = true;
							ShowGUI(i, Controller.instace.timeSprint);
						}else{
							itemStateSet[i].guiTexture.enabled = false;
							itemStateSet[i].guiText.enabled = false;
						}
					}
				}
				}
				
				for(int i = 0; i < menuButtonSet.Count; i++){
					ShowGUIButton(i);
					CheckTypeButtonActive(i);
					if(menuButtonSet[i].guiTexture.HitTest(Input.mousePosition)){
						if(Input.GetMouseButtonDown(0)){
							menuButtonSet[i].guiTexture.texture = menuButtonSet[i].buttonActive;	
						}
						
						if(Input.GetMouseButtonUp(0)){
							menuButtonSet[i].guiTexture.texture = menuButtonSet[i].buttonNormal;	
							CheckTypeButtonAction(i);
						}
					}else{
						menuButtonSet[i].guiTexture.texture = menuButtonSet[i].buttonNormal;	
					}
				}
			}else{
				for(int i = 0; i < guiTextArr.Length; i++){
					guiTextArr[i].text = "";
				}
			}
		}
		
	}
	
	//Check hide/active button
	private void CheckTypeButtonActive(int i){
		if(menuButtonSet[i].buttonType == ButtonType.exit){
			if(GameAttribute.gameAttribute.pause == true){
				menuButtonSet[i].guiTexture.enabled = true;
			}else{
				menuButtonSet[i].guiTexture.enabled = false;	
				
			}
		}
		
		if(menuButtonSet[i].buttonType == ButtonType.pause){
			if(GameAttribute.gameAttribute.pause == false){
				menuButtonSet[i].guiTexture.enabled = true;
			}else{
				menuButtonSet[i].guiTexture.enabled = false;	
			}
		}
		
		if(menuButtonSet[i].buttonType == ButtonType.restart){
			if(GameAttribute.gameAttribute.pause == true){
				menuButtonSet[i].guiTexture.enabled = true;
			}else{
				menuButtonSet[i].guiTexture.enabled = false;	
			}
		}
		
		if(menuButtonSet[i].buttonType == ButtonType.resume){
			if(GameAttribute.gameAttribute.pause == true){
				menuButtonSet[i].guiTexture.enabled = true;
			}else{
				menuButtonSet[i].guiTexture.enabled = false;	
			}
		}
		
		if(menuButtonSet[i].buttonType == ButtonType.sumRestart){
			if(GameAttribute.gameAttribute.life <= 0 && showSumGUI == true){
				menuButtonSet[i].guiTexture.enabled = true;
			}else{
				menuButtonSet[i].guiTexture.enabled = false;	
			}
		}
		
		if(menuButtonSet[i].buttonType == ButtonType.sumExit){
			if(GameAttribute.gameAttribute.life <= 0 && showSumGUI == true){
				menuButtonSet[i].guiTexture.enabled = true;
			}else{
				menuButtonSet[i].guiTexture.enabled = false;	
			}
		}
	}
	
	//This method use to input command in button
	private void CheckTypeButtonAction(int i){
		if(menuButtonSet[i].guiTexture.enabled == true)
		{
			//exit button back to title
			if(menuButtonSet[i].buttonType == ButtonType.exit){
				Application.LoadLevel("TitleScene");
			}
			
			//pause button
			if(menuButtonSet[i].buttonType == ButtonType.pause){
				if(GameAttribute.gameAttribute.life > 0){
					GameAttribute.gameAttribute.Pause(true);
				}
			}
			
			//restart button
			if(menuButtonSet[i].buttonType == ButtonType.restart){
				GameAttribute.gameAttribute.Reset();
			}
			
			//resume button
			if(menuButtonSet[i].buttonType == ButtonType.resume){
				GameAttribute.gameAttribute.Pause(false);
			}
			
			//restar button in summary screeen
			if(menuButtonSet[i].buttonType == ButtonType.sumRestart){
				GameAttribute.gameAttribute.Reset();
			}
			
			//exit button in summary screen
			if(menuButtonSet[i].buttonType == ButtonType.sumExit){
				Application.LoadLevel("TitleScene");
			}
		}
		
	}
	
	private void ShowGUI(int i, float time){
		if(itemStateSet[i].guiTexture != null){
			itemStateSet[i].guiTexture.pixelInset = new Rect(calOnGUI.SetGUI(itemStateSet[i].x, itemStateSet[i].y, itemStateSet[i].sizeX, itemStateSet[i].sizeY));
		}
		if(itemStateSet[i].guiText != null){
			itemStateSet[i].guiText.material.color = itemStateSet[i].colorText;
			itemStateSet[i].guiText.pixelOffset = new Vector2(calOnGUI.SetGUI(itemStateSet[i].x, itemStateSet[i].y, itemStateSet[i].sizeX).x + (calOnGUI.SetGUI(itemStateSet[i].x, itemStateSet[i].y, itemStateSet[i].sizeX).width/2),
															itemStateSet[i].guiTexture.pixelInset.y + (itemStateSet[i].guiTexture.pixelInset.height/2));
			itemStateSet[i].guiText.text = time.ToString("0")+"s";
		}
	}
	private void ShowGUIButton(int i){
		if(menuButtonSet[i].guiTexture != null){
			menuButtonSet[i].guiTexture.pixelInset = new Rect(calOnGUI.SetGUI(menuButtonSet[i].x, menuButtonSet[i].y, menuButtonSet[i].sizeX, menuButtonSet[i].sizeY));
		}
	}
	
	private void ShowGUIScore(int i){
		if(scoreSet[i].guiTexture != null){
			scoreSet[i].guiTexture.pixelInset = new Rect(calOnGUI.SetGUI(scoreSet[i].x, scoreSet[i].y, scoreSet[i].sizeX, scoreSet[i].sizeY));
		}
	}
	
	public void Reset(){
		for(int i = 0; i < menuButtonSet.Count; i++){
			menuButtonSet[i].guiTexture.enabled = false;	
		}
		
		for(int i = 0; i < itemStateSet.Count; i++){
			itemStateSet[i].guiTexture.enabled = false;
			itemStateSet[i].guiText.enabled = false;
		}

		showSumGUI = false;
	}
}

