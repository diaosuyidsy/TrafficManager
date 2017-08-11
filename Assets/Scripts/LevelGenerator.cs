using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
	public GameObject[] Roads;
	public GameObject PlayerPrefab;
	public int maxPlayers = 20;
	public GameObject PlayerParent;

	private List<Vector3> RoadPos;
	private List<int> numset = new List<int> ();
	// Use this for initialization
	void Start ()
	{
		init ();
	}

	void init ()
	{
		RoadPos = new List<Vector3> ();
		foreach (GameObject road in Roads) {
			for (int i = 0; i < road.transform.childCount; i++) {
				RoadPos.Add (road.transform.GetChild (i).transform.position);
			}
		}
		for (int i = 0; i < maxPlayers; i++) {
			int val = Random.Range (0, RoadPos.Count);
			while (numset.Contains (val)) {
				val = Random.Range (0, RoadPos.Count);
			}
			numset.Add (val);
			Collider2D[] colliders = Physics2D.OverlapCircleAll (RoadPos [val], 0.5f);
			Debug.Log (colliders.Length);
			Vector3 rot = Vector3.zero;
			foreach (Collider2D coll in colliders) {
				if (coll.gameObject.tag == "Step") {
					rot = coll.gameObject.transform.eulerAngles;
				}
			}
			GameObject newP = Instantiate (PlayerPrefab, RoadPos [val], Quaternion.identity, PlayerParent.transform);
			newP.transform.eulerAngles = new Vector3 (rot.x, rot.y, rot.z + 90);
		}
	}
}
