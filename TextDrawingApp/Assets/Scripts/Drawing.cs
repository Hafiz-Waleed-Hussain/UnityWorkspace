using UnityEngine;
using System.Collections;

public class Drawing {



	public static Samples NumSamples = Samples.Samples4;
	public static bool isOffsetInMiddleLine = true;
	static bool isInDrawMidLineMode = false;
	static Vector2 previousPoint;
	public static GameObject line;
	
	public static Texture2D DrawLine (Vector2 _from,Vector2 to, float w, Color col, Texture2D tex) {
		return DrawLine (_from,to,w,col,tex,false,Color.black,0);
	}
	
	public static Texture2D DrawLine (Vector2 _from,Vector2 to, float w, Color col, Texture2D tex, bool stroke, Color strokeCol, float strokeWidth) {
		w = Mathf.Round (w);//It is important to round the numbers otherwise it will mess up with the texture width
		strokeWidth = Mathf.Round (strokeWidth);
		
		float extent = w + strokeWidth;
		
		float stY = Mathf.Clamp (Mathf.Min (_from.y,to.y)-extent,0,tex.height);//This is the topmost Y value
		float stX = Mathf.Clamp ((int)Mathf.Min (_from.x,to.x)-extent,0,(int)tex.width);
		float endY = Mathf.Clamp ((int)Mathf.Max (_from.y,to.y)+extent,0,(int)tex.height);
		float endX = Mathf.Clamp ((int)Mathf.Max (_from.x,to.x)+extent,0,(int)tex.width);//This is the rightmost Y value
		
		strokeWidth = strokeWidth/2;
		float strokeInner = (w-strokeWidth)*(w-strokeWidth);
		float strokeOuter = (w+strokeWidth)*(w+strokeWidth);
		float strokeOuter2 = (w+strokeWidth+1)*(w+strokeWidth+1);
		float sqrW = w*w;//It is much faster to calculate with squared values
		
		float lengthX = Mathf.Floor( endX-stX);
		float lengthY = Mathf.Floor( endY-stY);
		Vector2 start = new Vector2 (stX,stY);
		Color[] pixels = tex.GetPixels ((int)stX,(int)stY,(int)lengthX,(int)lengthY,0);//Get all pixels
		
		for (float y=0;y<lengthY;y++) {
			for (float x=0;x<lengthX;x++) {//Loop through the pixels
				Vector2 p = new Vector2 (x,y) + start;
				Vector2 center = p + new Vector2(0.5f,0.5f);
				float dist = (center-NearestPointStrict(_from,to,center)).sqrMagnitude;//The squared distance from the center of the pixels to the nearest point on the line
				if (dist<=strokeOuter2) {
					Vector2 [] samples = Sample (p);
					Color c = Color.clear;
					Color pc = pixels[(int)(y*lengthX+x)];
					
					for (int i=0;i<samples.Length;i++) {//Loop through the samples
						dist = (samples[i]-NearestPointStrict(_from,to,samples[i])).sqrMagnitude;//The squared distance from the sample to the line
						if (stroke) {
							if (dist<=strokeOuter && dist >= strokeInner) {
								c+=strokeCol;
							} else if (dist<sqrW) {
								c+=col;
							} else {
								c+=pc;
							}
						} else {
							if (dist<sqrW) {//Is the distance smaller than the width of the line
								c+=col;
							} else {
								c+=pc;//No it wasn't, set it to be the original colour
							}
						}
					}
					c /= samples.Length;//Get the avarage colour
					pixels[(int)(y*lengthX+x)]=c;
				}
			}
		}
		tex.SetPixels ((int)stX,(int)stY,(int)lengthX,(int)lengthY,pixels,0);
		tex.Apply ();
		return tex;
	}
	
	
	public static Texture2D Paint (Vector2 pos,float rad, Color col, float hardness, Texture2D tex) {
		Vector2 start = new Vector2 (Mathf.Clamp (pos.x-rad,0,tex.width),Mathf.Clamp (pos.y-rad,0,tex.height));
		//float width = rad*2;
		Vector2 end = new Vector2 (Mathf.Clamp (pos.x+rad,0,tex.width),Mathf.Clamp (pos.y+rad,0,tex.height));
		float widthX = Mathf.Round (end.x-start.x);
		float widthY = Mathf.Round (end.y-start.y);
		//float sqrRad = rad*rad;
		float sqrRad2 = (rad+1)*(rad+1);
		
		Color[] pixels = tex.GetPixels ((int)start.x,(int)start.y,(int)widthX,(int)widthY,0);
		
		for (int y=0;y<widthY;y++) {
			for (int x=0;x<widthX;x++) {
				Vector2 p = new Vector2 (x,y) + start;
				Vector2 center = p + new Vector2(0.5f,0.5f);
				float dist = (center-pos).sqrMagnitude;
				if (dist>sqrRad2) {
					continue;
				}
				Vector2 [] samples = Sample (p);
				Color c = Color.black;
				for (int i=0;i<samples.Length;i++) {
					dist = GaussFalloff (Vector2.Distance(samples[i],pos),rad) * hardness;
					if (dist>0) {
						c+=Color.Lerp (pixels[(int) (y*widthX+x)],col,dist);
					} else {
						c+=pixels[(int)(y*widthX+x)];
					}
				}
				c /= samples.Length;
				
				pixels[(int)(y*widthX+x)]=c;
			}
		}
		
		tex.SetPixels ((int)start.x,(int)start.y,(int)widthX,(int)widthY,pixels,0);
		return tex;
	}
	
