using UnityEngine;
using System.Collections;

public class SelectMapMessageBox : Messagebox {
	[SerializeField]	protected MapInfo[] mapInfos;

	void Start () {
		//3 button for 3 stage.
		for (int i = 0; i < 3; i++) {
			int id = i;
			buttons [i].onClick.AddListener (()=>{
				Manager.manager().setMapInfo(mapInfos[id]);
			}); 
		}
	}

	public void setMapMode(int mode){
		Manager.manager().setMode((Mode)mode);
	}
}
