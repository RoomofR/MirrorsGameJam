using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	public int LevelID;

	private GameObject Level;
	private GameObject EndPortal;
	private PlayerMovementController Player;


	void Awake(){
		//EndPortal Resource
		EndPortal=Resources.Load("Objects/EndPortalEffect") as GameObject;
		//Init First Level
		LevelID=PlayerPrefs.GetInt("currentLevelID");
		Player = GetComponent<PlayerMovementController>();
		InitLevel(LevelID);
	}

	public void loadNextLevel(){
		PlayerPrefs.SetInt("solvedLevel",LevelID);LevelID++;
		GameObject nextLevel = Resources.Load("Levels/Level"+LevelID) as GameObject;
		if(nextLevel!=null){
			InitLevel(LevelID);
		}else{StartCoroutine("leaveToMainMenu");}
	}

	private void InitLevel(int id){
		Level = Instantiate(Resources.Load("Levels/Level"+LevelID) as GameObject);
		Level LevelData=Level.GetComponent<Level>();
		GameObject endPortal = Instantiate(EndPortal);
		endPortal.transform.position = Player.SetBoard(LevelData.board, LevelData.StartPos, LevelData.EndPos);
		endPortal.transform.parent = Level.transform;
	}

	IEnumerator leaveToMainMenu(){
		float fadeSpeed = GetComponent<FadingEffect>().BeginFade(1);
		yield return new WaitForSeconds(fadeSpeed);
		SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
	}
}
