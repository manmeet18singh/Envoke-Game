using System.Collections;
using UnityEngine;

public class IceTorrentController : MonoBehaviour
{
    [SerializeField] private float mDuration = 4;
    [SerializeField] private float mDestroyDuration = 1.5f;

    private Coroutine mCoroutine;
    private Coroutine mDestroyCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        mCoroutine = StartCoroutine(DestroyTorrentAfterSeconds());
    }

    public void DestroyTorrent()
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

            // TODO - SCALE DOWN EFFECTS
        }

        Destroy(gameObject);
    }

    private IEnumerator DestroyTorrentAfterSeconds()
    {
        yield return new WaitForSeconds(mDuration);
        DestroyTorrent();
    }
}
