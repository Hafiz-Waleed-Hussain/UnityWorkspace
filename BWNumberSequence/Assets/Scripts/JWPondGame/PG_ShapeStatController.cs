using UnityEngine;
using System.Collections;

public class PG_ShapeStatController : MonoBehaviour {
	
	string shapeName;
	public GameObject Ripple_Effect;
	GameObject emptyLandMass , icky=null , olly=null , referent_bubble=null , referent_shape=null , referent_hint=null , referent_shapeWithHint , camera ,ollyParent , shapeShadow;
	float tiltAngle = -10.0F , standingShapeTiltAngle , endTime=46F;
	int wrongAnimationCount=0;
	bool isAnswerShape=false;
	Quaternion target , shapeStandingTiltAnimaiton;
	PG_GameController  gameController;
	PG_OllyAnimations  ollyAnimationsSR;
	
	public bool isFirstShape , isLastShape;
	public int answerShapesCount;
	
	
	
	// Use this for initialization
	void Start () {
		standingShapeTiltAngle  =	5F;
		referent_shapeWithHint	=	GameObject.FindGameObjectWithTag("pg_referent_shapeWithHint");
				   olly			=	GameObject.Find("pg_olly");
				   icky			=	GameObject.FindGameObjectWithTag("pg_icky");
	        referent_bubble		=	GameObject.FindGameObjectWithTag("pg_referent_bubble");
		    referent_shape		=	GameObject.FindGameObjectWithTag("pg_referent_shape");
		    referent_hint		=	GameObject.FindGameObjectWithTag("pg_referent_hint");
			camera 				= 	GameObject.Find("Camera");
			ollyParent			=	GameObject.Find("OllyReferentContainer");
			shapeShadow			=   GameObject.Find("shape_shadow");
		// ---- Get game controller script on Main Camera
			gameController	= camera.GetComponent<PG_GameController>();
		if(olly!=null)
		ollyAnimationsSR	= olly.GetComponent<PG_OllyAnimations>();
		camera 				= 	GameObject.Find("Camera");
		gameController		= camera.GetComponent<PG_GameController>();
		
		
	}
	

//----- shape moving around its Axis
	public void  shapeWrongTapAnimation(){
		//Debug.Log("wrong tap animation...");
		if(wrongAnimationCount==0){
			StartCoroutine(spriteLeftTiltAnimaition(0.0F));
		}
	}
	
	
	// --- shape correct animation
	public void shapeCorrectTapAnimation( GameObject landMass){
		
		Vector3 finalScale = new Vector3(gameObject.transform.localScale.x-40F,gameObject.transform.localScale.y-40,1F);
		Vector3 finalPos   = landMass.transform.position;  // 32.5 
				finalPos.x-=10F;  
		        finalPos.y = 220F;
				finalPos.z = -60.0F; 	
		
		Vector3 pos=gameObject.transform.position;
		pos.z=-150F;
		gameObject.transform.position=pos;
		emptyLandMass=landMass;
		ArrayList args=new ArrayList();
		args.Add(finalPos);
		iTween.MoveTo(gameObject, iTween.Hash("position" , landMass.transform.position , "time", 0.4, "onComplete", "replaceEmptyLandmassWithFileldOne", "oncompleteparams", args , "easetype", iTween.EaseType.easeInOutSine));
		//iTween.ScaleTo(gameObject ,finalScale , 0.4F);

	}
	
	
	// --- replace empty landmass with filled one
	private void replaceEmptyLandmassWithFileldOne(ArrayList args)
	{
		Vector3 finalPos = (Vector3)args[0];
		float time = 0.0F;
		PG_LandmassController landmassStatController =emptyLandMass.GetComponent<PG_LandmassController>();
		landmassStatController.SetFilled();
		//Debug.Log("is first shape? : "+isFirstShape);
		
		if(isFirstShape){
			PG_OllyAnimations.changeTexture = true;
			ollyAnimationsSR.ollyState=ollyAnimationStates.olly_jumpLandToWater;
			ollyAnimationsSR.isLastShape=isLastShape;
			
			
			//if(answerShapesCount>=2)
				//finalPos.y-=75.5F; me
			finalPos.y-=75F;
			time =1.3F;
		}
		else{
			PG_OllyAnimations.changeTexture = true;
			ollyAnimationsSR.ollyState=ollyAnimationStates.olly_jumpWaterToWater;
			ollyAnimationsSR.isLastShape=isLastShape;
			//finalPos.y-=75.5F; me
			finalPos.y-=75F;
			time = 1.37F;
		}
		
		gameObject.transform.renderer.enabled =false;
		iTween.MoveTo(ollyParent , iTween.Hash("position",finalPos , "time",time , "delay",0.17));
		iTween.ScaleTo(ollyParent , iTween.Hash("time", time+0.15f - 20.0f/30.0f, "oncomplete","showRippleEffect", "oncompletetarget", gameObject));
	}
	
	private void  showRippleEffect(){
		//Debug.Log("In ripple effect");
		firstRippleEffect();
		StartCoroutine(secondRippleEffect(0.1F));
		StartCoroutine(thirdRippleEffect(0.4F));
		//StartCoroutine(fourthRippleEffect(0.6F));
	}
	
