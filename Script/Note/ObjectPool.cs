using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectInfo
{
    public GameObject prefab;
    public int maxCount;
    public Transform trParent;
}


public class ObjectPool : MonoBehaviour
{
    [SerializeField]
    private ObjectInfo[] objectInfos = null;

    public static ObjectPool Instance;

    public Queue<GameObject> objectQueue = new Queue<GameObject>();  

    private void Start()
    {
        Instance = this;
        objectQueue = InsertQueue(objectInfos[0]);
        Debug.Log(objectQueue.Count);
    }

    private Queue<GameObject> InsertQueue(ObjectInfo objectInfo)
    {
        Queue<GameObject> queue = new Queue<GameObject>();
        for(int i = 0; i < objectInfo.maxCount; i++)
        {
            GameObject clone = Instantiate(objectInfo.prefab, transform.position, Quaternion.identity);
            clone.SetActive(false);
            if(objectInfo.trParent != null)
            {
                clone.transform.SetParent(objectInfo.trParent);
            }
            else
            {
                clone.transform.SetParent(this.transform);
            }

            queue.Enqueue(clone);
        }

        return queue;
    }
}
