using UnityEngine;
using System.Collections.Generic;

public class RoomController : MonoBehaviour {
	const int maxRoomOccupancy = 4;

	public TowerController towerController;
	public int floor;
	public int face;
	public int position;

	public enum RoomType{power, farm, rubble, empty};

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
	
	public float workerEfficiency = .01f;

	public Dictionary<string, Color> roomColors = new Dictionary<string, Color>();

	public void Start() {
		this.roomColors.Add("power-focused",  new Color (1f,1f,.2f,0f));
		this.roomColors.Add("farm-focused",  new Color (.2f,1f,.2f,0f));
		this.roomColors.Add("rubble-focused",  new Color (.2f,.2f,.2f,0f));
		this.roomColors.Add("empty-focused",  new Color (.7f,.7f,.7f,0f));
		this.roomColors.Add("power-unfocused",  new Color (.8f,.8f,0f,0f));
		this.roomColors.Add("farm-unfocused",  new Color (0f,.8f,0f,0f));
		this.roomColors.Add("rubble-unfocused",  new Color (.1f,.1f,.1f,0f));
		this.roomColors.Add("empty-unfocused",  new Color (.5f,.5f,.5f,0f));
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

	public int Count {
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
			Debug.Log(roomOccupents.Length);
			Debug.Log(workerEfficiency);
			
			return (float)roomOccupents.Length * workerEfficiency;
		}
	}

	public bool AddPersonToRoom(PersonController person) {
		if (person == null) return false;

		for (int i = 0; i < roomOccupents.Length; i++) {
			if (roomOccupents[i] == null) {
				roomOccupents[i] = person;

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
			Debug.Log("Amount complete " + this.assignment.progress);
			if(this.assignment.progress > 1) {
				this.type = this.assignment.type;
				// Job's done!
				this.assignment.assigned = false;
			}
		}
	}

	public void Redraw() {
		Renderer[] children = transform.GetComponentsInChildren<Renderer>();

		string focused;
		if(this.focused){
			focused = "-focused";
		} else {
			focused = "-unfocused";
		}

    	foreach ( Renderer rend in children) {
			if (this.roomColors.ContainsKey(this.type + focused)) {
 				rend.material.color = this.roomColors[this.type + focused];
			}
		}
	}

	void OnMouseDown() {
		//gameObject.SetActive(false);
		Debug.Log(this.floor + ", " + this.face + ", " + this.position + ", " + this.type);
		this.towerController.FocusRoom(floor, face, position);
		//this.FocusRoom();
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
			this.assignment = new Assignment(RoomType.power, 0);
		}
	}

	public void BuildFarm() {
		if(this.CanBuild) {
			Debug.Log("building farm");
			this.assignment = new Assignment(RoomType.farm, 0);
		}
	}
	
	public bool CanBuild {
		get {
			return !this.assignment.assigned && this.type == RoomType.empty;
		}
	}

	public void Clear() {
		if (this.assignment.assigned ) {
			// clear assigment?
		} else {
			this.assignment = new Assignment(RoomType.empty, 0);
		}
	}
}