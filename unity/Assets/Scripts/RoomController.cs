using UnityEngine;
using System.Collections.Generic;

public class RoomController : MonoBehaviour {
	public const int maxRoomOccupancy = 4;

	public TowerController towerController;
	public GameObject roomModel;
	public int floor;
	public int face;
	public int position;

	public Dictionary<RoomType, ResourceCalculator.Income> incomeByType = new Dictionary<RoomType, ResourceCalculator.Income>();

	public RoomType type;

	public struct Assignment{
		public RoomType type;
		public float progress;
		public bool assigned;
		public Assignment(RoomType type, float progress) {
			this.type = type;
			this.progress = progress;
			this.assigned = true;
		}
	}

	public Assignment assignment;

	public bool focused;
	
	private float workerEfficiency = .001f;

	public void Start() {
		this.incomeByType.Add(RoomType.Power, new ResourceCalculator.Income(0f, 1f));
		this.incomeByType.Add(RoomType.Farm, new ResourceCalculator.Income(1f, -.25f));
		this.incomeByType.Add(RoomType.Rubble, new ResourceCalculator.Income(0f, 0f));
		this.incomeByType.Add(RoomType.Empty, new ResourceCalculator.Income(0f, -.05f));
		this.incomeByType.Add(RoomType.Filtration, new ResourceCalculator.Income(0.5f, -.05f));
		this.incomeByType.Add(RoomType.Converter, new ResourceCalculator.Income(0f, .5f));
	}


	public float personYOffset;
	public Vector2[] personPositions;

	public bool IsRoomFull {
		get {
			foreach (PersonController person in roomOccupents) {
				if (person == null) { 
					return false;
				}
			}
			return true;
		}
	}

	public int WorkerCount {
		get {
			int count = 0;
			foreach (PersonController person in roomOccupents) {
				if (person != null) {
					count += 1;
				}
			}
			return count;
		}
	}

	PersonController[] roomOccupents = new PersonController[maxRoomOccupancy];

	public System.Collections.IEnumerable IterOccupants {
		get {
			foreach (PersonController person in roomOccupents) {
				if (person != null) {
					yield return person;
				}
			}
		}
	}

	public float GetWorkforce {
		get {
			// TODO: different workers work different speeds.
			
			return (float)this.WorkerCount * workerEfficiency;
		}
	}

	public bool AddPersonToRoom(PersonController person) {
		if (person == null) return false;

		for (int i = 0; i < roomOccupents.Length; i++) {
			if (roomOccupents[i] == null) {
				roomOccupents[i] = person;

				if (person.CurrentRoom) {
					person.CurrentRoom.roomOccupents[person.currentPosition] = null;
				}
				person.currentPosition = i;

				person.gameObject.transform.SetParent(transform, false);

				if (personPositions.Length >= i) {
					/*Debug.Log(person);
					Debug.Log(person.gameObject);
					Debug.Log(person.gameObject.transform);*/
					person.gameObject.transform.localPosition = new Vector3(
						personPositions[i].x,
						personYOffset,
						personPositions[i].y
					);
				}

				return true;
			}
		}
		return false;
	}

	void Update() {
		this.Redraw();
		if (this.assignment.assigned) {
			this.assignment.progress += this.GetWorkforce;
			if(this.assignment.progress > 1) {
				this.type = this.assignment.type;
				// Job's done!
				this.assignment.assigned = false;

				if (this.type == RoomType.Empty) {
					SetAllOccupantsToIdle();
				} else {
					Debug.Log("setting occupants to work because room is done building");
					SetAllOccupantsToWork();
				}
			}
		}
	}

	void SetAllOccupantsToWork() {
		foreach (PersonController person in IterOccupants) {
			if (person.Job != JobAssignment.OperatingRoom) {
				person.SetJobAssignment(JobAssignment.OperatingRoom);
			}
		}
	}

	void SetAllOccupantsToBuild() {
		foreach (PersonController person in IterOccupants) {
			if (person.Job != JobAssignment.BuildingRoom) {
				person.SetJobAssignment(JobAssignment.BuildingRoom);
			}
		}
	}

	void SetAllOccupantsToIdle() {
		foreach (PersonController person in IterOccupants) {
			if (person.Job != JobAssignment.Idle) {
				person.SetJobAssignment(JobAssignment.Idle);
			}
		}
	}

	public void Redraw() {
		ColorKey key = new ColorKey(type, focused);
		RoomRenderer.RedrawRoom(this, key);
	}

	void OnMouseDown() {
		Debug.Log(this.floor + ", " + this.face + ", " + this.position + ", " + this.type);
		this.towerController.FocusRoom(floor, face, position);
		this.Redraw();
	}

	public void FocusRoom(){
		this.focused = true;
		this.Redraw();
	}

	public void UnFocusRoom(){
		this.focused = false;
		this.Redraw();
	}

	public void BuildPower() {
		this.Build(RoomType.Power);
	}

	public void BuildFarm() {
		this.Build(RoomType.Farm);
	}

	public void Build(RoomType type) {
		if(this.CanBuild) {
			Debug.Log("building " + type);
			this.assignment = new Assignment(type, 0);
			SetAllOccupantsToBuild();
		}
	}
	
	public bool CanBuild {
		get {
			return !this.assignment.assigned && this.type == RoomType.Empty;
		}
	}

	public ResourceCalculator.Income Income {
		get {
			//Debug.Log(this.type);
			return this.incomeByType[this.type];
		}
	}

	public void Clear() {
		if (this.assignment.assigned ) {
			// clear assigment?
		} else {
			this.assignment = new Assignment(RoomType.Empty, 0);
			SetAllOccupantsToBuild();
		}
	}
}