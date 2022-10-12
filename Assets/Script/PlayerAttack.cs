using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerAttack : MonoBehaviour
{
    public enum State
    {
        Ready,
        Empty,
        Reloading
    }
    public State state { get; set; }

    public Transform cameraTransform;
    public GameObject muzzlePrefab;

    private AudioSource gunAudioPlayer;
    private Animator animatorPlayer;
    public AudioClip shotClip;
    public AudioClip reloadClip;
    public AudioClip hitClip;

    public float timeBetFire = 0.15f;
    public float reloadTime = 1.8f;

    public Button shotButton;

    private void Awake()
    {
        gunAudioPlayer = GetComponent<AudioSource>();
        animatorPlayer = GetComponent<Animator>();
    }

    private void Start()
    {
        ButtonManager.instance.onShot += Fire;
        ButtonManager.instance.onReload += Reload;
        ButtonManager.instance.onRetry += Setup;
        GameManager.instance.emptyAmmo += OnEmptyAmmo;
    }

    public void Setup()
    {
        state = State.Ready;
    }

    public void Fire()
    {
        if (state == State.Ready)
        {
            StartCoroutine("ShotButtonActive");
            Shot();
        }
    }

    private void Shot()
    {
        RaycastHit hit;

        gunAudioPlayer.PlayOneShot(shotClip);
        animatorPlayer.SetTrigger("Shot");

        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit))
        {
            if (hit.transform.tag == "Enemy")
            {
                Instantiate(muzzlePrefab, hit.point, Quaternion.LookRotation(hit.normal));

                StartCoroutine(hit.transform.gameObject.GetComponent<EnemyControl>().EnemyDead());
                /*
                hit.transform.gameObject.GetComponent<AudioSource>().PlayOneShot(hitClip);
                hit.transform.gameObject.SetActive(false);
                */
                GameManager.instance.AddScore(1);
            }
        }
        GameManager.instance.UpdateAmmo(false);
    }

    private IEnumerator ShotButtonActive()
    {
        shotButton.gameObject.SetActive(false);
        yield return new WaitForSeconds(timeBetFire);
        shotButton.gameObject.SetActive(true);
    }

    // 재장전 여부 확인
    public bool Reload()
    {
        if(state == State.Reloading || !GameManager.instance.CheckAmmo())
        {
            return false;
        }
        StartCoroutine(ReloadRoutine());
        return true;
    }

    public void OnEmptyAmmo()
    {
        state = State.Empty;
    }

    // 재장전
    private IEnumerator ReloadRoutine()
    {
        state = State.Reloading;
        gunAudioPlayer.PlayOneShot(reloadClip);
        animatorPlayer.SetTrigger("Reload");
        yield return new WaitForSeconds(reloadTime);
        GameManager.instance.UpdateAmmo(true);
        state = State.Ready;
    }
}
