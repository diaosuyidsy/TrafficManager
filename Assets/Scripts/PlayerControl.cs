using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
	public enum Directions
	{
		Up,
		Down,
		Left,
		Right,
	};

	public Directions direction;
	public float MoveSpeed = 3f;
	public bool lockMove = false;

	private bool startMoving = false;

	// Use this for initialization
	void Start ()
	{
		
	}

	public void move ()
	{
		if (!lockMove)
			StartCoroutine (MoveAss (0.385f));
	}

	IEnumerator MoveAss (float time)
	{
		startMoving = true;
		yield return new WaitForSeconds (time);
//		startMoving = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!startMoving)
			return;
		transform.Translate (Vector3.right * Time.deltaTime * MoveSpeed);
//		switch (direction) {
//		case Directions.Right:
//			transform.Translate (Vector3.right * Time.deltaTime * MoveSpeed);
//			break;
//		case Directions.Left:
//			transform.Translate (Vector3.left * Time.deltaTime * MoveSpeed);
//			break;
//		case Directions.Down:
//			transform.Translate (Vector3.up * Time.deltaTime * MoveSpeed);
//			break;
//		case Directions.Up:
//			transform.Translate (Vector3.down * Time.deltaTime * MoveSpeed);
//			break;
//		}
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "Player") {
			GameManager.GM.GameOver ();
		} else if (other.gameObject.tag == "Step") {
			startMoving = false;
		}
	}
}
