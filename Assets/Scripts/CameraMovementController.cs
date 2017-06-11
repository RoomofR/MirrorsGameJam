using UnityEngine;

public class CameraMovementController : MonoBehaviour {

	public Transform cameraAnchor;
	public float rotationSpeed;

	private Vector3 currentAngle;
	private Vector3 targetAngle = new Vector3(0f, -45f, 0f);
	private PlayerMovementController playerController;

	void Awake(){
		currentAngle = cameraAnchor.eulerAngles;
		playerController = GetComponent<PlayerMovementController>();
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.Q)){
			targetAngle.y+=90;
			//cameraAnchor.Rotate(0,90,0);
			playerController.orientation = new pos2D[4] {playerController.orientation[2],
														 playerController.orientation[3],
														 playerController.orientation[1],
														 playerController.orientation[0]};
		}
		if(Input.GetKeyDown(KeyCode.E)){
			targetAngle.y-=90;
			//cameraAnchor.Rotate(0,-90,0);
			playerController.orientation = new pos2D[4] {playerController.orientation[3],
														 playerController.orientation[2],
														 playerController.orientation[0],
														 playerController.orientation[1]};
		}
		//Lerp Rotation
		currentAngle = new Vector3(0,Mathf.LerpAngle(currentAngle.y, targetAngle.y, Time.deltaTime*rotationSpeed),0);
        cameraAnchor.eulerAngles = currentAngle;
	}
}