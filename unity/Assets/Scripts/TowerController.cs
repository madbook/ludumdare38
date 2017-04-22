using UnityEngine;

public class TowerController : MonoBehaviour {

	public GameObject roomTemplate;
	public GameObject personTemplate;
	public int numFloors = 4;
	public int numRoomsPerFloor = 3;
	public int numFaces = 4;
	public float hope = 0;
	public float chaos = 0;
	public int numStartingPopulation = 10;

	RoomController[] rooms;
	PersonController[] population;
	RoomController focusedRoom;

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
	
	RoomController GetRoomByFloorFacePosition(int floor, int face, int position) {
		Debug.Log(floor * (numFaces * numRoomsPerFloor) + face * numRoomsPerFloor + position);
		return rooms[floor * (numFaces * numRoomsPerFloor) + face * numRoomsPerFloor + position];
	}

	public void FocusRoom (int floor, int face, int position) {
		RoomController room = this.GetRoomByFloorFacePosition(floor, face, position);
		if(this.focusedRoom) {
			this.focusedRoom.UnFocusRoom();
		}
		room.FocusRoom();
		this.focusedRoom = room;
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void Log () {
		Debug.Log("Hi");
	}
}
