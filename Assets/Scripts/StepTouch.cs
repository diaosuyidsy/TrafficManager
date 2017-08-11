using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepTouch : MonoBehaviour
{

	public GameObject Step;

	//0 is white, 1 is red
	private int stepColor = 0;
	private int lastStepColor = 0;
	private int lastRoundColor = 0;
	private Color startColor;

	void Start ()
	{
		Step.GetComponent<StepControl> ().canLockPlayer = true;
		startColor = Step.GetComponent<SpriteRenderer> ().color;
	}

	void OnMouseDown ()
	{
		if ((stepColor + 1) % 2 != lastStepColor && !GameManager.GM.canChange ()) {
			return;
		}
		if (stepColor == 0) {
			Step.GetComponent<SpriteRenderer> ().color = Color.red;
		} else {
			Step.GetComponent<SpriteRenderer> ().color = startColor;
		}
		stepColor = (stepColor + 1) % 2;
		LockDown ();

		if (stepColor == lastStepColor) {
			GameManager.GM.changeChanged (true);
		} else {
			GameManager.GM.changeChanged (false);
		}
	}

	public void step ()
	{
		lastRoundColor = lastStepColor;
		lastStepColor = stepColor;
	}

	public void unstep ()
	{
		stepColor = lastRoundColor;
		lastStepColor = lastRoundColor;
		if (stepColor == 0) {
			Step.GetComponent<SpriteRenderer> ().color = startColor;
		} else {
			Step.GetComponent<SpriteRenderer> ().color = Color.red;
		}

		LockDown ();
	}

	public void LockDown ()
	{
		Step.gameObject.SendMessage ("LockDown", stepColor == 1);
	}

	public int getStepColor ()
	{
		return stepColor;
	}
}
