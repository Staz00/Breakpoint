using UnityEngine;
using System.Collections;

public class Crosshairs : MonoBehaviour {

    void Start()
    {
        Cursor.visible = false;
    }

	void Update () {
        transform.Rotate(Vector3.forward * -40 * Time.deltaTime);
	}
}
