using UnityEngine;

public static class TowerSetup {
	public static RoomController[] CreateTower(int numFloors, int numRoomsPerFloor, GameObject template, GameObject parent, TowerController towerController)
    {
        // Note that numRoomsPerFloor is the number of rooms per floor *per side*
        // of the tower, so the total number of rooms is multiplied by 4
        RoomController[] rooms = new RoomController[4 * numFloors * numRoomsPerFloor];

        BoxCollider bc = template.GetComponent<BoxCollider>();
        if (bc == null)
        {
            Debug.Log("unable to get MeshRenderer from template GameObject");
            return rooms;
        }

        float width = bc.size.x;
        float height = bc.size.y;
        float depth = bc.size.z / 2;

        float horizontalArea = numRoomsPerFloor * width;
        float verticalArea = numFloors * height;
        float depthOffset = numRoomsPerFloor * width / 2;
        float offX = (numRoomsPerFloor - 1) * width / 2;
        float offY = (numFloors - 1) * height / 2;

        Debug.Log("" + width + " x " + height + " x " + depth);
        // Debug.Log("" + horizontalArea + " x " + verticalArea + " x " + depthOffset);

        int i = 0;
        for (int y = 0; y < numFloors; y++)
        {
            for (int r = 0; r < 4; r++)
            {
                Quaternion rotation = Quaternion.Euler(0f, r * 90, 0f);

                for (int x = 0; x < numRoomsPerFloor; x++)
                {
                    // Subtract depth offset to keep floor centered horizontally
                    float posX = ((float)x / numRoomsPerFloor) * horizontalArea - offX;
                    float posY = ((float)y / numFloors) * verticalArea;

                    Vector3 position = new Vector3(
                        posX,
                        posY,
                        -depthOffset - depth
                    );

                    position = rotation * position;

                    // Debug.Log("Spawning room (" + posX + "," + posY + ")");

                    GameObject room = GameObject.Instantiate(
                        template,
                        position,
                        rotation,
                        parent.transform
                    );
                    RoomController roomController = room.GetComponent<RoomController>();
                    room.AddComponent<MeshRenderer>();
                    roomController.towerController = towerController;
                    roomController.floor = y;
                    roomController.face = r;
                    roomController.position = x;


                    int typeRoll = Random.Range(0, 100);
                    if (typeRoll < 50)
                    {
                        roomController.type = RoomType.Empty;
                    }
                    else if (typeRoll < 55)
                    {
                        roomController.type = RoomType.Farm;
                    }
                    else if (typeRoll < 60)
                    {
                        roomController.type = RoomType.Power;
                    }
                    else if (typeRoll < 65)
                    {
                        roomController.type = RoomType.Filtration;
                    }
                    else if (typeRoll < 70)
                    {
                        roomController.type = RoomType.Converter;
                    }
                    else
                    {
                        roomController.type = RoomType.Rubble;
                    }

                    rooms[i] = roomController;
                    i++;
                }
            }
        }

        DrawPillerAt(parent, depth, verticalArea, depthOffset,
					-horizontalArea / 2 - depth / 2,
					offY,
					-depthOffset - depth / 2
				);
				DrawPillerAt(parent, depth, verticalArea, depthOffset,
					horizontalArea / 2 + depth / 2,
					offY,
					-depthOffset - depth / 2
				);
				DrawPillerAt(parent, depth, verticalArea, depthOffset,
					-horizontalArea / 2 - depth / 2,
					offY,
					depthOffset + depth / 2
				);
				DrawPillerAt(parent, depth, verticalArea, depthOffset,
					horizontalArea / 2 + depth / 2,
					offY,
					depthOffset + depth / 2
				);

				GameObject towerBase = GetBoringCube(parent);
				towerBase.transform.localPosition = new Vector3(
					0f,
					(-height / 2) - 2f,
					0f
				);
				towerBase.transform.localScale = new Vector3(
					horizontalArea + (depth * 5),
					4f,
					horizontalArea + (depth * 5)
				);

				GameObject towerCap = GetBoringCube(parent);
				towerCap.transform.localPosition = new Vector3(
					0f,
					verticalArea - (height / 2) + 1f,
					0f
				);
				towerCap.transform.localScale = new Vector3(
					horizontalArea + (depth * 3),
					2f,
					horizontalArea + (depth * 3)
				);

        return rooms;
    }

    static void DrawPillerAt(GameObject parent, float depth, float verticalArea, float depthOffset,
														 float offX, float offY, float offZ) {
			// Draw extra geometry
			GameObject pillar = GetBoringCube(parent);
			pillar.transform.parent = parent.transform;
			pillar.transform.localPosition = new Vector3(
					offX,
					offY,
					offZ
			);
			pillar.transform.localScale = new Vector3(
					depth,
					verticalArea,
					depth
			);
    }

		static GameObject GetBoringCube(GameObject parent) {
			GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
			cube.transform.parent = parent.transform;
			cube.GetComponent<BoxCollider>().enabled = false;
			cube.GetComponent<Renderer>().material.color = new Color(.3f,.3f,.3f,0f);
			return cube;
		}
}
