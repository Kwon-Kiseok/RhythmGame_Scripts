using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using DataEnumManager;

public class SongListData
{
    public List<string> songDatas;
}

public class MusicData
{
    public string musicName = "Untitled";
    public string artist = "Unnamed";
    public string musicPath = "musicName_music.wav";
    public string demoPath = "musicName_demo.wav";
    public string cover = "musicName_Cover";
    [JsonConverter(typeof(StringEnumConverter))]
    public DataEnumManager.Difficult difficult = DataEnumManager.Difficult.EASY;
    public int level = 1;
    public int BPM = 60;
    public int musicTime = 0;
    public int noteCount = 0;

    public List<NoteData> notes;

    public MusicData()
    {
    }

    public MusicData(string musicName, string artist, string musicPath, string demoPath, string cover, Difficult difficult, int level, int BPM, int musicTime, int noteCount, List<NoteData> notes)
    {
        this.musicName = musicName;
        this.artist = artist;
        this.musicPath = musicPath;
        this.demoPath = demoPath;
        this.cover = cover;
        this.difficult = difficult;
        this.level = level;
        this.BPM = BPM;
        this.musicTime = musicTime;
        this.noteCount = noteCount;
        this.notes = notes;
    }
}
