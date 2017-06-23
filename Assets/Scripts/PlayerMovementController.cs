using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct pos2D{
	public pos2D(int a,int b) { x = a; y = b;}public int x;public int y;
	public static bool operator ==(pos2D c1, pos2D c2) {return c1.Equals(c2);}
	public static bool operator !=(pos2D c1, pos2D c2) {return !c1.Equals(c2);}
}

[System.Serializable]
public struct Key{
    public pos2D pos;
    public Colors color;
    public Key(Colors c, pos2D p){color=c;pos=p;}
}

public enum Colors {Red, Blue, Green, Orange, Yellow}
    // Used to send requests to the Animator to play animations
public enum EAnimations {Idle, Jump, TurnLeft, TurnRight }
    // Used to handle turning and face direction of the character
public enum EFaceDirection { North, East, South, West }

public class PlayerMovementController : MonoBehaviour {

	[Header("Transform Objects")]
	public Transform FloorSpots;
	public Transform Player;
	public pos2D playerPos;
	public pos2D EndPos;
    public Key[] keysPos;
    public GameObject[] doorsPos;

	[Header("Shake Options")]
	public float shakeTime;
	public float shakeStrength;

	private Transform[,] floorSpots;
	private bool sideShake;

	//CONSTANTS
	private float playerYOffset=0.02f;
	private float raycasYOffset=0.1f;

	[HideInInspector]
    public pos2D[] orientation = new pos2D[4] { new pos2D(0, -1), new pos2D(1, 0), new pos2D(0, 1), new pos2D(-1, 0) };


    [Header("Animation Options")]
    public AnimationCurve jumpCurve;
    public AnimationCurve turnCurve;

    private Animator characterAnimator;

    private float playerYOffsetJump = 0.0f;

    public EFaceDirection currentFaceDir;
    private bool bIsTurningLeft = false;
    private float playerRotYTurn = 0.0f;

    private bool bInputDisabled = false;

    public Vector3 SetBoard(Transform board, pos2D startPos, pos2D endPos){
    	FloorSpots=board;
    	floorSpots=mapFloor();
    	playerPos=startPos;
    	setPlayerPos(playerPos.x,playerPos.y);
    	EndPos=endPos;
    	return getTilePos(endPos.x,endPos.y,0.164f);
    }

    //Set Keys/Doors
    public Vector3[] SetKeysDoor(Key[] k, GameObject[] d){
        keysPos=k;
        doorsPos=d;
        Vector3[] keyPositions = new Vector3[k.Length];int i=0;
        foreach(Key key in k){
            keyPositions[i] = getTilePos(key.pos.x,key.pos.y,0.164f);
            i++;
        }
        return keyPositions;
    }

    void Awake () {
		//floorSpots=mapFloor();
		//setPlayer(playerPos.x,playerPos.y);
	}

    void Start() {
        characterAnimator = Player.GetChild(0).GetComponent<Animator>();

        Player.eulerAngles = new Vector3(0, 0, 0); // TEMP - could initial start rot be decided by the level?
        currentFaceDir = EFaceDirection.East; // TEMP
    }

	void Update(){

        PlayerInput();

        if (shakeTimer>=0){
			Vector2 shakePos = Random.insideUnitCircle * shakeStrength;
			Vector3 shakePlayerPos = Player.position;
			if(!sideShake)shakePlayerPos.z+=shakePos.y;
			else shakePlayerPos.x+=shakePos.x;
			Player.position = shakePlayerPos;
			shakeTimer-=Time.deltaTime;
		}
	}

