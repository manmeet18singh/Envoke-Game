using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    [SerializeField]
    float mSpeed = 0f;

    private void FixedUpdate()
    {
        transform.Rotate(transform.up * mSpeed * Time.fixedDeltaTime);
    }
}
