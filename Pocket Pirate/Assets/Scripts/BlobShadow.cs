using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobShadow : MonoBehaviour
{
    public float HeightOffset = 0.03f;
    public float MaxHeight = 5f;
    public float MinHeight = 0.3f;
    public float MaxSize = 0.5f;
    public float MinSize = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = new Vector3(0, -transform.parent.position.y + HeightOffset, 0);
        float percentA = (Mathf.Min(transform.parent.position.y, MaxHeight) - MinHeight) / (MaxHeight - MinHeight);
        percentA = 1f - percentA;
        float scale = (MaxSize - MinSize) * percentA + MinSize;
        //Debug.Log("height %: " + percentA + ", scale: " + scale);
        transform.localScale = new Vector3(scale, 0, scale);
    }
}