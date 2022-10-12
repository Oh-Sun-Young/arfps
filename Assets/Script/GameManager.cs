using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 싱글턴 접근용 프로퍼티
    public static GameManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<GameManager>();
            }
            return m_instance;
        }
    }
    private static GameManager m_instance;// 싱글턴이 할당된 static 변수 

    private int hp;
    private int ammo; // 탄알 갯수
    private int maxAmmo = 25; // 최대 탄알 갯수
    private int score; // 현재 게임 점수
    private int cntEnemy; // 적의 수
    public bool isGameState { get; private set; } // 게임 상태

    public event Action emptyAmmo;

    private void Awake()
    {
        if(instance != this)
        {
            Destroy(gameObject);
        }
    }

    // 초기화하고 UI 갱신
    public void SetUp()
    {
        hp = 100;
        ammo = maxAmmo;
        score = 0;
        cntEnemy = 0;
        UIManager.instance.UpdateHpText(hp);
        UIManager.instance.UpdateAmmoText(ammo);
        UIManager.instance.UpdateScoreText(score);
    }


    // HP 업데이트 후 UI 갱신
    public void UpdateHp(int damage)
    {
        if (isGameState)
        {
            hp -= damage;

            if (hp <= 0)
            {
                hp = 0;
                EndGame();
            }
            else
            {
                UIManager.instance.UpdateHpText(hp);
            }

        }
    }
    // 총알 갯수 반환
    public bool CheckAmmo()
    {
        return ammo < maxAmmo;
    }

    // 총알 갯수 업데이트 후 UI 갱신
    public void UpdateAmmo(bool state)
    {
        if (isGameState)
        {
            if (state)
            {
                ammo = maxAmmo;
            }
            else
            {
                ammo--;
                if (ammo <= 0) emptyAmmo();
            }
            UIManager.instance.UpdateAmmoText(ammo);
        }
    }

    // 적의 수 업데이트
    public void UpdateEnemy(int num)
    {
        cntEnemy += num;
    }

    // 적의 수 반환
    public int CheckEnemy()
    {
        return cntEnemy;
    }

    // 점수를 추가하고 UI 갱신
    public void AddScore(int newScore)
    {
        if (isGameState)
        {
            score += newScore;
            UpdateEnemy(-1);
            UIManager.instance.UpdateScoreText(score);
        }
    }

    // 게임진행 처리
    public void PlayGame(bool active)
    {
        isGameState = active;
        Time.timeScale = (active ? 1 : 0);
    }

    // 게임오버 처리
    public void EndGame()
    {
        // 게임 진행 중단
        PlayGame(false);

        // 점수 정산
        int bestScore = PlayerPrefs.HasKey("BestScore") ? PlayerPrefs.GetInt("BestScore") : 0; // 기존 최대 점수
        if(score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt("BestScore", bestScore);
            PlayerPrefs.Save();
        }

        // UI
        UIManager.instance.UpdateTempText(hp, ammo, score);
        UIManager.instance.UpdateResultText(score, bestScore);
        UIManager.instance.SetActiveGameUI("gameover");
    }
}
