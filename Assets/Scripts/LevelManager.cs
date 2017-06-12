using UnityEngine.SceneManagement;
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

	private void loadLevel(string sceneName){
		//Scene scene = SceneManager.GetSceneByName("Levels/"+sceneName);
		//SceneManager.LoadScene(scene, LoadSceneMode.Additive);
	}

	private void unloadLevel(string sceneName){

	}
}
