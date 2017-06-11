using UnityEngine;

public class EventManager : MonoBehaviour {
	
	public delegate void Level(string scene);
	public static event Level LevelLoad;
	public static event Level LevelUnload;

	public static void LoadLevel(string scene){LevelLoad(scene);}
	public static void UnloadLevel(string scene){LevelUnload(scene);}

	public delegate void Events();
	public static event Events LevelEnd;
	public static event Events QuitLevel;
	
	public delegate void Init(Level level);
	public static event Init LevelInit;

	public static void InitLevel(Level level){LevelInit(level);}
}
