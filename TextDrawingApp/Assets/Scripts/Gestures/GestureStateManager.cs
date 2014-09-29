using UnityEngine;
using System.Collections;

public class GestureStateManager{

	public enum GestureState  {IDLE, MOVE,ROTATE,ZOOM};


	public static GestureState  mGestureState = GestureState.IDLE;


}
