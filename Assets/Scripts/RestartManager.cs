using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartManager : MonoBehaviour
{
    public void Awake()
    {
        if (FindObjectOfType<GameOverManager>().isRestart) {
            GameObject[] allObjects = FindObjectsOfType<GameObject>();
            foreach (GameObject obj in allObjects) {
                if(!(obj.name == "FadeCanvas" || obj.name == "FadePanel"))
                    Destroy(obj);
            }

            /*
            var re = new GameObject("RE");
            DontDestroyOnLoad(re);

            foreach (var root in re.scene.GetRootGameObjects())
                Destroy(root);
            */

            SceneManager.LoadScene("MainTitle");
        }
    }
}
