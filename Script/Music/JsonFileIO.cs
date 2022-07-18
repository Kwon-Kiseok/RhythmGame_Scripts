using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class JsonFileIO : MonoBehaviour
{
    public MusicData musicData;
    public GameObject testSprite;
    public TextMeshProUGUI musicNameText;
    public TextMeshProUGUI artistText;
    public TextMeshProUGUI difficultText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI bpmText;
    public TextMeshProUGUI musicTimeText;
    public TextMeshProUGUI noteCountText;

    private Image img;
    private Texture2D texture;

    private void Awake()
    {
        img = testSprite.GetComponent<Image>();
    }

    private void InitLoad(string path, string fileName)
    {
#if UNITY_EDITOR || UNITY_ANDROID
        if (!File.Exists(path + fileName))
        {
            var jsHandle = Addressables.LoadAssetAsync<TextAsset>(fileName);
            jsHandle.WaitForCompletion();
            JsonHandle_Complete(jsHandle, path);

            Addressables.Release(jsHandle);
        }
#endif
    }

    private void JsonHandle_Complete(AsyncOperationHandle<TextAsset> handle ,string path)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            var jsAsset = handle.Result;
            File.WriteAllText(path, jsAsset.text);
        }
    }


    public void OnClickTestButton()
    {
        //musicData = new MusicData(
        //    EditManager.instance.searchText.text,
        //    "DJMAX",
        //    MusicFilePath + String.Format($"{EditManager.instance.searchText.text}_music.wav"),
        //    MusicFilePath + String.Format($"{EditManager.instance.searchText.text}_demo.wav"),
        //    CoverFilePath + String.Format($"{EditManager.instance.searchText.text}_cover.png"),
        //    DataEnumManager.Difficult.EASY,
        //    4,
        //    175,
        //    127,
        //    2,
        //    new List<NoteData>()
        //    {
        //        new NoteData("NOTE1", DataEnumManager.NoteType.SHORT, DataEnumManager.NoteLine.AIR, 5, 0),
        //        new NoteData("NOTE2", DataEnumManager.NoteType.SHORT, DataEnumManager.NoteLine.ROAD, 10, 0)
        //    });

        //ShowData(musicData);
    }

    private Sprite LoadSprite(string address)
    {
        var texHandler = Addressables.LoadAssetAsync<Texture2D>(address);
        texHandler.WaitForCompletion();
        var tex = texHandler.Result;
        //var bytes = System.IO.File.ReadAllBytes(path);
        //if (bytes.Length > 0)
        //{
        //    texture = new Texture2D(0, 0);
        //    texture.LoadImage(bytes);
        //}

        Addressables.Release(texHandler);
        
        Rect rect = new Rect(0, 0, tex.width, tex.height);
        return Sprite.Create(tex, rect, new Vector2(0.5f, 0.5f));
    }

    private void ShowData(MusicData data)
    {
        musicNameText.text = "����: " + data.musicName;
        artistText.text = "��Ƽ��Ʈ: " + data.artist;
        img.sprite = LoadSprite(data.cover);
        difficultText.text = "���̵�: " + data.difficult.ToString();
        levelText.text = "����: " + data.level.ToString();
        bpmText.text = "BPM: " + data.BPM.ToString();
        TimeSpan ts = TimeSpan.FromSeconds(data.musicTime);
        musicTimeText.text = "�������: " + String.Format("{0:D2}:{1:D2}", ts.Minutes, ts.Seconds);
        noteCountText.text = "��Ʈ ��: " + data.noteCount.ToString();
    }

    public void OnClickSave()
    {
        var json = new JObject();
        var jArray = new JArray();
        
        SaveBeatsData();

        json.Add("musicName", musicData.musicName);
        json.Add("musicPath", musicData.musicPath);
        json.Add("demoPath", musicData.demoPath);
        json.Add("cover", musicData.cover);
        json.Add("artist", musicData.artist);
        json.Add("difficult", musicData.difficult.ToString());
        json.Add("level", musicData.level);
        json.Add("BPM", musicData.BPM);
        json.Add("musicTime", musicData.musicTime);
        json.Add("noteCount", musicData.notes.Count);


        for (int i = 0; i < musicData.notes.Count; ++i)
        {
            var jobj_note = new JObject();
            jobj_note.Add("ID", "NOTE"+(i+1));
            jobj_note.Add("type", musicData.notes[i].type.ToString());
            jobj_note.Add("line", musicData.notes[i].line.ToString());
            jobj_note.Add("startTime", musicData.notes[i].startTime);
            jobj_note.Add("endTime", musicData.notes[i].endTime);
            jArray.Add(jobj_note);
        }

        json.Add("notes", jArray);

        string js = JsonConvert.SerializeObject(json, Formatting.Indented);
        string filename = musicData.musicName + ".json";
        string path = Path.Combine(Application.persistentDataPath, filename);
        Debug.Log(path);
        File.WriteAllText(path, js);
    }

    public void OnClickLoad()
    {
        var fileName = EditManager.instance.searchText.text + ".json";
        var path = Application.persistentDataPath + "/" + fileName;
        InitLoad(path, fileName);
        var js = File.ReadAllText(path);
        if(js == null)
        {
            Debug.Log("Input Error or Null data Load");
            return;
        }
        
        musicData = JsonConvert.DeserializeObject<MusicData>(js);

        ShowData(musicData);

        MusicManager.instance.MusicSet(musicData);
        GameManager.instance.currentData = musicData;
        GridMaker.instance.Init();
        // musicData ������� ������ �ִ� �����͸� �ҷ��;� ��
    }

    public void OnClickTestPlay()
    {
        GameManager.instance.currentData = musicData;
        SceneManager.LoadScene("GamePlayScene");
    }

    public void SaveBeatsData()
    {
        musicData.notes.Clear();

        // �׸��忡 �߰��� �� ����->�ε� ������ �����ؼ�,,,
        // ���� ���� �� ~ �׸��� ���� ������ ������ ��
        for (int i = 0; i < GridMaker.instance.grids.Count; ++i)
        {
            var beats = GridMaker.instance.grids[i].GetComponent<Bar>().beats;
            for (int j = 0; j < beats.Count; ++j)
            {
                if (beats[j].GetComponent<EditNote>().isSetNote)
                {
                    musicData.notes.Add(beats[j].GetComponent<EditNote>().data);
                }
            }
        }
        musicData.notes = musicData.notes.OrderBy(x => x.startTime).ToList();
    }

    public void LoadBeatsData()
    {
        for(int i = 0; i < musicData.noteCount; ++i)
        {
            GridMaker.instance.FindLoadData(musicData.notes[i]);
        }
    }

    public void ReturnToSelect()
    {
        SceneManager.LoadScene("SelectScene");
    }
}
