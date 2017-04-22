using UnityEngine;

public class PersonController : MonoBehaviour {
	JobAssignment job = JobAssignment.Idle;
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
}
