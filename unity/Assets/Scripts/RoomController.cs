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

	public bool focused;

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

	PersonController[] roomOccupents = new PersonController[maxRoomOccupancy];

	public bool AddPersonToRoom(PersonController person) {
		for (int i = 0; i < roomOccupents.Length; i++) {
			if (roomOccupents[i] == null) {
				roomOccupents[i] = person;
				return true;
			}
		}
		return false;
	}

		void Update() {
		Renderer[] children = transform.GetComponentsInChildren<Renderer>();

		string focused;
		if(this.focused){
			focused = "-focused";
		} else {
			focused = "-unfocused";
		}

    	foreach ( Renderer rend in children) {
            rend.material.color = this.roomColors[this.type + focused];
		}
	}


	void OnMouseDown() {
		//gameObject.SetActive(false);
		Debug.Log(this.floor + ", " + this.face + ", " + this.position + ", " + this.type);
		this.towerController.FocusRoom(floor, face, position);
		//this.FocusRoom();
	}

	public void FocusRoom(){
    	Renderer[] children = transform.GetComponentsInChildren<Renderer>();
    	foreach ( Renderer  rend  in children) {
            rend.material.color = new Color (1,0,0,0);
		}
		this.focused = true;
	}

	public void UnFocusRoom(){
    	Renderer[] children = transform.GetComponentsInChildren<Renderer>();

    	foreach ( Renderer  rend  in children) {
            rend.material.color = new Color (1,1,1,1);
		}
		this.focused = false;
	}

	public void BuildPower() {
		if(this.CanBuild) {
			this.type = RoomType.power;
		}
	}

	public void BuildFarm() {
		if(this.CanBuild) {
			this.type = RoomType.farm;
		}
	}
	
	public bool CanBuild {
		get {
			return this.type == RoomType.empty;
		}
	}

	public void Clear() {
		this.type = RoomType.empty;
	}
}