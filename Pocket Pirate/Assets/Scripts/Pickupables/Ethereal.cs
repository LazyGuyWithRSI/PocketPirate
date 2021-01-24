using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ethereal : MonoBehaviour
{
    public float DefaultDuration = 10f;
    public GameObject Model;
    public Material EtherealMat;

    private Material[] originalMats;
    private MeshRenderer[] renderers;

    private Collider[] colliders;

    // Start is called before the first frame update
    void Start()
    {
        renderers = Model.GetComponentsInChildren<MeshRenderer>();
        originalMats = new Material[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            originalMats[i] = renderers[i].material;
        }

        colliders = GetComponentsInChildren<Collider>();
        List<Collider> colliderList = new List<Collider>();
        foreach (Collider collider in colliders)
        {
            if (collider.tag != "Pickup Collider")
                colliderList.Add(collider);
        }

        colliders = colliderList.ToArray();
    }

    public void ActivateEthereal(float duration = -1)
    {
        StopAllCoroutines();
        if (duration < 0)
            StartCoroutine(EtherealCoroutine(DefaultDuration));
        else
            StartCoroutine(EtherealCoroutine(duration));
    }

    private IEnumerator EtherealCoroutine(float duration)
    {
        PubSub.Publish(new OnPowerupEvent { Type = "Ethereal", Activating = true });

        // swap materials and turn off colliders
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material = EtherealMat;
            renderers[i].sortingOrder = 1;
        }

        for (int i = 0; i < colliders.Length; i++)
            colliders[i].enabled = false;

        yield return new WaitForSeconds(duration);

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material = originalMats[i];
            renderers[i].sortingOrder = 0;
        }

        for (int i = 0; i < colliders.Length; i++)
            colliders[i].enabled = true;

        PubSub.Publish(new OnPowerupEvent { Type = "Ethereal", Activating = false });
    }
}
