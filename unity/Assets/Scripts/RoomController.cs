using UnityEngine;

public class RoomController : MonoBehaviour {
	const int maxRoomOccupancy = 4;

	public TowerController towerController;
	public int floor;
	public int face;
	public int position;

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

	void OnMouseDown() {
		//gameObject.SetActive(false);
		Debug.Log(this.floor + ", " + this.face + ", " + this.position);
		this.towerController.Log();
		this.FocusRoom();
	}

	void FocusRoom(){
    	Renderer[] children = transform.GetComponentsInChildren<Renderer>();
    	foreach ( Renderer  rend  in children) {
            rend.material.color = new Color (1,0,0,0);
		}
	}

	void UnFocusRoom(){
    	Renderer[] children = transform.GetComponentsInChildren<Renderer>();

    	foreach ( Renderer  rend  in children) {
            rend.material.color = new Color (1,1,1,1);
		}
	}

	void Update() {
		if(Random.Range(0,100) == 0) {
			this.UnFocusRoom();
		}
	}
}
