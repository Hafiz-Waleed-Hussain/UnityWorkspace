using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Animation))]

public class PG_IslandController : MonoBehaviour {
	
	string[ , ] islandNamesForBackground_1 = new string[ , ]{ {"pg_bg_mountain_island_left_1" , "pg_bg_mountain_island_right_1"} , {"pg_bg_mountain_island_left_2" , "pg_bg_mountain_island_right_2"} , 
														  	  {"pg_bg_mountain_island_left_3" , "pg_bg_mountain_island_right_3"} , {"pg_bg_mountain_island_left_4" , "pg_bg_mountain_island_right_4"}}; 
	string[ , ] islandNamesForBackground_2 = new string[ , ]{ {"pg_bg_fl_island_left_1" 	  , "pg_bg_fl_island_right_1"}		 , {"pg_bg_fl_island_left_2"	   , "pg_bg_fl_island_right_2"} ,
														      {"pg_bg_fl_island_left_3"       , "pg_bg_fl_island_right_3"} 		 , {"pg_bg_fl_island_left_4"        , "pg_bg_fl_island_right_4"} }; 
	public  GameObject [] islandsForBackground_1, islandsForBackground_2 , landMasses ;
	private GameObject [] landMassesPlacedBWIslands=null ;
	float [,] islandWidthHeight_Left = 	new float[,]{	{554,158},{417,155},{344,155},{282,153} };
	float [,] islandWidthHeight_Right = new float[,]{	{541,157},{394,154},{331,153},{263,156} };
	float ansShapesCount , xPosition=0F , incrementInXIndexforLandMasses=160 ,landMassContentWidth=0F  ; //  landMassContentWidth:=> width of the total answer shapes
	GameObject leftTree, rightTree, leaf1, leaf2, leaf3, leaf4,ansShapesContainer;
	
	
	void Start(){
		 
	}
	
	//--------- Place Islands islandsAndLandmass
	public void placeIslandsAndLandmasses(int answerSpriteIndex, int answerSpriteCount , int background)
	{
		leftTree	= GameObject.Find("tree_left");
		rightTree	= GameObject.Find("tree_right");
		
		if(leaf1 == null && leaf2 == null && leaf3 == null && leaf4 == null ){
		leaf1 		= GameObject.Find("pg_leaf_1");
		leaf2 		= GameObject.Find("pg_leaf_1_large");
		leaf3		= GameObject.Find("pg_leaf_2");
		leaf4 		= GameObject.Find("pg_leaf_3");  
		}
		if(answerSpriteCount>0){
			this.ansShapesCount=answerSpriteCount;
		if(background==1){
				setIslandsForBackground_1(answerSpriteCount);
				hideLeaves();
			}
		else{
				setIslandsForBackground_2(answerSpriteCount);
				showLeaves();
			}
		landMassesPlacedBWIslands=new GameObject[answerSpriteCount];
		placeLandMasses(answerSpriteIndex , answerSpriteCount , background); // place landmasses	
		}
	}
	
	
	//  -------- Place islands for mountain background
	private void setIslandsForBackground_1(int answerSpriteCount){
		GameObject gObject=Instantiate(islandsForBackground_1[answerSpriteCount-1]) as GameObject;
			gObject.transform.parent=transform;
			gObject.transform.localPosition=new Vector3(0F,0F,0F);
			
		
		
		
		if(leftTree!=null && rightTree!=null){
			
		float screenResolution=Screen.currentResolution.width;
		float scaleFactor=screenResolution/PG_Constants.PG_SCREEN_WIDTH;
		float treeScale= rightTree.transform.localScale.x;
		leftTree.transform.renderer.enabled =true;
		rightTree.transform.renderer.enabled =true;
		leftTree.transform.position  = new Vector3((float)((-screenResolution/2F)/scaleFactor)+(treeScale/3F) , 194.6538F ,-5F);
		rightTree.transform.position = new Vector3((float)((screenResolution/2F)/scaleFactor)-(treeScale/3F), 194.6538F ,-5F);
			float posX = (float)-screenResolution/2+50;
			float NegX = (float)screenResolution/2-50;
		}
	}
	
	
	void showLeaves(){
		
		
//		leaf1.transform.renderer.enabled = true;
//		leaf2.transform.renderer.enabled = true;
//		leaf3.transform.renderer.enabled = true;
//		leaf4.transform.renderer.enabled = true;
		
		leaf1.SetActive(true);
		leaf2.SetActive(true);
		leaf3.SetActive(true);
		leaf4.SetActive(true);
	}
	
	
	void hideLeaves(){
//		leaf1 = GameObject.Find("pg_leaf_1");
//		leaf2 = GameObject.Find("pg_leaf_1_large");
//		leaf3 = GameObject.Find("pg_leaf_2");
//		leaf4 = GameObject.Find("pg_leaf_3");  
//		
//		leaf1.transform.renderer.enabled = false;
//		leaf2.transform.renderer.enabled = false;
//		leaf3.transform.renderer.enabled = false;
//		leaf4.transform.renderer.enabled = false;
		
		leaf1.SetActive(false);
		leaf2.SetActive(false);
		leaf3.SetActive(false);
		leaf4.SetActive(false);
		
		
	}
	
	//  ------ Place islands for floating leaves
	private void setIslandsForBackground_2(int answerSpriteCount){
		
		GameObject gObject=Instantiate(islandsForBackground_2[answerSpriteCount-1]) as GameObject;
			gObject.transform.parent=transform;
			gObject.transform.localPosition=new Vector3(0F,0F,0F);
		
		if(leftTree!=null && rightTree!=null){
					
		leftTree.transform.renderer.enabled =false;
		rightTree.transform.renderer.enabled =false;
		}
	}
	
