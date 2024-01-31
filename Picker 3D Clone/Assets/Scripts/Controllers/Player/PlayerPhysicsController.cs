using System;
using Controllers.Pool;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers.Player
{
    public class PlayerPhysicsController : MonoBehaviour
    {
        [SerializeField] private PlayerManager manager;
        [SerializeField] private new Collider collider;
        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private new Image miniGameScrollImage;

        private LevelData _levelData;
    

        private void Awake()
        {
            _levelData = GetLevelData();
            miniGameScrollImage = GameObject.FindGameObjectWithTag("Image").gameObject.GetComponent<Image>();
           

        }

        private LevelData GetLevelData()
        {
            return Resources.Load<CD_Level>("Data/CD_Level").Levels[(int)CoreGameSignals.Instance.onGetLevelValue()];
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(("StageArea")))
            {
                manager.ForceCommand.Execute();
                CoreGameSignals.Instance.onStageAreaEntered?.Invoke();
                InputSignals.instance.onDisableInput?.Invoke();
                if(miniGameScrollImage == null) miniGameScrollImage = GameObject.FindGameObjectWithTag("Image").gameObject.GetComponent<Image>();   


                DOVirtual.DelayedCall(3, () =>
                {
                    var result = other.transform.parent.GetComponentInChildren<PoolController>()
                        .TakeResults(manager.StageValue);

                    var result1 = other.transform.parent.GetComponentInChildren<PoolController>().TotalCollectedAmount(manager.currentCollectable);

                    manager.currentCollectable = result1;
                    var collectableCount = (float)manager.currentCollectable / (float)_levelData.TotalSpawnedCollectableCount * 100f;
                    miniGameScrollImage.fillAmount = collectableCount / 100f;


                    if (result)
                    {
                        CoreGameSignals.Instance.onStageAreaSuccessful?.Invoke(manager.StageValue);
                        InputSignals.instance.onEnableInput?.Invoke();
                    }
                    else CoreGameSignals.Instance.onLevelFailed?.Invoke();
                        
                    
                });
                return;
            }

            if (other.CompareTag("FinishArea"))
            {
                CoreGameSignals.Instance.onFinishAreaEntered?.Invoke();
                InputSignals.instance.onDisableInput?.Invoke();
                CoreGameSignals.Instance.onLevelSuccessful?.Invoke();
                return;
            }

            if (other.CompareTag("MiniGame"))
            {
                CoreGameSignals.Instance.onMiniGameAreaEntered?.Invoke();
                miniGameScrollImage.DOFillAmount(0f, 4f);
               
                
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color =Color.yellow;
            var transform1 = manager.transform;
            var position1 = transform1.position;
            
            Gizmos.DrawSphere(new Vector3(position1.x,position1.y,position1.z+0.1f), 1f);
        }

        public void OnReset()
        {
           
        }
    }
}