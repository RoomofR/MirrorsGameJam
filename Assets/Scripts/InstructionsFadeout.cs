using UnityEngine;

public class InstructionsFadeout : MonoBehaviour {
	private bool fade=false;
	public float fadeSpeed;
	void Update () {
		if(fade){
			Color color = GetComponent<TextMesh>().color;
			color.a-=fadeSpeed;
			GetComponent<TextMesh>().color = color;
		}
		if(Input.anyKeyDown){fade=true;}
	}
}
