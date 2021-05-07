using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisablePickupOnDeath : MonoBehaviour
{
    public Collider PickupCollider;

    // Start is called before the first frame update
    void Start()
    {
        PubSub.RegisterListener<OnGameOver>(OnGameOverHandler);
    }

    private void OnGameOverHandler(object publishedEvent)
    {
        PickupCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
