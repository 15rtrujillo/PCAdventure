using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

	public Color32 colorToChange;
	public string direction;
	private GameObject cam;
	private GameObject player;
	public bool cameraMoving = false;

	void Start()
	{
		//Stores the camera in a variable.
		cam = GameObject.FindGameObjectWithTag("MainCamera");
		player = GameObject.FindGameObjectWithTag("Player");
	}

	void OnCollisionEnter2D(Collision2D collider)
	{
		//Checks to see if it was the player that collided with the transition.
		if (collider.gameObject.tag == "Player")
		{
			//Changes the player's color.
			player.GetComponent<SpriteRenderer>().color = colorToChange;

			//Moves the player in the appropriate direction, through the transition object.
			if (direction == "Down")
				player.transform.Translate(0, -1.5f, 0);
			else if (direction == "Up")
				player.transform.Translate(0, 1.5f, 0);
			else if (direction == "Right")
				player.transform.Translate(1.5f, 0, 0);
			else if (direction == "Left")
				player.transform.Translate(-1.5f, 0, 0);

			//Starts the coroutine to move the camera.
			string co = "MoveCamera" + direction;
			StartCoroutine(co);
		}
	}

	//The following functions will smoothly move the camera to the next scene.
	IEnumerator MoveCameraUp()
	{
		player.GetComponent<PlayerController>().cameraMoving = true;
		for (int i = 0; i < 50; i++)
		{
			cam.transform.Translate(0, .2f, 0);
			yield return null;
		}
		player.GetComponent<PlayerController>().cameraMoving = false;
	}
	IEnumerator MoveCameraRight()
	{
		player.GetComponent<PlayerController>().cameraMoving = true;
		for (int i = 0; i < 75; i++)
		{
			cam.transform.Translate(.2f, 0, 0);
			yield return null;
		}
		player.GetComponent<PlayerController>().cameraMoving = false;
	}
	IEnumerator MoveCameraLeft()
	{
		player.GetComponent<PlayerController>().cameraMoving = true;
		for (int i = 0; i < 75; i++)
		{
			cam.transform.Translate(-.2f, 0, 0);
			yield return null;
		}
		player.GetComponent<PlayerController>().cameraMoving = false;
	}
	IEnumerator MoveCameraDown()
	{
		player.GetComponent<PlayerController>().cameraMoving = true;
		for (int i = 0; i < 50; i++)
		{
			cam.transform.Translate(0, -.2f, 0);
			yield return null;
		}
		player.GetComponent<PlayerController>().cameraMoving = false;
	}
}
