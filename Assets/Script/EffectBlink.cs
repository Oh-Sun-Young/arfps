using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectBlink : MonoBehaviour
{
    public float delayMin;
    public float delayMax;
    RawImage rawImage;

    private void Awake()
    {
        rawImage = transform.gameObject.GetComponent<RawImage>();
    }

    private void OnEnable()
    {
        rawImage.enabled = false;
        StartCoroutine("BlinkRepeat");
    }
    private void OnDisable()
    {
        StopCoroutine("BlinkRepeat");
    }
    private IEnumerator BlinkRepeat()
    {
        while (true)
        {
            yield return new WaitForSeconds((rawImage.enabled ? 0.001f : Random.Range(delayMin, delayMax)));
            rawImage.enabled = !rawImage.enabled;
        }
    }
}