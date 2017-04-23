using UnityEngine;

public class CameraController : MonoBehaviour {
	const float lerpDuration = 0.2f;
	const float rotateEasing = 0.25f;
	public float speed;
	
	int prevRotateBtnDown = 0;

	float targetRotate = 0;
	float currentRotate = 0;

	bool lerping = false;
	float lerpStartTime = 0f;
	float lerpUntilTime = -1f;
	Vector3 lerpStartPosition;
	Vector3 lerpTargetPosition;
	Quaternion lerpStartRotation;
	Quaternion lerpTargetRotation;

	// Update is called once per frame
	void Update () {
		if (lerping) {
			if (lerpUntilTime >= Time.time) {
				float deltaTime = (Time.time - lerpStartTime) / lerpDuration;
				transform.position = Vector3.Lerp(lerpStartPosition, lerpTargetPosition, deltaTime);
				transform.rotation = Quaternion.Lerp(lerpStartRotation, lerpTargetRotation, deltaTime);
			} else {
				lerping = false;
				transform.position = lerpTargetPosition;
				transform.rotation = lerpTargetRotation;
			}
		}

		Vector3 movement = new Vector3(
			0f,
			//Input.GetAxis("Horizontal"),
			0f,
			//Input.GetAxis("Vertical"),
			Input.GetAxis("Zoom")
		);
		
		transform.position += transform.rotation * movement * speed * Time.deltaTime;
		// transform.Translate(movement * speed * Time.deltaTime);

		int currentRotateBtnDown = (int) Input.GetAxisRaw("Rotate");			
		if (prevRotateBtnDown == 0) {
			if (currentRotateBtnDown > 0) {
				targetRotate -= 90f;
			} else if (currentRotateBtnDown < 0) {
				targetRotate += 90f;
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

	public void CancelTransition() {
		if (!lerping) { return; }
		lerping = false;
		transform.position = lerpTargetPosition;
		transform.rotation = lerpTargetRotation;
	}

	public void FocusCameraOnPoint(Transform target, int rotate) {
		CancelTransition();
		Vector3 cameraPositionUnrotated = Quaternion.Inverse(transform.rotation) * transform.position;
		Vector3 targetPositionUnrotated = Quaternion.Inverse(target.rotation) * target.position;

		float deltaZ = cameraPositionUnrotated.z - targetPositionUnrotated.z;
		Vector3 newPositionUnrotated = new Vector3(
			targetPositionUnrotated.x,
			targetPositionUnrotated.y,
			targetPositionUnrotated.z + deltaZ
		);

		lerping = true;
		lerpStartTime = Time.time;
		lerpUntilTime = Time.time + lerpDuration;
		lerpStartPosition = transform.position;
		lerpTargetPosition = target.rotation * newPositionUnrotated;
		lerpStartRotation = transform.rotation;
		lerpTargetRotation = target.rotation;
	}

}
