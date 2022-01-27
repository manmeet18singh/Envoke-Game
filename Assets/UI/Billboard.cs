using UnityEngine;

public class Billboard : MonoBehaviour
{
    Transform mCam;

    private void Start()
    {
        mCam = GameManager.Instance.mMainCam.transform;
            //GameObject.FindGameObjectWithTag("PromoCam").transform;
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + mCam.forward);
    }
}
