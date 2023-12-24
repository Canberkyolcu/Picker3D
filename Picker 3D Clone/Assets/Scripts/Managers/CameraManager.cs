
using Cinemachine;
using System;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera virtualCamera;

        private float3 _firstPosition;

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            _firstPosition = transform.position;
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CameraSignals.instance.onSetCameraTarger += OnSetCameraTarget;
            CoreGameSignals.Instance.onReset += OnReset;
        }

        private void OnSetCameraTarget()
        {
           // var player = FindAnyObjectByType<PlayerManager>().transform;
            //virtualCamera.Follow = player;

            //virtualCamera.LookAt= player;
        }


        private void OnReset()
        {
            transform.position = _firstPosition;

        }

        private void UnSubscribeEvents()
        {
            CameraSignals.instance.onSetCameraTarger -= OnSetCameraTarget;
            CoreGameSignals.Instance.onReset -= OnReset;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }
    }
}