using UnityEngine;

public class TowerController : MonoBehaviour {

	public GameObject roomTemplate;
	public GameObject personTemplate;
	public int numFloors = 4;
	public int numRoomsPerFloor = 3;
	public int numStartingPopulation = 10;
	public static float hope = 0;
	public static float chaos = 0;

	RoomController[] rooms;
	PersonController[] population;

	// Use this for initialization
	void Start () {
		rooms = TowerSetup.CreateTower(
			numFloors,
			numRoomsPerFloor,
			roomTemplate,
			gameObject,
			this
		);

		population = PopulationSetup.CreatePopulation(
			numStartingPopulation,
			personTemplate,
			rooms,
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
