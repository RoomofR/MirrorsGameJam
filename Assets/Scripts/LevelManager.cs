using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	public int LevelID;
	private GameObject Level;

	void Awake(){
		LevelID=PlayerPrefs.GetInt("currentLevelID");
		Debug.Log("Levels/Level"+LevelID);
		Level = Instantiate(Resources.Load("Levels/Level"+LevelID) as GameObject);
		Level LevelData=Level.GetComponent<Level>();
		PlayerMovementController Player = GetComponent<PlayerMovementController>();

		Player.FloorSpots=LevelData.board;
		Player.mapFloor();
		
		Player.setPlayer(LevelData.StartPos.x,LevelData.StartPos.y);
	}


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
