using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem.OnScreen;
using System.Text;
using System.Collections;

public class SpellSystemUIManager : MonoBehaviour
{

    [SerializeField]
    private Sprite[] mLumeSprites = null;
    [SerializeField]
    private Image[] mQueuedSpellSlotImages = null;
    [SerializeField]
    private TextMeshProUGUI[] mLumeKeybindTMPs = null;
    [SerializeField]
    private GameObject[] mQueuedLumes = null;
    [SerializeField]
    private Image[] mQueuedLumeImages = null;
    [SerializeField]
    private TextMeshProUGUI[] mLumeCapacityTMPs = null;
    [SerializeField]
    private GameObject[] mLumeLockedImages = null;
    [SerializeField]
    private LumeRegenUI[] mLumeRegenUIs = null;

    private Sprite mEmptyQueueSlotSprite;
    private StringBuilder strBuilder = new StringBuilder(10);

    private void Awake()
    {

        mLumeKeybindTMPs[0].text = InputManager.controls.Player.QueueEdur.bindings[0].ToDisplayString().Remove(0, 6);
        mLumeKeybindTMPs[1].text = InputManager.controls.Player.QueueCinos.bindings[0].ToDisplayString().Remove(0, 6);
        mLumeKeybindTMPs[2].text = InputManager.controls.Player.QueueSoleis.bindings[0].ToDisplayString().Remove(0, 6);

        mEmptyQueueSlotSprite = mQueuedSpellSlotImages[0].sprite;
        for (int i = 0; i < mQueuedLumes.Length; ++i)
        {
            mQueuedLumes[i].SetActive(false);
        }

        SpellEvents.Instance.mClearedLumesCallback += ClearLumes;
        SpellEvents.Instance.mQueuedLumeCallback += QueueLume;
        SpellEvents.Instance.mQueuedSpellCallback += QueueSpell;
        SpellEvents.Instance.mSpellCastCallback += ClearLumes;
        SpellEvents.Instance.mLumeAmountChangedCallback += UpdateCapacityText;
        SpellEvents.Instance.mLumeRegenStartedCallback += StartLumeRegen;
        SpellEvents.Instance.mLumeRegenStoppedCallback += StopLumeRegen;
        SpellEvents.Instance.mLockLumeCallback += LockLume;
        SpellEvents.Instance.mUnlockLumeCallback += UnlockLume;
    }

    private void OnDestroy()
    {
        SpellEvents.Instance.mClearedLumesCallback -= ClearLumes;
        SpellEvents.Instance.mQueuedLumeCallback -= QueueLume;
        SpellEvents.Instance.mQueuedSpellCallback -= QueueSpell;
        SpellEvents.Instance.mSpellCastCallback -= ClearLumes;
        SpellEvents.Instance.mLumeAmountChangedCallback -= UpdateCapacityText;
        SpellEvents.Instance.mLumeRegenStartedCallback -= StartLumeRegen;
        SpellEvents.Instance.mLumeRegenStoppedCallback -= StopLumeRegen;
        SpellEvents.Instance.mLockLumeCallback -= LockLume;
        SpellEvents.Instance.mUnlockLumeCallback -= UnlockLume;
    }

    private void LockLume(int _lume)
    {
        mLumeCapacityTMPs[_lume].enabled = false;
        mLumeLockedImages[_lume].SetActive(true);
    }

    private void UnlockLume(int _lume)
    {
        mLumeLockedImages[_lume].SetActive(false);
        mLumeCapacityTMPs[_lume].enabled = true;
    }

    private void ClearLumes()
    {
        SpellCast();
        for(int i = 0; i < mQueuedLumes.Length; ++i)
        {
            mQueuedLumes[i].SetActive(false);
        }
    }

    private void QueueLume(int _index, int _lumeQueued)
    {
        mQueuedLumes[_index].SetActive(true);
        mQueuedLumeImages[_index].sprite = mLumeSprites[_lumeQueued];
        AudioManager.instance.Play("Lumes Queued");
    }

    private void QueueSpell(Sprite _spellSprite)
    {
        mQueuedSpellSlotImages[0].sprite = _spellSprite;
    }

    private void SpellCast()
    {
        mQueuedSpellSlotImages[0].sprite = mEmptyQueueSlotSprite;
    }

    private void UpdateCapacityText(int _index, int _currentAmount, int _maxCap)
    {
        strBuilder.Append(_currentAmount);
        strBuilder.Append(" / ");
        strBuilder.Append(_maxCap);
        mLumeCapacityTMPs[_index].text = strBuilder.ToString();
        strBuilder.Clear();
    }

    private void StartLumeRegen(int _lumeIndex, float _duration)
    {
        mLumeRegenUIs[_lumeIndex].Initialize(_duration);
        mLumeRegenUIs[_lumeIndex].enabled = true;
    }


    private void StopLumeRegen(int _lumeIndex)
    {
        mLumeRegenUIs[_lumeIndex].enabled = false;
    }

}
