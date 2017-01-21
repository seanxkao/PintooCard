using UnityEngine;
using System.Collections;

public class PauseMessagebox : Messagebox {
	void Start () {
		buttons [0].onClick.AddListener (()=>Manager.manager().loadPuzzleMode());	//restart
		buttons [1].onClick.AddListener (()=>Manager.manager().loadMainMenu());		//return
	}
}
