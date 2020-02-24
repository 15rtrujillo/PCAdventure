using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleTele : MonoBehaviour
{

	//This script is mainly used to transfer between gates and castles, but can have other uses.
	public GameObject origin;
	public GameObject destination;

	public bool gateOpen = false;

	void OnCollisionEnter2D(Collision2D col)
	{
		//This line checks to see if the collision object is the player.
		if (col.gameObject.tag == "Player")
		{
			//If it is, the player object is stored in a variable.
			GameObject player = col.gameObject;
			//Then, we check to see if the origin tele was a gate.
			if (origin.name.Contains("Gate"))
			{
				//If it was, we then check to see if the gate is open. If it isn't, we halt the script.
				if (gateOpen == true)
				{
					//If the gate is open, we move the player to the inside of the castle,
					//just above the destination teleport pad.
					//But, only if the camera isn't moving.
					if (player.GetComponent<PlayerController>().cameraMoving != true)
					{
						Vector3 newPos = new Vector3(destination.transform.position.x, destination.transform.position.y + 1.5f);
						player.transform.position = newPos;
						//The camera is then moved to the correct position.
						Vector3 camPos = new Vector3(destination.transform.parent.gameObject.transform.position.x, destination.transform.parent.gameObject.transform.position.y, -10);
						Camera.main.transform.position = camPos;
					}
				}

				else
					return;
			}

			else
			{
				//If the origin pad was not a gate, we just send the player to where he wants to go.
				//This is usually a gate, so we place the player a few points below it.
				Vector3 newPos = new Vector3(destination.transform.position.x, destination.transform.position.y - 1.5f);
				player.transform.position = newPos;
				//The camera is then moved to the correct position.
				Vector3 camPos = new Vector3(destination.transform.parent.gameObject.transform.position.x, destination.transform.parent.gameObject.transform.position.y, -10);
				Camera.main.transform.position = camPos;
			}
		}
	}
}
