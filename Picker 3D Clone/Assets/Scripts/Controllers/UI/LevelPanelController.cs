
using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelPanelController : MonoBehaviour
{
    [SerializeField] private List<Image> stageImages = new List<Image>();
    [SerializeField] private List<TextMeshProUGUI> levelTexts = new List<TextMeshProUGUI>();

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        UISignals.instance.onSetLevelValue += OnSetLevelValue;
        UISignals.instance.onSetStageColor += OnSetStageColor;

    }

    private void OnSetStageColor(byte stageValue)
    {
        stageImages[stageValue].DOColor(new Color(1f, 0.5403299f, 0f), 0.5f);
    }

    private void OnSetLevelValue(byte levelValue)
    {
        var additionalValue = ++levelValue;
        levelTexts[0].text = additionalValue.ToString();
        additionalValue++;
        levelTexts[1].text = additionalValue.ToString();
    }
    private void UnSubscribeEvents()
    {
        UISignals.instance.onSetLevelValue -= OnSetLevelValue;
        UISignals.instance.onSetStageColor -= OnSetStageColor;

    }


    private void OnDisable()
    {
        UnSubscribeEvents();
    }
}
