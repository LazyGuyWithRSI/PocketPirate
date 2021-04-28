using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveInWater : MonoBehaviour
{
    public float waterDrag = 1f;

    bool hasHitWater = false;

    private Rigidbody rb;
    private BlobShadow blob;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
        blob = transform.parent.GetComponentInChildren<BlobShadow>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!hasHitWater && transform.position.y <= 0.01)
        {
            PubSub.Publish<OnHitWater>(new OnHitWater() { Position = new Vector2(transform.position.x, transform.position.z) });
            hasHitWater = true;
        }

        if (transform.position.y < 0.005f)
        {
            rb.drag = waterDrag;
            blob.enabled = false;
            GameObject.Destroy(transform.parent.gameObject, 2);
        }
    }
}
