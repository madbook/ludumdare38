using UnityEngine;

public class RoomController : MonoBehaviour {

	public TowerController towerController;

	public int floor;
	public int face;
	public int position;

	void OnMouseDown() {
		//gameObject.SetActive(false);
		Debug.Log(this.floor + ", " + this.face + ", " + this.position);
		this.towerController.FocusRoom(floor, face, position);
		//this.FocusRoom();
	}

	public void FocusRoom(){
    	Renderer[] children = transform.GetComponentsInChildren<Renderer>();
    	foreach ( Renderer  rend  in children) {
            rend.material.color = new Color (1,0,0,0);
		}
	}

	public void UnFocusRoom(){
    	Renderer[] children = transform.GetComponentsInChildren<Renderer>();

    	foreach ( Renderer  rend  in children) {
            rend.material.color = new Color (1,1,1,1);
		}
	}
}
