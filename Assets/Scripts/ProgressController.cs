using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressController : MonoBehaviour
{
    private static ProgressController instance = null;

    public int dialougeProgress = 0;
    public int questProgress = 0;

    private void Awake()
    {

        if (null == instance) {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else {
            Destroy(this.gameObject);
        }
    }

    /* 
     * Quest 0 - 입국심사장 퀘스트
    */
}
