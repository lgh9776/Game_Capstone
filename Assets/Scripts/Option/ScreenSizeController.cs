using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
    [�ý��� �ɼ� - �ػ� �� ��üȭ�� �ɼ�] ���� ��ũ��Ʈ
*/

public class ScreenSizeController : MonoBehaviour
{
    private static ScreenSizeController instance = null;

    public bool isFullScreen;

    FullScreenMode fullScreenMode = FullScreenMode.Windowed;
    
    
    // [SerializeField] private Toggle fullscreenToggle;    // ��üȭ�� ���
    public GameObject fullscreenSwitch;
    public TMP_Dropdown resolutionDropdown;    // �ػ� ��Ӵٿ�

    // ��� ���� �ػ� ����Ʈ
    List<Resolution> resolutions = new List<Resolution>();

    private int resolutionNum = 0;    // ���� �ػ��� �ĺ� ��ȣ

    public void Awake()
    {
        if (null == instance) {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else {
            Destroy(this.gameObject);
        }

        InitScreenOption();
    }

    // �ػ� �� ��üȭ�� �ɼ� �ʱ�ȭ
    public void InitScreenOption()
    {
        for (int i = 0; i < Screen.resolutions.Length; i++) {
            // ��� ������ �ػ� �� 60hz �ֻ��� �ػ󵵸� �ػ� ����Ʈ�� �߰�
            if(Screen.resolutions[i].refreshRateRatio.ToString().Equals("60"))
                resolutions.Add(Screen.resolutions[i]);
        }

        // �ػ� ��Ӵٿ� ��� �ʱ�ȭ
        resolutionDropdown.options.Clear();

        int optionIdx = 0;    // �ػ� �ɼ� �ʱ�ȭ index

        // ��Ӵٿ� ��Ͽ� ��� ���� �ػ� �߰�
        foreach(Resolution r in resolutions) {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = r.width + "x" + r.height + " " + r.refreshRateRatio.ToString() + "hz";
            resolutionDropdown.options.Add(option);

            // ���� ��Ӵٿ� ǥ�� value�� ���� �ػ� ǥ��
            if (r.width == Screen.width && r.height == Screen.height)
                resolutionDropdown.value = optionIdx;

            optionIdx++;
        }

        // ���� ��Ӵٿ� value �ֽ�ȭ
        resolutionDropdown.RefreshShownValue();

        // �ػ� �� ��üȭ�� ���� �⺻ ������ �ʱ�ȭ
        Screen.SetResolution(1920, 1080, FullScreenMode.Windowed);

        // �ʱ� ��üȭ�� ���ο� ���� ��� Ȱ��ȭ/��Ȱ��ȭ ǥ��
        fullscreenSwitch.GetComponent<Michsky.UI.Shift.SwitchManager>().isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;
    }

    // �ػ� ��Ӵٿ�� value ���� �� value ����
    public void ScreenSizeDropdownChange(int x)
    {
        resolutionNum = x;
    }

    // ��üȭ�� ��� ��ȣ�ۿ�
    public void FullScreenToggle(bool isFull)
    {
        fullScreenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }

    public void SetScreenSize()
    {
        if(fullScreenMode == FullScreenMode.FullScreenWindow) {
            isFullScreen = true;
        }
        else {
            isFullScreen = false;
        }
        /* �׽�Ʈ�� �ӽ� ������ �� �ּ�ó���� �ڵ�� �ٲ���� �� */
        // Screen.SetResolution(resolutions[resolutionNum].width, resolutions[resolutionNum].height, fullScreenMode);
        Screen.SetResolution(1920, 1080, fullScreenMode);
    }

    // �ػ� �� ��üȭ�� �ɼ� ���� ��ư Ŭ��
    public void ScreenSizeApplyClick()
    {
        // �ػ� ��Ӵٿ�� ������ �ػ󵵿� ��üȭ�� ��� ������ �ػ� ����
        Screen.SetResolution(resolutions[resolutionNum].width, resolutions[resolutionNum].height, fullScreenMode);
    }
}
