using UnityEngine;

public static class TowerSetup {
	public static GameObject[] CreateTower(int numFloors, int numRoomsPerFloor, GameObject template, GameObject parent) {
		// Note that numRoomsPerFloor is the number of rooms per floor *per side*
		// of the tower, so the total number of rooms is multiplied by 4
		GameObject[] rooms = new GameObject[4 * numFloors * numRoomsPerFloor];

		BoxCollider bc = template.GetComponent<BoxCollider>();
		if (bc == null) {
			Debug.Log("unable to get MeshRenderer from template GameObject");
			return rooms;
		}

		float width = bc.size.x;
		float height = bc.size.y;
		float depth = bc.size.z / 2;

		float horizontalArea = numRoomsPerFloor * width;
		float verticalArea = numFloors * height;
		float depthOffset = numRoomsPerFloor * width / 2;

		Debug.Log("" + width + " x " + height + " x " + depth);
		Debug.Log("" + horizontalArea + " x " + verticalArea + " x " + depthOffset);

		for (int y = 0; y < numFloors; y++) {
			for (int r = 0; r < 4; r++) {
				Quaternion rotation = Quaternion.Euler(0f, r * 90, 0f);

				for (int x = 0; x < numRoomsPerFloor; x++) {
					// Subtract depth offset to keep floor centered horizontally
					float offX = (numRoomsPerFloor - 1) * width / 2;
					float posX = ((float) x / numRoomsPerFloor) * horizontalArea - offX;
					float posY = ((float) y / numFloors) * verticalArea;

					Vector3 position = new Vector3(
						posX,
						posY,
						-depthOffset - depth
					);

					position = rotation * position;
					
					Debug.Log("Spawning room (" + posX + "," + posY + ")");

					GameObject.Instantiate(
						template,
						position,
						rotation,
						parent.transform
					);
				}
			}
		}

		return rooms;
	}
}
