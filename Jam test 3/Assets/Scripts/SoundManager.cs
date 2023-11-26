using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SoundType { BACKGROUND_MUSIC, TRASH, ENERGY, ROOMBA_TURNON, ROOMBA_MOVEMENT, ROOMBA_TURNOFF, ROTATE_ROOM}
[System.Serializable]
public struct SoundSet {
    public SoundType type;
    public AudioClip audioFile;
    public int volume;
}

public class SoundManager : MonoBehaviour
{
    [SerializeField] SoundType soundType;
    [SerializeField] SoundSet[] soundClips;
    AudioSource soundPlayer;

    List<SoundSet> selectedSounds;


    void Start()
    {
        soundPlayer = gameObject.AddComponent<AudioSource>();
        soundPlayer.playOnAwake = false;
        selectedSounds = new List<SoundSet>();
        foreach (SoundSet soundSet in soundClips) 
        { 
            if (soundType == soundSet.type)
                selectedSounds.Add(soundSet); 
        }

        if (soundType == SoundType.BACKGROUND_MUSIC)
        {
            DontDestroyOnLoad(this.gameObject);
            soundPlayer.loop = true;
            soundPlayer.volume = selectedSounds[0].volume / 100f;
            soundPlayer.clip = selectedSounds[0].audioFile;
            soundPlayer.Play(); 
        }
    }

    public void PlayVariation()
    {
        int randomIndex = Random.RandomRange(0, selectedSounds.Count);
        soundPlayer.volume = selectedSounds[randomIndex].volume / 100f;
        soundPlayer.clip = selectedSounds[randomIndex].audioFile;
        soundPlayer.Play();
    }

    public void PlayTurnOnSound()
    {
        SoundSet turnOnSound = new SoundSet();
        foreach (SoundSet soundSet in soundClips)
        {
            if (soundSet.type == SoundType.ROOMBA_TURNON) {
                turnOnSound = soundSet;
                break;
            }
        }
        soundPlayer.volume = turnOnSound.volume / 100f;
        soundPlayer.clip = turnOnSound.audioFile;
        soundPlayer.Play();
    }

    public void PlayTurnOffSound()
    {
        SoundSet turnOffSound = new SoundSet();

        foreach (SoundSet soundSet in soundClips)
        {
            if (soundSet.type == SoundType.ROOMBA_TURNOFF)
            {
                turnOffSound = soundSet;
                break;
            }
        }
        soundPlayer.volume = turnOffSound.volume / 100f;
        soundPlayer.clip = turnOffSound.audioFile;
        soundPlayer.Play();
    }
}
