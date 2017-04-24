using UnityEngine;
using System.Collections.Generic;

public class TowerController : MonoBehaviour {

	public GameObject roomTemplate;
	public GameObject personTemplate;
	public static int numFloors = 5;
	public static int numRoomsPerFloor = 2;
	public static int numFaces = 4;
	public float hope = 0;
	public float chaos = 0;
	public int numStartingPopulation = 10;

	public RoomController[] rooms;
	float elapsed;
	PersonController[] population;
	RoomController focusedRoom;
	CameraController cameraController;
	UIController uiController;

	public AudioSource[] musics;

	// Use this for initialization
	void Start () {
		uiController = GetComponent<UIController>();
		cameraController = FindObjectOfType<CameraController>();
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

		foreach (AudioSource music in musics ) {
			music.Play();
		}

		FocusRoom(0, 0, numRoomsPerFloor / 2);

		ResourceCalculator.stockpile = new ResourceCalculator.Income(0,0);
		UpdateWorkerText();
	}
	
	RoomController GetRoomByFloorFacePosition(int floor, int face, int position) {
		// Debug.Log(floor * (numFaces * numRoomsPerFloor) + face * numRoomsPerFloor + position);
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
		// Debug.Log("Workes: " + room.WorkerCount);
		cameraController.FocusCameraOnPoint(room.gameObject.transform, room.face);
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
			if (Input.GetKeyDown("r")) {
				this.focusedRoom.Clear();
			}
			if (Input.GetKeyDown("l")) { //water
				this.focusedRoom.Build(RoomType.Filtration);
			}
			if (Input.GetKeyDown("v")) {
				this.focusedRoom.Build(RoomType.Converter);
			}
			if (Input.GetKeyDown("h")) {
				this.focusedRoom.Build(RoomType.Hospital);
			}
		}

		if (focusedRoom != null) {
			/* face, position
						2,1 | 2,0
				1,0						3,1
				1,1						3,0
						0,0 | 0,1
			*/
			int currentPosition = focusedRoom.position;
			int currentFace = focusedRoom.face;
			int currentFloor = focusedRoom.floor;
			int hAxis = AxisInput.GetAxisDown("Horizontal");
			int vAxis = AxisInput.GetAxisDown("Vertical");
			
			if (hAxis != 0) {
				currentPosition += hAxis;
				if (currentPosition < 0) {
					currentPosition = numRoomsPerFloor - 1;
					currentFace = (currentFace + 1) % numFaces;
				} else if (currentPosition >= numRoomsPerFloor) {
					currentPosition = 0;
					currentFace = (currentFace + numFaces - 1) % numFaces;
				}
			}
			if (vAxis != 0) {
				currentFloor = (int) Mathf.Clamp(currentFloor + vAxis, 0, numFloors - 1);
			}
			if (hAxis != 0 || vAxis != 0) {
				FocusRoom(currentFloor, currentFace, currentPosition);
			}
		}

		if (Mathf.Floor(elapsed + Time.deltaTime) > Mathf.Floor(elapsed)) {
			ResourceCalculator.Income income = ResourceCalculator.CalculateRoomIncome(rooms);
			ResourceCalculator.Income expenses = ResourceCalculator.CalculatePeopleIncome(population);
			income += expenses;
			ResourceCalculator.stockpile += income;
			UpdateResourceText(income);

			MusicController.UpdateMusics(this.musics, income, ResourceCalculator.stockpile);
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
		//MusicController.FadeMusics(this.musics);
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
		uiController.UpdateWorkerText(population.Length);
		// workerText.text = IdleWorkerCount.ToString() + " / " + population.Length.ToString();
	}

	void UpdateResourceText(ResourceCalculator.Income income) {
		uiController.UpdateFoodText(income.food);
		uiController.UpdatePowerText(income.energy);
		// resourceText.text = "Food Income: " + income.food + " Energy Income: " + income.energy;
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
