using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeEffectController : MonoBehaviour
{
    // Singleton 패턴 인스턴스
    private static FadeEffectController instance = null;

    public GameObject fadePanel;
    public AnimationCurve fadeCurve;

    public bool isNewGame = false;

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
    }

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void SceneChange(int sceneNum)
    {
        fadePanel.SetActive(true);
        StartCoroutine(FadeOut(sceneNum));
    }

    /*
    public void RestartToTitle()
    {
        fadePanel.SetActive(true);
        StartCoroutine(FadeOut(0, true));
    }
    */

    IEnumerator FadeIn()
    {
        float t = 1f;
        while (t > 0f) {
            t -= Time.deltaTime;
            float alpha = fadeCurve.Evaluate(t);
            fadePanel.GetComponent<Image>().color = new Color(0f, 0f, 0f, alpha);
            yield return 0;
        }
        fadePanel.SetActive(false);
    }

    IEnumerator FadeOut(int sceneNumber)
    {
        float t = 0f;
        while (t < 1f) {
            t += Time.deltaTime;
            float alpha = fadeCurve.Evaluate(t);
            fadePanel.GetComponent<Image>().color = new Color(0f, 0f, 0f, alpha);
            yield return 0;
        }

        SceneManager.LoadScene(sceneNumber);
        StartCoroutine(FadeIn());
    }
}
