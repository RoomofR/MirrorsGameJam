using UnityEngine;

public struct pos2D{
	public pos2D(int a,int b) {
    x = a; y = b;
  	}
	public int x;
	public int y;
}

    // Used to send requests to the Animator to play animations
public enum EAnimations {Idle, Jump, TurnLeft, TurnRight }

public class PlayerMovementController : MonoBehaviour {

	[Header("Transform Objects")]
	public Transform FloorSpots;
	public Transform Player;
	public pos2D playerPos = new pos2D(0,0);

	[Header("Shake Options")]
	public float shakeTime;
	public float shakeStrength;

	private Transform[,] floorSpots;
	private bool sideShake;

	//CONSTANTS
	private float playerYOffset=0.05f;
	private float raycasYOffset=0.1f;

	[HideInInspector]
	public pos2D[] orientation = new pos2D[4] {new pos2D(-1,0),new pos2D(1,0),new pos2D(0,-1),new pos2D(0,1)};

    private Animator characterAnimator;

    // public AnimationCurve jumpCurve; // <===== if I fail to fix the current issue with the Jump animation's Y axis, then we can use this to control height over the duration of the jump

	void Awake () {
		floorSpots=mapFloor();
		setPlayer(playerPos.x,playerPos.y);
	}

    void Start() {
        characterAnimator = Player.GetChild(0).GetComponent<Animator>();
    }

	void Update(){

        // Do we need to ROTATE the character first?
        // Turn Left/Right animation could be played, then, if KeyDown is still true, move teh character and play the jump anim

		//Controls
		if(Input.GetKeyDown(KeyCode.A)){
			movePlayer(orientation[0]);
		}
		if(Input.GetKeyDown(KeyCode.D)){
			movePlayer(orientation[1]);
		}
		if(Input.GetKeyDown(KeyCode.W)){
			movePlayer(orientation[2]);
		}
		if(Input.GetKeyDown(KeyCode.S)){
			movePlayer(orientation[3]);
		}

		if(shakeTimer>=0){
			Vector2 shakePos = Random.insideUnitCircle * shakeStrength;//+shakePos.x
			Vector3 shakePlayerPos = Player.position;
			if(!sideShake)shakePlayerPos.z+=shakePos.y;
			else shakePlayerPos.x+=shakePos.x;
			Player.position = shakePlayerPos;
			shakeTimer-=Time.deltaTime;
		}
	}

	//Shake
	Vector2 velocity;
	[Header("Smoothing Player Options")]
	public float smoothTimeX;
	public float smoothTimeZ;

	void FixedUpdate(){
		//Player
		Vector3 anchorPos = getTilePos(playerPos.x,playerPos.y,playerYOffset);
		float posX = Mathf.SmoothDamp(Player.position.x, anchorPos.x, ref velocity.x, smoothTimeX);
		float posZ = Mathf.SmoothDamp(Player.position.z, anchorPos.z, ref velocity.y, smoothTimeZ);
		Player.position = new Vector3(posX,playerYOffset,posZ);
	}

	//Set Player Location
	private void setPlayer(int x, int y){
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
            if (Physics.Raycast(orig, directionRay, out hit, 1) &&
                hit.collider.GetComponent<Blockade>() != null) {
                shake(move.x == 0);
            }
            else { playerPos = tempPos; }
            PlayAnimation(EAnimations.Jump);
                                                 #region  === NOTE ON JUMP ANIMATION - Delete after reading ===
            /*
            Added new line here calling my new method which uses enums (for convenience) to set triggers to play the anim once.

            If you have a better system, then feel free to implement it! 

            You may also notice that the jump anim appears fast. I increased the speed of the anim for a better response time to the players input.
            I will make a short animation for this next time, but we may also wish to lengthen the period over which the jump takes place too.
            */
            #endregion
        }
        else shake(move.x==0);
	}

	float shakeTimer;
	//Shakes things up :D
	private void shake(bool side){
		sideShake=side;
		shakeTimer=shakeTime;
	}

	//Create Floor Map Based on FloorSpots Object
	private Transform[,] mapFloor(){
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
		foreach(Transform row in rows){
			int s=0;
			foreach(Transform plate in row){
				if(plate.gameObject.activeInHierarchy){
						map[r,s]=plate;
						//plate.Translate(Vector3.up * Random.Range(0.2f,1.5f), Space.World); 
					}
			s++;}
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
                    //characterAnimator.SetTrigger("tTurnLeft");
                    break;
                case EAnimations.TurnRight:
                    // characterAnimator.SetTrigger("tTurnRight");
                    break;
                default:
                    break;
            }
        }
        else Debug.LogWarning("Character ANimator not found!");     
    }

}
