using UnityEngine;

public class CameraController : MonoBehaviour {
	const float rotateEasing = 0.25f;
	public float speed;
	
	int prevRotateBtnDown = 0;

	float targetRotate = 0;
	float currentRotate = 0;

	// Update is called once per frame
	void Update () {
		Vector3 movement = new Vector3(
			Input.GetAxis("Horizontal"),
			Input.GetAxis("Vertical"),
			Input.GetAxis("Zoom")
		);
		transform.Translate(movement * speed * Time.deltaTime);

		int currentRotateBtnDown = (int) Input.GetAxisRaw("Rotate");			
		if (prevRotateBtnDown == 0) {
			if (currentRotateBtnDown > 0) {
				targetRotate -= 90f;
				// transform.RotateAround(Vector3.zero, Vector3.up, -90f);
			} else if (currentRotateBtnDown < 0) {
				targetRotate += 90f;
				// transform.RotateAround(Vector3.zero, Vector3.up, 90f);
			}
		}
		prevRotateBtnDown = currentRotateBtnDown;

		if (targetRotate != currentRotate) {
			float deltaRotate = targetRotate - currentRotate;
			if (Mathf.Abs(deltaRotate) < 0.01f) {
				transform.RotateAround(Vector3.zero, Vector3.up, deltaRotate);
				currentRotate = targetRotate;
			} else {
				deltaRotate = deltaRotate * rotateEasing;
				transform.RotateAround(Vector3.zero, Vector3.up, deltaRotate);
				currentRotate += deltaRotate;
			}
		}
	}

}
