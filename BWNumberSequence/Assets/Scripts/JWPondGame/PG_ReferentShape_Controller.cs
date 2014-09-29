using UnityEngine;
using System.Collections;

public class PG_ReferentShape_Controller : MonoBehaviour {
	
	private string[] 	referentShapesArray;
	private string[] 	ShapesArray;

	void Awake()
	{
		referentShapesArray = new string []{"CP_Circle_Referrent-hd" , "CP_Diamond_Referrent-hd" , "CP_Heart_Referrent-hd" , "CP_Hexagon_Referrent-hd" , "CP_Octagon_Referrent-hd" , "CP_Oval_Referrent-hd" , 
											"CP_Pentagon_Referrent-hd" , "CP_Rectangle_Referrent-hd" , "CP_Square_Referrent-hd" , "CP_Star_Referrent-hd" , "CP_Triangle_Referrent-hd" , "CP_tick-hd" , "CP_Cross-hd"};
		        ShapesArray = new string []{"CROSSINGPOND_CIRCLE_1-hd" , "CROSSINGPOND_DIAMOND_1-hd" , "CROSSINGPOND_HEART_1-hd" , "CROSSINGPOND_HEXAGON_1-hd" , "CROSSINGPOND_OCTAGON_1-hd" , "CROSSINGPOND_OVAL_1-hd" , 
											"CROSSINGPOND_PENTAGON_1-hd" , "CROSSINGPOND_RECTANGLE_1-hd" , "CROSSINGPOND_SQUARE_1-hd" , "CROSSINGPOND_STAR_1-hd" , "CROSSINGPOND_TRIANGLE_1-hd"};
	}
	

	

	public void setReferentSprite(int index)
	{   
		int _randShape = Random.Range(1,3);
		if(index==500){
			Texture shapeHint	=   Resources.Load(PG_Constants._questionMark) as Texture;
			
			float width  = shapeHint.width;
			float height = shapeHint.height;
			float dividingFactorX = 3.2F;
			float dividingFactorY = 3.2F;
			transform.localScale= new Vector3((float)(width/dividingFactorX) , (float)(height/dividingFactorY),1);
			//Debug.Log("x: "+width/dividingFactorX+"y: "+height/dividingFactorY);
			renderer.material.mainTexture = shapeHint;
		}
		
		else if(_randShape==1){
			string referentShapeName		=	referentShapesArray[index];
			Texture shapeHint	=   Resources.Load(PG_Constants._referentsPath + referentShapeName) as Texture;
			
			float width  = shapeHint.width;
			float height = shapeHint.height;
			float dividingFactorX = 3.2F;
			float dividingFactorY = 3.2F;
			
			transform.localScale= new Vector3((float)(width/dividingFactorX) , (float)(height/dividingFactorY),1);
			//Debug.Log("x: "+width/dividingFactorX+"y: "+height/dividingFactorY);
			renderer.material.mainTexture = shapeHint;
		}
		else{
			string referentShapeName		=	ShapesArray[index];
			Texture shapeHint	=   Resources.Load(PG_Constants._shapes + referentShapeName) as Texture;
			
			float width  = shapeHint.width;
			float height = shapeHint.height;
			float dividingFactorX = 3.2F;
			float dividingFactorY = 3.2F ;
			transform.localScale= new Vector3((float)(width/dividingFactorX) , (float)(height/dividingFactorY),1F);
			//Debug.Log("x: "+width/dividingFactorX+"y: "+height/dividingFactorY);
			renderer.material.mainTexture = shapeHint;
		}
		
		scaleOut();
		
		}
	
	
	void scaleOut(){
		Vector3 scale=gameObject.transform.localScale;
		scale.x*=1.2F;
		scale.y*=1.2F;
		iTween.ScaleTo(gameObject,iTween.Hash("time",1.3 ,"scale",scale, "easetype",iTween.EaseType.easeOutSine , "oncompletetarget" ,gameObject, "oncomplete", "scaleIn" ));
	}
	void scaleIn(){
		Vector3 scale=gameObject.transform.localScale;
		scale.x/=1.2F;
		scale.y/=1.2F;
		iTween.ScaleTo(gameObject,iTween.Hash("time",1.3 ,"scale",scale, "easetype",iTween.EaseType.easeInSine , "oncompletetarget" ,gameObject, "oncomplete", "scaleOut"));
	}
	
	
	public void hideReferentShape()
	{
		transform.renderer.enabled=false;
	}
	
	public void showReferentShape()
	{
		transform.renderer.enabled=true;
	}
	
}
