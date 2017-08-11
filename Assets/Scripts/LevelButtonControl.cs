using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelButtonControl : MonoBehaviour
{
	public Text LevelText;
	public Image LevelImage;

	private int Level;
	private Button _button;
	private bool locked = true;

	void Start ()
	{
		_button = GetComponent<Button> ();
		Level = transform.GetSiblingIndex () + 1;
		if (Level < 10) {
			LevelText.text = "0" + Level.ToString ();
		} else {
			LevelText.text = Level.ToString ();
		}
		init ();
	}

	void init ()
	{
		if (Level <= PlayerPrefs.GetInt ("LevelUnlocked", 1)) {
			locked = false;
			LevelText.gameObject.SetActive (true);
			LevelImage.gameObject.SetActive (false);
			var colors = _button.colors;
			colors.normalColor = new Color (239f / 255f, 101f / 255f, 101f / 255f);
			colors.highlightedColor = new Color (244f / 255f, 147f / 255f, 147f / 255f);
			colors.pressedColor = new Color (96f / 255f, 40f / 255f, 40f / 255f);
			_button.colors = colors;
		}
	}

	public void StartLevel ()
	{
		if (!locked) {
			EntrySceneControl.ESC.MoveOutLevelContainer ();
			StartCoroutine (startLevelHelper (1f));
		}
	}

	IEnumerator startLevelHelper (float time)
	{
		yield return new WaitForSeconds (time);
		SceneManager.LoadScene (Level);
	}
}
