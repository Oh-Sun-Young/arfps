using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // 싱글턴 접근용 프로퍼티
    public static EnemySpawner instance
    {
        get
        {
            if(m_instance == null)
            {
                m_instance = FindObjectOfType<EnemySpawner>();
            }
            return m_instance;
        }
    }
    private static EnemySpawner m_instance; // 싱글턴이 할당된 static 변수 

    public GameObject[] enemyPrefab; // 생성할 적
    public Camera target;
    public int count = 15;

    private float timeBetSpawnMin = 1f;
    private float timeBetSpawnMax = 5f;
    private float timeBetSpawn;

    private Vector3 randomPosition;
    private float distanceMin = 15f;
    private float distanceMax = 30f;

    private float xMin = 15f;
    private float xMax = 30f;
    private float heightRange = 10f;
    private float zMin = 15f;
    private float zMax = 30f;

    private GameObject[] enemys;
    private int currentIndex = 0;

    private Vector3 poolPosition = new Vector3(-9999, -9999, -9999);
    private float lastSpawnTime;

    private void Awake()
    {
        if(instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        enemys = new GameObject[count];
        for (int i = 0; i < count; i++)
        {
            enemys[i] = Instantiate(enemyPrefab[Random.Range(0, enemyPrefab.Length)], poolPosition, Quaternion.identity);
        }
        SetUp();

        ButtonManager.instance.onRetry += SetUp;
    }

    private float CheckPosition(float distance)
    {
        if (distanceMin * -1 < distance && distance <= 0) distance -= distanceMin;
        else if (0 < distance && distance < distanceMin) distance += distanceMin;
        return distance;
    }

    private void Update()
    {
        if (GameManager.instance != null && !GameManager.instance.isGameState)
        {
            return;
        }

        if(Time.time >= lastSpawnTime + timeBetSpawn && GameManager.instance.CheckEnemy() < count)
        {
            lastSpawnTime = Time.time;
            timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);
            randomPosition = Random.insideUnitSphere * distanceMax + target.transform.position;
            randomPosition.x = CheckPosition(randomPosition.x);
            randomPosition.y = Random.Range(-5, -10);
            randomPosition.z = CheckPosition(randomPosition.z);

            enemys[currentIndex].SetActive(true);
            enemys[currentIndex].transform.position = randomPosition;

            currentIndex++;
            if (currentIndex >= count)
            {
                currentIndex = 0;
            }

            GameManager.instance.UpdateEnemy(1);
        }
    }

    private void SetUp()
    {
        for (int i = 0; i < count; i++)
        {
            enemys[i].SetActive(false);
        }
        lastSpawnTime = 0f;
        timeBetSpawn = 0f;
        currentIndex = 0;
    }
}