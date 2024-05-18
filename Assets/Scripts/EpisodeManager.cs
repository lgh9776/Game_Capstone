using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EpisodeManager : MonoBehaviour
{
    public void EpisodeStartMotionTitleOn()
    {
        if(FindObjectOfType<QuestController>().episode1Start)
            FindObjectOfType<PlayerController>().cantMove = true;
    }

    public void EpisodeStartMotionTitleOff()
    {
        FindObjectOfType<PlayerController>().cantMove = false;
        FindObjectOfType<PlayerController>().autoTalking = true;
    }
}
