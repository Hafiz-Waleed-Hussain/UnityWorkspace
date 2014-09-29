/// <summary>
/// Cal on GUI
/// This script is use for calculate GUI with screen ratio
/// </summary>

using UnityEngine;
using System.Collections;

public class CalOnGUI : MonoBehaviour {
	
	[HideInInspector] public Vector2 GlobalFactor;
	
	public Rect SetGUI(float _x, float _y, float _size){
		Vector2 factor = SetFactor();
		Rect rect = new Rect();
		rect.x = (_x*factor.x) - ((_size*factor.x)/2);
		rect.y = (_y*factor.x) - ((_size*factor.x)/2);
		rect.width = _size*factor.x;
		rect.height = _size*factor.x;
		return rect;
	}
	
	public Rect SetGUI(float _x, float _y, float _size, bool useY){
		Vector2 factor = SetFactor();
		Rect rect = new Rect();
		rect.x = (_x*factor.y) - ((_size*factor.y)/2);
		rect.y = (_y*factor.y) - ((_size*factor.y)/2);
		rect.width = _size*factor.y;
		rect.height = _size*factor.y;
		return rect;
	}
	
	public Rect SetGUI(float _x, float _y, float _sizeX, float _sizeY, bool nonCenter){
		Vector2 factor = SetFactor();
		Rect rect = new Rect();
		rect.x = (_x*factor.x);
		rect.y = (_y*factor.y) - ((_sizeY*factor.x)/2);
		rect.width = _sizeX*factor.x;
		rect.height = _sizeY*factor.x;
		return rect;
	}
	
	public Rect SetGUI(float _x, float _y, float _sizeX, float _sizeY){
		Vector2 factor = SetFactor();
		Rect rect = new Rect();
		rect.x = (_x*factor.x) - ((_sizeX*factor.x)/2);
		rect.y = (_y*factor.y) - ((_sizeY*factor.x)/2);
		rect.width = _sizeX*factor.x;
		rect.height = _sizeY*factor.x;
		return rect;
	}
	
	public Rect SetGUI_Left(float _x, float _y, float _sizeX, float _sizeY){
		Vector2 factor = SetFactor();
		Rect rect = new Rect();
		rect.x = (_x*factor.x);
		rect.y = (_y*factor.y) - ((_sizeY*factor.x)/2);
		rect.width = _sizeX*factor.x;
		rect.height = _sizeY*factor.x;
		return rect;
	}
	
	public Rect SetGUI(float _x, float _y, float _sizeX, float _sizeY, bool nonCenter, bool extra){
		Vector2 factor = SetFactor();
		Rect rect = new Rect();
		rect.x = (_x*factor.x) - ((_sizeX*factor.x)/2);
		rect.y = (_y*factor.y) - ((_sizeY*factor.y)/2);
		rect.width = _sizeX*factor.y;
		rect.height = _sizeY*factor.y;
		return rect;
	}
	
	public Rect SetGUI(float _x, float _y, float _sizeX, float _sizeY, bool nonCenter, bool extra, bool Center){
		Vector2 factor = SetFactor();
		Rect rect = new Rect();
		rect.x = (_x*factor.x) - ((_sizeX*factor.x)/2);
		rect.y = (_y*factor.x) - ((_sizeY*factor.x)/2);
		rect.width = _sizeX*factor.x;
		rect.height = _sizeY*factor.x;
		return rect;
	}
	
	public Rect SetGUI(float _x, float _y, float _sizeX, float _sizeY, float _screenX, float _screenY){
		Vector2 factor = SetFactor(_screenX, _screenY);
		Rect rect = new Rect();
		rect.x = (_x*factor.x) - ((_sizeX*factor.x)/2);
		rect.y = (_y*factor.y) - ((_sizeY*factor.y)/2);
		rect.width = _sizeX*factor.x;
		rect.height = _sizeY*factor.x;
		return rect;
	}
	
	public Rect SetGUI(float _x, float _y, float _sizeX, float _sizeY, float _screenX, float _screenY, bool nonCenter){
		Vector2 factor = SetFactor(_screenX, _screenY);
		Rect rect = new Rect();
		rect.x = (_x*factor.y);
		rect.y = (_y*factor.y);
		rect.width = _sizeX*factor.y;
		rect.height = _sizeY*factor.y;
		return rect;
	}
	
	public Rect SetGUI(float _x, float _y, float _sizeX, float _sizeY, float _screenX, float _screenY, bool nonCenter, bool center){
		Vector2 factor = SetFactor(_screenX, _screenY);
		Rect rect = new Rect();
		rect.x = (_x*factor.x);
		rect.y = (_y*factor.x) - ((_sizeY*factor.x)/2);
		rect.width = _sizeX*factor.x;
		rect.height = _sizeY*factor.x;
		return rect;
	}
	
	private Vector2 SetFactor(float _screenX, float _screenY){
		Vector2 factor = Vector2.zero;
		factor.x = (_screenX/2048);
		factor.y = (_screenY/1536);
		return factor;
	}
	
	private Vector2 SetFactor(){
		Vector2 factor = Vector2.zero;
		factor.x = ((float)Screen.width/2048);
		factor.y = ((float)Screen.height/1536);
		return factor;
	}
	
	public Vector2 GetFactor(){
		GlobalFactor.x = ((float)Screen.width/2048);
		GlobalFactor.y = ((float)Screen.height/1536);
		return GlobalFactor;
	}
}
