using UnityEngine;

public class TowerController : MonoBehaviour {

	public GameObject roomTemplate;
	public int numFloors = 4;
	public int numRoomsPerFloor = 3;

	// Use this for initialization
	void Start () {
		TowerSetup.CreateTower(
			numFloors,
			numRoomsPerFloor,
			roomTemplate,
			gameObject
		);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
