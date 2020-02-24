using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonScript : MonoBehaviour
{

	public float dragonSpeed;
	public float aggroRange;

	private Vector3 direction;

	private GameObject player;

	public bool eaten = false;
	public GameObject stomach;

	void Start()
	{
		//Starts the ChangeDirection coroutine and sets a random direction for the dragon to travel.
		//The player object is also stored in a variable, since it is the only thing the dragon cares about.
		direction = (new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0f)).normalized;
		player = GameObject.FindGameObjectWithTag("Player");
		StartCoroutine("ChangeDirection");
	}

	void FixedUpdate()
	{
		//We check to make sure that the dragon hasn't been picked up by the bird.
		if (eaten == false)
		{
			//Here, the dragon will follow the player if they are inside the dragon's aggro range.
			if (Vector3.Distance(transform.position, player.transform.position) <= aggroRange && player.GetComponent<PlayerController>().eaten == false)
			{
				//If the player has the sword, however, the dragon will run away.
				if (player.GetComponent<PlayerController>().isCarrying == true && player.GetComponent<PlayerController>().carriedObject.name == "Sword")
				{
					GetComponent<Rigidbody2D>().transform.position = Vector3.MoveTowards(GetComponent<Rigidbody2D>().transform.position, -player.transform.position, dragonSpeed * Time.deltaTime);
				}

				else
					GetComponent<Rigidbody2D>().transform.position = Vector3.MoveTowards(GetComponent<Rigidbody2D>().transform.position, player.transform.position, dragonSpeed * Time.deltaTime);
			}

			//If not, the dragon will carry on in his random direction.
			else
			{
				GetComponent<Rigidbody2D>().transform.position += direction * dragonSpeed * Time.deltaTime;
			}
		}
		else
		{
			//If it has been eaten, it will follow the bird.
			transform.position = stomach.transform.position;
		}
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		//As with the bird, we check first to see if the dragon is running into an outside wall.
		//If it is, we send him off in a random direction.
		if (col.gameObject.tag == "Wall")
		{
			direction = (new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0f)).normalized;
		}

		//We then check to see if the dragon collided with the player.
		else if (col.gameObject.tag == "Player")
		{
			//Then we check to see if the player is carrying a sword.
			if (col.gameObject.GetComponent<PlayerController>().carriedObject != null)
			{
				if (col.gameObject.GetComponent<PlayerController>().carriedObject.name == "Sword")
				{
					//If he is, we kill the dragon.
					Destroy(this.gameObject);
					return;
				}
			}

			//Otherwise, the player is eaten.
			player.SendMessage("EatMe", gameObject);
			Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>(), true);
		}

		//If the object is none of the above, the dragon will ignore the collision.
		else
		{
			Physics2D.IgnoreCollision(col.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
		}
	}

	//Randomly changes the direction of the dragon every 1-10 seconds.
	IEnumerator ChangeDirection()
	{
		while (true)
		{
			yield return new WaitForSeconds(Random.Range(1f, 10f));
			direction = (new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0f)).normalized;
		}
	}

	//Called when the dragon is picked up by the bird.
	void EatMe(GameObject Feaster)
	{
		eaten = true;
		stomach = Feaster;
	}

	//Called when the dragon is dropped by the bird.
	void Dropped()
	{
		eaten = false;
		Physics2D.IgnoreCollision(stomach.GetComponent<Collider2D>(), GetComponent<Collider2D>(), false);
		stomach = null;
	}

}