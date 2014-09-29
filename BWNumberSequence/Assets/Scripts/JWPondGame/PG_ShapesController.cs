using UnityEngine;
using System.Collections;

public class PG_ShapesController : MonoBehaviour {
	
	public GameObject[] shapesToDisplay;
	
	GameObject objectToMove,destinationObject, shapeContainer_1=null,shapeContainer_2=null , objectFrom=null , objectTo;
	ArrayList shapesToShowOnScreen;
	PG_ShapeStatController shapeStatController;
	ArrayList availableShapes, qSpriteIndices;
	private PG_IslandController placeIslands;
	float xPositon=0F, xPosition_2=0F, yPosition=0F, negativeAxisPos=-120F, contentWidth=0F, contentHeight=0F, distance, speed = 10.0F, startTime, smooth = 5.0F;
	
	// Content width and height for container 1,2
	float contentWidthContainer_1=0F, contentWidthContainer_2=0F, contentHeightContainer_1=0F, contentHeightContainer_2=0F, incrementInXIndex=200F, incrementInYIndex=180F;
	float landMassContentWidth=0F;      // width of the total answer shapes
	float xPosition=0F,incrementInXIndexforLandMasses=180F;
	private LayerMask layerMask;
	bool startAniamtion=false;
	
	string ansShapeTag;
	int ansShapeIndex, i=0, answerSpriteIndex, answerShapeIndex;
	int shapesCountForContainer_1=0;
	int shapesCountForContainer_2=0;
	int shapeConainer1QuickSand=0;
	int shapeConainer2QuickSand=0;
	
	
	
	
	private void initializeDataContainers(){
		availableShapes=new ArrayList();
		shapesToShowOnScreen = new ArrayList();
		availableShapes.Add("Circle"); 
		availableShapes.Add("Diamond");
		availableShapes.Add("Heart");
		availableShapes.Add("Hexagon");
		availableShapes.Add("Octagon");
		availableShapes.Add("Oval");
		availableShapes.Add("Pentagon");
		availableShapes.Add("Rectangle");
		availableShapes.Add("Square");
		availableShapes.Add("Star");
		availableShapes.Add("Triangle");
		
		qSpriteIndices=new ArrayList();
	}
	
	

	//--------- Prepare Question Sprite Indices
	private void makeQSpriteIndices(ArrayList availableShapesForLevel, ArrayList availableShapes) // available shapes for specific level, 11 possible shapes
	{
		for(int i=0;i<availableShapesForLevel.Count;i++)
			for(int j=0;j<availableShapes.Count;j++)
				if(string.Compare((string)availableShapesForLevel[i],(string)availableShapes[j],true)==0)
					qSpriteIndices.Add(j);
	  //  Debug.Log("Available Shapes for Level Count: "+availableShapesForLevel.Count);
	}
	
	
	
	
//		//--------- Prepare Answer Sprite Indices
	private void makeASpriteIndices(string ansSpriteTitle,ArrayList availableShapes) {
		for(int i=0;i<availableShapes.Count;i++)
			if(string.Compare(ansSpriteTitle,(string)availableShapes[i],true)==0){
						answerSpriteIndex=i;
						break;
			}   
			
	}
	

	public void placeAvailableShapes(ArrayList shapes,string answerSpriteTitle){
		initializeDataContainers();
		makeQSpriteIndices(shapes,this.availableShapes);     // finalAvailAbleShapes, 11 shapes
		makeASpriteIndices(answerSpriteTitle,this.availableShapes);
		placeSpritesInScene();
	}
	