	public static Texture2D PaintLine (Vector2 _from,Vector2 to, float rad, Color col, float hardness, Texture2D tex) {
		//float width = rad*2;
		
		float extent = rad;
		float stY = Mathf.Clamp (Mathf.Min (_from.y,to.y)-extent,0,tex.height);
		float stX =  Mathf.Clamp (Mathf.Min (_from.x,to.x)-extent,0,tex.width);
		float endY = Mathf.Clamp (Mathf.Max (_from.y,to.y)+extent,0,tex.height);
		float endX = Mathf.Clamp (Mathf.Max (_from.x,to.x)+extent,0,tex.width);
		
		
		float lengthX = Mathf.Floor( endX-stX);
		float lengthY = Mathf.Floor( endY-stY);

		//float sqrRad = rad*rad;
		float sqrRad2 = (rad+1)*(rad+1);
		Color[] pixels = tex.GetPixels ((int)stX,(int)stY,(int)lengthX,(int)lengthY,0);
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
				
				int index = Mathf.FloorToInt((y*lengthX+x));
				if(pixels.Length > index){
				Color c = Color.clear;
				if (dist>0) {
					c =Color.Lerp (pixels[index],col,dist);
				} else {
					c =pixels[index];
				}
				pixels[index]=c;
				}
			}
		}
		tex.SetPixels ((int)start.x,(int)start.y,(int)lengthX,(int)lengthY,pixels,0);
		return tex;
	}

	public static Texture2D PaintLine (Vector2 _from,Vector2 to, float rad, Color col, float hardness, Texture2D tex, Texture2D tex2) {
		//float width = rad*2;
		/*if( isOffsetInMiddleLine ){
			previousPoint = _from;
			isOffsetInMiddleLine = false;
			isInDrawMidLineMode = false;
		}
		if( Vector2.Distance( previousPoint, _from ) > 30f ){
			isInDrawMidLineMode = true;
		}*/

		float extent = rad;
		float stY =  Mathf.Clamp (Mathf.Min (_from.y,to.y)-extent,0,tex.height);
		float stX =  Mathf.Clamp (Mathf.Min (_from.x,to.x)-extent,0,tex.width);
		float endY = Mathf.Clamp (Mathf.Max (_from.y,to.y)+extent,0,tex.height);
		float endX = Mathf.Clamp (Mathf.Max (_from.x,to.x)+extent,0,tex.width);
		
		
		float lengthX = endX-stX;
		float lengthY = endY-stY;
		
		//float sqrRad = rad*rad;
		float sqrRad2 = (rad+1)*(rad+1); //961
		Color[] pixels = tex.GetPixels ((int)stX,(int)stY,(int)lengthX,(int)lengthY,0);
		Vector2 start = new Vector2 (stX,stY);
		/*Debug.LogWarning( "lengthX : " + lengthX  );
		Debug.LogWarning( "lengthY : " + lengthY  );*/
		for (int y=0;y<lengthY;y++) {

			float alphaLevelOfMidLine = 0f;

			for (int x=0;x<lengthX;x++) {
			//	Debug.LogWarning( "x : " + x  );
				Vector2 p = new Vector2 (x,y) + start;
				Vector2 center = p + new Vector2(0.5f,0.5f);
				float dist = (center - NearestPointStrict(_from,to,center)).sqrMagnitude;
				if (dist>sqrRad2) {
					continue;
				}
				dist = GaussFalloff (Mathf.Sqrt(dist),rad) * hardness;
				//				Debug.Log(dist);
				//dist = (samples[i]-pos).sqrMagnitude;
				
				int index = Mathf.FloorToInt((y*lengthX+x));

				//Debug.LogWarning( "index : " + index  );

				Color c = Color.clear;
				if (dist>0) {

					/*if( x > 18 && x < 42 &&  isInDrawMidLineMode  ){
								Color c1;
								if( x < 30 )
									c1 = new Color(250f/255,155f/255,22f/255 , alphaLevelOfMidLine += 0.05f);
								else
									c1 = new Color(250f/255,155f/255,22f/255 , alphaLevelOfMidLine -= 0.05f);
								c =Color.Lerp (pixels[index],c1 ,dist);
					}else{*/
						c =Color.Lerp (pixels[index],col,dist);
					/*}*/

				} else {
					c =pixels[index];
				}
				pixels[index]=c;
			}
		}
		tex.SetPixels ((int)start.x,(int)start.y,(int)lengthX,(int)lengthY,pixels,0);
		//tex.SetPixels ((int)start.x,(int)start.y,(int)lengthX,(int)lengthY,tex2.GetPixels(),0);
		return tex;
	}

	
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
	
	static Vector2 [] Sample (Vector2 p) {
	switch (Drawing.NumSamples) {
		case Samples.None :
			return new Vector2 [] {p+ new Vector2(0.5f,0.5f)};
		case Samples.Samples2 :
			return new Vector2 [] {p+ new Vector2(0.25f,0.5f),p+ new Vector2(0.75f,0.5f)};
		case Samples.Samples4 : 
			//return [p+new Vector2(0.25,0.25),p+new Vector2(0.75,0.25), p+new Vector2(0.25,0.75),p+new Vector2(0.75,0.75)];
			return new Vector2 [] {
			/*p+new Vector2(0,0),
			p+new Vector2(1,0),
			p+new Vector2(0,1),
			p+new Vector2(1,1)*/
			
			p+new Vector2(0.25f,0.5f),
			p+new Vector2(0.75f,0.5f),
			p+new Vector2(0.5f,0.25f),
			p+new Vector2(0.5f,0.75f)
			
			};
		case Samples.Samples8 : 
			return new Vector2 []{
			
			/*p+new Vector2(0,0),
			p+new Vector2(1,0),
			p+new Vector2(0,1),
			p+new Vector2(1,1),*/
			
			/*p+new Vector2(0.25,0.25),
			p+new Vector2(0.75,0.25),
			p+new Vector2(0.25,0.75),
			p+new Vector2(0.75,0.75)*/
			
			p+new Vector2(0.25f,0.5f),
			p+new Vector2(0.75f,0.5f),
			p+new Vector2(0.5f,0.25f),
			p+new Vector2(0.5f,0.75f),
			
			p+new Vector2(0.25f,0.25f),
			p+new Vector2(0.75f,0.25f),
			p+new Vector2(0.25f,0.75f),
			p+new Vector2(0.75f,0.75f)
			
			/*p+new Vector2(0.2,0.25),
			p+new Vector2(0.4,0.25),
			p+new Vector2(0.6,0.25),
			p+new Vector2(0.8,0.25),
			
			p+new Vector2(0.2,0.75),
			p+new Vector2(0.4,0.75),
			p+new Vector2(0.6,0.75),
			p+new Vector2(0.8,0.75)*/
			};
		case Samples.Samples16 : 
			return new Vector2 []{
			
			p+new Vector2(0,0),
			p+new Vector2(0.3f,0),
			p+new Vector2(0.7f,0),
			p+new Vector2(1,0),
			
			p+new Vector2(0,0.3f),
			p+new Vector2(0.3f,0.3f),
			p+new Vector2(0.7f,0.3f),
			p+new Vector2(1,0.3f),
			
			p+new Vector2(0,0.7f),
			p+new Vector2(0.3f,0.7f),
			p+new Vector2(0.7f,0.7f),
			p+new Vector2(1,0.7f),
			
			p+new Vector2(0,1),
			p+new Vector2(0.3f,1),
			p+new Vector2(0.7f,1),
			p+new Vector2(1,1)
			};
		case Samples.Samples32 :
			return new Vector2 []{
			
			p+new Vector2(0,0),
			p+new Vector2(1,0),
			p+new Vector2(0,1),
			p+new Vector2(1,1),
			
			p+new Vector2(0.2f,0.2f),
			p+new Vector2(0.4f,0.2f),
			p+new Vector2(0.6f,0.2f),
			p+new Vector2(0.8f,0.2f),
			
			p+new Vector2(0.2f,0.4f),
			p+new Vector2(0.4f,0.4f),
			p+new Vector2(0.6f,0.4f),
			p+new Vector2(0.8f,0.4f),
			
			p+new Vector2(0.2f,0.6f),
			p+new Vector2(0.4f,0.6f),
			p+new Vector2(0.6f,0.6f),
			p+new Vector2(0.8f,0.6f),
			
			p+new Vector2(0.2f,0.8f),
			p+new Vector2(0.4f,0.8f),
			p+new Vector2(0.6f,0.8f),
			p+new Vector2(0.8f,0.8f),
			
			
			
			p+new Vector2(0.5f,0),
			p+new Vector2(0.5f,1f),
			p+new Vector2(0,0.5f),
			p+new Vector2(1,0.5f),
			
			p+new Vector2(0.5f,0.5f)
			
			};
		case Samples.RotatedDisc :
			return new Vector2 []{
			
			p+new Vector2(0,0),
			p+new Vector2(1,0),
			p+new Vector2(0,1),
			p+new Vector2(1,1),
			
			p+new Vector2(0.5f,0.5f)+new Vector2(0.258f,0.965f),//Sin (75Â°) && Cos (75Â°)
			p+new Vector2(0.5f,0.5f)+new Vector2(-0.965f,-0.258f),
			p+new Vector2(0.5f,0.5f)+new Vector2(0.965f,0.258f),
			p+new Vector2(0.5f,0.5f)+new Vector2(0.258f,-0.965f)
			};
			
		default:
			return new Vector2[]{};
	}
}
	
	static float GaussFalloff (float distance, float inRadius) {
		return Mathf.Clamp01 (Mathf.Pow (360.0f, -Mathf.Pow (distance / inRadius, 2.5f) - 0.01f));
	}

	public enum Samples {
		None,
		Samples2,
		Samples4,
		Samples8,
		Samples16,
		Samples32,
		RotatedDisc
	}


}
