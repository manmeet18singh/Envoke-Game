using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownRoomMusicManager : MonoBehaviour
{
    [Header("Music Properties")]
    [SerializeField] string mCooldownSong = "CooldownBG";

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Changing background music to CooldownBG");
        AudioManager.ChangeBackgroundMusic(mCooldownSong);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
