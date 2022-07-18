using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondLineMove : MonoBehaviour
{
    private Transform tr;

    private void Awake()
    {
        tr = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        tr.localPosition += 10 * Time.deltaTime * -1 * Vector3.right;
    }
}
