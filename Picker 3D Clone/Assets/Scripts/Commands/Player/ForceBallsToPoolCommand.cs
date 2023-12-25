
namespace Assets.Scripts.Commands.Player
{
    public class ForceBallsToPoolCommand
    {
        private PlayerManager _manager;
        private PlayerData.PlayerForceData _playerForce;
        public ForceBallsToPoolCommand(PlayerManager manager, PlayerData.PlayerForceData ForceData)
        {
            _manager = manager;
            _playerForce = ForceData;
        }


        public void Execute()
        {
            throw new System.NotImplementedException();
        }
    }
}