	// Place sprites in the scene
	private void placeSpritesInScene(){ 
		
		// ------- Get the first shape container
		shapeContainer_1=GameObject.FindGameObjectWithTag("pg_shapes_container_1");
		
		// ------- Get the second shape container
		shapeContainer_2=GameObject.FindGameObjectWithTag("pg_shapes_container_2");
		transform.localScale=new Vector3(1.0F,1.0F,1.0F);
		shapesCountForContainer_1=0;
		shapesCountForContainer_2=0;
		shapeConainer1QuickSand =0;
		shapeConainer2QuickSand=0;
		xPositon=100F;
		xPosition_2=100F;
		int shapesCount=qSpriteIndices.Count;
		
	//	print("Shapes Count:  "+shapesCount);
		for(int i=0;i<shapesCount;i++)
		{
			int qIndex=(int)qSpriteIndices[i];
			GameObject sObject=Instantiate(shapesToDisplay[qIndex]) as GameObject;
			shapesToShowOnScreen.Add(sObject);
			if(checkIfAnswer(qIndex)){   
				int childCount=0;
				
				Transform [] childs = sObject.GetComponentsInChildren<Transform>();
				Transform firstChild = childs[1];

				GameObject shape = firstChild.gameObject;
				shapeStatController=shape.GetComponent<PG_ShapeStatController>();
				shapeStatController.setAnswerShape(true);
				
			}
		
			
		if(shapesCount>4){
			if(i<=shapesCount/2-1){
					sObject.transform.parent=shapeContainer_1.transform;
					sObject.transform.localPosition=new Vector3(xPositon,0F, -sObject.transform.localPosition.z);
					xPositon+=incrementInXIndex;
					shapesCountForContainer_1++;	
		    }
				
			else if(i>shapesCount/2-1){
					sObject.transform.parent=shapeContainer_2.transform;
					sObject.transform.localPosition=new Vector3(xPosition_2,0F, -sObject.transform.localPosition.z);
					xPosition_2+=incrementInXIndex;
					shapesCountForContainer_2++;
			}		
		}
		else{
					sObject.transform.parent=shapeContainer_1.transform;
					sObject.transform.localPosition=new Vector3(xPositon,0F, -sObject.transform.localPosition.z);
					xPositon+=incrementInXIndex;
					shapesCountForContainer_1++;		
		}

		}
		
			float contentWidth_1 =shapesCountForContainer_1*200.0f/ 2.0f;
			float contentWidth_2 =shapesCountForContainer_2*200.0f/ 2.0f;
			shapeContainer_1.transform.localPosition=new Vector3(-contentWidth_1 , 80F , -shapeContainer_1.transform.localPosition.z);
			
		
		
		if(shapesCountForContainer_1 > 0 && shapesCountForContainer_2 > 0)
		{
			//print ("inside ....");
			//transform.localScale=new Vector3(0.7F,0.7F,1F);
			//shapeContainer_1.transform.localScale=new Vector3(0.9F,0.9F,1F);
			//shapeContainer_2.transform.localScale=new Vector3(0.9F,0.9F,1F);
			shapeContainer_2.transform.localPosition=new Vector3(-contentWidth_2 , -80F , -shapeContainer_2.transform.localPosition.z);
		}
		else 
		{
			Vector3 pos=shapeContainer_1.transform.position;
			pos.y -= 80F;
			shapeContainer_1.transform.transform.position=pos;
		}
		
	
		
	}
	
	public void showQuickSand(int _sand){
		Debug.Log("Quick Sand: shapes count "+shapesToShowOnScreen.Count);
		for(int i=0; i<shapesToShowOnScreen.Count;i++){
			GameObject shape = (GameObject)shapesToShowOnScreen[i];
				Debug.Log("Shape: "+shape);
			    Debug.Log("Shape Name: "+shape.name);
			Transform[] shapeTransform =shape.GetComponentsInChildren<Transform>();
			GameObject quick_sand = shapeTransform[2].gameObject;
			if(quick_sand!=null && quick_sand.name=="quick_sand"){
				PG_QuickSandController quickSandController = quick_sand.GetComponent<PG_QuickSandController>();
			    quickSandController.showQuickSand(_sand);
		 }
		}
		
		
		
//			shapeConainer1QuickSand++;
//			if(shapeContainer_1!=null && shapesCountForContainer_1>0 && shapeConainer1QuickSand<=shapesCountForContainer_1){
//			foreach(Transform child in shapeContainer_1.transform){
//			  GameObject childObject=child.gameObject;
//			  Transform[] shapeTransform =childObject.GetComponentsInChildren<Transform>();
//					if(shapeTransform.Length>=3){
//					GameObject shape = shapeTransform[2].gameObject;
//					
//				if(shape!=null && shape.name=="quick_sand"){
//					PG_QuickSandController quickSandController = shape.GetComponent<PG_QuickSandController>();
//				    quickSandController.showQuickSand(_sand);
//				}
//			}
//			}
//			}
//			
//			if(shapeContainer_2 !=null && shapesCountForContainer_2 > 0 && shapeConainer2QuickSand<=shapesCountForContainer_2){
//			foreach(Transform child in shapeContainer_2.transform)
//			{
//			  GameObject childObject=child.gameObject;
//			  Transform[] shapeTransform =childObject.GetComponentsInChildren<Transform>();
//				if(shapeTransform.Length>=3){
//				GameObject shape = shapeTransform[2].gameObject;
//				if(shape!=null && shape.name=="quick_sand"){	
//				PG_QuickSandController quickSandController = shape.GetComponent<PG_QuickSandController>();
//				quickSandController.showQuickSand(_sand);
//				}
//			}
//			}
//			}
	}
	
	
	
	
	//--------
	private bool checkIfAnswer(int questionIndex){
		
			if(questionIndex==answerSpriteIndex){
				return true;
			}
		
		return false;
	}
	
	
	
	//--------
	public IEnumerator replaceEmptyLandmassWithFilled(GameObject objectTo, float time)
	{
			yield return new WaitForSeconds(time); 
			PG_LandmassController landmassStatController =objectTo.GetComponent<PG_LandmassController>();
			landmassStatController.SetFilled();
	}
	
	
	//------
	public int getAnswerSpriteIndex()
	{
		return answerSpriteIndex;
	}
	
	
	//------
	public ArrayList getQSpriteIndices()
	{
		return qSpriteIndices;
		
	}
	
	
	
	
}
