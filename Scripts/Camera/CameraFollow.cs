using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraFollow : MonoBehaviour {

    [Header("Camera Values")]
	public Transform target;
	public float smoothing;
    public float duration = 0.1f;

    [Header("Shader Types")]
    public Shader transparent;
    public Shader standard;

    List<GameObject> buildings;
    MeshRenderer rend;
	Vector3 offset;

    bool m_IsTransparent = false;

	void Start()
	{
		offset = transform.position - target.position;

        buildings = new List<GameObject>();

        transparent = Shader.Find("Transparent/Diffuse");
        standard = Shader.Find("Standard");
	}

    void FixedUpdate()
    {
        if(target != null)
        {
            Vector3 targetCamPos = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);

            Vector3 cameraToTarget = (target.position + new Vector3(0, 1f, 0)) - transform.position;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, cameraToTarget, out hit))
            {
                if (hit.collider.CompareTag("Building"))
                {
                    rend = hit.collider.GetComponent<MeshRenderer>();

                    if (rend.material.shader != transparent)
                    {
                        rend.material.shader = transparent;

                        StartCoroutine(Fade(1f, 0.25f, .5f, hit));
                    }

                    if (!buildings.Contains(hit.collider.gameObject))
                    {
                        buildings.Add((GameObject)hit.collider.gameObject);
                    }
                }
                else
                {
                    if(m_IsTransparent)
                    {
                        foreach (GameObject obj in buildings)
                        {
                            StartCoroutine(FadeIn(0.25f, 1f, 0.5f, obj));
                            //obj.GetComponent<MeshRenderer>().material.color = new Color(1f, 1f, 1f, 1f);
                            //obj.GetComponent<MeshRenderer>().material.shader = standard;
                        }
                    }
                }
            }
        }
    }
    IEnumerator Fade(float from, float to, float time, RaycastHit building)
    {
        m_IsTransparent = true;
        float speed = 1 / time;
        float percent = 0;
        while (percent <= 1)
        {
            percent += Time.deltaTime * speed;
            building.collider.GetComponent<MeshRenderer>().material.color = new Color(1f, 1f, 1f, Mathf.Lerp(from, to, percent));
            yield return null;
        }
    }

    IEnumerator FadeIn(float from, float to, float time, GameObject obj)
    {
        m_IsTransparent = false;
        float speed = 1 / time;
        float percent = 0;

        while (percent <= 1)
        {
            percent += Time.deltaTime * speed;
            obj.GetComponent<MeshRenderer>().material.color = new Color(1f, 1f, 1f, Mathf.Lerp(from, to, percent));
            yield return null;

            if (percent >= 1)
            {
                obj.GetComponent<MeshRenderer>().material.shader = standard;
            }
        }
    }

}