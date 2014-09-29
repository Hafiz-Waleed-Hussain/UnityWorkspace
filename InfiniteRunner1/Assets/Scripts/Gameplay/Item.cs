/// <summary>
/// Item
/// this script use for control effect item(ex. duration item,effect item)
/// </summary>

using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {
	
	public float scoreAdd; //add money if item = coin
	public int decreaseLife; //decrease life if item = obstacle 
	[HideInInspector] public int itemID; //item id
	public float speedMove; //speed move for moving obstacle
	public float duration; // duration item
	public float itemEffectValue; // effect value(if item star = speed , if item multiply = multiply number)
	public ItemRotate itemRotate; // rotate item
	public GameObject effectHit; // effect when hit item
	

	[HideInInspector] public bool itemActive;

	[HideInInspector] public bool isEditing;
	[HideInInspector] public Object targetPref;
	[HideInInspector] public string scenePath;


	[HideInInspector] public Color colorPattern =  new Color(Color.white.r, Color.white.g, Color.white.b, 0.5f);
	
	public enum TypeItem{
		Null, Coin, Obstacle, Obstacle_Roll, ItemJump, ItemSprint, ItemMagnet, ItemMultiply, Moving_Obstacle
	}
	
	public TypeItem typeItem;
	
	[HideInInspector]
	public bool useAbsorb = false;
	
	public static Item instance;
	
	void Start(){
		instance = this;	
	}
	
	//Set item effect
	public void ItemGet(){
		if(GameAttribute.gameAttribute.deleyDetect == false){
			if(typeItem == TypeItem.Coin){
				HitCoin();
				//Play sfx when get coin
				SoundManager.instance.PlayingSound("GetCoin");
			}else if(typeItem == TypeItem.Obstacle){
				HitObstacle();
				//Play sfx when get hit
				SoundManager.instance.PlayingSound("HitOBJ");
			}else if(typeItem == TypeItem.Obstacle_Roll){
				if(Controller.instace.isRoll == false){
					HitObstacle();
					//Play sfx when get hit
					SoundManager.instance.PlayingSound("HitOBJ");
				}
			}else if(typeItem == TypeItem.ItemSprint){
				Controller.instace.Sprint(itemEffectValue,duration);
				//Play sfx when get item
				SoundManager.instance.PlayingSound("GetItem");
				HideObj();
				initEffect(effectHit);
			}else if(typeItem == TypeItem.ItemMagnet){
				Controller.instace.Magnet(duration);
				//Play sfx when get item
				SoundManager.instance.PlayingSound("GetItem");
				HideObj();
				initEffect(effectHit);
			}else if(typeItem == TypeItem.ItemJump){
				Controller.instace.JumpDouble(duration);
				//Play sfx when get item
				SoundManager.instance.PlayingSound("GetItem");
				HideObj();
				initEffect(effectHit);
			}else if(typeItem == TypeItem.ItemMultiply){
				Controller.instace.Multiply(duration);
				GameAttribute.gameAttribute.multiplyValue = itemEffectValue;
				//Play sfx when get item
				SoundManager.instance.PlayingSound("GetItem");
				HideObj();
				initEffect(effectHit);
			}else if(typeItem == TypeItem.Moving_Obstacle){
				HitObstacle();
				//Play sfx when get hit
				SoundManager.instance.PlayingSound("HitOBJ");
			}
		}
	}

	public void UseMovingItem(){
		StartCoroutine (MovingItem ());
	}

	IEnumerator MovingItem(){
		while (itemActive) {
			if(!GameAttribute.gameAttribute.pause)
			transform.Translate(Vector3.back * speedMove * Time.deltaTime);
			yield return 0;
		}
	}

	//Coin method
	private void HitCoin(){
		if(Controller.instace.isMultiply == false){
			GameAttribute.gameAttribute.coin += scoreAdd;
		}else{
			GameAttribute.gameAttribute.coin += (scoreAdd)*GameAttribute.gameAttribute.multiplyValue;
		}
		initEffect(effectHit);
		HideObj();
	}
	
	//Obstacle method
	private void HitObstacle(){
		if(GameAttribute.gameAttribute.ageless == false){
			if(Controller.instace.timeSprint <= 0){
				GameAttribute.gameAttribute.life -= decreaseLife;
				GameAttribute.gameAttribute.ActiveShakeCamera();
			}else{
				HideObj();
				GameAttribute.gameAttribute.ActiveShakeCamera();
			}
			
		}
	}
	
	//Spawn effect method
	private void initEffect(GameObject prefab){
		GameObject go = (GameObject) Instantiate(prefab, Controller.instace.transform.position, Quaternion.identity);
		go.transform.parent = Controller.instace.transform;
		go.transform.localPosition = new Vector3(go.transform.localPosition.x, go.transform.localPosition.y+0.5f, go.transform.localPosition.z);	
	}
	
	//Magnet method
	public IEnumerator UseAbsorb(GameObject targetObj){
		bool isLoop = true;
		useAbsorb = true;
		while(isLoop){
			this.transform.position = Vector3.Lerp(this.transform.position, targetObj.transform.position, GameAttribute.gameAttribute.speed*2f * Time.smoothDeltaTime);
			if(Vector3.Distance(this.transform.position, targetObj.transform.position) < 0.6f){
				isLoop = false;	
				SoundManager.instance.PlayingSound("GetCoin");
				HitCoin();
			}
			yield return 0;
		}
		Reset();
		StopCoroutine("UseAbsorb");
		yield return 0;
	}
	
	public void HideObj(){
		if(useAbsorb == false){
			this.transform.parent = null;
			this.transform.localPosition = new Vector3(-100,-100,-100);
		}
	}
	
	public void Reset(){
		itemActive = false;
		this.transform.position = new Vector3(-100,-100,-100);
		this.transform.parent = null;
		useAbsorb = false;
	}

	private bool isSelect = false;
	[HideInInspector] public GameObject point1, point2, point3;
	[HideInInspector] public GameObject textZ, textY;
	[HideInInspector] public Vector3 position1, position2, position3;

	[HideInInspector] public float distanceZ = 1, distanceY = 1;

	#if UNITY_EDITOR
	[ExecuteInEditMode]
	public void OnDrawGizmos(){
		if(Application.isPlaying == false && isEditing == true){
			if (UnityEditor.Selection.Contains (gameObject) && isSelect == false) {
				Debug.Log("Select");
				CreatePointSelect();
				isSelect = true;
			}else if(!UnityEditor.Selection.Contains (gameObject) && !UnityEditor.Selection.Contains (point1) 
			         && !UnityEditor.Selection.Contains (point2) && !UnityEditor.Selection.Contains (point3)
			         && !UnityEditor.Selection.Contains (textZ) && !UnityEditor.Selection.Contains (textY) && isSelect == true){
				Debug.Log("Discount Select");
				DistroyPointSelect();
				isSelect = false;
			}
			
			if(point1 != null && point2 != null && point3 != null){
				Gizmos.color = Color.blue;
				point2.transform.position = new Vector3(transform.position.x, transform.position.y, point2.transform.position.z);
				Gizmos.DrawLine (point1.transform.position, point2.transform.position);
				Gizmos.color = Color.green;
				point3.transform.position = new Vector3(transform.position.x, point3.transform.position.y, transform.position.z);
				Gizmos.DrawLine (point1.transform.position, point3.transform.position);
				Gizmos.color = Color.green;
				Gizmos.DrawLine (point3.transform.position, new Vector3(point2.transform.position.x, point3.transform.position.y, point2.transform.position.z));

				position1 = point1.transform.position;
				position2 = point2.transform.position;
				position3 = point3.transform.position;


				textZ.transform.rotation = UnityEditor.SceneView.lastActiveSceneView.rotation;
				textY.transform.rotation = UnityEditor.SceneView.lastActiveSceneView.rotation;

				textZ.transform.position = point1.transform.position + ((point2.transform.position - point1.transform.position).normalized * Vector3.Distance(point1.transform.position, point2.transform.position))/2;
				textY.transform.position = point1.transform.position + ((point3.transform.position - point1.transform.position).normalized * Vector3.Distance(point1.transform.position, point3.transform.position))/2;
				distanceZ = Vector3.Distance (point2.transform.position, point1.transform.position);
				distanceY = Vector3.Distance (point3.transform.position, point1.transform.position);
				textZ.GetComponent<TextMesh> ().text = Vector3.Distance (point2.transform.position, point1.transform.position).ToString ("0.00");
				textY.GetComponent<TextMesh> ().text = Vector3.Distance (point3.transform.position, point1.transform.position).ToString ("0.00");
			}
		}
	}

	private void CreatePointSelect(){
		point1 = new GameObject ("Point1");
		point1.transform.parent = transform;
		if(position1.x == 0 && position1.y == 0 && position1.z == 0){
			point1.transform.position = transform.position;
		}else{
			point1.transform.position = position1;
		}
		point1.transform.localRotation = Quaternion.identity;

		point2 = new GameObject ("Point2");
		point2.transform.parent = transform;
		if(position2.x == 0 && position2.y == 0 && position2.z == 0){
			point2.transform.position = transform.position + transform.forward;
		}else{
			point2.transform.position = position2;
		}
		point2.transform.localRotation = Quaternion.identity;

		point3 = new GameObject ("Point3");
		point3.transform.parent = transform;
		if(position3.x == 0 && position3.y == 0 && position3.z == 0){
			point3.transform.position = transform.position + transform.up;
		}else{
			point3.transform.position = position3;
		}
		point3.transform.localRotation = Quaternion.identity;

		textZ = (GameObject)Instantiate ((Object)Resources.Load ("TextMesh"), Vector3.zero, Quaternion.identity);
		textY = (GameObject)Instantiate ((Object)Resources.Load ("TextMesh"), Vector3.zero, Quaternion.identity);

		textZ.transform.parent = transform;
		textY.transform.parent = transform;

		textZ.transform.position = (point2.transform.position - point1.transform.position) / 2;
		textY.transform.position = (point3.transform.position - point1.transform.position) / 2;

		textZ.transform.localScale = new Vector3 (0.01f, 0.01f, 0.01f);
		textY.transform.localScale = new Vector3 (0.01f, 0.01f, 0.01f);


		textZ.GetComponent<TextMesh> ().text = Vector3.Distance (point2.transform.position, point1.transform.position).ToString ("0.00");
		textY.GetComponent<TextMesh> ().text = Vector3.Distance (point3.transform.position, point1.transform.position).ToString ("0.00");

		textZ.GetComponent<TextMesh> ().fontSize = 100;
		textY.GetComponent<TextMesh> ().fontSize = 100;

		textZ.GetComponent<TextMesh> ().anchor = TextAnchor.UpperCenter;
		textY.GetComponent<TextMesh> ().anchor = TextAnchor.UpperCenter;

		point1.AddComponent<PointSelection> ();
		point2.AddComponent<PointSelection> ();
		point3.AddComponent<PointSelection> ();
		point1.GetComponent<PointSelection> ().color =  (Color.yellow);
		point2.GetComponent<PointSelection> ().color = (Color.blue);
		point3.GetComponent<PointSelection> ().color = (Color.green);
	}

	private void DistroyPointSelect(){
		if(point1 != null && point2 != null && point3 != null){
			DestroyImmediate (point1.gameObject);
			DestroyImmediate (point2.gameObject);
			DestroyImmediate (point3.gameObject);
			DestroyImmediate (textZ.gameObject);
			DestroyImmediate (textY.gameObject);
		}
	}
	#endif
}
