using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EntrySceneControl : MonoBehaviour
{
	public static EntrySceneControl ESC;
	public GAui Title;
	public GAui PlayGame;
	public GAui Resume;
	public GAui Back;
	public GAui LevelContainer;
	public GAui LevelSelect;

	void Awake ()
	{
		ESC = this;
	}

	public void EnterSelectLevel ()
	{
		Title.MoveOut (GUIAnimSystem.eGUIMove.SelfAndChildren);
		PlayGame.MoveOut (GUIAnimSystem.eGUIMove.SelfAndChildren);
		Resume.MoveOut (GUIAnimSystem.eGUIMove.SelfAndChildren);
		StartCoroutine (SelectLevelEnter (0.5f));
	}

	IEnumerator SelectLevelEnter (float time)
	{
		yield return new WaitForSeconds (time);
		LevelSelect.MoveIn (GUIAnimSystem.eGUIMove.SelfAndChildren);
		LevelContainer.MoveIn (GUIAnimSystem.eGUIMove.SelfAndChildren);
		Back.MoveIn (GUIAnimSystem.eGUIMove.SelfAndChildren);
	}

	public void BackToMain ()
	{
		LevelSelect.MoveOut (GUIAnimSystem.eGUIMove.SelfAndChildren);
		LevelContainer.MoveOut (GUIAnimSystem.eGUIMove.SelfAndChildren);
		Back.MoveOut (GUIAnimSystem.eGUIMove.SelfAndChildren);
		StartCoroutine (TitleScreenEnter (0.75f));
	}

	IEnumerator TitleScreenEnter (float time)
	{
		yield return new WaitForSeconds (time);
		Title.MoveIn (GUIAnimSystem.eGUIMove.SelfAndChildren);
		PlayGame.MoveIn (GUIAnimSystem.eGUIMove.SelfAndChildren);
		Resume.MoveIn (GUIAnimSystem.eGUIMove.SelfAndChildren);
	}

	public void MoveOutLevelContainer ()
	{
		LevelSelect.MoveOut (GUIAnimSystem.eGUIMove.SelfAndChildren);
		LevelContainer.MoveOut (GUIAnimSystem.eGUIMove.SelfAndChildren);
		Back.MoveOut (GUIAnimSystem.eGUIMove.SelfAndChildren);
	}

	IEnumerator MoveOutMainAndResume (float time)
	{
		Title.MoveOut (GUIAnimSystem.eGUIMove.SelfAndChildren);
		PlayGame.MoveOut (GUIAnimSystem.eGUIMove.SelfAndChildren);
		Resume.MoveOut (GUIAnimSystem.eGUIMove.SelfAndChildren);
		yield return new WaitForSeconds (time);
		SceneManager.LoadScene (PlayerPrefs.GetInt ("LevelUnlocked", 0));

	}

	public void ResumeLevel ()
	{
		StartCoroutine (MoveOutMainAndResume (0.9f));
	}
}