	void firstRippleEffect(){
	    GameObject rippleEffect = Instantiate(Ripple_Effect) as GameObject;
		rippleEffect.transform.position=new Vector3(emptyLandMass.transform.position.x,emptyLandMass.transform.position.y,-5F);
		//rippleEffect.transform.parent = emptyLandMass.transform;
	}
	
	IEnumerator secondRippleEffect(float delay){
	 yield return new WaitForSeconds(delay);
	 GameObject rippleEffect = Instantiate(Ripple_Effect) as GameObject;
		//Debug.Log("setting landmass: "+emptyLandMass);
		rippleEffect.transform.position=new Vector3(emptyLandMass.transform.position.x,emptyLandMass.transform.position.y,-5F);
		//rippleEffect.transform.parent = emptyLandMass.transform;
	}
	
	IEnumerator thirdRippleEffect(float delay){
	 yield return new WaitForSeconds(delay);
	GameObject rippleEffect = Instantiate(Ripple_Effect) as GameObject;
		rippleEffect.transform.position=new Vector3(emptyLandMass.transform.position.x,emptyLandMass.transform.position.y,-5F);
		//rippleEffect.transform.parent = emptyLandMass.transform;
	}
	
	IEnumerator fourthRippleEffect(float delay){
	 yield return new WaitForSeconds(delay);
	GameObject rippleEffect = Instantiate(Ripple_Effect) as GameObject;
		rippleEffect.transform.position=new Vector3(emptyLandMass.transform.position.x,emptyLandMass.transform.position.y,-5F);
		//rippleEffect.transform.parent = emptyLandMass.transform;
		GameObject.Destroy(gameObject);
	}
	
	//----- Set shape name
	public void setShapeName(string name){
		shapeName=name;
	}
	
	public void setAnswerShape(bool stat){
		isAnswerShape=stat;
	}
	
	
	public bool getAnswerShape(){
		return isAnswerShape;
	}
	
	
	
	private IEnumerator spriteLeftTiltAnimaition(float waitTime){
			
			yield return new WaitForSeconds(waitTime);
			    transform.Translate(-10F , 0F , 0F); 
				wrongAnimationCount++;
				StartCoroutine(spriteRightTiltAnimaiton(0.15F));
			if(wrongAnimationCount==4){
				wrongAnimationCount = 0;
				this.StopAllCoroutines();
			}
		}
	
	private IEnumerator spriteRightTiltAnimaiton(float waitTime)
	{
			yield return new WaitForSeconds(waitTime);
				transform.Translate(10F , 0F , 0F);
				wrongAnimationCount++;
				StartCoroutine(spriteLeftTiltAnimaition(0.15F));
		if(wrongAnimationCount==4){	
			wrongAnimationCount = 0;
			this.StopAllCoroutines();
		}
	}
	
	
 public void shapesIdleAnimation(){
		gameController.enableTouchesFlag = true;
		gameController.shapesReachedPlayVO =true;
		PG_SoundManager.playIntroClip =true;
		rotateLeft();
		GameObject parent = gameObject.transform.parent.gameObject;
		if(parent!=null){
		Transform [] childs = parent.GetComponentsInChildren<Transform>();
		Transform quickSandObject	= childs[2];
		Transform shadowObject 		= childs[3];
			
		GameObject shadow = shadowObject.gameObject;
		if(shadow!=null){
	    PG_ShapeShadowController shadowController = shadow.GetComponent<PG_ShapeShadowController>();
		shadowController.playShowShapesShadowAnim();
		}
		
	    GameObject quickSand = quickSandObject.gameObject;
		
		}	
	}
	
	public void rotateLeft(){
		iTween.RotateTo(gameObject , iTween.Hash("delay",0.0, "time", 0.5, "rotation" , new Vector3(0, 0, 7) , "oncomplete","rotateRight", "easetype",iTween.EaseType.easeInOutSine));
	}
	
	public void rotateRight(){
		iTween.RotateTo(gameObject , iTween.Hash("delay",0.0, "time", 0.5, "rotation" ,new Vector3(0, 0, -7), "oncomplete","rotateLeft", "easetype",iTween.EaseType.easeInOutSine));
	}
	
	
	
	public  void playShadowAnimLeft(){
		if(shapeShadow!=null)
		iTween.PunchScale(shapeShadow, iTween.Hash( "delay",0.1 , "time",0.2 , new Vector3((float)shapeShadow.transform.localScale.x+40 , (float)shapeShadow.transform.localScale.y , 1F) ,"oncomplete","playShadowAnimLeft" ));
	}
	
	public  void playShadowAnimRight(){
		if(shapeShadow!=null)
			iTween.PunchScale(shapeShadow, iTween.Hash( "delay",0.1 , "time",0.2 , new Vector3((float)shapeShadow.transform.localScale.x+40 , (float)shapeShadow.transform.localScale.y , 1F) ,"oncomplete","playShadowAnim" ));
	}
	

	public void stopAllAnimations(){
		this.StopAllCoroutines();
		transform.localRotation=Quaternion.Euler(0f,0f,0f);
	}
	
}
