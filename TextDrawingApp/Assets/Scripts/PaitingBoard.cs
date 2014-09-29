using UnityEngine;
using System.Collections;

public class PaitingBoard : MonoBehaviour {


	private Texture2D temp;
	private Texture2D orignalTexture;
	public int radius ;
	public int hardness;
	public bool isPaint = false;
	public bool twoLines;

	public static bool isNotMovingGesture = true;

	public Color isEraserColor { get; set;}

	void Start(){

		orignalTexture = renderer.material.mainTexture as Texture2D;


		float quadHeight = Camera.main.orthographicSize * 2.0f; 
		float quadWidth = quadHeight * Screen.width / Screen.height;
		transform.localScale = new Vector3(quadWidth, quadWidth, 1);
		temp = Instantiate (renderer.material.mainTexture) as Texture2D;
		renderer.material.mainTexture = temp;
	}


	public void loadTexture(string textureName){

		orignalTexture = Resources.Load("Textures/background"+textureName) as Texture2D;
		
		
		float quadHeight = Camera.main.orthographicSize * 2.0f; 
		float quadWidth = quadHeight * Screen.width / Screen.height;
		transform.localScale = new Vector3(quadWidth, quadWidth, 1);
		temp = Instantiate (orignalTexture) as Texture2D;
		renderer.material.mainTexture = temp;

	}


	void Update(){
//		if (isMoveGesture () && GestureStateManager.mGestureState == GestureStateManager.GestureState.IDLE) {

		if (isMoveGesture() && GestureStateManager.mGestureState == GestureStateManager.GestureState.IDLE) {
						move ();
		} else {
			previous = Vector2.zero;		
		} 
	}

	Vector2 current;
	Vector2 previous = Vector2.zero;
	private void move(){
		current = Input.GetTouch (0).position;

		RaycastHit hit;
		if (!Physics.Raycast (Camera.main.ScreenPointToRay (current), out hit))
			return;
		
		Renderer renderer = hit.collider.renderer;
		MeshCollider meshCollider = hit.collider as MeshCollider;
		if (renderer == null || renderer.sharedMaterial == null || renderer.sharedMaterial.mainTexture == null || meshCollider == null)
			return;
		
		Vector2 pixelUV = hit.textureCoord;
		pixelUV.x *= temp.width;
		pixelUV.y *= temp.height;

		if (previous == Vector2.zero) {
						previous = pixelUV;		
			} else {

//		Paint (pixelUV, radius, isEraserColor);
//			Debug.Log(pixelUV);
//			temp = Drawing.PaintLine (previous, pixelUV, radius, isEraserColor, hardness,temp);
		
			if(isPaint == true){

				PaintLine(previous, pixelUV, radius, isEraserColor,hardness);
				temp.Apply();

			}else{				
				temp = Drawing.DrawLine(previous, pixelUV, radius, isEraserColor,temp,true,isEraserColor,2);
			}
//			paint(pixelUV);

			previous = pixelUV;		
		}

	}

	private void paint(Vector2 position){

		float extent = 57;

//		Color[] pixels = temp.GetPixels (Mathf.FloorToInt(position.x),Mathf.FloorToInt(position.y), Mathf.FloorToInt(extent),Mathf.FloorToInt(extent));
//		for (int y=0; y < extent; y++) {
//			for(int x = 0; x < extent;x++){
//				pixels[ y*Mathf.FloorToInt(extent)+x] = isEraserColor;
//			}		
//		
//		}
//		temp.SetPixels (pixels);

		temp.SetPixel( (int)position.x,(int)position.y,isEraserColor);
		for (int i=1; i<20; i++) {
//			temp.SetPixel ((int)position.x - i, (int)position.y - i, isEraserColor);
			temp.SetPixel ((int)position.x + i, (int)position.y + i, isEraserColor);
			temp.SetPixel ((int)position.x + i, (int)position.y , isEraserColor);
			//			temp.SetPixel ((int)position.x + i, (int)position.y - i, isEraserColor);
//			temp.SetPixel ((int)position.x - i, (int)position.y + i, isEraserColor);
//
//			temp.SetPixel ((int)position.x, (int)position.y + i, isEraserColor);
//			temp.SetPixel ((int)position.x, (int)position.y - i, isEraserColor);
//			temp.SetPixel ((int)position.x+1, (int)position.y , isEraserColor);
//			temp.SetPixel ((int)position.x-1, (int)position.y , isEraserColor);


		}


		temp.Apply ();
	}



