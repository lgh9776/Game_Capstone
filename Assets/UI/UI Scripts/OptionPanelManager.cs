using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionPanelManager : MonoBehaviour
{
    Animator windowAnimator;
    private bool isOn = false;

    void Start()
    {
        gameObject.SetActive(false);
        windowAnimator = gameObject.GetComponent<Animator>();
    }

    public void AnimateWindow()
    {
        if (isOn == false) {
            windowAnimator.CrossFade("Window In", 0.1f);
            isOn = true;
        }

        else {
            windowAnimator.CrossFade("Window Out", 0.1f);
            isOn = false;
        }
    }

    public void WindowIn()
    {
        gameObject.SetActive(true);

        if (isOn == false) {
            windowAnimator.CrossFade("Window In", 0.1f);
            isOn = true;
        }
    }

    public void WindowOut()
    {
        if (isOn == true) {
            windowAnimator.CrossFade("Window Out", 0.1f);
            isOn = false;
        }
    }
}
