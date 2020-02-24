using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gary : MonoBehaviour
{

	//This is Gary the Dragon. Gary has his own special script because he is a special dragon.
	//That is to say, Gary can't really do anything right, and is just in the game for fun.

	public float dragonSpeed;
	public float aggroRange;
	private bool touchedPlayer = false;

	private Vector3 direction;

	private GameObject player;

	public bool eaten = false;
	public GameObject stomach;

	void Start()
	{
		//Like the other directions, Gary will travel in a random direction.
		//The player object is also stored in a variable, because Gary cares about the player...
		//...Most the time.
		direction = (new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0f)).normalized;
		player = GameObject.FindGameObjectWithTag("Player");
		StartCoroutine("ChangeDirection");
	}


	void FixedUpdate()
	{
		if (eaten != true)
		{
			//Gary will only start moving if he has touched the player.
			if (touchedPlayer == true)
			{
				//Gary will follow the player if the player is in his aggro range, but only 2/3 times.
				if (Vector3.Distance(transform.position, player.transform.position) <= aggroRange && player.GetComponent<PlayerController>().eaten == false)
				{
					if (player.GetComponent<PlayerController>().isCarrying == true && player.GetComponent<PlayerController>().carriedObject.name == "Sword")
					{
						GetComponent<Rigidbody2D>().transform.position = Vector3.MoveTowards(GetComponent<Rigidbody2D>().transform.position, -player.transform.position, dragonSpeed * Time.deltaTime);
					}

					else
					{
						if (Random.Range(1, 3) <= 2)
							GetComponent<Rigidbody2D>().transform.position = Vector3.MoveTowards(GetComponent<Rigidbody2D>().transform.position, player.transform.position, dragonSpeed * Time.deltaTime);
						else
							direction = (new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0f)).normalized;
					}
				}
				//Gary likes to spin.
				transform.Rotate(0, 0, 180 * Time.deltaTime);
				GetComponent<Rigidbody2D>().transform.position += direction * dragonSpeed * Time.deltaTime;
			}

			else
			{
				if (Vector3.Distance(transform.position, player.transform.position) <= aggroRange && player.GetComponent<PlayerController>().eaten == false)
				{
					GetComponent<Rigidbody2D>().transform.position = Vector3.MoveTowards(GetComponent<Rigidbody2D>().transform.position, player.transform.position, dragonSpeed * Time.deltaTime);
				}
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
		//Makes sure that Gary can't leave the area. Because he most definitely would.
		if (col.gameObject.tag == "Wall")
		{
			direction = (new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0f)).normalized;
		}

		else if (col.gameObject.tag == "Player")
		{
			if (touchedPlayer == true)
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
				//player.SendMessage("EatMe", gameObject);
				//Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>(), true);
			}

			else
				touchedPlayer = true;
		}

		else
			Physics2D.IgnoreCollision(col.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
	}

	//As the other dragons do, Gary will randomly change directions, but more frequently.
	IEnumerator ChangeDirection()
	{
		while (true)
		{
			yield return new WaitForSeconds(Random.Range(1f, 6f));
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
