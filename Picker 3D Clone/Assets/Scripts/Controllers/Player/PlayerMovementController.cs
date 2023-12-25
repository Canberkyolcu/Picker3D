using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

namespace Controllers.Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] private Rigidbody rigidbody;

        private PlayerData.PlayerMovementData _data;
        private bool _isReadyToMove, _isReadyToPlay;
        private float _xValue;

        private float2 _clampValues;

        internal void SetData(PlayerData.PlayerMovementData data)
        {
            _data = data;
        }

        private void FixedUpdate()
        {
            if (!_isReadyToPlay)
            {
                StopPlayer();
                return;
            }

            if (_isReadyToPlay)
            {
                MovePlayer();
            }
            else
            {
                StopPlayerHorizontally();
            }
        }

        private void MovePlayer()
        {
            var velocity = rigidbody.velocity;
            velocity = new Vector3(_xValue * _data.sidewaySpeed, velocity.y, _data.forwardSpeed);
            rigidbody.velocity = velocity;
            var position1 = rigidbody.position;
            Vector3 position;
            position = new Vector3(Mathf.Clamp(position1.x,_clampValues.x, _clampValues.y),
                (position=rigidbody.position).y,position.z);
            rigidbody.position = position;
        }

        private void StopPlayer()
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
            
        }

        private void StopPlayerHorizontally()
        {
            rigidbody.velocity = new Vector3(0, rigidbody.velocity.y, _data.forwardSpeed);
            rigidbody.angularVelocity = Vector3.zero;
            
        }

        internal void IsReadyToPlay(bool condition)
        {
            _isReadyToPlay = condition;
        }

        internal void IsReadyToMove(bool condition)
        {
            _isReadyToMove = condition;
        }

       

        internal void UpdateInputParams(HorizontalInputParams inputParams)
        {
            _xValue = inputParams.horizontalValue;
            _clampValues = inputParams.ClampValues;
        }

        internal void OnReset()
        {
            StopPlayer();
            _isReadyToMove = false;
            _isReadyToPlay = false;
        }
    }
}