	private bool isMoveGesture(){
		return Input.touchCount == 1 ;
	}


//	private void line(int x,int y,int x2, int y2, Color color) {
//		int w = x2 - x ;
//		int h = y2 - y ;
//		int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0 ;
//		if (w<0) dx1 = -1 ; else if (w>0) dx1 = 1 ;
//		if (h<0) dy1 = -1 ; else if (h>0) dy1 = 1 ;
//		if (w<0) dx2 = -1 ; else if (w>0) dx2 = 1 ;
//		int longest = Mathf.Abs(w) ;
//		int shortest = Mathf.Abs(h) ;
//		if (!(longest>shortest)) {
//			longest = Mathf.Abs(h) ;
//			shortest = Mathf.Abs(w) ;
//			if (h<0) dy2 = -1 ; else if (h>0) dy2 = 1 ;
//			dx2 = 0 ;            
//		}
//		int numerator = longest >> 1 ;
//		for (int i=0;i<=longest;i++) {
//			temp.SetPixel(x,y,Color.white);
////			putpixel(x,y,color) ;
//			numerator += shortest ;
//			if (!(numerator<longest)) {
//				numerator -= longest ;
//				x += dx1 ;
//				y += dy1 ;
//			} else {
//				x += dx2 ;
//				y += dy2 ;
//			}
//		}
//		temp.Apply ();
//		renderer.material.mainTexture = temp;
//	}



//	private void Paint (Vector2 pos ,float rad,Color col) {
//		Vector2 start = new Vector2 (Mathf.Clamp (pos.x-rad,0,temp.width),Mathf.Clamp (pos.y-rad,0,temp.height));
//		Vector2 end = new Vector2 (Mathf.Clamp (pos.x+rad,0,temp.width),Mathf.Clamp (pos.y+rad,0,temp.height));
//		float widthX = Mathf.Round (end.x-start.x);
//		float widthY = Mathf.Round (end.y-start.y);
//		float sqrRad2 = (rad+1)*(rad+1);
//		Color[] pixels = temp.GetPixels ((int)start.x,(int)start.y,(int)widthX,(int)widthY,0);
//		
//		for (int y=0;y<widthY;y++) {
//			for (int x=0;x<widthX;x++) {
//				Vector2 p = new Vector2 (x,y) + start;
//				Vector2 center = p + new Vector2(0.5f,0.5f);
//
//				float dist = (center-pos).sqrMagnitude;
//				if (dist>sqrRad2) {
//					continue;
//				}
//
//				pixels[(int)(y*widthX+x)]=col;
//			}
//		}
//		
//		temp.SetPixels ((int)start.x,(int)start.y,(int)widthX,(int)widthY,pixels,0);
//		temp.Apply ();
//	}
	
