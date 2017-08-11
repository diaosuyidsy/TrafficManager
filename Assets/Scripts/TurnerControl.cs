using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnerControl : MonoBehaviour
{
	public enum Direction
	{
		right,
		left,
		down,
		up,
	};

	public Direction direction;

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "Player") {
			switch (direction) {
			case Direction.right:
				other.gameObject.transform.eulerAngles = Vector3.zero;
				break;
			case Direction.left:
				other.gameObject.transform.eulerAngles = new Vector3 (0, 0, 180);
				break;
			case Direction.down:
				other.gameObject.transform.eulerAngles = new Vector3 (0, 0, -90);
				break;
			case Direction.up:
				other.gameObject.transform.eulerAngles = new Vector3 (0, 0, 90);
				break;
			}
		}
	}
}
