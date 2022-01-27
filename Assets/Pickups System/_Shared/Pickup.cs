using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    // Data pertaining to the type of pickup loaded
    [SerializeField]
    public PickupData mPickupData;
    
    // Reference to the model's collider is needed to turn off during lerp animation
    [HideInInspector]
    public Collider mCollider;

    // Reference to the pickup model that is dynamically loaded
    [HideInInspector]
    public GameObject mModel;

    /// <summary>
    /// If a pickup is manually placed into the scene before runtime,
    /// this will ensure the pickup is properly loaded in.
    /// </summary>
    private void Start()
    {
        if (mPickupData != null)
            LoadPickup(mPickupData);
    }

    /// <summary>
    /// Dynamically loads a pickup by instantiating a new model
    /// and attaching it to this game object. The type of pickup
    /// (including what model to load) is determined via the PickupData
    /// passed.
    /// </summary>
    /// <param name="_pickupData">Type of pickup to load.</param>
    public virtual void LoadPickup(PickupData _pickupData)
    {
        // Remove children objects i.e. visuals
        foreach (Transform child in transform)
        {
#if UNITY_EDITOR
            DestroyImmediate(child.gameObject);
#else
            Destroy(child.gameObject);
#endif     
        }

        mPickupData = _pickupData;

        if (mPickupData == null)
            return;

        // Load current pickup visuals
        mModel = Instantiate(_pickupData.Model);
        mModel.transform.SetParent(this.transform);
        mModel.transform.localPosition = Vector3.zero;
        mModel.transform.rotation = Quaternion.identity;
        PickupCollision collisionScript = mModel.AddComponent<PickupCollision>();
        collisionScript.mPickup = this;

        // Find collider on pickup model
        mCollider = mModel.GetComponent<Collider>();

        if (mCollider == null)
            Debug.LogError("No collider found on parent!");
    }

    /// <summary>
    /// Manually destroys the loaded model (if any) and the parent 
    /// game object.
    /// </summary>
    public void CleanUp()
    {
        if (mModel != null)
            Destroy(mModel);
        Destroy(this);
    }

    #region Debug
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 0.5f);
    }
    #endregion
}
