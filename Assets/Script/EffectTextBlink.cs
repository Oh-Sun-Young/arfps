using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EffectTextBlink : MonoBehaviour
{
    public float delayMin;
    public float delayMax;
    private TextMeshProUGUI textMesh;

    private void Awake()
    {
        textMesh = transform.gameObject.GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        StartCoroutine("BlinkRepeat");
        textMesh.enabled = true;
    }
    private void OnDisable()
    {
        StopCoroutine("BlinkRepeat");
        textMesh.enabled = false;
    }
    private IEnumerator BlinkRepeat()
    {
        while (true)
        {
            yield return new WaitForSeconds((textMesh.enabled ? Random.Range(delayMin, delayMax) : 0.1f));
            textMesh.enabled = !textMesh.enabled;
        }
    }
}
