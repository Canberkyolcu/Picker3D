using System;
using Unity.Mathematics;
using UnityEngine;
using DG.Tweening;

namespace Controllers.Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private PlayerManager manager;
        

        private PlayerData.PlayerMovementData _data;
        private LevelData _levelData;
        private bool _isReadyToMove, _isReadyToPlay;
        private float _xValue;

        private float2 _clampValues;

        private void Awake()
        {
            _levelData = GetLevelData();
           
        }
        private LevelData GetLevelData()
        {
            return Resources.Load<CD_Level>("Data/CD_Level").Levels[(int)CoreGameSignals.Instance.onGetLevelValue()];
        }


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

        internal void MiniGameMovement()
        {
          //  StopPlayerHorizontally();
            StopPlayer();       
           var collectableCount =  (float)manager.currentCollectable / (float)_levelData.TotalSpawnedCollectableCount* 100f;
            float movePlayer = collectableCount * 0.01f * 10f*4f;
            Debug.Log(collectableCount);
            Debug.Log(movePlayer);
            if (collectableCount < 0) CoreGameSignals.Instance.onLevelSuccessful?.Invoke();
       
            else
            {
                this.gameObject.transform.DOMoveZ(transform.localPosition.x+movePlayer, 4f).OnComplete(() =>

                     CoreGameSignals.Instance.onLevelSuccessful?.Invoke()
                );
           

            }

            Debug.Log(transform.localPosition.x + movePlayer);
      
        }
    }
}