	private float GaussFalloff (float distance , float inRadius) {
		return Mathf.Clamp01 (Mathf.Pow (360.0f, -Mathf.Pow (distance / inRadius, 2.5f) - 0.01f));
//				return Mathf.Clamp01( 1.0f - distance / inRadius);
//				return -(distance*distance) / (inRadius * inRadius) + 1.0f;

	}


//	static function LinearFalloff (distance : float , inRadius : float) {
//		return Mathf.Clamp01(1.0 - distance / inRadius);
//	}
//	
//	static function GaussFalloff (distance : float , inRadius : float) {
//		return Mathf.Clamp01 (Mathf.Pow (360.0, -Mathf.Pow (distance / inRadius, 2.5) - 0.01));
//	}
//	
//	function NeedleFalloff (dist : float, inRadius : float)
//	{
//		return -(dist*dist) / (inRadius * inRadius) + 1.0;
//	}
//

//
//
//
//
	public  Texture2D PaintLine (Vector2 _from,Vector2 to, float rad, Color col, float hardness) {
		//float width = rad*2;
		
		float extent = rad;
		float stY = Mathf.Clamp (Mathf.Min (_from.y,to.y)-extent,0,temp.height);
		float stX =  Mathf.Clamp (Mathf.Min (_from.x,to.x)-extent,0,temp.width);
		float endY = Mathf.Clamp (Mathf.Max (_from.y,to.y)+extent,0,temp.height);
		float endX = Mathf.Clamp (Mathf.Max (_from.x,to.x)+extent,0,temp.width);
		
		
		float lengthX = Mathf.Floor( endX-stX);
		float lengthY = Mathf.Floor( endY-stY);
		int ccc = 0;
		float sqrRad2 = (rad+1)*(rad+1);
		Color[] pixels = temp.GetPixels (Mathf.FloorToInt(stX)+ccc,Mathf.FloorToInt(stY)+ccc,Mathf.FloorToInt(lengthX)+ccc,Mathf.FloorToInt(lengthY)+ccc,0);
//		Debug.Log (pixels.ToString ());
		Vector2 start = new Vector2 (stX,stY);
		//Debug.Log (widthX + "   "+ widthY + "   "+ widthX*widthY);
		for (int y=0;y<lengthY;y++) {
			for (int x=0;x<lengthX;x++) {
				Vector2 p = new Vector2 (x,y) + start;
				Vector2 center = p + new Vector2(0.5f,0.5f);
				float dist = (center-NearestPointStrict(_from,to,center)).sqrMagnitude;
				if (dist>sqrRad2) {
					continue;
				}
				dist = GaussFalloff (Mathf.Sqrt(dist),rad) * hardness;
				//				Debug.Log(dist);
				//dist = (samples[i]-pos).sqrMagnitude;
				int index = Mathf.FloorToInt((y*Mathf.Floor(lengthX)+x));
				//				int index = (int)(y*lengthX+x);

					Color c = Color.clear;
					if (dist>0) {
						c =Color.Lerp (pixels[index],col,dist);
					} else {
						c =pixels[index];
					}

					if(twoLines == true){
					if( x < lengthX/2 )
						pixels[index]=c;
					else{
						c = Color.cyan;
						pixels[index]=c;
					}
					}else{
						pixels[index]=c;

					}

			
			}
		}
		
		temp.SetPixels (Mathf.FloorToInt(start.x),Mathf.FloorToInt(start.y),Mathf.FloorToInt(lengthX),Mathf.FloorToInt(lengthY),pixels,0);
		
		
		
		return temp;
	}
//
//
//
	static Vector3 NearestPointStrict(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
	{
		Vector3 fullDirection = lineEnd-lineStart;
		Vector3 lineDirection = Vector3.Normalize(fullDirection);
		float closestPoint = Vector3.Dot((point-lineStart),lineDirection)/Vector3.Dot(lineDirection,lineDirection);
		return lineStart+(Mathf.Clamp(closestPoint,0.0f,Vector3.Magnitude(fullDirection))*lineDirection);
	}
	
	static Vector2 NearestPointStrict(Vector2 lineStart, Vector2 lineEnd, Vector2 point)
	{
		Vector2 fullDirection = lineEnd-lineStart;
		Vector2 lineDirection = Vector3.Normalize(fullDirection);
		float closestPoint = Vector2.Dot((point-lineStart),lineDirection)/Vector2.Dot(lineDirection,lineDirection);
		return lineStart+(Mathf.Clamp(closestPoint,0.0f,fullDirection.magnitude)*lineDirection);
	}

	public void reset(){

		temp = Instantiate(orignalTexture) as Texture2D;
		renderer.material.mainTexture = temp;

	}


}
