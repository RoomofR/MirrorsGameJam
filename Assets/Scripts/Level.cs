using UnityEngine;

public class Level : MonoBehaviour {
	public int levelID;
	public Transform board;
	public Key[] Keys;
	public Transform[] Doors;
	public pos2D Dimensions;
	public pos2D StartPos;
	public pos2D EndPos;
	public bool CameraRotate;
}