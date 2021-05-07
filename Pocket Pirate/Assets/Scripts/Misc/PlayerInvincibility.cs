using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInvincibility : MonoBehaviour
{
    Health health;

    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<Health>();
        PubSub.RegisterListener<OnWaveOver>(OnWaveOverHandler);
    }

    private void OnWaveOverHandler(object publishedObject)
    {
        health.SetInvincible(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
