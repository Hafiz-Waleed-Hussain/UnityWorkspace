using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Parallax scrolling script that should be assigned to a layer
/// </summary>
public class BackgroundLayerMovement : MonoBehaviour
{
	public Vector2 speed = new Vector2(.1f, 10);
	public Vector2 direction = new Vector2(-1, 0);
	private List<Transform> backgroundPart;
	
	void Start()
	{
			backgroundPart = new List<Transform>();
			
			for (int i = 0; i < transform.childCount; i++)
			{
				Transform child = transform.GetChild(i);
					backgroundPart.Add(child);
			}
			
		}

	
	void Update()
	{
		Vector3 movement = new Vector3(
			speed.x * direction.x,
			speed.y * direction.y,
			0);
		
		movement *= Time.deltaTime;
		transform.Translate(movement);
		
		Transform firstChild = backgroundPart [0];
			
			if (firstChild != null)
			{
//			Debug.Log(firstChild.position.x+":firstChild.position.x:------"+Camera.main.transform.position.x+" :Camera.main.transform.position.x");
//				if (firstChild.position.x < Camera.main.transform.position.x)
//				{
					if (firstChild.renderer.IsVisibleFrom(Camera.main) == false)
					{
						// Get the last child position.
				Transform lastChild = backgroundPart[2];
						Vector3 lastPosition = lastChild.transform.position;
						Vector3 lastSize = (lastChild.renderer.bounds.max - lastChild.renderer.bounds.min);
						
						// Set the position of the recyled one to be AFTER
						// the last child.
						// Note: Only work for horizontal scrolling currently.
						firstChild.position = new Vector3(lastPosition.x + lastSize.x, firstChild.position.y, firstChild.position.z);
						
						// Set the recycled child to the last position
						// of the backgroundPart list.
						backgroundPart.Remove(firstChild);
						backgroundPart.Add(firstChild);
					}
				}
			}
//		}
	}
