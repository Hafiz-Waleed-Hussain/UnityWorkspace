using UnityEngine;
using System.Collections;

public class EnemyGenrator : MonoBehaviour {

	private float elapsedTime = 0;
	public float genrateEnemyAfterSeconds = 0;


	private const int SMALL = 0;
	private const int MEDIUM = 1;
	private const int LARGE = 2;
	private const int EXTRA_LARGE = 3;


	void Update () {

		elapsedTime += Time.deltaTime;
		if (elapsedTime > genrateEnemyAfterSeconds) {
			elapsedTime = 0;		
			int whichEnemyGenerate = Random.Range(0,4); 
			Debug.Log(whichEnemyGenerate+"");
			switch( whichEnemyGenerate){
			case SMALL:
				small();
				break;
			case MEDIUM:
				medium();
				break;
			case LARGE:
				large();
				break;
			case EXTRA_LARGE:
				extraLarge();
				break;
			}
		}

	}


	private void instanciateEnemy(){

	}

	private void small(){
		generateEnemy ("Stone Small","Tree Extra Large");
	}

	private void medium(){
		generateEnemy ("Stone Medium","Tree Large");
	}
	private void large(){
		generateEnemy ("Stone Large","Tree Medium");
	}
	private void extraLarge(){
		generateEnemy ("Stone Extra Large","Tree Small");
	}

	private void generateEnemy(string stoneEnemyName,string treeEnemyName){
				GameObject.Instantiate(Resources.Load (stoneEnemyName),new Vector3(7,4,0),Quaternion.identity);
				GameObject.Instantiate(Resources.Load (treeEnemyName),new Vector3(7,-4,0),Quaternion.identity);

	}

//	private void generateEnemy(string stoneEnemyName,float stoneY, string treeEnemyName){
//		GameObject.Instantiate(Resources.Load (stoneEnemyName),new Vector3(7,stoneY,0),Quaternion.identity);
//		GameObject.Instantiate(Resources.Load (treeEnemyName),new Vector3(7,-3.4f,0),Quaternion.identity);

//		GameObject.Instantiate(Resources.Load (stoneEnemyName),new Vector3(7,0,0),Quaternion.identity);
//		GameObject.Instantiate(Resources.Load (treeEnemyName),new Vector3(7,0,0),Quaternion.identity);

//	}


}
