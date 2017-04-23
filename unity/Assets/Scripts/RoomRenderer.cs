using UnityEngine;
using System.Collections.Generic;

public class RoomRenderer : MonoBehaviour {
	public GameObject roomModel;
	public GameObject farmScene;

	RoomController room;

	static Dictionary<RoomType, Color> roomColors = new Dictionary<RoomType, Color>();

	static RoomRenderer() {
		roomColors.Add(RoomType.Power, new Color(.8f,.8f,0f,0f));
		// roomColors.Add(RoomType.Farm, new Color(0f,.8f,0f,0f));
		roomColors.Add(RoomType.Rubble, new Color(.1f,.1f,.1f,0f));
		roomColors.Add(RoomType.Empty, new Color(.5f,.5f,.5f,0f));
		roomColors.Add(RoomType.Filtration, new Color(0f,0f,.5f,0f));
		roomColors.Add(RoomType.Converter, new Color(.8f,.2f,0f,0f));
	}

	void Awake() {
		room = GetComponent<RoomController>();
	}

	public void Redraw() {
		RoomType key = room.type;

		Renderer[] children = roomModel.GetComponentsInChildren<Renderer>();

		foreach (Renderer rend in children) {
			if (roomColors.ContainsKey(key)) {
				rend.material.color = roomColors[key];
			}
		}

		if (room.type == RoomType.Farm && !farmScene.activeSelf) {
			farmScene.SetActive(true);
		} else if (room.type != RoomType.Farm && farmScene.activeSelf) {
			farmScene.SetActive(false);
		}
	}

	public static void RedrawRoom(RoomController room, ColorKey key) {
	}
}
