using UnityEngine;
using System.Collections;

public class Wiggler : MonoBehaviour {
	float lifetime;
	float frequency;
	Vector3 minVector;
	Vector3 maxVector;
	Vector3 defaultScale;

	void Start() {
		enabled = false;
	}

	public void Enable(Vector3 wiggle, float _lifetime=.2f, float _frequency=.05f) {
		if (enabled) {
				return;
		}
		defaultScale = transform.localScale;
		Vector3 scaledWiggle = Vector3.Scale(defaultScale, wiggle);
		lifetime = _lifetime;
		frequency = _frequency;
		minVector = defaultScale - scaledWiggle;
		maxVector = defaultScale + scaledWiggle;
		enabled = true;
		StartCoroutine(Countdown());
	}

	IEnumerator Countdown() {
		yield return new WaitForSeconds(lifetime);
		transform.localScale = defaultScale;
		enabled = false;
	}

	void Update() {
			float lerp = Mathf.PingPong(Time.time, frequency) / frequency;        
			transform.localScale = Vector3.Lerp(minVector, maxVector, lerp);
	}
}
