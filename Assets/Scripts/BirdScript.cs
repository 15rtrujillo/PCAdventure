using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdScript : MonoBehaviour
{
	public float birdSpeed;

	private Vector3 direction;

	//Used to determine if and what the bird is carrying.
	public GameObject carriedObject;
	public bool isCarrying = false;

	// Use this for initialization
	void Start()
	{
		//Ensures the bird travels in a random direction and starts various coroutines
		//(The definitions for which can be found in the individual function).
		direction = (new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0f)).normalized;
		StartCoroutine("ChangeDirection");
		StartCoroutine("RandomDrop");
		//Reduces the speed of the flap animation.
		GetComponent<Animator>().speed = .75f;
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		//Moves the bird in the specified direction, which is random.
		GetComponent<Rigidbody2D>().transform.position += direction * birdSpeed * Time.deltaTime;
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		//Used to handle how the bird collides with objects.
		//If the bird hits a outside wall, it will bounce off in a random direction.
		if (col.gameObject.tag == "Wall")
		{

			direction = (new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0f)).normalized;
		}

		//If the object is an item, the bird will attempt to pick it up if it isn't already carried.
		else if (col.gameObject.tag == "Item")
		{
			if (col.gameObject.name == "Sword")
			{
				if (col.gameObject.GetComponent<SwordScript>().isCarried != true)
				{
						col.gameObject.GetComponent<SwordScript>().carrier = gameObject;
						col.gameObject.GetComponent<SwordScript>().isCarried = true;
				}
			}

			else if (col.gameObject.name.Contains("Key"))
			{
				if (col.gameObject.GetComponent<KeyScript>().isCarried != true)
				{
						col.gameObject.GetComponent<KeyScript>().carrier = gameObject;
						col.gameObject.GetComponent<KeyScript>().isCarried = true;
				}
			}

			//This makes sure that the bird cannot carry two objects at the same time,
			//And causes the bird to drop the previously held object if there is one.
			if (isCarrying == true)
				carriedObject.SendMessage("Dropped");
			isCarrying = true;
			carriedObject = col.gameObject;
			//Since the object will be inside the bird, collisions between the two must be ignored.
			Physics2D.IgnoreCollision(col.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>(), true);

		}

		//If the object is the player, the bird will attempt to eat him/her.
		else if (col.gameObject.tag == "Player")
		{
			//Checks to make sure the player isn't already eaten by a dragon.
			if (col.gameObject.GetComponent<PlayerController>().eaten != true)
			{
				//Sends a message to the player object, updating the eaten variables, then
				//drops other item if carried.
				col.gameObject.SendMessage("EatMe", gameObject);
				Physics2D.IgnoreCollision(col.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>(), true);
				if (isCarrying == true)
					carriedObject.SendMessage("Dropped");
				isCarrying = true;
				carriedObject = col.gameObject;
			}

		}

		//If the object is a dragon, the bird will eat it and drop the currently carried item.
		else if (col.gameObject.tag == "Dragon")
		{
			col.gameObject.SendMessage("EatMe", gameObject);
			Physics2D.IgnoreCollision(col.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>(), true);
			if (isCarrying == true)
				carriedObject.SendMessage("Dropped");
			isCarrying = true;
			carriedObject = col.gameObject;

		}

		else
		{
			//If the object is nothing listed above, the bird will ignore the collision and pass through.
			Physics2D.IgnoreCollision(col.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
		}


	}

	//Causes the bird to change direction every 1-10 seconds.
	IEnumerator ChangeDirection()
	{
		while (true)
		{
			yield return new WaitForSeconds(Random.Range(1f, 10f));
			direction = (new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0f)).normalized;
		}
	}

	//The bird will drop the currently held object every 3-10 seconds, just to make the game more fair.
	IEnumerator RandomDrop()
	{
		while (true)
		{
			yield return new WaitForSeconds(Random.Range(3f, 10f));
			if (isCarrying == true && carriedObject.tag != "Player")
			{
				isCarrying = false;
				carriedObject.SendMessage("Dropped");
				if (carriedObject.tag != "Dragon")
					carriedObject.transform.position = transform.position + (Vector3.down * 1.5f);
				else
					carriedObject.transform.position = transform.position + (Vector3.down * 5);
				Physics2D.IgnoreCollision(carriedObject.GetComponent<Collider2D>(), GetComponent<Collider2D>(), false);
				carriedObject = null;
			}
		}
	}
}
