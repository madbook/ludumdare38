using UnityEngine;
using System.Collections.Generic;

public class RoomController : MonoBehaviour {
	public const int maxRoomOccupancy = 4;

	public TowerController towerController;
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

	struct ColorKey {
		public readonly RoomType type;
		public readonly bool isFocused;

		public ColorKey(RoomType type, bool isFocused) {
			this.type = type;
			this.isFocused = isFocused;
		}
	}

	Dictionary<ColorKey, Color> roomColors = new Dictionary<ColorKey, Color>();

	public void Start() {
		this.roomColors.Add(new ColorKey(RoomType.Power, true),  new Color (1f,1f,.2f,0f));
		this.roomColors.Add(new ColorKey(RoomType.Farm, true),  new Color (.2f,1f,.2f,0f));
		this.roomColors.Add(new ColorKey(RoomType.Rubble, true),  new Color (.2f,.2f,.2f,0f));
		this.roomColors.Add(new ColorKey(RoomType.Empty, true),  new Color (.7f,.7f,.7f,0f));
		this.roomColors.Add(new ColorKey(RoomType.Power, false),  new Color (.8f,.8f,0f,0f));
		this.roomColors.Add(new ColorKey(RoomType.Farm, false),  new Color (0f,.8f,0f,0f));
		this.roomColors.Add(new ColorKey(RoomType.Rubble, false),  new Color (.1f,.1f,.1f,0f));
		this.roomColors.Add(new ColorKey(RoomType.Empty, false),  new Color (.5f,.5f,.5f,0f));
		this.roomColors.Add(new ColorKey(RoomType.Filtration, true),  new Color (.1f,.1f,.7f,0f));
		this.roomColors.Add(new ColorKey(RoomType.Filtration, false),  new Color (0f,0f,.5f,0f));
		this.roomColors.Add(new ColorKey(RoomType.Converter, true),  new Color (1f,.5f,0f,0f));
		this.roomColors.Add(new ColorKey(RoomType.Converter, false),  new Color (.8f,.2f,0f,0f));

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

				person.gameObject.transform.SetParent(transform, false);

				if (personPositions.Length >= i) {
					Debug.Log(person);
					Debug.Log(person.gameObject);
					Debug.Log(person.gameObject.transform);
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
					SetAllOccupantsToWork();
				}
			}
		}
	}

	void SetAllOccupantsToWork() {
		foreach (PersonController person in roomOccupents) {
			if (person.Job != JobAssignment.OperatingRoom) {
				person.SetJobAssignment(JobAssignment.OperatingRoom);
			}
		}
	}

	void SetAllOccupantsToIdle() {
		foreach (PersonController person in roomOccupents) {
			if (person.Job != JobAssignment.Idle) {
				person.SetJobAssignment(JobAssignment.Idle);
			}
		}
	}

	public void Redraw() {
		Renderer[] children = transform.GetComponentsInChildren<Renderer>();

		ColorKey key = new ColorKey(type, focused);
    foreach ( Renderer rend in children) {
			if (this.roomColors.ContainsKey(key)) {
 				rend.material.color = this.roomColors[key];
			}
		}
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
		if(this.CanBuild) {
			this.assignment = new Assignment(RoomType.Power, 0);
		}
	}

	public void BuildFarm() {
		if(this.CanBuild) {
			Debug.Log("building farm");
			this.assignment = new Assignment(RoomType.Farm, 0);
		}
	}
	
	public bool CanBuild {
		get {
			return !this.assignment.assigned && this.type == RoomType.Empty;
		}
	}

	public ResourceCalculator.Income Income {
		get {
			return this.incomeByType[this.type];
		}
	}

	public void Clear() {
		if (this.assignment.assigned ) {
			// clear assigment?
		} else {
			this.assignment = new Assignment(RoomType.Empty, 0);
		}
	}
}