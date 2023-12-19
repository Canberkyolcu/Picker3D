
using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    private InputData _data;
    private bool _isAvailableForTouch, _isFirstTimeTouchTaken, _isTouching;

    private float _currentVelocity;
    private float3 _moveVector;
    private Vector2? _mousePoisiton;



    private void Awake()
    {
        _data = GetInputData();
    }

    private InputData GetInputData()
    {
        return Resources.Load<CD_Input>("Data/CD_Input").data;

    }

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        CoreGameSignals.Instance.onReset += OnReset;
        InputSignals.instance.onEnableInput += OnEnableInput;
        InputSignals.instance.onDisableInput += OnDisableInput;
    }

    private void OnDisableInput()
    {
        _isAvailableForTouch = false;
    }

    private void OnEnableInput()
    {
        _isAvailableForTouch = true;
    }

    private void OnReset()
    {
        _isAvailableForTouch = false;
        //  _isFirstTimeTouchTaken = false;
        _isTouching = false;

    }

    private void UnSubscribeEvents()
    {
        CoreGameSignals.Instance.onReset -= OnReset;
        InputSignals.instance.onEnableInput -= OnEnableInput;
        InputSignals.instance.onDisableInput -= OnDisableInput;
    }

    private void OnDisable()
    {
        UnSubscribeEvents();
    }

    private void Update()
    {
        if (!_isAvailableForTouch) return;

        if (Input.GetMouseButtonUp(0) && !IsPointerOverUIElement())
        {
            _isTouching = false;
            InputSignals.instance.onInputReleased?.Invoke();
            Debug.LogWarning("Executed -----> OnInputReleased");
        }

        if (Input.GetMouseButtonDown(0) && !IsPointerOverUIElement())
        {
            _isTouching = true;
            InputSignals.instance.onInputTaken?.Invoke();
            Debug.LogWarning("Executed ---> OnInputTaken");
            if (!_isFirstTimeTouchTaken)
            {
                _isFirstTimeTouchTaken = true;
                InputSignals.instance.onFirstTimeTouchTaken?.Invoke();
                Debug.LogWarning("Executed ---> OnFirstTimeTouchTaken");
            }
            _mousePoisiton = Input.mousePosition;
        }

        if (Input.GetMouseButton(0) && !IsPointerOverUIElement())
        {
            if (_isTouching)
            {
                if (_mousePoisiton != null)
                {
                    Vector2 mouseDeltaPos = (Vector2)Input.mousePosition - _mousePoisiton.Value;
                    if (mouseDeltaPos.x > _data.horizontalInputSpeed)
                    {
                        _moveVector.x = _data.horizontalInputSpeed / 10f * mouseDeltaPos.x;

                    }
                    else if (mouseDeltaPos.x < _data.horizontalInputSpeed)
                    {
                        _moveVector.x = -_data.horizontalInputSpeed / 10f * mouseDeltaPos.x;
                    }
                    else
                    {
                        _moveVector.x = Mathf.SmoothDamp(-_moveVector.x, 0f, ref _currentVelocity, _data.clampSpeed);
                    }

                    _mousePoisiton = Input.mousePosition;

                    InputSignals.instance.onInputDragged?.Invoke(new HorizontalInputParams()
                    {
                        horizontalValue = _moveVector.x,
                        ClampValues = _data.clampValues
                    });
                }
            }

        }
    }

    private bool IsPointerOverUIElement()
    {
        var eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> result = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, result);
        return result.Count > 0;
    }
}

