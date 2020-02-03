using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    static SoundManager _instance;

    static Transform _cam;
    static Transform Cam { get { if (_cam == null) _cam = Camera.main.GetComponent<Transform>(); return _cam; } }


    public SoundArray[] Pairs;
    List<AudioSource> sources = new List<AudioSource>();

    IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(.3f, 2f));
            PlaySound(9, Camera.main.transform.position, .3f);
        }
    }


    public static void PlaySound(int soundIndex, Vector2 position, float volume = 1f, bool global = false)
    {
        AudioClip[] clips = _instance.Pairs[soundIndex].Clips;
        for (int i = 0; i < _instance.sources.Count; i++)
        {
            AudioSource sor = _instance.sources[i];
            if (!sor.isPlaying)
            {
                sor.transform.position = new Vector3(position.x, position.y, Cam.position.z);
                sor.clip = _instance.Pairs[soundIndex].Clips[Random.Range(0, clips.Length)];
                sor.volume = volume;
                sor.Play();
                return;
            }
        }
        AudioSource src = new GameObject(Random.Range(0,100) + " src").AddComponent<AudioSource>();
        src.transform.position = new Vector3(position.x, position.y, Cam.position.z);
        src.volume = volume;
        src.clip = _instance.Pairs[soundIndex].Clips[Random.Range(0, clips.Length)];
        _instance.sources.Add(src);
        src.Play();

    }

    private void Awake()
    {
        _instance = this;
    }
}

[System.Serializable]
public struct SoundArray
{
    public AudioClip[] Clips;
}