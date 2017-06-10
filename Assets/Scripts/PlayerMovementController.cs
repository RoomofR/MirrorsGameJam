using UnityEngine;

public struct pos2D{
	public pos2D(int a,int b) {
    x = a; y = b;
  	}
	public int x;
	public int y;
}

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
	private float playerYOffset=0.25f;
	private float raycasYOffset=0.1f;

	[HideInInspector]
	public pos2D[] orientation = new pos2D[4] {new pos2D(-1,0),new pos2D(1,0),new pos2D(0,-1),new pos2D(0,1)};

	void Awake () {
		floorSpots=mapFloor();
		setPlayer(playerPos.x,playerPos.y);
	}

	void Update(){
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
		if(floorSpots.GetLength(0)>tempPos.y && tempPos.y>=0 && 
			floorSpots.GetLength(1)>tempPos.x && tempPos.x>=0
			&& getTilePos(tempPos.x,tempPos.y,raycasYOffset)!=Vector3.zero){
			//Test if blockade found
			Vector3 orig = new Vector3(Player.position.x,raycasYOffset,Player.position.z);
			Vector3 directionRay = getTilePos(tempPos.x,tempPos.y,raycasYOffset) - orig;		
			RaycastHit hit;
			if(Physics.Raycast(orig, directionRay,out hit,1) &&
				hit.collider.GetComponent<Blockade>()!=null){
				shake(move.x==0);
			}else {playerPos=tempPos;}
		}else shake(move.x==0);
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


}
