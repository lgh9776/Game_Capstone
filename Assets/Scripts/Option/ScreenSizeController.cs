using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
    [시스템 옵션 - 해상도 및 전체화면 옵션] 제어 스크립트
*/

public class ScreenSizeController : MonoBehaviour
{
    private static ScreenSizeController instance = null;

    public bool isFullScreen;

    FullScreenMode fullScreenMode = FullScreenMode.Windowed;
    
    
    // [SerializeField] private Toggle fullscreenToggle;    // 전체화면 토글
    public GameObject fullscreenSwitch;
    public TMP_Dropdown resolutionDropdown;    // 해상도 드롭다운

    // 사용 가능 해상도 리스트
    List<Resolution> resolutions = new List<Resolution>();

    private int resolutionNum = 0;    // 현재 해상도의 식별 번호

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

    // 해상도 및 전체화면 옵션 초기화
    public void InitScreenOption()
    {
        for (int i = 0; i < Screen.resolutions.Length; i++) {
            // 사용 가능한 해상도 중 60hz 주사율 해상도만 해상도 리스트에 추가
            if(Screen.resolutions[i].refreshRateRatio.ToString().Equals("60"))
                resolutions.Add(Screen.resolutions[i]);
        }

        // 해상도 드롭다운 목록 초기화
        resolutionDropdown.options.Clear();

        int optionIdx = 0;    // 해상도 옵션 초기화 index

        // 드롭다운 목록에 사용 가능 해상도 추가
        foreach(Resolution r in resolutions) {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = r.width + "x" + r.height + " " + r.refreshRateRatio.ToString() + "hz";
            resolutionDropdown.options.Add(option);

            // 현재 드롭다운 표시 value에 현재 해상도 표시
            if (r.width == Screen.width && r.height == Screen.height)
                resolutionDropdown.value = optionIdx;

            optionIdx++;
        }

        // 현재 드롭다운 value 최신화
        resolutionDropdown.RefreshShownValue();

        // 해상도 및 전체화면 여부 기본 값으로 초기화
        Screen.SetResolution(1920, 1080, FullScreenMode.Windowed);

        // 초기 전체화면 여부에 따라 토글 활성화/비활성화 표시
        fullscreenSwitch.GetComponent<Michsky.UI.Shift.SwitchManager>().isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;
    }

    // 해상도 드롭다운에서 value 선택 시 value 변경
    public void ScreenSizeDropdownChange(int x)
    {
        resolutionNum = x;
    }

    // 전체화면 토글 상호작용
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
        /* 테스트용 임시 설정임 꼭 주석처리한 코드로 바꿔줘야 함 */
        // Screen.SetResolution(resolutions[resolutionNum].width, resolutions[resolutionNum].height, fullScreenMode);
        Screen.SetResolution(1920, 1080, fullScreenMode);
    }

    // 해상도 및 전체화면 옵션 적용 버튼 클릭
    public void ScreenSizeApplyClick()
    {
        // 해상도 드롭다운에서 선택한 해상도와 전체화면 토글 값으로 해상도 변경
        Screen.SetResolution(resolutions[resolutionNum].width, resolutions[resolutionNum].height, fullScreenMode);
    }
}
