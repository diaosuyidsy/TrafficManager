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
	public GameObject HitEffectPrefab;
	public GameObject PlayerSprite;
	public int maxStepIndurence = 1;

	public bool startMoving = false;
	private int stepIndurence = 1;

	void Start ()
	{
		stepIndurence = maxStepIndurence;
		if (stepIndurence == 2) {
			PlayerSprite.GetComponent<SpriteRenderer> ().color = new Color (122f / 255f, 128f / 255f, 1f);
		}
	}

	public void move ()
	{
		stepIndurence = maxStepIndurence - 1;
		if (!lockMove)
			startMoving = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!startMoving) {
			return;
		}
		transform.Translate (Vector3.right * Time.deltaTime * MoveSpeed);
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "Player") {
			Instantiate (HitEffectPrefab, gameObject.transform.position, Quaternion.identity);
			gameObject.SetActive (false);
			GameManager.GM.GameOver ();
		} else if (other.gameObject.tag == "Step") {
			stepIndurence--;
			if (stepIndurence <= -1)
				startMoving = false;
		}
	}
}
