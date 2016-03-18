using UnityEngine;
using System.Collections;

public class PickUp : MonoBehaviour {
    public LayerMask layerMask;
    public Transform objectHolder;

    private bool m_isHolding;

    Vector3 m_Offset;
    Vector3 m_ObjectHolderOrigin;

	void Start () {
        m_Offset = new Vector3(0f, 1f, 0f);
	}

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + m_Offset, (transform.forward), out hit, 1.5f, layerMask))
        {
            if (hit.collider.CompareTag("MoveableObject") && Input.GetKeyUp(KeyCode.E) && !m_isHolding)
            {
                //remove all physics to the rigidbody
                hit.collider.GetComponent<Rigidbody>().isKinematic = true;

                //parent it under the player so it follows it
                hit.collider.transform.parent = objectHolder;

                //move this game object upwards if it is grounded
                if(hit.collider.GetComponent<CompanionCube>().m_isGrounded)
                    objectHolder.Translate(Vector3.up * 0.35f);

                m_isHolding = true;
            }
            else if (hit.collider.CompareTag("MoveableObject") && Input.GetKeyUp(KeyCode.E) && m_isHolding)
            {
                m_isHolding = false;

                //re-enable physics to this game object
                hit.collider.GetComponent<Rigidbody>().isKinematic = false;

                //remove its parent object
                objectHolder.DetachChildren();
            }
        }
    }

}
