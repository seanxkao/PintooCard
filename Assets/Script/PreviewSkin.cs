using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PreviewSkin : MonoBehaviour {

    [SerializeField]
    protected Sprite[] Cardskin;
    // Use this for initialization
    void Start () {
        int skin = Manager.manager().getSkin();
        this.GetComponent<Image>().sprite = Cardskin[skin];
    }
	
}
