/// <summary>
/// Set FPS
/// this script use to set FPS if platform is mobile
/// </summary>

using UnityEngine;
using System.Collections;

public class SetFPS : MonoBehaviour {
	
	public int fpsTarget;
	
	void Start () {
		Application.targetFrameRate = fpsTarget;
	}
}
