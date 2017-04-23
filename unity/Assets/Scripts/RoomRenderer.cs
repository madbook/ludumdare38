using UnityEngine;
using System.Collections.Generic;

public static class RoomRenderer {
	public static Dictionary<ColorKey, Color> roomColors = new Dictionary<ColorKey, Color>();

	static RoomRenderer() {
		roomColors.Add(new ColorKey(RoomType.Power, true),  new Color (1f,1f,.2f,0f));
		roomColors.Add(new ColorKey(RoomType.Farm, true),  new Color (.2f,1f,.2f,0f));
		roomColors.Add(new ColorKey(RoomType.Rubble, true),  new Color (.2f,.2f,.2f,0f));
		roomColors.Add(new ColorKey(RoomType.Empty, true),  new Color (.7f,.7f,.7f,0f));
		roomColors.Add(new ColorKey(RoomType.Power, false),  new Color (.8f,.8f,0f,0f));
		roomColors.Add(new ColorKey(RoomType.Farm, false),  new Color (0f,.8f,0f,0f));
		roomColors.Add(new ColorKey(RoomType.Rubble, false),  new Color (.1f,.1f,.1f,0f));
		roomColors.Add(new ColorKey(RoomType.Empty, false),  new Color (.5f,.5f,.5f,0f));
		roomColors.Add(new ColorKey(RoomType.Filtration, true),  new Color (.1f,.1f,.7f,0f));
		roomColors.Add(new ColorKey(RoomType.Filtration, false),  new Color (0f,0f,.5f,0f));
		roomColors.Add(new ColorKey(RoomType.Converter, true),  new Color (1f,.5f,0f,0f));
		roomColors.Add(new ColorKey(RoomType.Converter, false),  new Color (.8f,.2f,0f,0f));
	}

	public static void RedrawRoom(RoomController room, ColorKey key) {
		Renderer[] children = room.roomModel.GetComponentsInChildren<Renderer>();

		foreach (Renderer rend in children) {
			if (roomColors.ContainsKey(key)) {
				rend.material.color = roomColors[key];
			}
		}
	}
}
