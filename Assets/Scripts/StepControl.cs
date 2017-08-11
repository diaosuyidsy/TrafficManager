using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepControl : MonoBehaviour
{
	public bool canLockPlayer = false;
	public GameObject stepTouch;

	public void LockDown (bool locked)
	{
		Collider2D[] colliders = Physics2D.OverlapCircleAll (transform.position, 0.5f);
		foreach (Collider2D coll in colliders) {
			if (coll.gameObject.tag == "Player") {
				coll.gameObject.GetComponent<PlayerControl> ().lockMove = locked;
			}
		}
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "Player" && stepTouch.GetComponent <StepTouch> ().getStepColor () == 1 && canLockPlayer) {
			other.gameObject.GetComponent<PlayerControl> ().lockMove = true;
			other.gameObject.GetComponent<PlayerControl> ().startMoving = false;
		}
	}
}
