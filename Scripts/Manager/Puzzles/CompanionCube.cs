using UnityEngine;
using System.Collections;

public class CompanionCube : MonoBehaviour {

    public bool m_isGrounded;

	void Update () {

        FireRaycasts();
    }

    //fire raycast from all side to check if the cube is grounded or not
    void FireRaycasts()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, -transform.up, out hit, 0.75f))
        {
            if (hit.collider.CompareTag("Floor"))
            {
                m_isGrounded = true;
            }
            else
                m_isGrounded = false;

        }

        if (Physics.Raycast(transform.position, transform.up, out hit, 0.75f))
        {
            if (hit.collider.CompareTag("Floor"))
            {
                m_isGrounded = true;
            }
            else
                m_isGrounded = false;
        }

        if (Physics.Raycast(transform.position, -transform.forward, out hit, 0.75f))
        {
            if (hit.collider.CompareTag("Floor"))
            {
                m_isGrounded = true;
            }
            else
                m_isGrounded = false;
        }

        if (Physics.Raycast(transform.position, transform.forward, out hit, 0.75f))
        {
            if (hit.collider.CompareTag("Floor"))
            {
                m_isGrounded = true;
            }
            else
                m_isGrounded = false;
        }
        if (Physics.Raycast(transform.position, transform.right, out hit, 0.75f))
        {
            if (hit.collider.CompareTag("Floor"))
            {
                m_isGrounded = true;
            }
            else
                m_isGrounded = false;
        }
        if (Physics.Raycast(transform.position, -transform.right, out hit, 0.75f))
        {
            if (hit.collider.CompareTag("Floor"))
            {
                m_isGrounded = true;
            }
            else
                m_isGrounded = false;
        }
    }

}
