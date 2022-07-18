using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : MonoBehaviour
{
    public List<GameObject> beats = new List<GameObject>();
    public DataEnumManager.NoteLine line;
    public int barNumber = 0;

    private void Start()
    {
        for(int i = 0; i < beats.Count; i++)
        {
            if (beats[i].GetComponent<EditNote>().timetext == null)
            {
                beats[i].GetComponent<EditNote>().data.startTime = (8* (barNumber-1) + i + 1) * (int)(MusicManager.instance.SecPerBeat * 0.5f * 1000);
                beats[i].GetComponent<EditNote>().data.line = line;
            }
            if (beats[i].GetComponent<EditNote>().timetext != null)
            {
                beats[i].GetComponent<EditNote>().timetext.text = string.Format("{0}-{1}", barNumber, ((i+1) * 0.5));
            }
        }
    }
}
