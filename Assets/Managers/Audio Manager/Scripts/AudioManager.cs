using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] mSounds;
    [SerializeField]
    private AudioMixer mAudioMixer = null;
    [SerializeField]
    private string[] mAudioMixerNames = null;

    public static AudioManager instance;

    public static string MainSong = "MainBG";

    private string mCurrentBackgroundTrack = MainSong;

    private AudioMixerGroup mGameMixer = null;
    private AudioMixerGroup mMenuMixer = null;

    public static void PlayRandomSFX(string[] _sfxList)
    {
        if (_sfxList.Length == 0)
        {
            //Debug.Log("No SFX");
            return;
        }
        else
        {
            //Debug.Log("Playing " + _sfxList[UnityEngine.Random.Range(0, _sfxList.Length)]);
            instance.Play(_sfxList[UnityEngine.Random.Range(0, _sfxList.Length)]);
        }
    }

    private void Awake()
    {

        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        float volume = 0f;
        for(int i = 0; i < mAudioMixerNames.Length; ++i)
        {
            volume = PlayerPrefs.GetFloat(mAudioMixerNames[i], 1f);
            mAudioMixer.SetFloat(mAudioMixerNames[i], Mathf.Log10(volume) * 20);
        }

        foreach (Sound s in mSounds)
        {
            s.Source = gameObject.AddComponent<AudioSource>();
            s.Source.clip = s.Clip;
            s.Source.outputAudioMixerGroup = s.Mixer;

            s.Source.playOnAwake = false;
            s.Source.ignoreListenerPause = s.IgnoreAudioListener;
            s.Source.loop = s.Loop;
            s.Source.volume = s.volume;
            s.Source.pitch = s.pitch;

            if (s.Name.CompareTo(MainSong) == 0)
            {
                Debug.Log("Found soundtrack!");
                s.Source.Play();
                mMenuMixer = s.Mixer;
                Debug.Log("Menu Mixer: " + mMenuMixer.name);
            }
            else if (mGameMixer == null && s.Source.outputAudioMixerGroup != null && s.Source.outputAudioMixerGroup.name.CompareTo("GameMusic") == 0)
            {
                mGameMixer = s.Mixer;
                Debug.Log("game Mixer: " + mGameMixer.name);
            }
        }

        DontDestroyOnLoad(gameObject);
    }

   public void Play(string name, bool playContinuously = false)
    {
        Sound s = Array.Find(mSounds, sound => sound.Name == name);
        if (s == null)
        {
            Debug.Log("Could not find " + name);
            return;
        }

        if (playContinuously)
            s.Source.Play();
        else
            s.Source.PlayOneShot(s.Clip);
    }

    public bool IsThisPlaying(string name)
    {
        Sound s = Array.Find(mSounds, sound => sound.Name == name);

        if (s.Source.isPlaying)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(mSounds, sound => sound.Name == name);
        if (s != null && s.Source.isPlaying)
            s.Source.Stop();
    }

    public static void MainMenuToGame()
    {
        Sound s = Array.Find(AudioManager.instance.mSounds, sound => AudioManager.MainSong == sound.Name);
        if (s == null)
            return;

        Debug.Log("Changing mixer for music to Game Mixer.");

        s.Mixer = AudioManager.instance.mGameMixer;
        s.Source.outputAudioMixerGroup = AudioManager.instance.mGameMixer;
    }

    public static void GameToMainMenu()
    {
        Sound s = Array.Find(AudioManager.instance.mSounds, sound => AudioManager.MainSong == sound.Name);
        if (s == null)
            return;

        Debug.Log("Changing mixer for music to Game Mixer.");

        s.Mixer = AudioManager.instance.mMenuMixer;
        s.Source.outputAudioMixerGroup = AudioManager.instance.mMenuMixer;
    }

    public static void ChangeBackgroundMusic(string trackName)
    {
        if (AudioManager.instance.mCurrentBackgroundTrack.CompareTo(trackName) == 0)
            return;

        AudioManager.instance.Stop(AudioManager.instance.mCurrentBackgroundTrack);
        AudioManager.instance.Play(trackName, true);
        AudioManager.instance.mCurrentBackgroundTrack = trackName;
    }
}
