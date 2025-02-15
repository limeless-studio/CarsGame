using Game;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [BeginGroup("HUD")]
    [SerializeField] private GameObject controlsCanvas;
    [SerializeField] private Text scoreText;
    [SerializeField, EndGroup] private Text countdownText;

    [BeginGroup("Death Panel")]
    [SerializeField] private GameObject deathPanel;
    [SerializeField] private CanvasGroup deathCanvas;
    [SerializeField] private GameObject newHighscoreTag;
    [SerializeField] private Text finalScoreText;
    [EndGroup]
    [SerializeField] private Text highScoreText;

    public enum FadeDirection { FadeOut, FadeIn }

    private void Start()
    {
        TriggerDeathPanel(false);
    }

    public void UpdateScoreText(int score)
    {
        scoreText.text = score.ToString("D2");
    }

    public void UpdateCountDown(int num)
    {
        if (num > 0)
        {
            countdownText.text = num.ToString("D2");
        } else
        {
            countdownText.text = "GO!";
            StartCoroutine(EmptyText(countdownText, .5f, false));
        }
    }

    public void TriggerDeathPanel(bool state)
    {
        deathPanel.SetActive(state);
        controlsCanvas.SetActive(!state);
        if (state)
        {
            int score = GameManager.Instance.Score;
            finalScoreText.text = score.ToString();
            bool isNewHighScore = ScoreManager.CheckAndSaveHighScore(GameManager.Instance.GameID, score);
            if (isNewHighScore)
            {
                newHighscoreTag?.SetActive(true);
                highScoreText.text = score.ToString();
            }
            else
            {
                newHighscoreTag?.SetActive(false);
                highScoreText.text = ScoreManager.GetHighScore(GameManager.Instance.GameID).ToString();
            }

            StartCoroutine(FadeCanvas(deathCanvas, FadeDirection.FadeIn, .5f));
        }
    }

    IEnumerator EmptyText(Text text, float duration, bool state = true)
    {
        yield return new WaitForSeconds(duration);
        text.text = string.Empty;
        text.gameObject.SetActive(state);
    }

    public IEnumerator FadeCanvas(CanvasGroup canvasGroup, FadeDirection direction, float duration)
    {
        // keep track of when the fading started, when it should finish, and how long it has been running
        var startTime = Time.time;
        var endTime = Time.time + duration;
        var elapsedTime = 0f;

        // set the canvas to the start alpha � this ensures that the canvas is �reset� if you fade it multiple times
        if (direction == FadeDirection.FadeIn) canvasGroup.alpha = 0f;
        else canvasGroup.alpha = 1f;

        // loop repeatedly until the previously calculated end time
        while (Time.time <= endTime)
        {
            elapsedTime = Time.time - startTime; // update the elapsed time
            var percentage = 1 / (duration / elapsedTime); // calculate how far along the timeline we are
            if ((direction == FadeDirection.FadeOut)) // if we are fading out
            {
                canvasGroup.alpha = 1f - percentage;
            }
            else // if we are fading in/up
            {
                canvasGroup.alpha = percentage;
            }

            yield return new WaitForEndOfFrame(); // wait for the next frame before continuing the loop
        }

        // force the alpha to the end alpha before finishing � this is here to mitigate any rounding errors, e.g. leaving the alpha at 0.01 instead of 0
        if (direction == FadeDirection.FadeIn) canvasGroup.alpha = 1f;
        else canvasGroup.alpha = 0f;
    }
}