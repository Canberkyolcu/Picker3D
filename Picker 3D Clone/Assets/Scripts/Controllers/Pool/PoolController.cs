using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;


namespace Controllers.Pool
{
    public class PoolController : MonoBehaviour
    {

        [SerializeField] private GameObject tweens;
        [SerializeField] private List<GameObject> barrier = new List<GameObject>();
        [SerializeField] private TextMeshPro poolText;
        [SerializeField] private byte stageID;
        [SerializeField] private new Renderer renderer;

        private PoolData _data;
  
        private byte _collectedCount;

        private void Awake()
        {
            _data = GetPoolData();
          
        }

      
        private PoolData GetPoolData()
        {
            return Resources.Load<CD_Level>("Data/CD_Level").Levels[(int)CoreGameSignals.Instance.onGetLevelValue()]
                .pools[stageID];
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            
            CoreGameSignals.Instance.onStageAreaSuccessful += OnActivateTweens;
            CoreGameSignals.Instance.onStageAreaSuccessful += OnChangePoolColor;
            
        }

        private void OnChangePoolColor(byte stageValue)
        {
            if (stageValue != stageID) return;
            renderer.material.DOColor((new Color(0.1607842f, 0.6039216f, 0.1766218f)), 1).SetEase(Ease.Linear);
        }

        private void OnActivateTweens(byte stageValue)
        {
            if (stageValue != stageID) return;
            foreach (var barriers in barrier)
            {
                barriers.gameObject.SetActive(false);
            }
                tweens.transform.DOLocalMoveY(-5.3f, 1f);
           
        }

        private void Start()
        {
            SetRequiredAmountText();
        }

        private void SetRequiredAmountText()
        {
            poolText.text = $"0/{_data.requiredObjectCount}";
        }

        public bool TakeResults(byte managerStageValue)
        {
            if (stageID == managerStageValue)
            {
                return _collectedCount >= _data.requiredObjectCount;
            }

            return false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(!other.CompareTag("Collectable")) return;
            IncreaseCollectedAmount();
            SetCollectedAmountToPool();
           
        }
        private void IncreaseCollectedAmount()
        {
            _collectedCount++;
        }
        private void SetCollectedAmountToPool()
        {
            poolText.text = $"{_collectedCount}/{_data.requiredObjectCount}";
        }

        private void OnTriggerExit(Collider other)
        {
            if(!other.CompareTag("Collectable")) return;
            DecreaseCollectedAmount();
            SetCollectedAmountToPool();
        }

        private void DecreaseCollectedAmount()
        {
            _collectedCount--;
        }

        public short TotalCollectedAmount(short value)
        {
            return value += _collectedCount;
        }

        private void UnSubscribeEvents()
        {

            CoreGameSignals.Instance.onStageAreaSuccessful -= OnActivateTweens;
            CoreGameSignals.Instance.onStageAreaSuccessful -= OnChangePoolColor;

        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }
    }

    
}