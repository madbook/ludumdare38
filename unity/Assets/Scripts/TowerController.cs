using UnityEngine;

public class TowerController : MonoBehaviour {

	public GameObject roomTemplate;
	public int numFloors = 4;
	public int numRoomsPerFloor = 3;

	public static float hope = 0;
	public static float chaos = 0;

	// Use this for initialization
	void Start () {
		TowerSetup.CreateTower(
			numFloors,
			numRoomsPerFloor,
			roomTemplate,
			gameObject,
			this
		);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Log () {
		Debug.Log("Hi");
	}
}
