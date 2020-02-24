using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

	//private Vector3 playerPosition;
	public float playerSpeed;
	private Vector3 originPos = new Vector3();

	public GameObject carriedObject;
	public bool isCarrying = false;

	public GameObject stomach;
	public bool eaten = false;

	private GameObject cam;
	public bool cameraMoving = false;

	private int timesReset = 0;

	public Font Terminal;

	public bool won = false;

	void Start()
	{
		originPos = transform.position;
		cam = GameObject.FindGameObjectWithTag("MainCamera");
	}

	void Update()
	{
		//Returns to the main menu if "Escape" is pressed.
		if (Input.GetKeyDown(KeyCode.Escape) == true)
		{
			SceneManager.LoadScene("mainMenu", LoadSceneMode.Single);
		}

		//Drops the currently held item when "Space" is pressed.
		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (isCarrying == true && carriedObject != null)
			{
				carriedObject.SendMessage("Dropped");
				isCarrying = false;
				carriedObject = null;
			}
		}

		//Calls the Reset method when 'R' is pressed.
		if (Input.GetKeyDown(KeyCode.R))
			Reset();
	}

	void FixedUpdate()
	{
		//Camera follow (Testing purposes only)
		//Vector3 newCamPos = new Vector3 (transform.position.x, transform.position.y, -10f);
		//cam.transform.position = newCamPos;

		//Player movement.
		if (eaten != true)
		{
			//If the player has won, they can't move.
			if (won != true)
			{
				GetComponent<Rigidbody2D>().transform.position += Vector3.right * Input.GetAxis("Horizontal") * playerSpeed * Time.deltaTime;
				GetComponent<Rigidbody2D>().transform.position += Vector3.up * Input.GetAxis("Vertical") * playerSpeed * Time.deltaTime;
			}
		}

		//If the player is eaten by the bird or dragon however, it will follow whatever has eaten it.
		else
		{
			cam.transform.position = gameObject.transform.position + (Vector3.back * 10);
			transform.position = stomach.transform.position;
		}
	}

	void OnGUI()
	{
		GUI.skin.font = Terminal;

		//Creates the reset button that, when pressed, calls the Reset method.
		if (GUI.Button(new Rect(25, 20, 60, 15), "Reset"))
		{
			Reset();
		}

		GUIStyle style = new GUIStyle();
		style.font = Terminal;
		style.normal.textColor = Color.black;

		//Creates the GUI label to tell the player how many times they have had to reset.
		GUI.Label(new Rect(25, 40, 50, 15), "Times Reset: " + timesReset.ToString(), style);
	}

	//Clears variables, and returns the player to the original position while increasing the display
	//number for how many times the player has had to reset.
	void Reset()
	{
		//Drop the trophy so they can't carry it back to the start.
		if (isCarrying == true)
		{
			carriedObject.SendMessage("Dropped");
			isCarrying = false;
			carriedObject = null;
		}
		transform.position = originPos;
		cam.transform.position = new Vector3(0, 0, -10);
		GetComponent<SpriteRenderer>().color = Color.yellow;
		eaten = false;
		if (stomach != null)
			Physics2D.IgnoreCollision(GetComponent<Collider2D>(), stomach.GetComponent<Collider2D>(), false);
		stomach = null;
		timesReset++;
	}

	//Called when the player is eaten by the bird or the dragon.
	void EatMe(GameObject Feaster)
	{
		eaten = true;
		stomach = Feaster;
	}

	//Handles the ability of the player to pick up objects. Currently, the player can steal items from the
	//bird.
	void OnCollisionEnter2D(Collision2D col)
	{
		if (eaten != true)
		{
			if (col.gameObject.tag == "Item")
			{
				if (col.gameObject.name == "Sword")
				{
					col.gameObject.GetComponent<SwordScript>().carrier = gameObject;
					col.gameObject.GetComponent<SwordScript>().isCarried = true;
				}
				else if (col.gameObject.name.Contains("Key"))
				{
					col.gameObject.GetComponent<KeyScript>().carrier = gameObject;
					col.gameObject.GetComponent<KeyScript>().isCarried = true;
				}
				else if (col.gameObject.name == ("Trophy"))
				{
					col.gameObject.GetComponent<TrophyScript>().carrier = gameObject;
					col.gameObject.GetComponent<TrophyScript>().isCarried = true;
				}
				isCarrying = true;
				carriedObject = col.gameObject;
			}
		}
	}

	//Called when the bird drops the player.
	void Dropped()
	{
		eaten = false;
		Physics2D.IgnoreCollision(stomach.GetComponent<Collider2D>(), GetComponent<Collider2D>(), false);
		stomach = null;
	}
}