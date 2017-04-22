using UnityEngine;

public class CameraController : MonoBehaviour {
	public float speed;
	
	int prevRotate = 0;

	// Update is called once per frame
	void Update () {
		Vector3 movement = new Vector3(
			Input.GetAxis("Horizontal"),
			Input.GetAxis("Vertical"),
			Input.GetAxis("Zoom")
		);
		transform.Translate(movement * speed * Time.deltaTime);

		int currentRotate = (int) Input.GetAxisRaw("Rotate");			
		if (prevRotate == 0) {
			if (currentRotate > 0) {
				transform.RotateAround(Vector3.zero, Vector3.up, -90f);
			} else if (currentRotate < 0) {
				transform.RotateAround(Vector3.zero, Vector3.up, 90f);
			}
		}
		prevRotate = currentRotate;
	}
}
