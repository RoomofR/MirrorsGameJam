using UnityEngine;

public class CameraMovementController : MonoBehaviour {

	public Transform cameraAnchor;
	public float rotationSpeed;
	public bool rotate;

	private Vector3 currentAngle;
	private Vector3 targetAngle = new Vector3(0f, -45f, 0f);
	private PlayerMovementController playerController;

	void Awake(){
		currentAngle = cameraAnchor.eulerAngles;
		playerController = GetComponent<PlayerMovementController>();
	}

	void Update(){
		if(rotate){
			if(Input.GetKeyDown(KeyCode.Q)){
				targetAngle.y+=90;
				//cameraAnchor.Rotate(0,90,0);
				switch (playerController.currentFaceDir) {
                    case EFaceDirection.North:playerController.currentFaceDir=EFaceDirection.West;break;
                    case EFaceDirection.East:playerController.currentFaceDir=EFaceDirection.North;break;
                    case EFaceDirection.South:playerController.currentFaceDir=EFaceDirection.East;break;
                    case EFaceDirection.West:playerController.currentFaceDir=EFaceDirection.South;break;
                }

				playerController.orientation = new pos2D[4] {playerController.orientation[1],// W 2310
															 playerController.orientation[2],// D
															 playerController.orientation[3],// S
															 playerController.orientation[0]};//A
			}
			if(Input.GetKeyDown(KeyCode.E)){
				targetAngle.y-=90;
				//cameraAnchor.Rotate(0,-90,0);
				switch (playerController.currentFaceDir) {
                    case EFaceDirection.North:playerController.currentFaceDir=EFaceDirection.East;break;
                    case EFaceDirection.East:playerController.currentFaceDir=EFaceDirection.South;break;
                    case EFaceDirection.South:playerController.currentFaceDir=EFaceDirection.West;break;
                    case EFaceDirection.West:playerController.currentFaceDir=EFaceDirection.North;break;
                }
				Debug.Log(playerController.currentFaceDir);
				playerController.orientation = new pos2D[4] {playerController.orientation[3],//3201
															 playerController.orientation[0],
															 playerController.orientation[1],
															 playerController.orientation[2]};
			}
			//Lerp Rotation
			currentAngle = new Vector3(0,Mathf.LerpAngle(currentAngle.y, targetAngle.y, Time.deltaTime*rotationSpeed),0);
	        cameraAnchor.eulerAngles = currentAngle;
		}
	}
}