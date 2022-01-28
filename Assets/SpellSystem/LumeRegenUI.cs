using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LumeRegenUI : MonoBehaviour
{
    [SerializeField]
    private int mLumeIndex = 0;
    [SerializeField]
    Image mFillImage = null;

    float mDuration = 0f;
    float mTimer = 0f;

    private void OnDisable()
    {
        mFillImage.fillAmount = 0;
    }

    public void Initialize(float _duration)
    {
        mDuration = _duration;
        mTimer = 1;
    }

    private void Update()
    {
        mTimer -= Time.deltaTime / mDuration;
        mFillImage.fillAmount = mTimer;
        if(mTimer <= 0)
        {
            SpellEvents.Instance.LumeRegened(mLumeIndex);
            enabled = false;
        }
    }

}
