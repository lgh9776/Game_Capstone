using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionController : MonoBehaviour
{
    ScreenSizeController _ScreenSizeController;
    SoundManager _SoundManager;
    KeyManager _KeyManager;

    public bool onOption = false;
    private bool saveInitOption = false;
    // private bool isApplied = false;

    private int resolutionValue;
    private bool fullScreen;

    private float backSound;
    private float effectSound;

    private Dictionary<KeyAction, KeyCode> keySettings = new Dictionary<KeyAction, KeyCode>();

    private void Awake()
    {
        _ScreenSizeController = FindObjectOfType<ScreenSizeController>();
        _SoundManager = FindObjectOfType<SoundManager>();
        _KeyManager = FindObjectOfType<KeyManager>();
    }

    public void OptionBtnClick()
    {
        if (!saveInitOption) {
            SaveOptionData();
            saveInitOption = true;
        }

        onOption = true;
    }

    public void OptionExitBtnClick()
    {
        onOption = false;
        saveInitOption = false;

        _ScreenSizeController.resolutionDropdown.value = resolutionValue;
        _ScreenSizeController.fullscreenSwitch.GetComponent<Michsky.UI.Shift.SwitchManager>().isOn = fullScreen;
        _ScreenSizeController.fullscreenSwitch.GetComponent<Michsky.UI.Shift.SwitchManager>().RestoreSwitch();
        
        _SoundManager.SetBackMusicVol(backSound);
        _SoundManager.SetBtnSoundVol(effectSound);
        _SoundManager.backSlider.GetComponent<Michsky.UI.Shift.SliderManager>().RestoreSlider(backSound);
        _SoundManager.effectSlider.GetComponent<Michsky.UI.Shift.SliderManager>().RestoreSlider(effectSound);

        for (int i=0; i<8; i++) {
            KeySet.keys[(KeyAction)i] = keySettings[(KeyAction)i];
        }

        /*
        if (isApplied) {
            isApplied = false;
            return;
        }

        else {
            _ScreenSizeController.resolutionDropdown.value = resolutionValue;
            _ScreenSizeController.fullscreenSwitch.GetComponent<Michsky.UI.Shift.SwitchManager>().isOn = fullScreen;
            _ScreenSizeController.fullscreenSwitch.GetComponent<Michsky.UI.Shift.SwitchManager>().CancleSwitch();
        }
        */
    }

    public void OptionApplyClick()
    {
        // isApplied = true;
        _ScreenSizeController.SetScreenSize();
        SaveOptionData();
    }

    private void SaveOptionData()
    {
        resolutionValue = _ScreenSizeController.resolutionDropdown.value;
        fullScreen = _ScreenSizeController.isFullScreen;

        backSound = _SoundManager.backgroundSource.volume;
        effectSound = _SoundManager.btnSource.volume;

        for (int i = 0; i < (int)KeyAction.KEYCOUNT; i++) {
            keySettings[(KeyAction)i] = KeySet.keys[(KeyAction)i];
        }
    }
}
