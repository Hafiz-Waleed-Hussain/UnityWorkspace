#region AGNITUS 2013
/* JungleWorld- Menu
 * Developer - Asema Hassan
 * Unity3D*/
#endregion
using UnityEngine;
using System.Collections;

public class JW_GameIcon : MonoBehaviour {
	
private int _currentGameIndex;
private AGGameState gameState;	
// Use this for initialization
public void gameIconInitializer(int gameIndex, string textureName,Vector3 gameIconPos){
		
	_currentGameIndex=gameIndex;
	this.gameObject.transform.position= gameIconPos;
	this.gameObject.renderer.material.mainTexture = Resources.Load("JungleWorldMenu/Sprites/Game Icon/"+textureName) as Texture;
	}

public void goToGame(){
	
		AGGameIndex index = (AGGameIndex)_currentGameIndex;
		AGGameState.startGame(index);
	}
}
