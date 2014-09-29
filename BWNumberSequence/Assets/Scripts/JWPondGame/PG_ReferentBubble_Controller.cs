using UnityEngine;
using System.Collections;

public class PG_ReferentBubble_Controller : MonoBehaviour {

	public void hideReferentBubble()
	{
		transform.renderer.enabled=false;
	}
	
	public void showReferentBubble()
	{
		transform.renderer.enabled=true;
	}
}
