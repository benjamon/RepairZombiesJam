using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    static SoundManager _instance;

    static Transform _cam;
    static Transform Cam { get { if (_cam == null) _cam = Camera.main.GetComponent<Transform>(); return _cam; } }


    public SoundPair[] Pairs;
    AudioClip[] Clips;
    List<AudioSource> sources = new List<AudioSource>();


    public static void PlaySound(Zound sound, Vector2 position, float volume = 1f, bool global = false)
    {
        for (int i = 0; i < _instance.sources.Count; i++)
        {
            AudioSource sor = _instance.sources[i];
            if (!sor.isPlaying)
            {
                sor.transform.position = new Vector3(position.x, position.y, Cam.position.z);
                sor.clip = _instance.Clips[(int)sound];
                sor.volume = volume;
                sor.Play();
                return;
            }
        }
        AudioSource src = new GameObject(sound.ToString() + " src").AddComponent<AudioSource>();
        src.transform.position = new Vector3(position.x, position.y, Cam.position.z);
        src.volume = volume;
        src.clip = _instance.Clips[(int)sound];
        _instance.sources.Add(src);
        src.Play();

    }

    private void Awake()
    {
        _instance = this;
        Clips = new AudioClip[64];
        for (int i = 0; i < Pairs.Length; i++)
        {
            Clips[(int)Pairs[i].Sound] = Pairs[i].Clip;
        }
    }
}

[System.Serializable]
public struct SoundPair
{
    public Zound Sound;
    public AudioClip Clip;
}


public enum Zound
{
    CrowCaw,
    Hit1
}
