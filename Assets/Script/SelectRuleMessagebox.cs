using UnityEngine;
using System.Collections;

public class SelectRuleMessagebox : Messagebox {
	void Start () {
		//4 buttons for 4 rules.
		for (int i = 0; i < 4; i++) {
			int id = i;
			buttons [i].onClick.AddListener (() => Manager.manager ().loadPuzzleMode (id)); 

		}
	}

}
