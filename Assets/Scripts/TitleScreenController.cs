using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenController : MonoBehaviour
{
    public GameObject gameTitle;
    public GameObject newGame;
    public GameObject loadGame;
    public GameObject exitGame;

    private int menuActiveCount = -1;

    [SerializeField] private float menuActiveSpeed = 0.75f;

    private void Awake()
    {
        ActivateMenuInvoke();
    }

    public void ActivateMenuInvoke()
    {
        Invoke("ActivateMenu", menuActiveSpeed);
    }

    private void ActivateMenu()
    {
        menuActiveCount++;

        switch (menuActiveCount) {
            case 0:
                gameTitle.SetActive(true);
                break;
            case 1:
                newGame.SetActive(true);
                break;
            case 2:
                loadGame.SetActive(true);
                break;
            case 3:
                exitGame.SetActive(true);
                break;
        }
    }

    public void NewGame()
    {
        FindObjectOfType<FadeEffectController>().isNewGame = true;
        FindObjectOfType<FadeEffectController>().SceneChange(2);
    }

    public void LoadGame()
    {
        FindObjectOfType<FadeEffectController>().isNewGame = false;
        FindObjectOfType<FadeEffectController>().SceneChange(1);
    }

    public void ExitGame()
    {
        Debug.Log("EXIT GAME");
        Application.Quit();
    }    
}
