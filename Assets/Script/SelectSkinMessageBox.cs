using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class SelectSkinMessageBox : Messagebox {

	[SerializeField] protected Sprite[] Cardskin;
	void Start () {
		for (int i = 0; i < 2; i++) {
			//2 buttons for 2 skins.
			int skin = i; 
			buttons[i].onClick.AddListener(()=>{
				Manager.manager().setSkin(skin);
				this.transform.parent.GetComponent<Image> ().sprite = Cardskin[skin];
			});

		}
	}

}
