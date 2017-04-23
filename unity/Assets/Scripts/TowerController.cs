using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TowerController : MonoBehaviour {

	public GameObject roomTemplate;
	public GameObject personTemplate;
	public Text workerText;
	public Text resourceText;
	private int numFloors = 2;
	private int numRoomsPerFloor = 1;
	private int numFaces = 4;
	public float hope = 0;
	public float chaos = 0;
	public int numStartingPopulation = 10;

	public RoomController[] rooms;
	float elapsed;
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

		foreach (PersonController person in population) {
			RoomController room = person.CurrentRoom;
			if (room == null) {
				continue;
			}
			if (room.type == RoomType.Empty || room.type == RoomType.Rubble) {
				continue;
			}
			person.SetJobAssignment(JobAssignment.OperatingRoom);
		}

		UpdateWorkerText();
	}
	
	RoomController GetRoomByFloorFacePosition(int floor, int face, int position) {
		Debug.Log(floor * (numFaces * numRoomsPerFloor) + face * numRoomsPerFloor + position);
		return rooms[floor * (numFaces * numRoomsPerFloor) + face * numRoomsPerFloor + position];
	}

	public int IdleWorkerCount {
		get {
			int num = 0;
			foreach (PersonController person in population) {
				if (person.Job == JobAssignment.Idle) {
					num += 1;
				}
			}
			return num;
		}
	}

	public List<PersonController> GetWorkersByJob(JobAssignment job) {
		List<PersonController> workers = new List<PersonController>();
		foreach (PersonController person in population) {
			if (person.Job == job) {
				workers.Add(person);
			}
		}
		return workers;
	}

	public List<PersonController> GetBoredWorkers() {
		List<PersonController> workers = new List<PersonController>();
		foreach (PersonController person in population) {
			if (person.Bored) {
				workers.Add(person);
			}
		}
		return workers;
	}


	public List<RoomController> GetJobVacancies() {
		List<RoomController> vacancies = new List<RoomController>();
		foreach (RoomController room in rooms) {
			if (room.IsRoomFull) { continue; }
			if ((room.type == RoomType.Empty || 
				 room.type == RoomType.Rubble) &&
				!room.assignment.assigned 
				) {
				 continue; 
				}
			int numVacancies = RoomController.maxRoomOccupancy - room.WorkerCount;
			for (int i = 0; i < numVacancies; i++) {
				vacancies.Add(room);
			}
		}
		return vacancies;
	}

	public void FocusRoom (int floor, int face, int position) {
		RoomController room = this.GetRoomByFloorFacePosition(floor, face, position);
		if(this.focusedRoom) {
			this.focusedRoom.UnFocusRoom();
		}
		room.FocusRoom();
		this.focusedRoom = room;
		Debug.Log("Workes: " + room.WorkerCount);
	}

	// Update is called once per frame
	public void Update () {
		//actions on the focused room
		if(this.focusedRoom) {
			if (Input.GetKeyDown("p")) {
				this.focusedRoom.BuildPower();
			}
			if (Input.GetKeyDown("f")) {
				this.focusedRoom.BuildFarm();
			}
			if (Input.GetKeyDown("c")) {
				this.focusedRoom.Clear();
			}
		}

		if (Mathf.Floor(elapsed + Time.deltaTime) > Mathf.Floor(elapsed)) {
			ResourceCalculator.Income income = ResourceCalculator.CalculateIncome(rooms);
			UpdateResourceText(income);
		}
		elapsed += Time.deltaTime;

		List<PersonController> idleWorkers = GetBoredWorkers();
		List<RoomController> vacancies = GetJobVacancies();

		shuffleRooms(vacancies);

		int workerIndex = 0;
		int vacancyIndex = 0;

		if (idleWorkers.Count + vacancies.Count == 0) {
			return;
		}

		while (workerIndex < idleWorkers.Count && vacancyIndex < vacancies.Count) {
			PersonController person = idleWorkers[workerIndex];
			RoomController room = vacancies[vacancyIndex];
			PutPersonInRoom(person, room);
			if(room.assignment.assigned) {
				person.SetJobAssignment(JobAssignment.BuildingRoom);
			} else {
				person.SetJobAssignment(JobAssignment.OperatingRoom);
			}
			
			workerIndex += 1;
			vacancyIndex += 1;
		}

		UpdateWorkerText();
	}

	public void PutPersonInRoom(PersonController person, RoomController room) {
		room.AddPersonToRoom(person);
		person.SetCurrentRoom(room);
	}

	public void Log () {
		Debug.Log("Hi");
	}

	void UpdateWorkerText() {
		workerText.text = IdleWorkerCount.ToString() + " / " + population.Length.ToString();
	}

	void UpdateResourceText(ResourceCalculator.Income income) {
		resourceText.text = "Food: " + income.food + " Energy: " + income.energy;
	}

	void shuffleRooms(List<RoomController> rooms)
	{
		for (int t = 0; t < rooms.Count; t++ )
		{
			RoomController tmp = rooms[t];
			int r = Random.Range(t, rooms.Count);
			rooms[t] = rooms[r];
			rooms[r] = tmp;
		}
	}
 
	
}
