using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour {
	protected static Manager m;
	protected MapInfo mapInfo;
	protected int rule;

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

	public MapInfo getMapInfo(){
		return mapInfo;
	}

	public void setMapInfo(MapInfo mapInfo){
		this.mapInfo = mapInfo;
	}
}
