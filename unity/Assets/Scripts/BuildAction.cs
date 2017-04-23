using UnityEngine;

public class BuildAction : MonoBehaviour {
	public RoomType type;

	RoomController room;

	void Start() {
		room = GetComponentInParent<RoomController>();
	}

	void OnMouseDown() {
		room.Build(type);
	}
}
