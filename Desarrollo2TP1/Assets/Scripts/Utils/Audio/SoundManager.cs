using System;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.VisualScripting.Member;
using UnityEngine.Rendering;

public enum SFXType
{
    SHOOT,
    EXPLOSION,
    JUMP,
    FOOTSTEPS,
    CONFIRM,
    LEVEL_UP
}


[CreateAssetMenu(fileName = "SoundManager", menuName = "ScriptableObjects/SoundManger")]
public class SoundManager : ScriptableObject
{
    [SerializeField] private AudioClip[] soundClips;

    private void OnDestroy()
    {
        ServiceProvider.SetService<SoundManager>(null);
    }

    public void PlaySound(SFXType sound, AudioSource source, float volume = 1f)
    {
        source.PlayOneShot(soundClips[(int)sound], volume);
    }
    internal void PlaySound(AudioClip clip, AudioSource source)
    {
        source.PlayOneShot(clip);
    }
}
