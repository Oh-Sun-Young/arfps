using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    // 싱글턴 접근용 프로퍼티
    public static ButtonManager instance
    {
        get
        {
            if(m_instance == null)
            {
                m_instance = FindObjectOfType<ButtonManager>();
            }
            return m_instance;
        }
    }
    private static ButtonManager m_instance;// 싱글턴이 할당된 static 변수

    public event Action onShot;
    public event Func<bool> onReload;
    public event Action onRetry;

    private void Awake()
    {
        if(instance != this)
        {
            Destroy(gameObject);
        }
    }

    // 게임 시작
    public void GamePlay()
    {
        UIManager.instance.SetActiveGameUI("play");
        GameManager.instance.SetUp();
        GameManager.instance.PlayGame(true);
    }

    // 일시 정지
    public void GamePause()
    {
        UIManager.instance.SetActiveGameUI("palse");
        GameManager.instance.PlayGame(false);
    }

    // 재도전
    public void GameRetry()
    {
        onRetry();
        UIManager.instance.SetActiveGameUI("play");
        GameManager.instance.SetUp();
        GameManager.instance.PlayGame(true);
    }

    // 계속 진행
    public void GameContinue()
    {
        UIManager.instance.SetActiveGameUI("play");
        GameManager.instance.PlayGame(true);
    }

    // 탄알 발사
    public void GunShot()
    {
        onShot();
    }

    // 탄알 재장전
    public void GunReload()
    {
        onReload();
    }

    // 게임 종료
    public void GameQuit()
    {
        Application.Quit();
    }
}
