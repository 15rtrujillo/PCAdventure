using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{

	public bool isCarried = false;
	public GameObject carrier;

	private Vector3 newKeyPos;

	void Update()
	{
		//The key will follow the position of whatever is holding the key.
		if (isCarried == true)
		{
			newKeyPos.x = carrier.transform.position.x;
			newKeyPos.y = carrier.transform.position.y + .95f;
			transform.position = newKeyPos;
		}
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		//If the key is being carried by the player, and collides with the corrisponding gate of the
		//same color...
		if (isCarried == true)
		{
			if (carrier.tag == "Player")
			{
				if (col.gameObject.GetComponent<SpriteRenderer>().color == GetComponent<SpriteRenderer>().color && col.gameObject.name == "Gate")
				{
					//The GateUp animation will play, and the player is granted the ability to enter the
					//castle, as the gateOpen variable is update in the Teleport script.
					col.gameObject.GetComponent<Animator>().Play("GateUp");
					carrier.GetComponent<PlayerController>().carriedObject = null;
					carrier.GetComponent<PlayerController>().isCarrying = false;
					col.gameObject.GetComponent<CastleTele>().gateOpen = true;
					Destroy(gameObject);
				}
			}
		}
	}

	//Called when something drops the key to reset variables.
	void Dropped()
	{
		isCarried = false;
		Physics2D.IgnoreCollision(carrier.GetComponent<Collider2D>(), GetComponent<Collider2D>(), false);
		carrier = null;
	}
}
