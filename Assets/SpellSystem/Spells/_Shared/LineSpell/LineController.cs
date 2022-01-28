using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineController : MonoBehaviour
{
    [SerializeField] private float mTimeToLive = 1;
    [HideInInspector] public GameObject mSource;
    [HideInInspector] public GameObject mDestination;
    [HideInInspector] public float mStaticDestinationYPos = 1.4f;
    [HideInInspector] public float mStaticYSourcePos = 0f;

    private LineRenderer mLineRender;

    private void Awake()
    {
        mLineRender = GetComponent<LineRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, mTimeToLive);
    }

    // Update is called once per frame
    void Update()
    {
        if (mSource == null || mDestination == null)
        {
            Destroy(gameObject);
        }
        else
        {
            Vector3 sourcePos = new Vector3(mSource.transform.position.x, mSource.transform.position.y + mStaticYSourcePos, mSource.transform.position.z);
            Vector3 destinationPos = new Vector3(mDestination.transform.position.x, mDestination.transform.position.y + mStaticDestinationYPos, mDestination.transform.position.z);
            mLineRender.SetPosition(0, sourcePos);
            mLineRender.SetPosition(1, destinationPos);
        }
    }
}
