using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HiddenPlatformScript : MonoBehaviour {

    public GameObject[] hiddenObjects;
    public GameObject panel;
    public GameObject toolTip;
    public Canvas m_EventUI;

    public GameObject edge;
    public GameObject railings;
    public Text conditionText;
    public Text execText;
    public bool isOpen;
    public AudioClip m_Select;
    public AudioClip m_ErrorSelect;
    [Range(0, 1)]
    public float m_Volume = 0.5f;


    string[] m_Code;
    string[] m_Exec;


    void Start()
    {
        panel.SetActive(false);
        toolTip.SetActive(false);
    }

    public void OpenTerminal()
    {
        PlayerMovement player = FindObjectOfType(typeof(PlayerMovement)) as PlayerMovement;
        if (player != null)
        {
            player.IsPaused = true;
            player.DisableMove = true;
        }

        isOpen = true;
        panel.SetActive(true);

        
    }

    bool CheckCode(string codeText)
    {

        if (codeText.Contains("foreach("))
        {
            m_Code = codeText.Split(' ');
            if (m_Code[0] == "foreach(GameObject" && m_Code[2] == "in" && m_Code[3] == "hiddenObjects)")
            {
                return true;
            }
        }
        else if (codeText.Contains("for("))
        {
            m_Code = codeText.Split(';');

            if (m_Code[0] == "for(int i = 0" && m_Code[1] == " i < hiddenObjects.Length" && m_Code[2] == " i++)")
            {
                return true;
            }
        }

        return false;

    }
    bool CheckExec(string execText)
    {
        m_Exec = execText.Split('.');

        if (m_Code[0].Contains("foreach("))
        {
            if (m_Exec[0] == m_Code[1] && m_Exec[1] == "SetActive(true);")
            {
                return true;
            }
        }
        else if (m_Code[0].Contains("for("))
        {
            if (m_Exec[0] == "hiddenObjects[i]" && m_Exec[1] == "SetActive(true);")
            {
                return true;
            }
        }

        return false;
    }

    public void OnCompilePress()
    {
        isOpen = false;

        if (CheckCode(conditionText.text) && CheckExec(execText.text))
        {
            AudioScript.m_Audio.PlaySoundFx(m_Select, m_Volume);

            if(this.isActiveAndEnabled)
            {
                if(hiddenObjects.Length >= 1)
                {
                    foreach (GameObject obj in hiddenObjects)
                    {
                        obj.SetActive(true);
                    }

                    Destroy(edge);
                    Destroy(railings);
                    Destroy(gameObject);
                    OnClosePress();

                    m_EventUI.enabled = false;
                }
            }
        }
        else
            AudioScript.m_Audio.PlaySoundFx(m_ErrorSelect, m_Volume);
    }

    public void OnClosePress()
    {

        PlayerMovement player = FindObjectOfType(typeof(PlayerMovement)) as PlayerMovement;
        if (player != null)
        {
            player.IsPaused = false;
            player.DisableMove = false;
        }

        panel.SetActive(false);
        toolTip.SetActive(false);
        ClearFields();
    }

    public void OnTipsPress()
    {
        toolTip.SetActive(true);
    }

    IEnumerator InstantiateGameObjects()
    {
        foreach (GameObject obj in hiddenObjects)
        {
            obj.SetActive(true);
            yield return new WaitForEndOfFrame();
        }
    }
    
    private void ClearFields()
    {
        conditionText.GetComponent<InputField>().text = "//Loop Statement Here";
        execText.GetComponent<InputField>().text = "//Execution Statement Here";
    }
}
