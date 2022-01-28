using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthUI : MonoBehaviour
{
    [SerializeField]
    Image mForeground = null;
    [SerializeField]
    Image mBackground = null;
    [SerializeField]
    BossHealthData mHealthData = null;

    int mStage = 0;

    private void Awake()
    {
        mForeground.color = mHealthData.mHealthBarColors[mStage];
        mBackground.color = mHealthData.mHealthBarColors[mStage + 1];
    }

    public void UpdateHealth(int _currentHealth)
    {
        mForeground.fillAmount = (float) _currentHealth / (float)mHealthData.mHealthThresholds[mStage];
    }

    public void StageChange(int _stage)
    {
        mStage = _stage;
        StartCoroutine(BossStageChange());
    }

    public IEnumerator BossStageChange()
    {
        float timer = 0f;
        mForeground.color = mHealthData.mHealthBarColors[mStage];
        mBackground.color = mHealthData.mHealthBarColors[mStage + 1];

        while(timer <= 1)
        {
            timer += Time.deltaTime;
            yield return null;
            mForeground.fillAmount = Mathf.Lerp(0f, 1f, timer);
        }
       
    }
}
