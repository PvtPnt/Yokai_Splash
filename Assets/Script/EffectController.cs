using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    public float timeToDestroy;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(destruction());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator destruction()
    {
        yield return new WaitForSeconds(timeToDestroy);
        Destroy(this.gameObject);
}
}
