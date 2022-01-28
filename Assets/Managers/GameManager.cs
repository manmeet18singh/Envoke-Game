using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

    [Header("VFX References")]
    [SerializeField] public GameObject mBurningEffect;
    [SerializeField] public GameObject mSlowEffect;
    [SerializeField] public GameObject mStunEffect;
    [SerializeField] public GameObject mFreezeEffect;

    [HideInInspector]
    public PlayerMovement mPlayerMovement;
    [HideInInspector]
    public GameObject mPlayer;
    [HideInInspector]
    public PlayerHealth mPlayerHealth;
    [HideInInspector]
    public Transform mSpellFirePoint;
    [HideInInspector]
    public GameObject mSpellFirePointObj;
    [HideInInspector]
    public Camera mMainCam;
    [HideInInspector]
    public Vector3 mDoorCamPos;
    [HideInInspector]
    public Quaternion mDoorCamRot;
    [Header("Boss References")]
    [SerializeField] public GameObject mBoss;
    [SerializeField] public GameObject mBossUI;


    //public event Action onGameResume;
    public event Action<bool> onGamePaused;
    public event Action onRoomCompleted;
    public event Action onChangeRoom;
    public event Action onWinGame;
    public event Action<BaseUpgradeSO[], bool> onToggleUpgradeMenu;
    public event Action<BaseUpgradeSO[], bool> onTogglePurchUpgradeMenu;
    public event Action onUpgradeChose;
    public event Action onSaveGame;

    private int mEnemiesRemaining = 0;
    public VoidDelegate EscapeButtonPressed;

    //  [SerializeField]
    // public List<PickupData> mPickupsLootTable;

    private bool mGamePaused = false;

    private void Awake()
    {
        Instance = this;

        EscapeButtonPressed = TogglePause;
        InputManager.controls.UI.EscButton.performed += ctx => EscapeButtonPressed();

        mMainCam = Camera.main;
        mPlayer = GameObject.FindGameObjectWithTag("Player");

        mPlayerMovement = mPlayer.GetComponent<PlayerMovement>();
        mPlayerHealth = mPlayer.GetComponent<PlayerHealth>();
        mSpellFirePointObj = GameObject.FindGameObjectWithTag("PlayerFirePoint");
        mSpellFirePoint = mSpellFirePointObj.transform;
        
        AudioListener.pause = false;
        UnStopGame();
    }

    public void TogglePause()
    {
        if (!mGamePaused)
        {
            mGamePaused = true;
            AudioListener.pause = true;
            PausePlayerInput();
            InputManager.controls.UI.Interact.Disable();
            InputManager.controls.UI.Tab.Disable();
            //InputManager.controls.UI.Enable();
            Time.timeScale = 0;
        }
        else
        {
            mGamePaused = false;
            AudioListener.pause = false;
            ResumePlayerInput();
            InputManager.controls.UI.Interact.Enable();
            InputManager.controls.UI.Tab.Enable();
            Time.timeScale = 1;
        }
        onGamePaused?.Invoke(mGamePaused);
    }

    public void PausePlayerInput()
    {
        mPlayerMovement.inputX = mPlayerMovement.inputZ = 0;
        InputManager.controls.Player.Disable();
        Cursor.visible = true;
        CursorManager.Instance.SetActiveCursorAnimation(CursorType.Regular);
    }

    public void ResumePlayerInput()
    {
        InputManager.controls.Player.Enable();
    }

    public void StopGame()
    {
        PausePlayerInput();
        InputManager.controls.Disable();
        Time.timeScale = 0;
    }

    public void UnStopGame()
    {
        InputManager.controls.Enable();
        Time.timeScale = 1;
    }

    public void GameWon()
    {
        onWinGame?.Invoke();
    }

    public void ChangeRoom()
    {
        onChangeRoom?.Invoke();
    }

    public void ToggleUpgradeMenu(BaseUpgradeSO[] _upgradesRolled, bool _active)
    {
        onToggleUpgradeMenu?.Invoke(_upgradesRolled, _active);
    }

    public void TogglePurchUpgradeMenu(BaseUpgradeSO[] _upgradesRolled, bool _active)
    {
        onTogglePurchUpgradeMenu?.Invoke(_upgradesRolled, _active);
    }

    public void UpgradeChose()
    {
        onUpgradeChose?.Invoke();
    }

    public void SaveGame()
    {
        onSaveGame?.Invoke();
    }

    public void UpdateEnemiesRemaining(int _numEnemies)
    {
        mEnemiesRemaining += _numEnemies;

        if (mEnemiesRemaining <= 0 && !mBossUI.activeInHierarchy)
        {
            //AudioManager.instance.Play("WinFlag");
            BaseProjectile[] projectiles = FindObjectsOfType<BaseProjectile>();
            foreach (BaseProjectile proj in projectiles)
            {
                if (proj)
                    Destroy(proj);
            }

            AudioManager.instance.Play("Room Complete");
            onRoomCompleted?.Invoke();
        }
    }

    public PickupData GetRandomPickup()
    {
        while (true)
        {
            PickupData data = DynamicLoot.Instance.GetDrop();
            if (DynamicLoot.Instance.GetSpawnablePickups.Contains(data))
            {
                return data;
            }
        }
    }

    #region Pickups

    public GameObject SpawnPickup(PickupData _pickupData, Vector3 _destination, Vector3 _startPosition, float _duration)
    {
        _destination.y = mPlayer.transform.position.y;
        GameObject pickup = SpawnPickup(_pickupData, _startPosition);
        LerpPickupToPosition(pickup.GetComponent<Pickup>(), _startPosition, _destination, _duration);
        return pickup;
    }

    public GameObject SpawnPickup(PickupData _pickupData, Vector3 _position)
    {
        GameObject newPickup = new GameObject("Pickup");
        newPickup.AddComponent<Pickup>();
        newPickup.GetComponent<Pickup>().LoadPickup(_pickupData);
        newPickup.transform.position = _position;
        newPickup.transform.localPosition = Vector3.zero;

        return newPickup;
    }

    public void LerpPickupToPosition(Pickup _pickup, Vector3 _startPosition, Vector3 _destination, float _duration)
    {

        StartCoroutine(LerpPickupToPositionCoroutine(_pickup, _startPosition, _destination, _duration));
    }

    private IEnumerator LerpPickupToPositionCoroutine(Pickup _pickup, Vector3 _startPosition, Vector3 _destination, float _duration)
    {
        if (_pickup != null && _pickup.mCollider != null)
        {
            _pickup.mCollider.enabled = false;

            float maxHeight = 8;
            float timeElapsed = 0;
            if (_duration == 0)
                _duration = 1;
            while (timeElapsed < _duration && _pickup != null)
            {
                timeElapsed += Time.deltaTime;
                yield return null;
                float fractionOfJourney = timeElapsed / _duration;

                float lerpX = Mathf.Lerp(_startPosition.x, _destination.x, fractionOfJourney);
                float lerpY = _startPosition.y + (Mathf.Sin((float)Math.PI * fractionOfJourney) * maxHeight);
                float lerpZ = Mathf.Lerp(_startPosition.z, _destination.z, fractionOfJourney);

                if (_pickup != null)
                    _pickup.transform.position = new Vector3(lerpX, lerpY, lerpZ);
            }

            if (_pickup != null)
                _pickup.mCollider.enabled = true;
        }

        yield return null;
    }
    #endregion

    public void LerpObjectToPosition(GameObject _object, Vector3 _startPosition, Vector3 _destination, float _duration)
    {
        StartCoroutine(LerpObjectToPositionCoroutine(_object, _startPosition, _destination, _duration));
    }

    private IEnumerator LerpObjectToPositionCoroutine(GameObject _obj, Vector3 _startPosition, Vector3 _destination, float _duration)
    {
        Collider collider = _obj.GetComponent<Collider>();
        if (collider != null)
            collider.enabled = false;

        float maxHeight = 8;
        float timeElapsed = 0;
        if (_duration == 0)
            _duration = 1;
        while (timeElapsed < _duration && _obj != null)
        {
            timeElapsed += Time.deltaTime;
            yield return null;
            float fractionOfJourney = timeElapsed / _duration;

            float lerpX = Mathf.Lerp(_startPosition.x, _destination.x, fractionOfJourney);
            float lerpY = _startPosition.y + (Mathf.Sin((float)Math.PI * fractionOfJourney) * maxHeight);
            float lerpZ = Mathf.Lerp(_startPosition.z, _destination.z, fractionOfJourney);

            if (_obj != null)
                _obj.transform.position = new Vector3(lerpX, lerpY, lerpZ);
        }

        if (collider != null)
            collider.enabled = true;
    }
}
