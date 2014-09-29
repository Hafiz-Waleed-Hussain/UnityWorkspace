#region AGNITUS 2013
/* JungleWorld- Naming Fruits Game
 * Developer- Asema Hassan
 * Unity3D*/
#endregion
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NF_BreakRopeJoints : MonoBehaviour {
	

	
public Component[] hingeJoints;
public Component[] rigidbodies;
public List<GameObject> brokenRopeObj;
public GameObject _hangingObject;
public GameObject _hangingObjectText;
public GameObject _newParent;
public GameObject ickyModel;
GameObject ropeCutUp;	
private Transform obj;
private Vector3 _newParentPosition;
private NF_BrokenRopeObjects _brokenRopeObjects;
int currentCategory;
int countForBrokenRope;	
	
public void showTextAccordingToCategory(int currentCategoryID){
		currentCategory= currentCategoryID;
	}
public void setPropertiesOfObject(string loadedQuestionTitle){
		
		// properties 
		_hangingObject.tag = "tag_Fruits";
		_hangingObject.transform.parent.tag = "tag_Rope";
		this.transform.FindChild("Link_10").tag = "tag_BreakPoint";
		
		// initialize 
		ickyModel = GameObject.Find("Icky_Model");
		hingeJoints = GetComponentsInChildren<HingeJoint>();
		rigidbodies = GetComponentsInChildren<Rigidbody>();
		createAParentObject();
		addfallingRopeObj(); // create new array of broken joints for reference
		
}
void createAParentObject(){
	
    _newParent= Instantiate(Resources.Load(NF_Constants._ropePrefabsPath, typeof(GameObject)),_newParentPosition,Quaternion.identity) as GameObject;
	_newParent.transform.position = new Vector3(_hangingObject.transform.parent.position.x,-900,400);
	_newParentPosition= new Vector3(_newParent.transform.position.x,_newParent.transform.position.y-450,_newParent.transform.position.z);	
}
	
public IEnumerator deleteRopeCutUp(float delay){
		
		yield return new WaitForSeconds(delay);
		Destroy(ropeCutUp);
	}
public void breakRopeNow(){ // called when a correct answer is Tapped 
		
		pickAllJointsFromRope();
		if(_hangingObjectText!=null){
			_hangingObjectText.SetActive(false);
		}
		_brokenRopeObjects = (NF_BrokenRopeObjects) _newParent.GetComponent<NF_BrokenRopeObjects>();
		_brokenRopeObjects.sendBrokenRopeArray(brokenRopeObj,_newParentPosition);
		
		// create rope up animation sprite here
		
		ropeCutUp = Instantiate(Resources.Load(NF_Constants._ropeCutUpPath, typeof(GameObject)),
			new Vector3(this.transform.position.x,this.transform.position.y-40,this.transform.position.z),Quaternion.identity) as GameObject;
		
		Destroy(this.gameObject);
		Debug.Log("Rope Destroyed");	
		
	}

public void resetRopeProperties(){
		
	Destroy(_newParent);
	_newParent=null;
	_hangingObject = null;
	if(_hangingObjectText!=null){
				_hangingObjectText = null;
		}
	_newParentPosition = new Vector3(0,0,0);
	
	for(int i=0;i<brokenRopeObj.Count;i++){
			Destroy(brokenRopeObj[i]);
		}
	brokenRopeObj =null;
	
}
#region private_Methods
void addfallingRopeObj(){
		
	brokenRopeObj = new List<GameObject>();
	for(int i=11; i<=16; i++){
		GameObject obj = (GameObject)hingeJoints[i].gameObject;
		brokenRopeObj.Add(obj);
		}
		brokenRopeObj.Add(_hangingObject);
		brokenRopeObj.Add(this.transform.FindChild("rope_Collider").gameObject);
		if(_hangingObjectText!=null && currentCategory==(int)NamingFruits_CategoryID.id_AlplabeticOrder){
			brokenRopeObj.Add(_hangingObjectText);
		}
}
	
void pickAllJointsFromRope(){

		foreach (Rigidbody body in rigidbodies) {
			if(body.collider.tag == "tag_BreakPoint"){
				body.useGravity=true;
				body.isKinematic=false;
				body.constraints &= RigidbodyConstraints.FreezeAll;
				body.constraints &= ~RigidbodyConstraints.FreezePositionY;
				body.constraints &= ~RigidbodyConstraints.FreezePositionZ;
					}
    			}
		   foreach (HingeJoint joint in hingeJoints){
				if(joint.collider.tag == "tag_BreakPoint"){
						joint.connectedBody=null;
						joint.breakForce=20;
						joint.breakTorque=20;
				}
			}
}

void OnJointBreak(float breakForce) {
    Debug.Log("A joint has just been broken,force: " + breakForce);
	}
#endregion

}
