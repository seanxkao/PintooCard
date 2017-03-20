using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour {
	protected static Manager m;
	protected int rule;

	public static Manager manager(){
		return m;
	}

	void Awake(){
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

	public void onSceneLoaded(Scene scene, LoadSceneMode loadSceneMode){

	}

	public void loadPuzzleMode(int rule){
		this.rule = rule;
		loadPuzzleMode ();
	}

	public void loadPuzzleMode(){
		SceneManager.LoadScene ("puzzle_mode_9x9");
	}

	public void loadMainMenu(){
		SceneManager.LoadScene ("main_menu");
	}

	public int getRule(){
		return rule;
	}
}