	//--------- Place landmasses between islands
	public void placeLandMasses(int answerSpriteIndex,int answerSpriteCount , int background){
	    ansShapesContainer=GameObject.FindGameObjectWithTag("pg_landmass_container"); //  answer shapes container
		GameObject shape;
		PG_LandmassController prefabController;
		landMassContentWidth=0F;
		xPosition=0F;
		
		if(ansShapesCount>=2){		
			for(int i=0; i<answerSpriteCount;i++){
					shape=Instantiate(landMasses[answerSpriteIndex]) as GameObject;  // answer shape
				    prefabController=shape.GetComponent<PG_LandmassController>(); 
					prefabController.SetEmpty();	
					
					shape.transform.parent=ansShapesContainer.transform;
					Bounds  bounds= shape.renderer.bounds;
					landMassContentWidth+=bounds.extents.x;
					
					shape.transform.localPosition=new Vector3(xPosition,0F,-10F);
					xPosition+=incrementInXIndexforLandMasses;
					landMassesPlacedBWIslands[i]=shape;
				}
			setLandMassContainerPosition(background,answerSpriteCount);
		 }
	 else{
			shape=Instantiate(landMasses[answerSpriteIndex]) as GameObject;  // answer shapes
			prefabController=shape.GetComponent<PG_LandmassController>(); 
			shape.transform.parent=ansShapesContainer.transform;
			shape.transform.localPosition=new Vector3(xPosition,0F,-10F);
			landMassesPlacedBWIslands[0]=shape;
			setLandMassContainerPosition(background,answerSpriteCount);
		}
		
	}
	
	
	
	void setLandMassContainerPosition(int background, int answerSpriteCount)
	{
		if(background==1)
		 switch(answerSpriteCount){
			case 1:
				ansShapesContainer.transform.localPosition=new Vector3(-landMassContentWidth-10F,ansShapesContainer.transform.localPosition.y,-20);
				break;
			case 2:
				ansShapesContainer.transform.localPosition=new Vector3(-landMassContentWidth+73F,ansShapesContainer.transform.localPosition.y,-20);
				break;
			case 3:
				ansShapesContainer.transform.localPosition=new Vector3(-landMassContentWidth+60F,ansShapesContainer.transform.localPosition.y,-20);
				break;
			case 4:
				ansShapesContainer.transform.localPosition=new Vector3(-landMassContentWidth+40F,ansShapesContainer.transform.localPosition.y,-20);
				break;
			}
				
	 else switch(answerSpriteCount){
			case 1:
				ansShapesContainer.transform.localPosition=new Vector3(-landMassContentWidth-30F,ansShapesContainer.transform.localPosition.y,-20);
				break;
			case 2:
				ansShapesContainer.transform.localPosition=new Vector3(-landMassContentWidth+46F,ansShapesContainer.transform.localPosition.y,-20);
				break;
			case 3:
				ansShapesContainer.transform.localPosition=new Vector3(-landMassContentWidth+34F,ansShapesContainer.transform.localPosition.y,-20);
				break;
			case 4:
				ansShapesContainer.transform.localPosition=new Vector3(-landMassContentWidth+40F,ansShapesContainer.transform.localPosition.y,-20);
				break;
			}
	}
	
	
	public ArrayList ReplaceEmptyLandMassesWithFilledOne(int answerSpriteCount) //getAnswersListCount
	{
//		Debug.Log("answerSpriteCount: "+answerSpriteCount);
		GameObject shapeObject=null;
		ArrayList responseList=new ArrayList();
		PG_LandmassController landmassStatController = null;
		int index=-1;
			
			for(int i=0; i<answerSpriteCount;i++)
			{
				shapeObject=landMassesPlacedBWIslands[i];
			if(shapeObject != null)
			 {
				landmassStatController=shapeObject.GetComponent<PG_LandmassController>();
				index++;
			 }
			//else 
			//	Debug.Log("some thing wrong");
			
			if(landmassStatController != null && (bool)landmassStatController.getLandmassStatus()==false)
				{
					responseList.Add(shapeObject);
					break;
				}
			}
		
		if(index==answerSpriteCount-1)
			responseList.Add(true);
		 else 	
			responseList.Add(false);
		return responseList;
	}
	
	
	
//	public ArrayList ReplaceEmptyLandMassesWithFilledOne(int answerSpriteCount , int landMassIndex) //getAnswersListCount
//	{
//		Debug.Log("answerSpriteCount: "+answerSpriteCount);
//		GameObject shapeObject=null;
//		ArrayList responseList=new ArrayList();
//		LandmassController landmassStatController = null;
//		int myIndex=-1;
//		int index;	
//			for(index=landMassIndex; index<answerSpriteCount ;index++)
//			{
//				shapeObject=landMassesPlacedBWIslands[index];
//			if(shapeObject != null)
//			 {
//				landmassStatController=shapeObject.GetComponent<LandmassController>();
//				myIndex=index;
//			 }
//			else 
//				Debug.Log("some thing wrong");
//			
//			if(landmassStatController != null && (bool)landmassStatController.getLandmassStatus()==false)
//				{
//					responseList.Add(shapeObject);
//					break;
//				}
//			}
//		
//		Debug.Log("landMassIndex: "+myIndex);
//		if(myIndex==answerSpriteCount-1)
//			responseList.Add(true);
//		 else 	
//			responseList.Add(false);
//		return responseList;
//	}
	
	

}



