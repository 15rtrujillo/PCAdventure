using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrophyScript : MonoBehaviour
{

	public bool isCarried = false;
	public GameObject carrier;
	private GameObject player;

	public Color[] colorList;
	private int colorInt = 0;

	public Font terminal;

	public GameObject winRoom;

	private Vector3 newTrophyPos;

	void Start()
	{
		//Win state testing.
		//StartCoroutine(WinState());
		//Save the player in a variable.
		player = GameObject.FindGameObjectWithTag("Player");
	}

	void Update()
	{
		//If the trophy is carried, it will follow whatever is holding it.
		if (isCarried == true)
		{
			newTrophyPos.x = carrier.transform.position.x;
			newTrophyPos.y = carrier.transform.position.y + 1;
			transform.position = newTrophyPos;
		}
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if (isCarried == true)
		{
			if (carrier.tag == "Player")
			{
				if (col.gameObject.tag == "Win")
				{
					transform.position = col.gameObject.transform.position + (Vector3.up * 1.3f);
					StartCoroutine(WinState());
					Destroy(GetComponent<Collider2D>());
					carrier.GetComponent<PlayerController>().isCarrying = false;
					carrier.GetComponent<PlayerController>().carriedObject = null;
					Dropped();
					player.GetComponent<PlayerController>().won = true;
				}
			}
		}
	}

	//Shows a congrats GUI if the player has won.
	void OnGUI()
	{
		if (player.GetComponent<PlayerController>().won)
		{
			GUIStyle styleBaby = new GUIStyle();
			styleBaby.alignment = TextAnchor.UpperCenter;
			styleBaby.font = terminal;
			GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 3 - 25, 100, 50), "Congradulations!", styleBaby);
		}
	}

	//This will change the color of the "winroom" and the player in a rainbow pattern.
	IEnumerator WinState()
	{
		while (true)
		{
			yield return new WaitForSeconds(.5f);

			foreach (Transform child in winRoom.transform)
			{
				if (child.tag == "Wall")
				{
					child.GetComponent<SpriteRenderer>().color = colorList[colorInt];
				}
			}

			player.GetComponent<SpriteRenderer>().color = colorList[colorInt];

			if (colorInt < 5)
				colorInt++;
			else
				colorInt = 0;
		}
	}

	//Called when the trophy is dropped to reset variables.
	void Dropped()
	{
		isCarried = false;
		Physics2D.IgnoreCollision(carrier.GetComponent<Collider2D>(), GetComponent<Collider2D>(), false);
		carrier = null;
	}

}