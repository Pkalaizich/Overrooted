using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEvents : MonoBehaviour
{
    private static AudioEvents instance;
    public static AudioEvents Instance { get => instance;}

    [SerializeField] private AudioSource audioSource;
    public AudioClip wrongSFX;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);       
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    public void Delayed(AudioClip clip)
    {
        StartCoroutine(playDelayedSound(clip));
    }

    private IEnumerator playDelayedSound(AudioClip clip)
    {
        yield return new WaitForSeconds(0.2f);
        AudioEvents.Instance.PlaySound(clip);
    }
}
