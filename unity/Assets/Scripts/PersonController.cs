using UnityEngine;

public class PersonController : MonoBehaviour {
	private JobAssignment job = JobAssignment.Idle;
	RoomController currentRoom;

	public RoomController CurrentRoom {
		get { return currentRoom; }
	}

	public JobAssignment Job {
		get { return job; }
	}

	public void SetJobAssignment(JobAssignment job) {
		this.job = job;
	}

	public void SetCurrentRoom(RoomController currentRoom) {
		this.currentRoom = currentRoom;
	}

	public void Start () {
		this.job = JobAssignment.Idle;
	}

	public bool Bored {
		get {
			bool bored;
			switch (this.job) {
				case JobAssignment.Idle:
					bored = Random.Range(0,100) == 0;
					break;
				case JobAssignment.GoingToRoom:
					bored = false;
					break;
				case JobAssignment.OperatingRoom:
					bored = Random.Range(0,1000) == 0;
					break;
				case JobAssignment.BuildingRoom:
					bored = Random.Range(0,10000) == 0;
					break;
				default:
					bored = false; // people are never bored when they get to do impossible things
					break;
			}
			return bored;
		}
	}
}
