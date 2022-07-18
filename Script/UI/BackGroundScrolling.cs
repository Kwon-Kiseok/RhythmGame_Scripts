using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScrolling : MonoBehaviour
{
    public float scrollspeed = 0.2f;
    Material myMaterial;

    private void Start()
    {
        myMaterial = GetComponent<Renderer>().material;
    }
    // Update is called once per frame
    void Update()
    {
        float newOffSetX = myMaterial.mainTextureOffset.x + scrollspeed * Time.deltaTime;
        Vector2 newOffset = new Vector2(newOffSetX, 0);

        myMaterial.mainTextureOffset = newOffset;
    }
}