    private void PlayerInput() {
        if (!bInputDisabled) {
            
            #region MoveNorth
            if (Input.GetKey(KeyCode.W)) {
                switch (currentFaceDir) {
                    case EFaceDirection.North:
                        movePlayer(orientation[(int)EFaceDirection.North]);
                        break;
                    case EFaceDirection.East:
                        bIsTurningLeft = true;
                        StartCoroutine(HandleTurnRotation());
                        currentFaceDir = EFaceDirection.North;
                        break;
                    case EFaceDirection.South:
                        bIsTurningLeft = false;
                        StartCoroutine(HandleTurnRotation());
                        currentFaceDir = EFaceDirection.West;
                        break;
                    case EFaceDirection.West:
                        bIsTurningLeft = false;
                        StartCoroutine(HandleTurnRotation());
                        currentFaceDir = EFaceDirection.North;
                        break;
                }
            }
            #endregion

            #region MoveEast
            if (Input.GetKey(KeyCode.D)) {
                switch (currentFaceDir) {
                    case EFaceDirection.North:
                        bIsTurningLeft = false;
                        StartCoroutine(HandleTurnRotation());
                        currentFaceDir = EFaceDirection.East;
                        break;
                    case EFaceDirection.East:
                        movePlayer(orientation[(int)EFaceDirection.East]);
                        break;
                    case EFaceDirection.South:
                        bIsTurningLeft = true;
                        StartCoroutine(HandleTurnRotation());
                        currentFaceDir = EFaceDirection.East;
                        break;
                    case EFaceDirection.West:
                        bIsTurningLeft = true;
                        StartCoroutine(HandleTurnRotation());
                        currentFaceDir = EFaceDirection.South;
                        break;
                }
            }
            #endregion

            #region MoveSouth
            if (Input.GetKey(KeyCode.S)) {
                switch (currentFaceDir) {
                    case EFaceDirection.North:
                        bIsTurningLeft = true;
                        StartCoroutine(HandleTurnRotation());
                        currentFaceDir = EFaceDirection.West;
                        break;
                    case EFaceDirection.East:
                        bIsTurningLeft = false;
                        StartCoroutine(HandleTurnRotation());
                        currentFaceDir = EFaceDirection.South;
                        break;
                    case EFaceDirection.South:
                        movePlayer(orientation[(int)EFaceDirection.South]);
                        break;
                    case EFaceDirection.West:
                        bIsTurningLeft = true;
                        StartCoroutine(HandleTurnRotation());
                        currentFaceDir = EFaceDirection.South;
                        break;
                }
            }
            #endregion

            #region MoveWest
            if (Input.GetKey(KeyCode.A)) {
                switch (currentFaceDir) {
                    case EFaceDirection.North:
                        bIsTurningLeft = true;
                        StartCoroutine(HandleTurnRotation());
                        currentFaceDir = EFaceDirection.West;
                        break;
                    case EFaceDirection.East:
                        bIsTurningLeft = true;
                        StartCoroutine(HandleTurnRotation());
                        currentFaceDir = EFaceDirection.North;
                        break;
                    case EFaceDirection.South:
                        bIsTurningLeft = false;
                        StartCoroutine(HandleTurnRotation());
                        currentFaceDir = EFaceDirection.West;
                        break;
                    case EFaceDirection.West:
                        movePlayer(orientation[(int)EFaceDirection.West]);
                        break;
                }
            }
            #endregion

            /*
            if (Input.GetKeyDown(KeyCode.A)) {
                movePlayer(orientation[(int)EFaceDirection.North]);
            }
            if (Input.GetKeyDown(KeyCode.D)) {
                movePlayer(orientation[(int)EFaceDirection.East]);
            }
            if (Input.GetKeyDown(KeyCode.W)) {
                movePlayer(orientation[(int)EFaceDirection.South]);
            }
            if (Input.GetKeyDown(KeyCode.S)) {
                movePlayer(orientation[(int)EFaceDirection.West]);
            }
            */

        }
    }

	//Shake
	Vector2 velocity;
	[Header("Smoothing Player Options")]
	public float smoothTimeX;
	public float smoothTimeZ;

	void FixedUpdate(){
		//Player
		if(floorSpots!=null){
			Vector3 anchorPos = getTilePos(playerPos.x,playerPos.y,playerYOffset);
			float posX = Mathf.SmoothDamp(Player.position.x, anchorPos.x, ref velocity.x, smoothTimeX);
			float posZ = Mathf.SmoothDamp(Player.position.z, anchorPos.z, ref velocity.y, smoothTimeZ);
			Player.position = new Vector3(posX,playerYOffset + playerYOffsetJump, posZ);
	        
	        Player.eulerAngles = new Vector3(0, playerRotYTurn, 0); 
		}
	}

	//Set Player Location
	public void setPlayerPos(int x, int y){
		if(floorSpots.GetLength(0)>y && y>=0 && floorSpots.GetLength(1)>x && x>=0){
			playerPos.x=x;
			playerPos.y=y;
		}
	}

	//Moves...
	private void movePlayer(pos2D move){
		pos2D tempPos = playerPos;
		tempPos.x+=move.x;
		tempPos.y+=move.y;

        //Test if Valid Spot in Array
        if (floorSpots.GetLength(0) > tempPos.y && tempPos.y >= 0 &&
            floorSpots.GetLength(1) > tempPos.x && tempPos.x >= 0
            && getTilePos(tempPos.x, tempPos.y, raycasYOffset) != Vector3.zero) {
            //Test if blockade found
            Vector3 orig = new Vector3(Player.position.x, raycasYOffset, Player.position.z);
            Vector3 directionRay = getTilePos(tempPos.x, tempPos.y, raycasYOffset) - orig;
            RaycastHit hit;
            if (Physics.Raycast(orig, directionRay, out hit, 0.5f) &&
                hit.collider.GetComponent<Blockade>() != null) {
                shake(move.x == 0);
            }
            else { playerPos = tempPos; }
            PlayAnimation(EAnimations.Jump);
            StartCoroutine(HandleJumpHeight());
        }
        else shake(move.x==0);

        //Check if Player Hits EndPortal
        if(playerPos==EndPos){
        	Debug.Log("Level Complete!!!");
        	GetComponent<LevelManager>().loadNextLevel();
        }
        //Check if Player Hits Key
        if(keysPos.Length>0){
            foreach(Key key in keysPos){
                if(playerPos==key.pos){
                    foreach(GameObject door in doorsPos){
                        Door d = door.GetComponent<Door>();
                        if(d.color==key.color){
                            d.trigger();
                        }
                    }
                }
            }
        }
	}

