using UnityEngine;

public class CameraMovementController : MonoBehaviour {

	public Transform cameraAnchor;

	void Update(){
		if(Input.GetKeyDown(KeyCode.Q)){
			cameraAnchor.Rotate(0,90,0);
		}
		if(Input.GetKeyDown(KeyCode.E)){
			cameraAnchor.Rotate(0,-90,0);
		}
	}
}