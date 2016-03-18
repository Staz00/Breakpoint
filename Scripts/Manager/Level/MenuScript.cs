using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {

    public Canvas m_QuitMenu;
    public Canvas m_ControlsMenu;
    public Canvas m_PrologueScreen;
    public Canvas m_PlayTutorial;
    public Button m_Start;
    public Button m_Exit;
    public Button m_Controls;
    public Button m_Prologue;

    public AudioClip selectFx;
    [Range(0,1)]
    public float volume = 0.75f;

    void Awake()
    {
        m_PlayTutorial = m_PlayTutorial.GetComponent<Canvas>();
        m_QuitMenu = m_QuitMenu.GetComponent<Canvas>();
        m_Start = m_Start.GetComponent<Button>();
        m_Exit = m_Exit.GetComponent<Button>();
        m_Controls = m_Controls.GetComponent<Button>();
        m_Prologue = m_Prologue.GetComponent<Button>();

        m_PlayTutorial.enabled = false;
        m_QuitMenu.enabled = false;
        m_ControlsMenu.enabled = false;
        m_PrologueScreen.enabled = false;
    }

    void Start()
    {
        Cursor.visible = true;
    }

    public void ExitPress()
    {
        PlaySoundFx();

        m_QuitMenu.enabled = true;

        m_Start.enabled = false;
        m_Exit.enabled = false;
        m_Controls.enabled = false;
        m_Prologue.enabled = false;
    }

    public void PlayTutorial()
    {
        LoadingScreen loadingScreen = FindObjectOfType(typeof(LoadingScreen)) as LoadingScreen;

        if (loadingScreen != null)
        {
            loadingScreen.levelToLoad = "Tutorial";
            loadingScreen.StartPlaying();
        }
            
    }

    public void CancelPress()
    {
        PlaySoundFx();

        m_PlayTutorial.enabled = false;

        m_Start.enabled = true;
        m_Exit.enabled = true;
        m_Controls.enabled = true;
        m_Prologue.enabled = true;
    }

    public void PlayPress()
    {
        PlaySoundFx();

        m_PlayTutorial.enabled = true;

        m_Start.enabled = false;
        m_Exit.enabled = false;
        m_Controls.enabled = false;
        m_Prologue.enabled = false;
    }


    public void ControlsPress()
    {
        PlaySoundFx();

        m_ControlsMenu.enabled = true;

        m_Start.enabled = false;
        m_Exit.enabled = false;
        m_Controls.enabled = false;
        m_Prologue.enabled = false;
    }

    public void FromControlsToMenu()
    {
        PlaySoundFx();

        m_ControlsMenu.enabled = false;

        m_Controls.enabled = true;
        m_Start.enabled = true;
        m_Exit.enabled = true;
        m_Prologue.enabled = true;
    }

    public void FromPrologueToMenu()
    {
        PlaySoundFx();

        m_PrologueScreen.enabled = false;

        m_Start.enabled = true;
        m_Exit.enabled = true;
        m_Controls.enabled = true;
        m_Prologue.enabled = true;
    }

    public void NoPress()
    {
        PlaySoundFx();

        m_QuitMenu.enabled = false;

        m_Start.enabled = true;
        m_Exit.enabled = true;
        m_Controls.enabled = true;
        m_Prologue.enabled = true;
    }

    public void YesPess()
    {
        PlaySoundFx();

        Application.Quit();
    }

    public void OnProloguePress()
    {
        PlaySoundFx();

        m_PrologueScreen.enabled = true;

        m_Start.enabled = false;
        m_Exit.enabled = false;
        m_Controls.enabled = false;
        m_Prologue.enabled = false;
    }


    void PlaySoundFx()
    {
        AudioScript.m_Audio.PlaySoundFx(selectFx, volume);
    }

}
