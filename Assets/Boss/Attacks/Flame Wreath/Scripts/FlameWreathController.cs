using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class FlameWreathController : MonoBehaviour
{
    [SerializeField] private Transform mBrightFlames = null;
    [SerializeField] private Transform mLight = null;
    [SerializeField] private Transform mVFXFlames = null;
    [SerializeField] private Collider mCollider = null;

    [SerializeField] private float mDuration = 4;
    [SerializeField] private float mDestroyDuration = 1.5f;

    private Coroutine mCoroutine;
    private Coroutine mDestroyCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        mCoroutine = StartCoroutine(DestroyFlamesAfterSeconds());
    }

    public void DestroyFlames()
    {
        if (mDestroyCoroutine == null)
        {
            mDestroyCoroutine = StartCoroutine(StartDestroyAnimation());
        }
    }

    private IEnumerator StartDestroyAnimation()
    {
        
        float currentDuration = 0;
        Vector3 scale = gameObject.transform.localScale;
        
        while (currentDuration < mDestroyDuration)
        {
            yield return null;
            currentDuration += Time.deltaTime;
            float multiplier = (mDestroyDuration - currentDuration) / mDestroyDuration;
            scale *= Mathf.Max(multiplier, 0);

            mCollider.enabled = scale.x > 0.35;
            mVFXFlames.localScale = scale;
            mBrightFlames.localScale = scale;
            mLight.localScale = scale;
        }

        Destroy(gameObject);
    }

    private IEnumerator DestroyFlamesAfterSeconds()
    {
        yield return new WaitForSeconds(mDuration);
        DestroyFlames();
    }
}
