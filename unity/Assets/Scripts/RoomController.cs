using UnityEngine;

public class RoomController : MonoBehaviour {

	public TowerController towerController;

	public int floor;
	public int face;
	public int position;

	void OnMouseDown() {
		//gameObject.SetActive(false);
		Debug.Log(this.floor + ", " + this.face + ", " + this.position);
		this.towerController.Log();
	}
}
