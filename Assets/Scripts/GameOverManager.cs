using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    private static GameOverManager instance = null;

    public GameObject gameoverFadeEffect;
    public GameObject gameoverWaterEffect;
    public GameObject gameoverTitle;
    public GameObject restartButtons;
    
    public bool isGameOver = false;
    public bool isRestart = false;

    FadeEffectController _FadeEffectController;

    private void Awake()
    {
        // Singleton 패턴 구현
        if (null == instance) {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else {
            Destroy(this.gameObject);
        }

        _FadeEffectController = FindObjectOfType<FadeEffectController>();
    }

    private void Update()
    {
        transform.position = new Vector3(GameObject.Find("Player").transform.position.x, 0, 0);
    }

    public void GameOver()
    {        
        StartCoroutine(GameOverWaterEffect());
        isGameOver = true;
    }

    /*
    private IEnumerator GameOverRedEffect()
    {
        float t = 0f;
        while (t < 0.5f) {
            gameoverRedEffect.GetComponent<Image>().color = new Color(255f, 0f, 0f, t);
            yield return new WaitForSecondsRealtime(0.01f);
            t += 0.05f;
        }
    }
    */

    private IEnumerator GameOverWaterEffect()
    {
        float t = -10f;
        while (t < 2f) {
            gameoverWaterEffect.transform.position = new Vector3(transform.position.x, t, 0f);
            yield return new WaitForSecondsRealtime(0.015f);
            t += 0.1f;
        }
        StartCoroutine(GameOverFadeEffect());
    }

    private IEnumerator GameOverFadeEffect()
    {
        gameoverFadeEffect.SetActive(true);
        float t = 0f;
        while (t <= 1.0f) {
            gameoverFadeEffect.GetComponent<Image>().color = new Color(0f, 0f, 0f, t);
            yield return new WaitForSecondsRealtime(0.01f);
            t += 0.05f;
        }
        gameoverTitle.SetActive(true);
        StartCoroutine(GameOverButtonsActivate());
    }

    private IEnumerator GameOverButtonsActivate()
    {
        float t = 0f;
        while (t <= 10.0f) {
            yield return new WaitForSecondsRealtime(0.01f);
            t += 0.05f;
        }
        restartButtons.SetActive(true);
    }

    public void LoadTitle()
    {
        Debug.Log("LoadTitle");
        /*
        isRestart = true;
        _FadeEffectController.SceneChange(1);
        */
    }

    public void LoadRecentGame()
    {
        /*
        Debug.Log("LoadRecentGame");
        _FadeEffectController.isNewGame = false;
        _FadeEffectController.SceneChange(1);
        */
    }
}
