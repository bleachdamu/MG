using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioData
{
    public AudioClip audioClip;
    public int id;
    public string name;
}

public class AudioHandler : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private List<AudioData> audioClips;

    public static AudioHandler Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void PlayOneShot(int id)
    {
        audioSource.PlayOneShot(audioClips.Find(x => x.id == id).audioClip);
    }
}
