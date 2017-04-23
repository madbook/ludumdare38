using UnityEngine.UI;
using UnityEngine;

public class UIController : MonoBehaviour {
	public Text workerText;
	public Text powerText;
	public Text foodText;
	
	public void UpdateWorkerText (int numWorkers) {
		workerText.text = numWorkers.ToString();
	}

	public void UpdatePowerText (float power) {
		powerText.text = GetPowerString(power);
	}

	public void UpdateFoodText (float food) {
		foodText.text = GetFoodString(food);
	}

	public static string GetPowerString (float power) {
		return ((int) (power * 100)).ToString();
	}

	public static string GetFoodString (float food) {
		return ((int) (food * 100)).ToString();
	}
}
