using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager GM;
	public GameObject gameOverText;
	public Text StepNum;
	public int maxStepNumber = 9;
	public int maxChangeTime = 1;
	public GameObject[] Roads;
	public Text ChangeNum;
	public int totalTraffic;
	public GameObject youWinText;
	public Button AdvanceButton;

	private float stepNumber = 0;
	private int changed = 0;
	private int passedTraffic = 0;

	void Awake ()
	{
		GM = this;
	}

	public void RestartGame ()
	{
		SceneManager.LoadScene ("Main");
	}

	public void Score ()
	{
		passedTraffic++;
		if (passedTraffic == totalTraffic) {
			youWinText.SetActive (true);
		}
	}

	public bool canChange ()
	{
		return changed < maxChangeTime;
	}

	public void changeChanged (bool unchange)
	{
		if (unchange)
			changed--;
		else
			changed++;
		ChangeNum.text = changed.ToString () + " / " + maxChangeTime.ToString ();
	}

	public void GameOver ()
	{
		gameOverText.SetActive (true);
	}

	public void Step ()
	{
		changed = 0;
		ChangeNum.text = changed.ToString () + " / " + maxChangeTime.ToString ();
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
		foreach (GameObject road in Roads) {
			road.SendMessage ("step");
		}

		foreach (GameObject player in players) {
			player.SendMessageUpwards ("move");
		}
		stepNumber++;
		if (stepNumber > maxStepNumber) {
			StepNum.color = Color.red;
		}
		StepNum.text = stepNumber.ToString () + " / " + maxStepNumber.ToString ();
		StartCoroutine (AdvanceCoolDown (0.5f));
	}

	IEnumerator AdvanceCoolDown (float time)
	{
		AdvanceButton.interactable = false;
		yield return new WaitForSeconds (time);
		AdvanceButton.interactable = true;
	}
}
