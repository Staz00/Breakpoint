using UnityEngine;
using System.Collections;

public class ShatteringScript : MonoBehaviour{

    public Shatter cube;

    private int m_count;

    Rigidbody m_rigidBody;
    Vector3 m_torque;

    public void ShatterEffect()
    {
        m_count = Random.Range(5, 15);

        for (int i = 0; i < m_count; i++)
        {
            m_torque.x = Random.Range(-.5f, 0.5f);
            m_torque.y = Random.Range(-.5f, 0.5f);
            m_torque.z = Random.Range(-.5f, 0.5f);


            Shatter newCube = Instantiate(cube, transform.position + new Vector3(m_torque.x, 0, m_torque.z), transform.rotation) as Shatter;

            m_rigidBody = cube.GetComponent<Rigidbody>();

            m_rigidBody.AddTorque(m_torque, ForceMode.Impulse);
            m_rigidBody.AddForce(Vector3.up * Random.Range(-1, 1), ForceMode.Impulse);

        }
    }
    
}
