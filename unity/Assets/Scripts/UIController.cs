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
		powerText.text = ((int) (power * 100)).ToString();
	}

	public void UpdateFoodText (float food) {
		foodText.text = ((int) (food * 100)).ToString();
	}
}
