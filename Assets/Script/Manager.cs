using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public enum Mode{
	PUZZLE,
	FREE
}

public class Manager : MonoBehaviour {
	protected static Manager m;
	protected MapInfo mapInfo;
	protected int rule;
	protected int skin; //0:Original, 1:V_butterfly
	protected Mode mode; 

    void Awake(){
		//Manager is singleton.
		//destroy it self if there is already an instance
		if (m != null) {
			Destroy (gameObject);
		} else {
			DontDestroyOnLoad (gameObject);
			m = this;
			SceneManager.sceneLoaded += onSceneLoaded;
		}
	}

	void Destroy(){
		SceneManager.sceneLoaded -= onSceneLoaded;
	} 

	public static Manager manager(){
		return m;
	}

	public void onSceneLoaded(Scene scene, LoadSceneMode loadSceneMode){
		//currently useless
	}
		
	public void loadPuzzleMode(){
		if (mode == Mode.FREE) {
			mapInfo = new MapInfo();
			mapInfo.row = 9;
			mapInfo.col = 9;
			mapInfo.enable = new Row[9];
			for (int i = 0; i < 9; i++) {
				mapInfo.enable [i] = new Row ();
				mapInfo.enable[i].cell = new bool[9];
				for (int j = 0; j < 9; j++) {
					mapInfo.enable[i].cell[j] = true;
				}
			}
		}
		SceneManager.LoadScene ("puzzle_mode");
	}

	public void loadPuzzleMode(int rule){
		this.rule = rule;
		loadPuzzleMode ();
	}


	public void loadMainMenu(){
		SceneManager.LoadScene ("main_menu");
	}

	public int getRule(){
		return rule;
	}
	public int getSkin(){
		return skin;
	}
	public void setSkin(int skin){
		this.skin = skin;
	}
	public MapInfo getMapInfo(){
		return mapInfo;
	}
	public void setMapInfo(MapInfo mapInfo){
		this.mapInfo = mapInfo;
	}

	public void setMode(Mode mode){
		this.mode = mode;
	}

	public Mode getMode(){
		return mode;
	}
}
