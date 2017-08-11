using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager GM;
	public Text StepNum;
	public int maxStepNumber = 9;
	public int excellentStepNumber;
	public int maxChangeTime = 1;
	public Text ChangeNum;
	public Button AdvanceButton;
	public GameObject Players;
	public Button BackwardButton;
	public Text LevelText;
	public GAui ResultPanel;
	public Text ResultP_ResultText;
	public Image ResultP_StarImg;
	public GameObject[] ResultP_Next_Redo_Button;
	public GAui RestartButton;
	public GAui HomeButton;
	public CanvasGroup maskPanel;

	private int stepNumber = 0;
	private int changed = 0;
	private int passedTraffic = 0;
	private int totalTraffic;
	private Vector3[] PlayerPosRecorder;
	private bool[] PlayerAvailableRecorder;
	private Vector3[] PlayerRotRecorder;
	private GameObject Roads;
	private int lastPassedTraffic;
	private bool MenuMoveIn = true;


	void Awake ()
	{
		GM = this;
	}

	void Start ()
	{
		Roads = GameObject.FindGameObjectWithTag ("Roads");
		totalTraffic = Players.transform.childCount;
		ChangeNum.text = "0 / " + maxChangeTime.ToString ();
		StepNum.text = "0 / " + maxStepNumber.ToString ();
		LevelText.text = "LEVEL " + SceneManager.GetActiveScene ().buildIndex.ToString ();
		StartCoroutine (init (0.5f));

	}

	IEnumerator init (float time)
	{
		yield return new WaitForSeconds (time);
		PlayerPosRecorder = new Vector3[Players.transform.childCount];
		PlayerAvailableRecorder = new bool[Players.transform.childCount];
		PlayerRotRecorder = new Vector3[Players.transform.childCount];
		//initialize playerposrecorder;
		for (int i = 0; i < Players.transform.childCount; i++) {
			PlayerPosRecorder [i] = Players.transform.GetChild (i).position;
			PlayerRotRecorder [i] = Players.transform.GetChild (i).eulerAngles;
			PlayerAvailableRecorder [i] = Players.transform.GetChild (i).gameObject.activeSelf;
		}
		lastPassedTraffic = 0;
	}

	public void Score ()
	{
		passedTraffic++;
		if (passedTraffic == totalTraffic) {
			if (stepNumber <= maxStepNumber)
				EnterResult (true);
			else
				EnterResult (false);
		}
	}

	void EnterResult (bool win)
	{
		ResultPanel.gameObject.SetActive (true);
		if (win) {
			ResultP_ResultText.text = "VICTORY";
			PlayerPrefs.SetInt ("LevelUnlocked", SceneManager.GetActiveScene ().buildIndex + 1);
			BackwardButton.gameObject.SetActive (false);
			AdvanceButton.gameObject.SetActive (false);
			if (stepNumber > excellentStepNumber)
				ResultP_StarImg.gameObject.SetActive (false);
		} else {
			ResultP_ResultText.text = "DEFEAT";
			ResultP_StarImg.gameObject.SetActive (false);
			ResultP_Next_Redo_Button [1].SetActive (true);
			ResultP_Next_Redo_Button [0].SetActive (false);
			AdvanceButton.gameObject.SetActive (false);
		}
		maskPanel.alpha = 0.5f;
		ResultPanel.MoveIn (GUIAnimSystem.eGUIMove.SelfAndChildren);
	}

	public void FoldMenu ()
	{
		if (MenuMoveIn) {
			RestartButton.MoveIn (GUIAnimSystem.eGUIMove.SelfAndChildren);
			HomeButton.MoveIn (GUIAnimSystem.eGUIMove.SelfAndChildren);
		} else {
			RestartButton.MoveOut (GUIAnimSystem.eGUIMove.SelfAndChildren);
			HomeButton.MoveOut (GUIAnimSystem.eGUIMove.SelfAndChildren);
		}
		MenuMoveIn = !MenuMoveIn;
	}

	public void RestartGame ()
	{
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

	public void ReturnToLevelSelect ()
	{
		SceneManager.LoadScene (0);
	}

	public void nextLevel ()
	{
		int currentLevel = SceneManager.GetActiveScene ().buildIndex;
		if (currentLevel < SceneManager.sceneCountInBuildSettings - 1) {
			SceneManager.LoadScene (currentLevel + 1);
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
		EnterResult (false);
	}

	public void Step ()
	{
		//First of all, need to record where we are so as to step back
		Record ();
		changed = 0;
		ChangeNum.text = changed.ToString () + " / " + maxChangeTime.ToString ();
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
		foreach (Transform road in Roads.transform) {
			road.gameObject.SendMessage ("step");
		}

		foreach (GameObject player in players) {
			player.SendMessageUpwards ("move");
		}
		stepNumber++;
		if (stepNumber > maxStepNumber) {
			StepNum.color = Color.red;
		}
		StepNum.text = stepNumber.ToString () + " / " + maxStepNumber.ToString ();
		StartCoroutine (AdvanceCoolDown (1f));

	}

	public void UnStep ()
	{
		maskPanel.alpha = 0f;
		AdvanceButton.gameObject.SetActive (true);

		// Unroll player action
		for (int i = 0; i < Players.transform.childCount; i++) {
			Players.transform.GetChild (i).position = PlayerPosRecorder [i];
			Players.transform.GetChild (i).gameObject.SetActive (PlayerAvailableRecorder [i]);
			Players.transform.GetChild (i).eulerAngles = PlayerRotRecorder [i];
			Players.transform.GetChild (i).gameObject.GetComponent<PlayerControl> ().lockMove = false;
			Players.transform.GetChild (i).gameObject.GetComponent<PlayerControl> ().startMoving = false;
		}
		foreach (Transform road in Roads.transform) {
			road.gameObject.GetComponent<RoadControl> ().unstep ();
		}
		// Advance Step Number
		stepNumber++;
		if (stepNumber > maxStepNumber) {
			StepNum.color = Color.red;
		}
		StepNum.text = stepNumber.ToString () + " / " + maxStepNumber.ToString ();
		// Unroll change Number
		changed = 0;
		ChangeNum.text = changed.ToString () + " / " + maxChangeTime.ToString ();
		passedTraffic = lastPassedTraffic;
		ResultPanel.MoveOut (GUIAnimSystem.eGUIMove.SelfAndChildren);
		BackwardButton.interactable = false;
	}

	IEnumerator AdvanceCoolDown (float time)
	{
		AdvanceButton.interactable = false;
		yield return new WaitForSeconds (time);
		//Post Step era
		AdvanceButton.interactable = true;
		BackwardButton.interactable = true;
	}

	void Record ()
	{
		for (int i = 0; i < Players.transform.childCount; i++) {
			PlayerPosRecorder [i] = Players.transform.GetChild (i).position;
			PlayerRotRecorder [i] = Players.transform.GetChild (i).eulerAngles;
			PlayerAvailableRecorder [i] = Players.transform.GetChild (i).gameObject.activeSelf;
		}
		lastPassedTraffic = passedTraffic;
	}
}
