using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject m_PauseMenu = null;

    private void Awake()
    {
        GameManager.Instance.onGamePaused += TogglePause;
    }

    private void OnDestroy()
    {
        GameManager.Instance.onGamePaused -= TogglePause;
    }

    public void TogglePause(bool _paused)
    {
        m_PauseMenu.SetActive(_paused);
    
    }

    //TODO Scenemanager
    public void LoadScene(int scene)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(scene);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}
