using UnityEngine;
using System.Collections;

public class PG_Referent_ShapeHint : MonoBehaviour {
	
	private string[] referentShapesArray;
	
	void Awake()
	{
		referentShapesArray = new string []{"CIRCLE" , "DIAMOND" , "HEART" , "HEXAGON" , "OCTAGON" , "OVAL" , 
											"PENTAGON" , "RECTANGLE" , "SQUARE" , "STAR" , "TRIANGLE"};
	}
	
	public void hideHint()
	{
		transform.renderer.enabled=false;
	}
	
	
	public void showHint()
	{
		transform.renderer.enabled=true;
	}
	
	public void setShapeName(int index)
	{
		  GetComponent<TextMesh>().text = referentShapesArray[index];
	}
}
