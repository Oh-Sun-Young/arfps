using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    // 싱글턴 접근용 프로퍼티
    public static UIManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<UIManager>();
            }
            return m_instance;
        }
    }
    private static UIManager m_instance; // 싱글턴이 할당된 static 변수 

    [Header("HUD")]
    public GameObject introHud;
    public GameObject noticeContent;
    public GameObject introContent;
    public GameObject introBackground;
    public GameObject playHud;
    public GameObject tempHud;
    public GameObject pauseHud;
    public GameObject gameoverHud;

    [Header("Text")]
    public TextMeshProUGUI hpText; // HP 표시용 텍스트
    public TextMeshProUGUI ammoText; // 총알 표시용 텍스트
    public TextMeshProUGUI scoreText; // 점수 표시용 텍스트
    public TextMeshProUGUI hpTextTemp; // HP 표시용 텍스트 (임시)
    public TextMeshProUGUI ammoTextTemp; // 총알 표시용 텍스트 (임시)
    public TextMeshProUGUI scoreTextTemp; // 점수 표시용 텍스트 (임시)
    public TextMeshProUGUI scoreTextResult; // 최종 점수 표시용 텍스트
    public TextMeshProUGUI scoreTextBest; // 최고 점수 표시용 텍스트

    [Header("Notice Text")]
    public GameObject[] noticeKr; // 주의사항 텍스트 (한국어)
    public GameObject[] noticeEng; // 주의사항 텍스트 (영어)
    private float noticeActiveTime = 5f; // 주의사항 보여주는 시간

    private void Start()
    {
        introContent.SetActive(false);
        playHud.SetActive(false);
        tempHud.SetActive(false);
        pauseHud.SetActive(false);
        gameoverHud.SetActive(false);

        for (int i = 0; i < noticeKr.Length; i++)
        {
            noticeKr[i].gameObject.SetActive(true);
            noticeEng[i].gameObject.SetActive(false);
        }
        StartCoroutine("NoticeActive");
    }

    private IEnumerator NoticeActive()
    {
        yield return new WaitForSeconds(noticeActiveTime);
        for (int i = 0; i < noticeKr.Length; i++)
        {
            noticeKr[i].gameObject.SetActive(false);
            noticeEng[i].gameObject.SetActive(true);
        }
        yield return new WaitForSeconds(noticeActiveTime);
        noticeContent.SetActive(false);
        introContent.SetActive(true);
    }

    private IEnumerator IntroFadeOut()
    {
        RawImage bg = introBackground.GetComponent<RawImage>();
        float fadeCount = bg.color.a;
        while (fadeCount > 0)
        {
            fadeCount -= 0.01f;
            bg.color = new Color(255, 0, 0, fadeCount);
            yield return new WaitForSeconds(0.05f);
        }
        introHud.SetActive(false);
        playHud.SetActive(true);
        GameManager.instance.PlayGame(true);
    }

    // HP 텍스트 갱신
    public void UpdateHpText(int hp)
    {
        hpText.text = "" + hp;
    }

    // 탄알 텍스트 갱신
    public void UpdateAmmoText(int ammo)
    {
        ammoText.text = "" + ammo;
    }

    // 점수 텍스트 갱신
    public void UpdateScoreText(int score)
    {
        scoreText.text = "" + score;
    }

    // 임시용 텍스트 갱신
    public void UpdateTempText(int hp, int ammo, int score)
    {
        hpTextTemp.text = "" + hp;
        ammoTextTemp.text = "" + ammo;
        scoreTextTemp.text = "" + score;
    }

    // 결과 텍스트 갱신
    public void UpdateResultText(int now, int best)
    {
        scoreTextResult.text = "" + now;
        scoreTextBest.text = "" + best;
    }

    // 게임오버 UI 활성화
    public void SetActiveGameUI(string active)
    {
        // active 종류 : play, palse, gameover
        if (active == "play" && introHud.activeSelf)
        {
            StartCoroutine("IntroFadeOut");
            GameManager.instance.SetUp();
        }
        introHud.SetActive(false);
        playHud.SetActive(active == "play" ? true : false);
        tempHud.SetActive(active == "play" ? false : true);
        pauseHud.SetActive(active == "palse" ? true : false);
        gameoverHud.SetActive(active == "gameover" ? true : false);
    }
}
