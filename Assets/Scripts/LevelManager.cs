﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	public int LevelID;
	public bool DebugLevel;
	private GameObject Level;
	private GameObject EndPortal;
	private GameObject KEY;
	private PlayerMovementController Player;
	private CameraMovementController Cam;
	private MirrorFollow Mirror;


	void Awake(){
		//EndPortal Resource
		EndPortal=Resources.Load("Objects/EndPortalEffect") as GameObject;
		//TODO DEFINE KEY KEY=Resources.Load("Objects/KEY") as GameObject;
		//Init First Level
		if(!DebugLevel){LevelID=PlayerPrefs.GetInt("currentLevelID");}
		Player = GetComponent<PlayerMovementController>();
		Cam = GetComponent<CameraMovementController>();
		Mirror = GetComponent<MirrorFollow>();
		InitLevel();
	}

	public void loadNextLevel(){
		PlayerPrefs.SetInt("solvedLevel",LevelID);LevelID++;
		GameObject nextLevel = Resources.Load("Levels/Level"+LevelID) as GameObject;
		if(nextLevel!=null){
			Invoke("InitLevel", 1);
		}else{StartCoroutine("leaveToMainMenu");}
	}

	private void InitLevel(){
		Level = Instantiate(Resources.Load("Levels/Level"+LevelID) as GameObject);
		Level.transform.position = new Vector3(Level.transform.position.x,Level.transform.position.y,(LevelID-1)*10);
		Level LevelData=Level.GetComponent<Level>();
		GameObject endPortal = Instantiate(EndPortal);
		endPortal.transform.position = Player.SetBoard(LevelData.board, LevelData.StartPos, LevelData.EndPos);
		endPortal.transform.parent = Level.transform;
		//Keys
		if(LevelData.Keys.Length>0){
			Vector3[] keysPos = Player.SetKeysDoor(LevelData.Keys,LevelData.Doors);
			foreach(Vector3 pos in keysPos){
				//TODO DEFINE KEY OBJECT IN RESOURCE
				GameObject key = Instantiate(KEY);
				key.transform.position = pos;
				key.transform.parent = Level.transform;
			}
		}
		//Other Settings
		Mirror.follow=true;
		Cam.rotate=LevelData.CameraRotate;
	}

	IEnumerator leaveToMainMenu(){
		float fadeSpeed = GetComponent<FadingEffect>().BeginFade(1);
		yield return new WaitForSeconds(fadeSpeed);
		SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
	}
}