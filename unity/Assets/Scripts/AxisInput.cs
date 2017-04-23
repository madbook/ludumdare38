using UnityEngine;
using System.Collections.Generic;

public static class AxisInput {
	static Dictionary<string, bool> wasInputDown = new Dictionary<string, bool>();

	static AxisInput() {
		wasInputDown.Add("Horizontal", false);
		wasInputDown.Add("Vertical", false);
		wasInputDown.Add("Zoom", false);
	}

	public static int GetAxisDown(string name) {
		if (!wasInputDown.ContainsKey(name)) { return 0; }
		int isDown = (int) Input.GetAxisRaw(name);
		bool wasDown = wasInputDown[name];
		wasInputDown[name] = isDown != 0;
		if (isDown != 0 && !wasDown) { return isDown; }
		return 0;
	}
}
