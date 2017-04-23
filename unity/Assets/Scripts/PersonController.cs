using UnityEngine;

public class PersonController : MonoBehaviour {
	Wiggler wiggler;	
	JobAssignment job = JobAssignment.Idle;
	RoomController currentRoom;

	public int currentPosition;
	private float boredom;

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

		transform.Rotate(0f, Random.Range(0f, 360f), 0f);
		wiggler.Enable(new Vector3(0f, 0.1f, 0f), 0.1f);
		this.boredom = (float)Random.Range(0,1000)/2000;
	}

	public void Awake () {
		job = JobAssignment.Idle;
		wiggler = GetComponent<Wiggler>();
	}

	public bool Bored {
		get {

			int boringness = 0;
			switch (this.job) {
				case JobAssignment.Idle:
					boringness = 100;
					break;
				case JobAssignment.GoingToRoom:
					boringness = 0;
					break;
				case JobAssignment.OperatingRoom:
					if(this.CurrentRoom.WorkerCount > 1) {
						boringness = 10;
					} else {
						boringness = 1;
					}
						
					break;
				case JobAssignment.BuildingRoom:
					boringness = 1;
					break;
				default:
					boringness = 0; // people are never boringness when they get to do impossible things
					break;
			}
			this.boredom += (float)(Random.Range(1,boringness))/4000;
			return this.boredom > 1;
		}
	}
}
