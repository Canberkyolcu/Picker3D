using System;
using UnityEngine;

namespace Controllers.Player
{
    public class PlayerPhysicsController : MonoBehaviour
    {
        [SerializeField] private PlayerManager manager;
        [SerializeField] private new Collider collider;
        [SerializeField] private new Rigidbody rigidbody;
        
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(("StageArea")))
            {
                manager.ForceCommand.Execute();
                CoreGameSignals.Instance.onStageAreaEntered?.Invoke();
                InputSignals.instance.onDisableInput?.Invoke();
                
                
                //Stage Area Kontrol Süreci
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
                //Write the MiniGame Mechanics
            }
        }

        public void OnReset()
        {
            throw new NotImplementedException();
        }
    }
}