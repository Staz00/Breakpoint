using UnityEngine;
using System.Collections;

public class FpsCounter : MonoBehaviour
{
	private const float FPS_UPDATE_INTERVAL = 0.5f;
    private int m_FpsFrames = 0;

    private float m_FpsAccum = 0;
	private float m_FpsTimeLeft = FPS_UPDATE_INTERVAL;
	private float m_Fps = 0;

    private bool m_Enabled;

    public bool Enabled
    {
        get { return m_Enabled; }
        set { m_Enabled = value; }
    }

    void Start()
    {
        m_Enabled = false;
    }

	void Update()
	{
        if(m_Enabled)
        {
            m_FpsTimeLeft -= Time.deltaTime;
            m_FpsAccum += Time.timeScale / Time.deltaTime;
            m_FpsFrames++;

            if (m_FpsTimeLeft <= 0)
            {
                m_Fps = m_FpsAccum / m_FpsFrames;
                m_FpsTimeLeft = FPS_UPDATE_INTERVAL;
                m_FpsAccum = 0;
                m_FpsFrames = 0;
            }
        }
	}

	void OnGUI()
	{
        if(m_Enabled)
        {
            GUILayout.BeginArea(new Rect(5, 5, 500, 500));
            GUILayout.Label("FPS: " + m_Fps.ToString("f1"));
            GUILayout.EndArea();
        }
		
	}
}
