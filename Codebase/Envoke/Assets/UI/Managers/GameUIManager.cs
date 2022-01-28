using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameUIManager : MonoBehaviour
{
    [SerializeField]
    GameObject mDeathScreen = null;
    [SerializeField]
    GameObject mBlackoutPanel = null;
    [SerializeField]
    GameObject mDeathScreenSelectedButton = null;
    [SerializeField]
    GameObject mPauseScreen = null;
    [SerializeField]
    GameObject mPauseScreenSelectedButton = null;
    [SerializeField]
    GameObject mWinScreen = null;
    [SerializeField]
    GameObject mWinScreenSelectedButton = null;


    void Start()
    {
        PlayerHealth.mPlayerDeath += DisplayDeathScreen;
        GameManager.Instance.onGamePaused += TogglePauseScreen;
        GameManager.Instance.onWinGame += DisplayWinScreen;
    }

    public void TogglePanel(GameObject _panel)
    {
        _panel.SetActive(!_panel.activeInHierarchy);
    }

    //Sets escape button to disable object when escape is pressed and reset escape to unpause game
    public void SetEscapeButton(GameObject _panel)
    {
        GameManager.Instance.EscapeButtonPressed = () =>
        {
            _panel.SetActive(false);
            GameManager.Instance.EscapeButtonPressed = GameManager.Instance.TogglePause;

        };
    }

    public void DisablePanel(GameObject _panel)
    {
        _panel.SetActive(false);
        GameManager.Instance.EscapeButtonPressed = GameManager.Instance.TogglePause;
    }


    public void ToggleAudioListener()
    {
        AudioListener.pause = !AudioListener.pause;
    }

    private void OnDestroy()
    {
        PlayerHealth.mPlayerDeath -= DisplayDeathScreen;
        GameManager.Instance.onGamePaused -= TogglePauseScreen;
        GameManager.Instance.onWinGame -= DisplayWinScreen;
    }

    void DisplayDeathScreen()
    {
        StartCoroutine(FadeOutToDeathScrene());

    }

    IEnumerator FadeOutToDeathScrene()
    {
        float fadeTime = 1.5f;
        float currentTime = 0;
        Image blackoutImage = mBlackoutPanel.GetComponent<Image>();
        mBlackoutPanel.SetActive(true);

        while (currentTime < fadeTime)
        {
            yield return null;
            blackoutImage.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, currentTime / fadeTime));
            currentTime += Time.deltaTime;
        }

        yield return new WaitForSeconds(0.25f);

        mDeathScreen.SetActive(true);
        ButtonSelectedManager.Instance.SetSelectedButton(mDeathScreenSelectedButton);
        GameManager.Instance.StopGame();
    }

    void TogglePauseScreen(bool _paused)
    {
        ButtonSelectedManager.Instance.SetSelectedButton(mPauseScreenSelectedButton);
        mPauseScreen.SetActive(_paused);
    }

    void DisplayWinScreen()
    {
        ButtonSelectedManager.Instance.SetSelectedButton(mWinScreenSelectedButton);
        mWinScreen.SetActive(true);
        GameManager.Instance.StopGame();
    }

    public void LoadScene(int _buildIndex)
    {
        Envoke.SceneManager.Instance.LoadScene(_buildIndex);
        //AudioManager.instance.Play("Button");
    }

    public void LoadScene(string _sceneName)
    {
        Envoke.SceneManager.Instance.LoadScene(_sceneName);
    }

    public void ResumeGame()
    {
        GameManager.Instance.TogglePause();
        //AudioManager.instance.Play("Button");
    }

    public void RestartLevel()
    {
        Envoke.SceneManager.Instance.ReloadLevel();
        //LumeInventory.ResetInventory();
        //AudioManager.instance.Play("Button");
    }

    public void QuitGame()
    {
        Envoke.SceneManager.Instance.QuitGame();
        //AudioManager.instance.Play("Button");
    }
}
