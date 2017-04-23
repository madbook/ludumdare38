using UnityEngine;

public class PersonController : MonoBehaviour {
	Wiggler wiggler;	
	JobAssignment job = JobAssignment.Idle;
	RoomController currentRoom;

	public int currentPosition;

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
		wiggler.Enable(new Vector3(0f, 0.1f, 0f), 0.1f);
	}

	public void Awake () {
		job = JobAssignment.Idle;
		wiggler = GetComponent<Wiggler>();
	}

	public bool Bored {
		get {
			bool bored;
			int rand = Random.Range(0,1000);

			switch (this.job) {
				case JobAssignment.Idle:
					bored = (rand <= 100);
					break;
				case JobAssignment.GoingToRoom:
					bored = false;
					break;
				case JobAssignment.OperatingRoom:
					if(this.CurrentRoom.WorkerCount > 1) {
						bored = (rand <= 10);
					} else {
						bored = (rand <= 1);
					}
						
					break;
				case JobAssignment.BuildingRoom:
					bored = (rand <= 1);
					break;
				default:
					bored = false; // people are never bored when they get to do impossible things
					break;
			}
			return bored;
		}
	}
}
