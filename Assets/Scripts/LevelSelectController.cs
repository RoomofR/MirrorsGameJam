using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class LevelSelectController : MonoBehaviour {
	
	public int level;
	public Transform PlayerSelector;
	public Transform LevelBarrier;
	public Transform[] levelSpots;
	[Header("Shake Options")]
	public float shakeTime;
	public float shakeStrength;

	private Vector3 targetPosition;
	private int solvedLevel = 0;
	void Start(){
		targetPosition=levelSpots[0].position;
		//Get Levels Solved From PlayerPrefs
		solvedLevel=PlayerPrefs.GetInt("solvedLevel");
		//Set Level Barrier Location
		if(levelSpots.Length > solvedLevel+1){LevelBarrier.position = new Vector3(levelSpots[solvedLevel+1].position.x+1,LevelBarrier.position.y,LevelBarrier.position.z);
		}else {LevelBarrier.Translate(0,100,0);}
	}	

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
                if(levelSpots.Length-1>level && level-1<solvedLevel){
                	level++;
                	targetPosition=levelSpots[level].position;
                }else{
                	shake();
                }
            }

            //Select
            if((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))){
            	if(level!=0){StartCoroutine("levelSelect");}
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

	IEnumerator levelSelect(){
		Debug.Log(level);
		PlayerPrefs.SetInt("currentLevelID", level);
		float fadeSpeed = GetComponent<FadingEffect>().BeginFade(1);
		yield return new WaitForSeconds(fadeSpeed);
		SceneManager.LoadScene("Player", LoadSceneMode.Single);
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
