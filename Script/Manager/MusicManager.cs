using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using System.IO;
using TMPro;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public GameObject secondLineGO;
    public Slider audioProgressBar;
    public Transform slAppearTr;
    public TextMeshProUGUI currentTimeText;
    public AudioSource audioSource;
    public static MusicManager instance;
    
    public int musicBPM = 0;
    public float SecPerBeat { get; set; } = 0f;   // sec per beat
    public float offset = 0f;
    public int tempo = 4;   // tempo
    public float dspTimeOfSong = 0f;
    public int frequency = 44100;

    public float BarPerSec { get; set; } = 0f; // 
    public int BarPerSample { get; set; } = 0; // 1마디

    public float BeatPerSec { get; set; } = 0f;
    public int BeatPerSample { get; set; } = 0; // 1박자

    public float BeatPerSec32rd { get; set; } = 0f;
    public int BeatPerTimeSample32rd { get; set; } = 0;

    private int currMin;
    private int currSec;
    public int MusicMin { get; private set; } = 0;
    public int MusicSec { get; private set; } = 0;

    public int musicTotalSec = 0;
    public float musicCurrSec = 0f;

    public bool MusicStart { get; set; } = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        instance = this;
        if (GameManager.instance.currentData != null)
        {
            MusicSet(GameManager.instance.currentData);
            dspTimeOfSong = (float)AudioSettings.dspTime;

            Debug.Log("MusicSet After");
            // clip이 null이 들어오는데,,, MusicSet에서 clip에 들어가는게 끝나기전에 들어옴?
            if (audioSource.clip != null)
            {
                audioProgressBar.minValue = 0;
                audioProgressBar.maxValue = audioSource.clip.length;
            }
        }
    }

    private void Update()
    {
        // 끝나고 총 길이와의 약간의 오차가 있음(int, float라 그럴수도)
        if (SceneManager.GetActiveScene().name == "GamePlayScene" && audioSource.isPlaying)
        {
            TimeProgress();
        }
    }

    // 곡 선택 시 데모 재생
    public void DemoSet(MusicData data)
    {
        musicBPM = data.BPM;

        AsyncOperationHandle<AudioClip> clip = Addressables.LoadAssetAsync<AudioClip>(data.demoPath);
        clip.Completed += AudioClipHandle_Complete;
    }

    public void MusicSet(MusicData data)
    {
        musicTotalSec = data.musicTime;
        musicBPM = data.BPM;

        SecPerBeat = 60f / musicBPM;

        BarPerSec = 240f / musicBPM;
        BarPerSample = (int)BarPerSec * frequency;

        BeatPerSec = 60f / musicBPM;
        BeatPerSample = (int)BeatPerSec * frequency;

        BeatPerSec32rd = BeatPerSec / 8f;
        BeatPerTimeSample32rd = (int)BeatPerSec32rd * frequency;

        offset = SecPerBeat;

        Debug.Log("MusicSet Before");
        AsyncOperationHandle<AudioClip> clip = Addressables.LoadAssetAsync<AudioClip>(data.musicPath);
        clip.Completed += AudioClipHandle_Complete;

    }

    private void AudioClipHandle_Complete(AsyncOperationHandle<AudioClip> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            audioSource.clip = handle.Result;
            Debug.Log("Complete After");

            Init();
        }
    }

    private void TimeProgress()
    {
        int currTime = (int)audioSource.time;

        audioProgressBar.value = currTime / audioSource.clip.length;

        if(currTime != 0)
        {
            currMin = currTime / 60;
            currSec = currTime - currMin * 60;
        }

        currentTimeText.text = String.Format("{0:D2}:{1:D2} / {2:D2}:{3:D2}", currMin, currSec, MusicMin, MusicSec);
    }

    private void Init()
    {
        MusicMin = (int)audioSource.clip.length / 60;
        MusicSec = (int)audioSource.clip.length - MusicMin * 60;
    }

    public void AudioPlay()
    {
        audioSource.Play();
        MusicStart = true;
    }

    public void AudioPause()
    {
        audioSource.Pause();
    }
    public void AudioStop()
    {
        audioSource.Stop();
        MusicStart = false;
    }
}
