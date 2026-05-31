using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] CanvasGroup screenOverlay;
    [SerializeField] private float fadeSpeed = 2;
    [SerializeField] private GameObject raceOverPanel;
    [SerializeField] private int nextLevelIndex = 1;
    [SerializeField] private TMP_Text leaderboardText;

    //delete below 4 --------------------------------------------
    [SerializeField] private TMP_Text penaltyText; 
    [SerializeField] private float penaltyMoveDistance = 60;
    [SerializeField] private float penaltyAnimTime = 0.8f;
    private Vector3 penaltyTextStartPos;
    //-----------------------------------------------------------
   
    void Start()
    {
        screenOverlay.gameObject.SetActive(true);
        raceOverPanel.SetActive(false);
        StartCoroutine(FadeOutOverlay());

        //delete this two -------------------------
        penaltyTextStartPos = penaltyText.rectTransform.localPosition;
        penaltyText.gameObject.SetActive(false);
        // yupp -----------------------------------
    }

    private void OnEnable()
    {
        FinishGate.FinishRace += OnRaceFinished;
        GameManager.PenaltyAdded += ShowPenaltyText; //delete
    }

    private void OnDisable()
    {
        FinishGate.FinishRace -= OnRaceFinished;
        GameManager.PenaltyAdded -= ShowPenaltyText; //delete
    }

    private void OnRaceFinished()
    {
        raceOverPanel.SetActive(true);

        if (GameData.Instance != null)
        {
            leaderboardText.text = GameData.Instance.GetLeaderboardText();
        }
    }

    private IEnumerator FadeOutOverlay()
    {
        while (screenOverlay.alpha > 0)
        {
            screenOverlay.alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }
    }

    private IEnumerator FadeInOverlay()
    {
        while (screenOverlay.alpha < 1)
        {
            screenOverlay.alpha += Time.deltaTime * fadeSpeed;
            yield return null;
        }
    }

    //delete BELOW ---------------------------------------
    private void ShowPenaltyText(int penalty)
    {
        StopCoroutine("PenaltyTextCoroutine");
        StartCoroutine(PenaltyTextCoroutine(penalty));
    }

    private IEnumerator PenaltyTextCoroutine(int penalty)
    {
        penaltyText.gameObject.SetActive(true);
        penaltyText.text = "+" + penalty;

        Color color = penaltyText.color;
        color.a = 1;
        penaltyText.color = color;

        RectTransform rect = penaltyText.rectTransform;
        rect.localPosition = penaltyTextStartPos;

        float timer = 0;

        while (timer < penaltyAnimTime)
        {
            timer += Time.deltaTime;

            float t = timer / penaltyAnimTime;

            rect.localPosition = penaltyTextStartPos + Vector3.up * penaltyMoveDistance * t;

            color.a = 1 - t;
            penaltyText.color = color;

            yield return null;
        }

        penaltyText.gameObject.SetActive(false);
        rect.localPosition = penaltyTextStartPos;
    }
    // above -------------------------------------------------------------------

    public void Restart()
    {
        StartCoroutine(RestartCoroutine());
    }

    public IEnumerator RestartCoroutine()
    {
        yield return StartCoroutine(FadeInOverlay());
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        StartCoroutine(NextLevelCoroutine());
    }

    public IEnumerator NextLevelCoroutine()
    {
        yield return StartCoroutine(FadeInOverlay());
        SceneManager.LoadScene(nextLevelIndex);
    }

    public void Quit()
    {
        StartCoroutine(QuitCoroutine());
    }

    public IEnumerator QuitCoroutine()
    {
        yield return StartCoroutine(FadeInOverlay());
        Application.Quit();
    }


}
