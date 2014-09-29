/// <summary>
/// Shop character.
/// This script use for create shop menu
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShopCharacter : MonoBehaviour {

	[System.Serializable]
	public class CharacterData
	{
		public GameObject player;
		public string name;
		public int price;
		public bool isUnLock;
	}

	[System.Serializable]
	public class Button
	{
		public Rect rect;
		public Texture2D normal;
		public Texture2D active;
	}

	[System.Serializable]
	public class Label
	{
		public GUIText guiText;
		public string text;
		public int fontSize;
		public Color fontColor;
		public FontStyle fontStyle;
		public TextAnchor alignment;
		public Rect rect;
	}

	public CharacterData[] players;
	public float factorSpace;
	public int coin;

	public Button btnBuy, btnSelect, btnOnSelect, btnBack, btnArrowLeft, btnArrowRight, iconCoinBuy, iconCoinCurrent;
	public Label guiLabelName, guiLabelPrice, guiLabelCoinCurrent;
	
	public AudioClip sfxButton;
	
	private CalOnGUI calGUI;
	private int indexSelect;
	private int selecCorrect;
	private List<Vector3> point = new List<Vector3> ();
	private Vector3 getMousePos;
	[HideInInspector] public GUIStyle guiStyleBtnBuy;
	[HideInInspector] public GUIStyle guiStyleBtnSelect;
	[HideInInspector] public GUIStyle guiStyleBtnOnSelect;
	[HideInInspector] public GUIStyle guiStyleBtnBack;
	[HideInInspector] public GUIStyle guiStyleBtnArrowLeft;
	[HideInInspector] public GUIStyle guiStyleBtnArrowRight;

	void SetStyle(){
		guiStyleBtnBuy.normal.background = btnBuy.normal;
		guiStyleBtnBuy.active.background = btnBuy.active;
		guiStyleBtnSelect.normal.background = btnSelect.normal;
		guiStyleBtnSelect.active.background = btnSelect.active;
		guiStyleBtnBack.normal.background = btnBack.normal;
		guiStyleBtnBack.active.background = btnBack.active;
		guiStyleBtnOnSelect.normal.background = btnOnSelect.normal;
		guiStyleBtnOnSelect.active.background = btnOnSelect.active;
		guiStyleBtnArrowLeft.normal.background = btnArrowLeft.normal;
		guiStyleBtnArrowLeft.active.background = btnArrowLeft.active;
		guiStyleBtnArrowRight.normal.background = btnArrowRight.normal;
		guiStyleBtnArrowRight.active.background = btnArrowRight.active;
	}

	void ResetData(){
		for (int i = 0; i < players.Length; i++) {
			PlayerPrefs.SetString("Player_"+i, "False");
		}
	}

	void Start(){
		//ResetData ();
		SetStyle ();
		coin = GameData.LoadCoin ();
		selecCorrect = PlayerPrefs.GetInt ("SelectPlayer");
		Vector3 pos = Vector3.zero;
		for (int i = 0; i < players.Length; i++) {
			players [i].player.transform.localPosition = new Vector3 (pos.x + (i * factorSpace), 0, 0);
			point.Add (new Vector3 (-1 * (pos.x + (i * factorSpace)) + transform.position.x, transform.position.y, transform.position.z));

			if (i == 0) {
				players [i].isUnLock = true;
			} else {
				Debug.Log (PlayerPrefs.GetString ("Player_" + i));
				if(PlayerPrefs.GetString ("Player_" + i) == ""){
					players [i].isUnLock = false;
				}else{
					players [i].isUnLock = bool.Parse (PlayerPrefs.GetString ("Player_" + i));
				}

			}
		}

		StartCoroutine (WaitInput ());
	}

	void OnGUI(){
		SetStyle ();
		
		guiLabelName.guiText.text = players[indexSelect].name;
		
		guiLabelName.guiText.pixelOffset = new Vector2(GUI_Calculate.RectWithScrren_WidthAndHeight_Sizeheight(new Vector2(guiLabelName.rect.x, guiLabelName.rect.y),
																									new Vector2(guiLabelName.rect.width, guiLabelName.rect.height)).x,
											GUI_Calculate.RectWithScrren_WidthAndHeight_Sizeheight(new Vector2(guiLabelName.rect.x, guiLabelName.rect.y),
																									new Vector2(guiLabelName.rect.width, guiLabelName.rect.height)).y);
		guiLabelName.guiText.fontSize = GUI_Calculate.FontSize(guiLabelName.fontSize);
		
		
		guiLabelCoinCurrent.guiText.text = coin.ToString ();
		
		guiLabelCoinCurrent.guiText.pixelOffset = new Vector2(GUI_Calculate.RectWithScrren_WidthAndHeight_Sizeheight(new Vector2(guiLabelCoinCurrent.rect.x, guiLabelCoinCurrent.rect.y),
																									new Vector2(guiLabelCoinCurrent.rect.width, guiLabelCoinCurrent.rect.height)).x,
											GUI_Calculate.RectWithScrren_WidthAndHeight_Sizeheight(new Vector2(guiLabelCoinCurrent.rect.x, guiLabelCoinCurrent.rect.y),
																									new Vector2(guiLabelCoinCurrent.rect.width, guiLabelCoinCurrent.rect.height)).y);
		guiLabelCoinCurrent.guiText.fontSize = GUI_Calculate.FontSize(guiLabelCoinCurrent.fontSize);
		
		GUI.DrawTexture(GUI_Calculate.RectWithScrren_WidthAndHeight_Sizeheight(new Vector2(iconCoinCurrent.rect.x, iconCoinCurrent.rect.y),
		                                                                       new Vector2(iconCoinCurrent.rect.width, iconCoinCurrent.rect.height)), iconCoinCurrent.normal);

	
		if (players [indexSelect].isUnLock == false) 
		{

			GUI.DrawTexture(GUI_Calculate.RectWithScrren_WidthAndHeight_Sizeheight(new Vector2(iconCoinBuy.rect.x, iconCoinBuy.rect.y),
			                                                                       new Vector2(iconCoinBuy.rect.width, iconCoinBuy.rect.height)), iconCoinBuy.normal);

			guiLabelPrice.guiText.enabled = true;
			guiLabelPrice.guiText.text = players[indexSelect].price.ToString();
			guiLabelPrice.guiText.pixelOffset = new Vector2(GUI_Calculate.RectWithScrren_WidthAndHeight_Sizeheight(new Vector2(guiLabelPrice.rect.x, guiLabelPrice.rect.y),
																									new Vector2(guiLabelPrice.rect.width, guiLabelPrice.rect.height)).x,
											GUI_Calculate.RectWithScrren_WidthAndHeight_Sizeheight(new Vector2(guiLabelPrice.rect.x, guiLabelPrice.rect.y),
																									new Vector2(guiLabelPrice.rect.width, guiLabelPrice.rect.height)).y);
			guiLabelPrice.guiText.fontSize = GUI_Calculate.FontSize(guiLabelPrice.fontSize);
			
			
			if(GUI.Button(GUI_Calculate.RectWithScrren_WidthAndHeight_Sizeheight(new Vector2(btnBuy.rect.x, btnBuy.rect.y),
				                                                                     new Vector2(btnBuy.rect.width, btnBuy.rect.height)),"",guiStyleBtnBuy)){
				if(coin >= players[indexSelect].price)
				{
					coin -= players[indexSelect].price;
					GameData.SaveCoin(coin);
					players[indexSelect].isUnLock = true;
					PlayerPrefs.SetString("Player_"+indexSelect, "True");
					PlayerPrefs.SetInt("SelectPlayer", selecCorrect);
					Debug.Log("Buy : "+indexSelect+ " : " + PlayerPrefs.GetString("Player_"+indexSelect));
				}
				
				if(sfxButton != null)
				AudioSource.PlayClipAtPoint(sfxButton,transform.position);
			}
		}

		if (players [indexSelect].isUnLock == true) 
		{
			guiLabelPrice.guiText.enabled = false;
			if(indexSelect == selecCorrect)
			{
				GUI.Button(GUI_Calculate.RectWithScrren_WidthAndHeight_Sizeheight(new Vector2(btnOnSelect.rect.x, btnOnSelect.rect.y),
					                                                                  new Vector2(btnOnSelect.rect.width, btnOnSelect.rect.height)), "", guiStyleBtnOnSelect );
			}
			else
			{
				if(GUI.Button(GUI_Calculate.RectWithScrren_WidthAndHeight_Sizeheight(new Vector2(btnSelect.rect.x, btnSelect.rect.y),
					                                                                     new Vector2(btnSelect.rect.width, btnSelect.rect.height)), "", guiStyleBtnSelect))
				{
					selecCorrect = indexSelect;
					PlayerPrefs.SetInt("SelectPlayer", selecCorrect);
					
					if(sfxButton != null)
					AudioSource.PlayClipAtPoint(sfxButton,transform.position);
				}
				
			}
		}

		if(GUI.Button(GUI_Calculate.RectWithScrren_WidthAndHeight_Sizeheight(new Vector2(btnArrowLeft.rect.x, btnArrowLeft.rect.y),
			                                                                     new Vector2(btnArrowLeft.rect.width, btnArrowLeft.rect.height)), "", guiStyleBtnArrowLeft )){
			indexSelect ++;
			if(indexSelect >= players.Length-1){
				indexSelect = players.Length-1;
			}
			
			if(sfxButton != null)
			AudioSource.PlayClipAtPoint(sfxButton,transform.position);
		}

		if(GUI.Button(GUI_Calculate.RectWithScrren_WidthAndHeight_Sizeheight(new Vector2(btnArrowRight.rect.x, btnArrowRight.rect.y),
			                                                                     new Vector2(btnArrowRight.rect.width, btnArrowRight.rect.height)), "", guiStyleBtnArrowRight )){
			indexSelect --;
			if(indexSelect <= 0){
				indexSelect = 0;
			}
			
			if(sfxButton != null)
			AudioSource.PlayClipAtPoint(sfxButton,transform.position);
		}

		if(GUI.Button(GUI_Calculate.RectWithScrren_WidthAndHeight_Sizeheight(new Vector2(btnBack.rect.x, btnBack.rect.y),
			                                                                     new Vector2(btnBack.rect.width, btnBack.rect.height)), "", guiStyleBtnBack )){
			PlayerPrefs.SetInt("SelectPlayer", selecCorrect);
			Application.LoadLevel("TitleScene");
			
			if(sfxButton != null)
			AudioSource.PlayClipAtPoint(sfxButton,transform.position);
		}
	}

	IEnumerator WaitInput(){

		bool input = false;

		while (input == false) {
			if(Input.GetMouseButtonDown(0)){
				getMousePos = Input.mousePosition;
				input = true;
			}
			yield return 0;
		}
		StartCoroutine (SelectDirection ());
	}

	IEnumerator SelectDirection(){
		bool input = false;
		while (input == false) {
			if((Input.mousePosition.x - getMousePos.x) < -40){
				indexSelect++;
				if(indexSelect >= players.Length-1){
					indexSelect = players.Length-1;
				}
				input = true;
			}

			if((Input.mousePosition.x - getMousePos.x) > 40){
				indexSelect--;
				if(indexSelect <= 0){
					indexSelect = 0;
				}
				input = true;
			}

			if(Input.GetMouseButtonUp(0)){
				input = true;
			}
			yield return 0;
		}

		StartCoroutine (MoveToPoint ());
	}

	IEnumerator MoveToPoint(){
		while (Vector3.Distance(transform.position, point[indexSelect]) > 0.01f) {
			transform.position = Vector3.Lerp(transform.position, point[indexSelect], 10 * Time.deltaTime);
			yield return 0;
		}

		transform.position = point [indexSelect];

		StartCoroutine (WaitInput ());
	}
	
}
