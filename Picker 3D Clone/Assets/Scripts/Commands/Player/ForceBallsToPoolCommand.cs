
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Commands.Player
{
    public class ForceBallsToPoolCommand
    {
        private PlayerManager _manager;
        private PlayerData.PlayerForceData _playerForce;
        public ForceBallsToPoolCommand(PlayerManager manager, PlayerData.PlayerForceData forceData)
        {
            _manager = manager;
            _playerForce = forceData;
        }


        internal void Execute()
        {
            var transform1 = _manager.transform;
            var position1 = transform1.position;
            var forcePos = new Vector3(position1.x,position1.y,position1.z+0.1f);

            var collider = Physics.OverlapSphere(forcePos,  1.7f);

            var collectableColliderList = collider.Where(col => col.CompareTag("Collectable")).ToList();

            foreach (var col in collectableColliderList)
            {
                if (col.GetComponent<Rigidbody>() == null) continue;
                var rb = col.GetComponent<Rigidbody>();
                rb.AddForce(new Vector3(0,_playerForce.forceParameters.y,_playerForce.forceParameters.z),ForceMode.Impulse);
            }
            collectableColliderList.Clear();
        }
    }
}