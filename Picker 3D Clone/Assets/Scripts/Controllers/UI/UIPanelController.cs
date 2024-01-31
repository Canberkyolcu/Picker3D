
using System;
using System.Collections.Generic;
using Signals;
using UnityEngine;

public class UIPanelController : MonoBehaviour
{
    [SerializeField] private List<Transform> layers = new List<Transform>();


    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        CoreUISignals.Instance.onOpenPanel += OnOpenPanel;
        CoreUISignals.Instance.onClosePanel += OnClosePanel;
        CoreUISignals.Instance.onCloseAllPanels += OnCloseAllPanels;
    }

    
    private void OnClosePanel(int value)
    {
        if (layers[value].childCount <= 0) return;

        Destroy(layers[value].GetChild(0).gameObject);

    }

    private void OnOpenPanel(UIPanelTypes panelTypes, int value)
    {
        OnClosePanel(value);
       Instantiate(Resources.Load<GameObject>($"Screens/{panelTypes}Panel"), layers[value]);
    }

    private void OnCloseAllPanels()
    {
        foreach (var layer in layers)
        {
            if (layer.childCount <= 0) return;

            Destroy(layer.GetChild(0).gameObject);

        }
    }

    private void UnSubscribeEvents()
    {
        CoreUISignals.Instance.onClosePanel -= OnClosePanel;
        CoreUISignals.Instance.onOpenPanel -= OnOpenPanel;
        CoreUISignals.Instance.onCloseAllPanels -= OnCloseAllPanels;
    }

    private void OnDisable()
    {
        UnSubscribeEvents();
    }
}
