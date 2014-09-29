using UnityEngine;
using System.Collections;

public class GUI_Calculate : MonoBehaviour {
	
	/// <summary>
	/// position.x = screen width
	/// position.y = screen width
	/// size.x = screen width
	/// size.y = screen width
	/// </summary>
	public static Rect RectWithScreen_Width(Vector2 _position, Vector2 _size){
		Rect returnVal = new Rect();
		returnVal.x = (_position.x*Factor().x) - ((_size.x*Factor().x)/2);
		returnVal.y = (_position.y*Factor().x) - ((_size.y*Factor().x)/2);
		returnVal.width = _size.x*Factor().x;
		returnVal.height = _size.y*Factor().x;
		return returnVal;
	}

	/// <summary>
	/// position.x = screen height
	/// position.y = screen height
	/// size.x = screen height
	/// size.y = screen height
	/// </summary>
	public static Rect RectWithScreen_Height(Vector2 _position, Vector2 _size){
		Rect returnVal = new Rect();
		returnVal.x = (_position.x*Factor().y) - ((_size.x*Factor().y)/2);
		returnVal.y = (_position.y*Factor().y) - ((_size.y*Factor().y)/2);
		returnVal.width = _size.x*Factor().y;
		returnVal.height = _size.y*Factor().y;
		return returnVal;
	}
	
	/// <summary>
	/// position.x = screen width
	/// position.y = screen height
	/// size.x = screen width
	/// size.y = screen height
	/// </summary>
	public static Rect RectWithScrren_WidthAndHeight_SizeMix(Vector2 _position, Vector2 _size){
		Rect returnVal = new Rect();
		returnVal.x = (_position.x*Factor().x) - ((_size.x*Factor().x)/2);
		returnVal.y = (_position.y*Factor().y) - ((_size.y*Factor().y)/2);
		returnVal.width = _size.x*Factor().x;
		returnVal.height = _size.y*Factor().y;
		return returnVal;
	}
	
	/// <summary>
	/// position.x = screen width
	/// position.y = screen height
	/// size.x = screen width
	/// size.y = screen width
	/// [============ Work ============]
	/// </summary>
	
	public static Rect RectWithScrren_WidthAndHeight_SizeWidth(Vector2 _position, Vector2 _size){
		Rect returnVal = new Rect();
		returnVal.x = (_position.x*Factor().x) - ((_size.x*Factor().x)/2);
		returnVal.y = (_position.y*Factor().y) - ((_size.y*Factor().y)/2);
		returnVal.width = _size.x*Factor().x;
		returnVal.height = _size.y*Factor().x;
		return returnVal;
	}
	
	/// <summary>
	/// position.x = screen width
	/// position.y = screen height
	/// size.x = screen heigth
	/// size.y = screen heigth
	/// </summary>
	public static Rect RectWithScrren_WidthAndHeight_Sizeheight(Vector2 _position, Vector2 _size){
		Rect returnVal = new Rect();
		returnVal.x = (_position.x*Factor().x) - ((_size.x*Factor().x)/2);
		returnVal.y = (_position.y*Factor().y) - ((_size.y*Factor().y)/2);
		returnVal.width = _size.x*Factor().y;
		returnVal.height = _size.y*Factor().y;
		return returnVal;
	}
	
	public static int FontSize(int _fontSize){
		int returnVal = (int)(Factor().x*10)*(int)_fontSize;
		if(returnVal <= 0){
			returnVal = 1;	
		}
		return returnVal;
	}
	
	private static Vector2 Factor(){
		Vector2 returnVal = Vector2.zero;
		returnVal.x = (float)Screen.width/2048;
		returnVal.y = (float)Screen.height/1024;
		return returnVal;
	}
	
}
