using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MainMenuUIManager : MonoBehaviour
{
    [SerializeField]
    Image mLoadingFillImage = null;

    private void Awake()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        SavePointSystem.SavedStats = false;
        SavePointSystem.LastRoomSaved = -1;
        LumeInventory.ResetInventory();
    }

    public void SetActive(GameObject _panel)
    {
        if (_panel.activeInHierarchy)
            _panel.SetActive(false);
        else
            _panel.SetActive(true);
    }

    public void ToggleAudioListener()
    {
        if(AudioListener.pause != true)
        {
            AudioListener.pause = true;
        }    
        else
        {
            AudioListener.pause = false;
        }
    }


    public void LoadLevel(int _buildIndex)
    {
        Envoke.SceneManager.Instance.LoadScene(_buildIndex);    
    }

    public void LoadAsync(int _buildIndex)
    {
        StartCoroutine(LoadAsynchrounously(_buildIndex));
    }

    IEnumerator LoadAsynchrounously(int _buildIndex)
    {
        // Swap to either menu mixer or main mixer for bg music
        if (_buildIndex == 0)
            AudioManager.GameToMainMenu();
        else
            AudioManager.MainMenuToGame();

        AsyncOperation operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(_buildIndex);
        float progress = 0f;

        while (!operation.isDone)
        {
            progress = Mathf.Clamp01(operation.progress / .9f);
            mLoadingFillImage.fillAmount = progress;

            yield return null;
        }
    }

    public void QuitGame()
    {      
        Envoke.SceneManager.Instance.QuitGame();       
    }

}
