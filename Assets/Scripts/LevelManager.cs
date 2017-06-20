using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	public int LevelID;

	private GameObject Level;
	private GameObject EndPortal;

	void Awake(){
		//EndPortal Resource
		EndPortal=Resources.Load("Objects/EndPortalEffect") as GameObject;
		//Init First Level
		LevelID=PlayerPrefs.GetInt("currentLevelID");
		Debug.Log("Levels/Level"+LevelID);
		Level = Instantiate(Resources.Load("Levels/Level"+LevelID) as GameObject);
		Level LevelData=Level.GetComponent<Level>();
		PlayerMovementController Player = GetComponent<PlayerMovementController>();

		GameObject endPortal = Instantiate(EndPortal);
		endPortal.transform.position = Player.SetBoard(LevelData.board, LevelData.StartPos, LevelData.EndPos);
		endPortal.transform.parent = Level.transform;
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
