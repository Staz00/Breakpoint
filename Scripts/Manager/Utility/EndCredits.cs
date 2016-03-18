using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndCredits : MonoBehaviour {

    public Text returnToMenu;

    void Start()
    {
        Cursor.visible = true;
    }

    public void OnPress()
    {
        Application.LoadLevel(0);
    }
}
