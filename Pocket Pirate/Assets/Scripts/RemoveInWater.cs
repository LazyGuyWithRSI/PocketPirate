using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveInWater : MonoBehaviour
{
    bool hasHitWater = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (!hasHitWater && transform.position.y <= 0.1)
        {
            PubSub.Publish<OnHitWater>(new OnHitWater() { Position = new Vector2(transform.position.x, transform.position.z) });
            hasHitWater = true;
        }

        if (transform.position.y < 0.005f)
            GameObject.Destroy(transform.parent.gameObject);
    }
}
