using UnityEngine;
using System.Collections;

public class CatController : MonoBehaviour {

	void GrantCatTheSweetReleaseOfDeath()
	{
		DestroyObject( gameObject );
	}
}
