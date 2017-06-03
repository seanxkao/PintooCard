using UnityEngine;
using System.Collections;

public class SelectFreeRuleMessagebox : Messagebox {
	void Start () {
		//2 buttons for 2 rules.
		buttons[0].onClick.AddListener(() => Manager.manager().loadPuzzleMode(1));
		buttons[1].onClick.AddListener(() => Manager.manager().loadPuzzleMode(3));
	}

}
