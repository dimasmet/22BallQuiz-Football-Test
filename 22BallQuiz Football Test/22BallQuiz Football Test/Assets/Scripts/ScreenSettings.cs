using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.iOS;

public class ScreenSettings : MonoBehaviour
{
    [SerializeField] private Button _backBtn;

    [SerializeField] private Animator _backgroundAnimator;

    [SerializeField] private Text _title;
    [SerializeField] private Image _imgSwicher;
    [SerializeField] private Sprite _switcherOn;
    [SerializeField] private Sprite _switcherOff;

    public static Action<DarkMode> OnChangeMode;

    private DarkMode currentMode;

    [SerializeField] private Button _privacyBtn;
    [SerializeField] private Button _rateUsBtn;
    [SerializeField] private Button _termsBtn;
    [SerializeField] private Button _appThemeBtn;

    [Header("Privacy")]
    [SerializeField] private Text _titleText;
    [SerializeField] private GameObject _panelText;
    [SerializeField] private Text _privacy;
    [SerializeField] private Text _terms;
    [SerializeField] private Button _closeBtn;

    [SerializeField] private Image _imgBackground;
    [SerializeField] private Color _colordark;

    public enum DarkMode
    {
        None,
        Dark
    }

    private void Awake()
    {
        _closeBtn.onClick.AddListener(() =>
        {
            _panelText.SetActive(false);
        });

        _backBtn.onClick.AddListener(() =>
        {
            ScreensHandler.OpenScreen?.Invoke(ScreensHandler.ScreenName.HomeScreen);
        });

        _privacyBtn.onClick.AddListener(() =>
        {
            _titleText.text = "Privacy policy";

            _panelText.SetActive(true);

            _privacy.gameObject.SetActive(true);
            _terms.gameObject.SetActive(false);
        });

        _rateUsBtn.onClick.AddListener(() =>
        {
            Device.RequestStoreReview();
        });

        _termsBtn.onClick.AddListener(() =>
        {
            _titleText.text = "Terms of  use";

            _panelText.SetActive(true);

            _privacy.gameObject.SetActive(false);
            _terms.gameObject.SetActive(true);
        });

        _appThemeBtn.onClick.AddListener(() =>
        {
            if (currentMode == DarkMode.None)
            {
                currentMode = DarkMode.Dark;
                _imgSwicher.sprite = _switcherOn;
                _backBtn.GetComponent<Image>().color = Color.white;
                _title.color = Color.white;
                _closeBtn.GetComponent<Image>().color = Color.white;
                _titleText.color = Color.white;
                _imgBackground.color = _colordark;
                _privacy.color = Color.white;
                _terms.color = Color.white;
            }
            else
            {
                currentMode = DarkMode.None;
                _imgSwicher.sprite = _switcherOff;
                _backBtn.GetComponent<Image>().color = Color.black;
                _title.color = Color.black;
                _closeBtn.GetComponent<Image>().color = Color.black;
                _titleText.color = Color.black;
                _imgBackground.color = Color.white;
                _privacy.color = Color.black;
                _terms.color = Color.black;
            }

            OnChangeMode?.Invoke(currentMode);

            if (currentMode == DarkMode.Dark)
            {
                _privacyBtn.transform.GetChild(0).GetComponent<Text>().color = Color.white;
                _privacyBtn.transform.GetChild(1).GetComponent<Text>().color = Color.white;
                _privacyBtn.transform.GetChild(2).GetComponent<Image>().color = Color.white;

                _rateUsBtn.transform.GetChild(0).GetComponent<Text>().color = Color.white;
                _rateUsBtn.transform.GetChild(1).GetComponent<Text>().color = Color.white;
                _rateUsBtn.transform.GetChild(2).GetComponent<Image>().color = Color.white;

                _termsBtn.transform.GetChild(0).GetComponent<Text>().color = Color.white;
                _termsBtn.transform.GetChild(1).GetComponent<Text>().color = Color.white;
                _termsBtn.transform.GetChild(2).GetComponent<Image>().color = Color.white;

                _appThemeBtn.transform.GetChild(0).GetComponent<Text>().color = Color.white;
                _appThemeBtn.transform.GetChild(1).GetComponent<Text>().color = Color.white;

                _backgroundAnimator.Play("Dark");
            }
            else
            {
                _privacyBtn.transform.GetChild(0).GetComponent<Text>().color = Color.black;
                _privacyBtn.transform.GetChild(1).GetComponent<Text>().color = Color.black;
                _privacyBtn.transform.GetChild(2).GetComponent<Image>().color = Color.black;

                _rateUsBtn.transform.GetChild(0).GetComponent<Text>().color = Color.black;
                _rateUsBtn.transform.GetChild(1).GetComponent<Text>().color = Color.black;
                _rateUsBtn.transform.GetChild(2).GetComponent<Image>().color = Color.black;

                _termsBtn.transform.GetChild(0).GetComponent<Text>().color = Color.black;
                _termsBtn.transform.GetChild(1).GetComponent<Text>().color = Color.black;
                _termsBtn.transform.GetChild(2).GetComponent<Image>().color = Color.black;

                _appThemeBtn.transform.GetChild(0).GetComponent<Text>().color = Color.black;
                _appThemeBtn.transform.GetChild(1).GetComponent<Text>().color = Color.black;

                _backgroundAnimator.Play("White");
            }
        });
    }
}
