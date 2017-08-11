using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadControl : MonoBehaviour
{
	//0 is white, 1 is red
	private int stepColor = 0;
	private Color startColor;

	public void step ()
	{
		foreach (Transform child in transform) {
			child.GetChild (0).SendMessage ("step");
		}
	}

	public void unstep ()
	{
		foreach (Transform child in transform) {
			child.GetChild (0).SendMessage ("unstep");
		}
	}

	public int getStepColor ()
	{
		return stepColor;
	}
}
