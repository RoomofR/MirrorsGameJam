using UnityEngine;

public class LevelSelectController : MonoBehaviour {
	
	public int level;
	public Transform PlayerSelector;
	public Transform[] levelSpots;
	[Header("Shake Options")]
	public float shakeTime;
	public float shakeStrength;

	private Vector3 targetPosition;

	void Start(){targetPosition=levelSpots[0].position;}

	void Update () {
            if (Input.GetKeyDown(KeyCode.A)) { //Move Left
                if(level>0){
                	level--;
                	targetPosition=levelSpots[level].position;
                }else{
                	shake();
                }
            }
            if (Input.GetKeyDown(KeyCode.D)) { //Move Right
                if(levelSpots.Length-1>level){
                	level++;
                	targetPosition=levelSpots[level].position;
                }else{
                	shake();
                }
            }

            //Select
            if((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))){
            	if(level!=0){levelSelect(level);}
            	else {shake();}
            }

            //Shake Stuff
            if (shakeTimer>=0){
				Vector2 shakePos = Random.insideUnitCircle * shakeStrength;
				Vector3 shakePlayerPos = PlayerSelector.position;
				shakePlayerPos.x+=shakePos.x;
				PlayerSelector.position = shakePlayerPos;
				shakeTimer-=Time.deltaTime;
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

	float shakeTimer;
	private void shake(){shakeTimer=shakeTime;}
}
