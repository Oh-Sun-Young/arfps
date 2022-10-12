using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
/*
 * 참고
 * - [Unity] 7. 특정 지점으로 오브젝트 이동 : https://senti-mech.tistory.com/43
 * 
 */
public class EnemyControl : MonoBehaviour
{
    private float speedMin = 0.001f;
    private float speedMax = 0.0035f;
    private float speed;

    private int damageMin = 1;
    private int damageMax = 10;

    private bool dead;

    public Vector3 target;
    private float hitRange = 5f;

    public AudioClip[] enableClip;
    public AudioClip[] hitClip;

    private void OnEnable()
    {
        target = Random.insideUnitSphere * hitRange + target;
        target.y = transform.GetComponent<BoxCollider>().size.y * Random.Range(-0.85f, -1.35f);
        speed = Random.Range(speedMin, speedMax);
        dead = false;

        transform.gameObject.GetComponent<AudioSource>().PlayOneShot(enableClip[Random.Range(0, enableClip.Length)]);
    }

    private void Update()
    {
        if (GameManager.instance.isGameState && !dead)
        {
            if (hitRange * -1 < transform.position.x && transform.position.x < hitRange && hitRange * -1 < transform.position.z && transform.position.z < hitRange)
            {
                dead = true;
                GameManager.instance.UpdateHp(Random.Range(damageMin, damageMax));
                Handheld.Vibrate(); // 진동
                StartCoroutine(EnemyDead());
            }
            else
            {
                transform.rotation = Quaternion.LookRotation(target - transform.position);
                /*
                 * transform.Translate(transform.forward * speed * Time.time); // 한쪽 방향으로 진입하는 이슈 발생 
                 * transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.time);
                 * transform.position = Vector3.Lerp(transform.position, target, speed);
                */
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.time);
            }
        }
    }

    public IEnumerator EnemyDead()
    {
        transform.gameObject.GetComponent<AudioSource>().PlayOneShot(hitClip[Random.Range(0, hitClip.Length)]);

        yield return new WaitForSeconds(0.25f);

        transform.gameObject.SetActive(false);

        GameManager.instance.UpdateEnemy(-1);
    }
}
