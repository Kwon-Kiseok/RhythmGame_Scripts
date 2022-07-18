using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Networking;

public class MusicPanelController : MonoBehaviour
{
    public TextMeshProUGUI SongTitleText;
    public TextMeshProUGUI SongArtistText;
    public AudioSource audioSource;

    // Ŀ�� ������
    public GameObject coverPrefab;
    public List<GameObject> trackList = new List<GameObject>();
    public Transform coverParentTransform;

    // Ŀ�� �������� �޾ƿͼ� ���� ������ ����ִ� ��ŭ ��ũ�Ѻ� ������ �Ʒ��� �ڽİ�ü�� ������־�� ��
    // �� �� ���� Ȱ��ȭ �Ǿ��ִ� �������� ������ �ϴܿ� �ؽ�Ʈ�� ���, Ŭ�� �� �÷��� �Ǿ�� ��
    private List<string> tracks = new List<string>();
    private MusicData musicData;
    private Texture2D texture;

    private void Start()
    {
        LoadSongList();
    }

    public void LoadSongList()
    {
        var path = Application.persistentDataPath + "/";
        InitLoad(path, "song_list.json");
        var js = File.ReadAllText(path + "song_list.json");
        if (js == null)
        {
            Debug.Log("Input Error or Null data Load");
            return;
        }

        tracks = JsonConvert.DeserializeObject<SongListData>(js).songDatas;

        for(int i = 0; i < tracks.Count; i++)
        {
            var musicPath = Application.persistentDataPath + "/";
            InitLoad(musicPath, tracks[i]);
            var js_music = File.ReadAllText(musicPath + tracks[i]);

            musicData = JsonConvert.DeserializeObject<MusicData>(js_music);

            var trackObject = Instantiate(coverPrefab, coverParentTransform);
            trackObject.GetComponent<Cover>().data = musicData;
            trackObject.GetComponent<Cover>().image.sprite = LoadSprite(musicData.cover);
            trackList.Add(trackObject);
        }
    }

    private Sprite LoadSprite(string address)
    {
        //var bytes = System.IO.File.ReadAllBytes(path);

        //if (bytes.Length > 0)
        //{
        //    texture = new Texture2D(0, 0);
        //    texture.LoadImage(bytes);
        //}
        var texHandler = Addressables.LoadAssetAsync<Texture2D>(address);
        texHandler.WaitForCompletion();
        var tex = texHandler.Result;

        Addressables.Release(texHandler);

        Rect rect = new Rect(0, 0, tex.width, tex.height);
        return Sprite.Create(tex, rect, new Vector2(0.5f, 0.5f));
    }

    public void DemoSet(string address)
    {
        AsyncOperationHandle<AudioClip> clip = Addressables.LoadAssetAsync<AudioClip>(address);
        clip.Completed += AudioClipHandle_Complete;

        Addressables.Release(clip);
    }

    private void AudioClipHandle_Complete(AsyncOperationHandle<AudioClip> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            audioSource.clip = handle.Result;
            audioSource.Play();
        }
    }

    private void InitLoad(string path, string fileName)
    {
#if UNITY_EDITOR || UNITY_ANDROID
        if (!File.Exists(path + fileName))
        {
            var jsHandle = Addressables.LoadAssetAsync<TextAsset>(fileName);
            jsHandle.WaitForCompletion();
            JsonHandle_Complete(jsHandle, path + fileName);

            Addressables.Release(jsHandle);
        }
#endif
    }

    private void JsonHandle_Complete(AsyncOperationHandle<TextAsset> handle, string filePath)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            var jsAsset = handle.Result;
            File.WriteAllText(filePath, jsAsset.text);
            Debug.Log("Complete : " + filePath);
        }
    }
}
