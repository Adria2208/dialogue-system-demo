using UnityEngine;
using System;
public enum SoundType
{
    TAP
}
[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private SoundList[] soundList;
    private static SoundManager instance;
    private AudioSource audioSource;
    private AudioSource lastPlayed;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(SoundType sound, int number, bool cancelOnNextSound, float volume = 1)
    {
        if (instance.lastPlayed != null && instance.lastPlayed.isPlaying)
        {
            instance.lastPlayed.Stop();
        }

        AudioClip audioClip = GetAudioClip(sound, number);
        instance.audioSource.PlayOneShot(audioClip, volume);

        if (cancelOnNextSound)
        {
            instance.lastPlayed = instance.audioSource;
        }
    }

    public static AudioClip GetAudioClip(SoundType sound, int number)
    {
        AudioClip[] clips = instance.soundList[(int)sound].Sounds;
        return clips[number];
    }

    public static void PlayRandomSound(SoundType sound, float volume = 1)
    {
        AudioClip[] clips = instance.soundList[(int)sound].Sounds;
        AudioClip audioClip = clips[UnityEngine.Random.Range(0, clips.Length)];
        instance.audioSource.PlayOneShot(audioClip, volume);
    }

#if UNITY_EDITOR
    private void OnEnable()
    {
        string[] names = Enum.GetNames(typeof(SoundType));
        Array.Resize(ref soundList, names.Length);
        for (int i = 0; i < soundList.Length; i++)
        {
            soundList[i].name = names[i];
        }
    }

}
#endif

[Serializable]
public struct SoundList
{
    public AudioClip[] Sounds { get => sounds; }
    [HideInInspector] public string name;
    [SerializeField] private AudioClip[] sounds;
}