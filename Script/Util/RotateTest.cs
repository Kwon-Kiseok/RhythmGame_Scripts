using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTest : MonoBehaviour
{
    public void Rotate()
    {
        transform.Rotate(Vector3.back * 45);
    }
}
