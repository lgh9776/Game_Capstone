using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    FadeEffectController _FadeEffectController;

    private void Awake()
    {
        _FadeEffectController = FindObjectOfType<FadeEffectController>();

        if(!FindObjectOfType<GameOverManager>().isRestart)
            if (_FadeEffectController.isNewGame)
                SceneManager.LoadScene(4);
            else
                SceneManager.LoadScene(6);
                // SceneManager.LoadScene(13);
    }
}
