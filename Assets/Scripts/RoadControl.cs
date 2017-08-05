using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadControl : MonoBehaviour
{
	public GameObject[] Steps;

	//0 is white, 1 is red
	private int stepColor = 0;
	private int lastStepColor = 0;
	// Use this for initialization
	void Start ()
	{
		Steps [2].GetComponent<StepControl> ().canLockPlayer = true;
	}

	void OnMouseDown ()
	{
		if ((stepColor + 1) % 2 != lastStepColor && !GameManager.GM.canChange ()) {
			return;
		}
		foreach (GameObject step in Steps) {
			if (stepColor == 0) {
				step.GetComponent<SpriteRenderer> ().color = Color.red;
			} else {
				step.GetComponent<SpriteRenderer> ().color = Color.white;
			}
		}
		LockDown ();
		stepColor = (stepColor + 1) % 2;
		if (stepColor == lastStepColor) {
			GameManager.GM.changeChanged (true);
		} else {
			GameManager.GM.changeChanged (false);
		}
	}

	public void step ()
	{
		lastStepColor = stepColor;
	}

	public void LockDown ()
	{
		Steps [2].gameObject.SendMessage ("LockDown", stepColor == 0);
	}

	public int getStepColor ()
	{
		return stepColor;
	}
}
