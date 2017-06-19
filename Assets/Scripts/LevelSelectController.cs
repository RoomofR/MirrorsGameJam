using UnityEngine;

public class LevelSelectController : MonoBehaviour {
	
	public int level;
	public Transform PlayerSelector;
	public Transform[] levelSpots;

	private Vector3 targetPosition;

	void Start(){targetPosition=levelSpots[0].position;}

	void Update () {
            if (Input.GetKeyDown(KeyCode.A)) {
                if(level>0){
                	level--;
                	targetPosition=levelSpots[level].position;
                }else{

                }
            }
            if (Input.GetKeyDown(KeyCode.D)) {
                if(levelSpots.Length-1>level){
                	level++;
                	targetPosition=levelSpots[level].position;
                }else{
                	
                }
            }

            if((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) && level!=0){
            	levelSelect(level);
            }
	}

	public void levelSelect(int levelID){
		Debug.Log(levelID);
	}

	Vector2 velocity;
	private float smoothTime=0.1f;
	void FixedUpdate(){
		float posX = Mathf.SmoothDamp(PlayerSelector.position.x, targetPosition.x, ref velocity.x, smoothTime);
		float posZ = Mathf.SmoothDamp(PlayerSelector.position.z, targetPosition.z, ref velocity.y, smoothTime);
		PlayerSelector.position = new Vector3(posX,0.26f, posZ);
	}
}
