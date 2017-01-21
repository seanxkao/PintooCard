using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Messagebox : MonoBehaviour {
	[SerializeField]
	protected Button[] buttons;

	public virtual void showMessage(string text){
		gameObject.SetActive (true);
		GetComponentInChildren<Text> ().text = text;
	}
}
