using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SyncMetronome : MonoBehaviour
{
	private AudioSource audioSource;
	public AudioClip clip;

    private AudioSource musicAudioSource;
    public GameObject testGloryDay;
    public GameObject testSquare;
    private bool isSet = false;

	public float musicBPM = 162f;
    [SerializeField]
	private float SPB = 60f;

    float offsetForSample = 0f;
    float nextSample = 0f;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = clip;
        
        musicAudioSource = testGloryDay.GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (musicAudioSource.clip != null)
        {
            if (musicAudioSource.isPlaying && !isSet)
            {
                SetBPM();
            }

            if (musicAudioSource.timeSamples >= nextSample)
            {
                StartCoroutine(PlayTick());
            }
        }
    }

    private IEnumerator PlayTick()
    {
        audioSource.PlayOneShot(clip);
        testSquare.GetComponent<RotateTest>().Rotate();

        nextSample += SPB * musicAudioSource.clip.frequency;

        yield return null;
    }

    private void SetBPM()
    {
        SPB /= musicBPM;
        offsetForSample = musicAudioSource.clip.frequency;
        nextSample = SPB * musicAudioSource.clip.frequency - offsetForSample;
        isSet = true;
    }
}
