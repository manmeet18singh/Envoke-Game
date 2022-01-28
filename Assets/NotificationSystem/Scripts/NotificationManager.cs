using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum NotificationColors
{
    Negative = 0,
    Positive = 1,
}

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance;

    [SerializeField]
    TextMeshProUGUI mNotificationTMP = null;
    [SerializeField]
    Animator mAnim = null;
    #region NameArray
#if UNITY_EDITOR
    [NamedArray(new string[] { "Negative", "Positive" })]
#endif
    #endregion
    [SerializeField]
    Color[] mColors = null;
    int mAnimationHash = -1;
    [SerializeField] private string[] mSfx = null;

    LinkedList<NotificationSettings> mMessages = new LinkedList<NotificationSettings>();
    bool mDisplayingMessages = false;


    private void Awake()
    {
        Instance = this;
        mAnimationHash = Animator.StringToHash("DisplayNotification");
    }

    public void AddNotification(string _message, float _animationSpeed = 1, NotificationColors _color = NotificationColors.Negative)
    {
        NotificationSettings settings = new NotificationSettings(_message, _color, _animationSpeed);

        if (!mMessages.Contains(settings))
        {
            mMessages.AddLast(settings);

            if (!mDisplayingMessages)
            {
                mAnim.enabled = true;
                mDisplayingMessages = true;
                DisplayNotification();
            }
        }
    }

    public void DisplayNotification()
    {
        if (mMessages.First == null)
        {
            mAnim.enabled = false;
            mDisplayingMessages = false;
            return;
        }

        NotificationSettings settings = mMessages.First.Value;

        mMessages.RemoveFirst();
        mAnim.speed = 1 * (1 / settings.animationSpeed);
        mNotificationTMP.text = settings.message;
        mNotificationTMP.color = mColors[(int)settings.color];
        AudioManager.instance.Play(mSfx[(int)settings.color]);
    }

    struct NotificationSettings
    {
        public string message;
        public NotificationColors color;
        public float animationSpeed;

        public NotificationSettings(string _message, NotificationColors _color, float _speed)
        {
            message = _message;
            color = _color;
            animationSpeed = _speed;
        }
    }
}
