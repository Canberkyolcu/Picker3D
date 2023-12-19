using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputSignals : MonoBehaviour
{
    public static InputSignals instance;

    public UnityAction onFirstTimeTouchTaken = delegate { };
    public UnityAction onEnableInput = delegate { };
    public UnityAction onDisableInput = delegate { };
    public UnityAction onInputTaken = delegate { };
    public UnityAction onInputReleased = delegate { };
    public UnityAction<HorizontalInputParams> onInputDragged = delegate { };
   

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
}
