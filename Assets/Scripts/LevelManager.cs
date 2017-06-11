using UnityEngine;

public class LevelManager : MonoBehaviour {

	public int LevelLoaded;


	void OnEnable(){
		EventManager.LevelLoad += loadLevel;
		EventManager.LevelUnload += unloadLevel;
	}
	void OnDisable(){
		EventManager.LevelLoad -= loadLevel;
		EventManager.LevelUnload -= unloadLevel;
	}

	private void loadLevel(string scene){

	}

	private void unloadLevel(string scene){

	}
}
