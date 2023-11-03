using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject timePanel;
    [SerializeField] private GameObject nextLevelPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject cardPanel;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI levelText;
    private float currentTime;
    private float startTime = 180f;
    private int level;
    void Start()
    {
        level = PlayerPrefs.GetInt("Level", 1);
        levelText.text = "Level " + level;
        SetTime();
    }
    private void SetTime()
    {
        if (level >= 8)
        {
            timePanel.SetActive(true);
            int timeDeduct = (level - 1) / 5;
            float time = startTime - timeDeduct;
            StartCoroutine(StartTimer(time));
        }
    }
    IEnumerator StartTimer(float time)
    {
        currentTime = time;
        while (currentTime > 0)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);

            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            yield return new WaitForSeconds(1.0f);

            currentTime -= 1.0f;
        }

        timerText.text = "00:00";
        SetGameOverPanel();
    }
    public void SetGameOverPanel()
    {
        cardPanel.transform.DOScale(0, 1f);
        gameOverPanel.SetActive(true);
        StopAllCoroutines();
    }
    public void SetVictoryPanel()
    {
        cardPanel.transform.DOScale(0, 1f);
        nextLevelPanel.SetActive(true);
        StopAllCoroutines();
    }
    public void VictoryButton()
    {
        GameManager.Instance.NextLevel();
    }
    public void TryAgainButton()
    {
        GameManager.Instance.ReloadScene();
    }
}