	float shakeTimer;
	//Shakes things up :D
	private void shake(bool side){
		sideShake=side;
		shakeTimer=shakeTime;
	}

	//Create Floor Map Based on FloorSpots Object
	public Transform[,] mapFloor(){
		//Rows
		Transform[] rows = new Transform[FloorSpots.childCount];
		int rx=0;foreach(Transform row in FloorSpots){rows[rx]=row;rx++;}

		//Init Output Multi-Array
		pos2D maxMap = new pos2D(0,0);
		foreach(Transform row in rows){if(row.childCount>maxMap.y)maxMap.y=row.childCount;}
		for(int i=0;i<maxMap.y;i++){int m=0;foreach(Transform row in rows)
		{if(i<row.childCount)m++;}if(m>maxMap.x)maxMap.x=m;}Transform[,] map = new Transform[maxMap.x,maxMap.y];

		//Populate Multi-Array by Mapping
		int r=0;
		//float rr=0f;float pp=0f;
		foreach(Transform row in rows){
			int s=0;
			//row.position=new Vector3(rr,0,0);rr+=0.7f;
			foreach(Transform plate in row){
				if(plate.gameObject.activeInHierarchy){
						//plate.position=new Vector3(plate.position.x,plate.position.y,pp);pp+=0.7f;
						map[r,s]=plate;
						//plate.Translate(Vector3.up * Random.Range(0.2f,1.5f), Space.World); 
					}
			s++;}//pp=0f;	
		r++;}

		//Returns Map
		Debug.Log(map.GetLength(0)+"/"+map.GetLength(1));
		return map;
	}

	//Gets tile pos with y offset from floorspots
	private Vector3 getTilePos(int x,int y,float yOffset){
		if(floorSpots[y,x]!=null){
			Vector3 pos = floorSpots[y,x].position;
			return new Vector3(pos.x,yOffset,pos.z);
		}
		else return Vector3.zero;
	}

    IEnumerator HandleJumpHeight() {
        bInputDisabled = true;
        //Vector3 init_position = this.transform.position;
        float initYPos = playerYOffset;

        float running_time = 0;
        float end_time = jumpCurve.keys[jumpCurve.length - 1].time;
        while (running_time < end_time) {
            running_time += Time.deltaTime;
            //float yPos = Player.position.y;
            //yPos = init_yPos + jumpCurve.Evaluate(running_time);
            //this.transform.position = position;
            playerYOffsetJump = initYPos + jumpCurve.Evaluate(running_time);

            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(0.1f);
        bInputDisabled = false;
    }

    IEnumerator HandleTurnRotation() {
        bInputDisabled = true;
        //Vector3 init_position = this.transform.position;
        float initYRot = Player.eulerAngles.y;

        float running_time = 0;
        float end_time = jumpCurve.keys[jumpCurve.length - 1].time;
        while (running_time < end_time) {
            running_time += Time.deltaTime;

            if (bIsTurningLeft) playerRotYTurn = initYRot + turnCurve.Evaluate(running_time) * -1;

            else playerRotYTurn = initYRot + turnCurve.Evaluate(running_time);
            
                yield return new WaitForEndOfFrame();
        }
        bInputDisabled = false;
    }


    // Sends a request to the animator to play an animation
    public void PlayAnimation(EAnimations animation) {
        if (characterAnimator != null) {
            switch (animation) {
                case EAnimations.Idle:
                    //necesary? Could force set to idle from "AnyState" ?
                    break;
                case EAnimations.Jump:
                    characterAnimator.SetTrigger("tJump");
                    break;
                case EAnimations.TurnLeft:
                    characterAnimator.SetTrigger("tTurnLeft");
                    break;
                case EAnimations.TurnRight:
                    characterAnimator.SetTrigger("tTurnRight");
                    break;
                default:
                    break;
            }
        }
        else Debug.LogWarning("Character Animator not found!");     
    }

}
