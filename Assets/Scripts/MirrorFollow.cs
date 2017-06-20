using UnityEngine;

public class MirrorFollow : MonoBehaviour {

	public Transform Mirrors;
	public Transform Player;
	public float MirrorTime;
	public bool follow;

	Vector2 velocity;
	void FixedUpdate () {
		if(follow){
			float posX = Mathf.SmoothDamp(Mirrors.position.x, Player.position.x, ref velocity.x, MirrorTime);
			float posZ = Mathf.SmoothDamp(Mirrors.position.z, Player.position.z, ref velocity.y, MirrorTime);
			Mirrors.position = new Vector3(posX,Mirrors.position.y,posZ);
		}
		
	}
}
