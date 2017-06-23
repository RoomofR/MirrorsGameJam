using UnityEngine;

public class Door : MonoBehaviour {

	public Colors color;
	public bool isOpen;
	public Transform doorObject; //Actualy thing to be enabled and disabled
	
	public void trigger(){
		if(isOpen){
			doorObject.GameObject.SetActive(true);
			isOpen=false;
		}else{
			doorObject.SetActive(false);
			isOpen=true;	
		}
	}
}
