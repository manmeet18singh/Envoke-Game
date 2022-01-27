using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] mRoomList = null;
    [SerializeField]
    GameObject mCurrentRoom = null;
    [SerializeField]
    Animator mAnim = null;
    [SerializeField]
    Image mFadeImage = null;
    [SerializeField]
    Color mTransparentColor = default;
    [HideInInspector] public static int mCurrRoomIndex = 0;

    [SerializeField] TextMeshProUGUI mLevelCounter = null;

    GameObject mSpawnPoint;
    int mFadeIn;
    int mFadeOut;
    bool beenInCooldown = false;

    [HideInInspector] public bool respawn = false;

    private void Awake()
    {
        mFadeImage.color = Color.black;
    }

    private void Start()
    {
        mFadeIn = Animator.StringToHash("FadeIn");
        mFadeOut = Animator.StringToHash("FadeOut");

        if (SavePointSystem.SavedStats)
        {
            GameManager.Instance.StopGame();
            mAnim.enabled = true;
            mCurrRoomIndex = SavePointSystem.LastRoomSaved;
            StartCoroutine(UpdateRooms());
        }
        else
        {
            mCurrRoomIndex = 0;
            mFadeImage.color = mTransparentColor;
        }

        GameManager.Instance.onChangeRoom += StartRoomChange;
    }

    private void OnDestroy()
    {
        GameManager.Instance.onChangeRoom -= StartRoomChange;
    }

    private void StartRoomChange()
    {
        mAnim.enabled = true;
        mAnim.Play(mFadeOut);
    }

    public IEnumerator UpdateRooms()
    {
        if(mCurrentRoom != null)
            Destroy(mCurrentRoom);

        Pickup[] pickups = FindObjectsOfType<Pickup>();

        foreach(Pickup pick in pickups)
        {
            if (pick)
                pick.CleanUp();
        }

        mCurrentRoom = Instantiate(mRoomList[mCurrRoomIndex++]);

        while(mCurrentRoom == null || mCurrentRoom.name == string.Empty)
        {
            yield return new WaitForSecondsRealtime(.16f);
        }

        GameManager.Instance.mPlayer.SetActive(false);
        mSpawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint");
        GameManager.Instance.mPlayer.transform.position = mSpawnPoint.transform.position;
        Destroy(mSpawnPoint);
        GameManager.Instance.mPlayer.SetActive(true);
        GameManager.Instance.mMainCam.transform.position = GameManager.Instance.mPlayer.transform.position;

        if (mCurrRoomIndex == 4 || mCurrRoomIndex == 8 || mCurrRoomIndex == 13)
        {
            AudioManager.ChangeBackgroundMusic("CooldownBG");
            mLevelCounter.gameObject.SetActive(false);
            beenInCooldown = true;
        }
        else if (mCurrRoomIndex == 14)
        {
            AudioManager.ChangeBackgroundMusic("BossBG");
            mLevelCounter.gameObject.SetActive(false);
        }
        else
        {
            AudioManager.ChangeBackgroundMusic("MainBG");
            //if (!mLevelCounter.IsActive())
            mLevelCounter.gameObject.SetActive(true);
            
            if(beenInCooldown) 
                mLevelCounter.text = $"Level: {mCurrRoomIndex-1}";
            else
                mLevelCounter.text = $"Level: {mCurrRoomIndex}";
        }

        mAnim.Play(mFadeIn);
    }

/*    public void GoToRoom(int _roomIndex)
    {//Might be buggy
        mCurrRoomIndex = _roomIndex;
        StartRoomChange();
    }*/

    public void TransitionFinished()
    {
        mAnim.enabled = false;
        GameManager.Instance.UnStopGame();
    }
}
