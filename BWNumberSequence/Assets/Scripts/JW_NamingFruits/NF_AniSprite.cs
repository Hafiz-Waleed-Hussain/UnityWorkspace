#region AGNITUS 2013
/* JungleWorld- Naming Fruits Game
 * Developer- Asema Hassan
 * Unity3D*/
#endregion
using UnityEngine;
using System.Collections;

public class NF_AniSprite : MonoBehaviour
{
// function for animating sprites
public void  playSpriteSheetAnimation(int colCount ,int rowCount ,int colNumber,int rowNumber,int totalCells,int fps, bool bLoop){
	    // Calculate index
    int index  = (int)(Time.time * fps);
		if(bLoop)
		index = index % totalCells;
	else if(index >= totalCells){  // Repeat when exhausting all cells
		index = totalCells - 1;
	}	
    // Size of every cell
    float sizeX = 1.0f / colCount;
    float sizeY = 1.0f / rowCount;
    Vector2 size =  new Vector2(sizeX,sizeY);
 
    // split into horizontal and vertical index
    var uIndex = index % colCount;
    var vIndex = index / colCount;
 
    // build offset
    // v coordinate is the bottom of the image in opengl so we need to invert.
    float offsetX = (uIndex+colNumber) * size.x;
    float offsetY = (1.0f - size.y) - (vIndex + rowNumber) * size.y;
    Vector2 offset = new Vector2(offsetX,offsetY);
 
    renderer.material.SetTextureOffset ("_MainTex", offset);
    renderer.material.SetTextureScale  ("_MainTex", size);
	}

}
