using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordScript : MonoBehaviour {

	public bool isCarried = false;
	public GameObject carrier;

	private Vector3 newSwordPos;

	void Update () {
		//If the sword is carried, it will follow whatever is holding it.
		if (isCarried == true) {
			newSwordPos.x = carrier.transform.position.x;
			newSwordPos.y = carrier.transform.position.y + .75f;
			transform.position = newSwordPos;
		}
	}

	void OnCollisionEnter2D(Collision2D col) {
		//If the sword held by the player, it will kill the dragon.
		if (isCarried == true) {
			if (col.gameObject.tag == "Dragon" && carrier.tag == "Player")
				Destroy (col.gameObject);
		}
	}

	//Called when the sword is dropped to reset variables.
	void Dropped() {
		isCarried = false;
		Physics2D.IgnoreCollision (carrier.GetComponent<Collider2D> (), GetComponent<Collider2D> (), false);
		carrier = null;
	}
}
