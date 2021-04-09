using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitFXSystem : MonoBehaviour
{
    public GameObject HitPrefab;

    // Start is called before the first frame update
    void Start()
    {
        PubSub.RegisterListener<OnHitEvent>(OnHitHandler);
    }

    public void OnHitHandler (object publishedEvent)
    {
        OnHitEvent args = publishedEvent as OnHitEvent;
        Vector3 newPos = args.Position + ((Camera.main.transform.position - args.Position).normalized * 2f);
        GameObject.Destroy(Instantiate(HitPrefab, newPos, Quaternion.identity), 3);
    }
}
