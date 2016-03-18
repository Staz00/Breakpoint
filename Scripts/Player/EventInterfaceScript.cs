using UnityEngine;
using System.Collections;

public class EventInterfaceScript : MonoBehaviour {

	// Use this for initialization

    public Camera camera;

    private RectTransform m_Transform;
	void Start () {
        m_Transform = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
        m_Transform.forward = camera.transform.forward;
	}
}
