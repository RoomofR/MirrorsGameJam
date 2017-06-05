using UnityEngine;

public struct pos2D{
	public int x;
	public int y;
}

public class PlayerMovementController : MonoBehaviour {

	[Header("Transform Objects")]
	public Transform FloorSpots;
	public Transform Player;
	public pos2D playerPos;

	[Header("Shake Options")]
	public float shakeTime;
	public float shakeStrength;

	private Transform[,] floorSpots;
	private bool sideShake;

	//CONSTANTS
	private float playerYOffset=0.25f;
	private float raycasYOffset=0.1f;

	void Awake () {
		floorSpots=mapFloor();
		setPlayer(playerPos.x,playerPos.y);
	}

	void Update(){

		//Controls
		if(Input.GetKeyDown(KeyCode.A)){
			movePlayer(-1,0);
		}
		if(Input.GetKeyDown(KeyCode.D)){
			movePlayer(1,0);
		}
		if(Input.GetKeyDown(KeyCode.W)){
			movePlayer(0,-1);
		}
		if(Input.GetKeyDown(KeyCode.S)){
			movePlayer(0,1);
		}

		if(shakeTimer>=0){
			Vector2 shakePos = Random.insideUnitCircle * shakeStrength;//+shakePos.x
			Vector3 shakePlayerPos = Player.position;
			if(!sideShake)shakePlayerPos.z+=shakePos.y;
			else shakePlayerPos.x+=shakePos.x;
			Player.position = shakePlayerPos;
			//Player.position = new Vector3(Player.position.x,Player.position.y,Player.position.z+shakePos.y);
			shakeTimer-=Time.deltaTime;
		}
	}

	//Shake
	Vector2 velocity;
	public float smoothTimeX;
	public float smoothTimeZ;

	void FixedUpdate(){
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
	private void movePlayer(int x, int y){
		pos2D tempPos = playerPos;
		tempPos.x+=x;
		tempPos.y+=y;
		//Test if Valid Spot in Array
		if(floorSpots.GetLength(0)>tempPos.y && tempPos.y>=0 && 
			floorSpots.GetLength(1)>tempPos.x && tempPos.x>=0){
			//Test if blockade found
			Vector3 orig = new Vector3(Player.position.x,raycasYOffset,Player.position.z);
			Vector3 directionRay = getTilePos(tempPos.x,tempPos.y,raycasYOffset) - orig;		
			RaycastHit hit;
			if(Physics.Raycast(orig, directionRay,out hit,1) &&
				hit.collider.GetComponent<Blockade>()!=null){
				shake(x==0);
			}else playerPos=tempPos;
		}else shake(x==0);
	}

	float shakeTimer;
	//Shakes things up :D
	private void shake(bool side){
		Debug.Log("SHAKE");
		sideShake=side;
		shakeTimer=shakeTime;
	}

	//Create Floor Map Based on FloorSpots Object
	private Transform[,] mapFloor(){
		//Rows
		Transform[] rows = new Transform[FloorSpots.childCount];
		int rx=0;foreach(Transform row in FloorSpots){rows[rx]=row;rx++;}

		//Init Output Multi-Array
		Transform[,] map = new Transform[FloorSpots.childCount,rows[0].childCount];

		//Populate Multi-Array
		int r=0;
		foreach(Transform row in rows){
			int s=0;
			foreach(Transform spot in row){
				map[r,s]=spot;
				spot.Translate(Vector3.up * Random.Range(-0.5f,0.5f), Space.World); 
				s++;
			}
			r++;
		}
		//Returns Map
		return map;
	}

	//Gets tile pos with y offset from floorspots
	private Vector3 getTilePos(int x,int y,float yOffset){
		Vector3 pos = floorSpots[y,x].position;
		return new Vector3(pos.x,yOffset,pos.z);
	}
}
