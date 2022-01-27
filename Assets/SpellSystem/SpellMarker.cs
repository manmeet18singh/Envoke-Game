using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellMarker : MonoBehaviour
{
    [SerializeField]
    LayerMask mIndicatorLayerMask = 0;
    [SerializeField]
    GameObject[] mSpellMarker = null;

     RaycastHit mHit;
    int mCurMarker = 0;


    public void EnableMarker()
    {
        mCurMarker = 1;
        mSpellMarker[1].SetActive(true);
        enabled = true;
    }

    public void DisableMarker()
    {
        enabled = false;
        mSpellMarker[1].SetActive(false);
        mSpellMarker[0].SetActive(false);
    }

    public void UpdateMarker(int _valid)
    {
        mSpellMarker[mCurMarker].SetActive(false);
        mSpellMarker[_valid].SetActive(true);
        mCurMarker = _valid;
    }

    private void Update()
    {
        if (Physics.Raycast(CursorManager.Instance.ScreenToRay, out mHit, Mathf.Infinity, mIndicatorLayerMask))
        {
            mSpellMarker[mCurMarker].transform.position = new Vector3(mHit.point.x, mHit.point.y + .25f, mHit.point.z);
        }
    }
}
