using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public MusicData currentData;

    public int currentScore;

    public bool GameStart { get; set; } = false;
    public bool GameStop { get; set; } = false;

    public bool GameClear { get; set; } = false;
    public bool GameFail { get; set; } = false;

    private void Awake()
    {
        var obj = FindObjectsOfType<GameManager>();
        if(obj.Length == 1)
        {
            DontDestroyOnLoad(gameObject);  
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        instance = this;
    }

    private void Update()
    {
        if (MusicManager.instance != null && SceneManager.GetActiveScene().name != "EditorScene")
        {
            if (GameStop && Time.timeScale == 1)
            {
                Time.timeScale = 0;
                MusicManager.instance.AudioPause();
                return;
            }
            else if (!GameStop && Time.timeScale == 0)
            {
                Time.timeScale = 1;
                MusicManager.instance.AudioPlay();
                return;
            }

            if (GameStart && !MusicManager.instance.MusicStart)
            {
                MusicManager.instance.AudioPlay();
            }

            if (!GameStart && !GameStop)
            {
                MusicManager.instance.AudioStop();
            }
        }

        // 결과창 디버그용
        if(SceneManager.GetActiveScene().name == "GamePlayScene" && Input.GetKeyDown(KeyCode.F12))
        {
            GameClear = true;
            SceneLoader.LoadScene("ResultScene");
        }
    }
}
