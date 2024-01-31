using Assets.Scripts.Commands.Player;
using Controllers.Player;
using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public byte StageValue;

    public short currentCollectable;

    internal ForceBallsToPoolCommand ForceCommand;

    [SerializeField] private PlayerMovementController movementController;
    [SerializeField] private PlayerMeshController meshController;
    [SerializeField] private PlayerPhysicsController physicsController;

    private PlayerData _data;


    private void Awake()
    {
        _data = GetPlayerData();
      
        SendDataToControllers();
        Init();
    }

  

    private PlayerData GetPlayerData()
    {
        return Resources.Load<CD_Player>("Data/CD_Player").playerData;
    }
    
    private void SendDataToControllers()
    {
        movementController.SetData(_data.MovementData);
        meshController.SetData(_data.MeshData);
    }
    
    private void Init()
    {
        ForceCommand = new ForceBallsToPoolCommand(this, _data.ForceData);
    }

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        InputSignals.instance.onInputTaken += () => movementController.IsReadyToMove(true);
        InputSignals.instance.onInputReleased += () =>  movementController.IsReadyToMove(false);
        InputSignals.instance.onInputDragged += OnInputDragged;
        UISignals.instance.onPlay +=() =>  movementController.IsReadyToPlay(true);
        CoreGameSignals.Instance.onLevelSuccessful += () => movementController.IsReadyToPlay(false);
        CoreGameSignals.Instance.onLevelFailed += () => movementController.IsReadyToPlay(false);
        CoreGameSignals.Instance.onStageAreaEntered += ()=>movementController.IsReadyToPlay(false);
        CoreGameSignals.Instance.onStageAreaSuccessful += OnStageAreaSuccessful;
        CoreGameSignals.Instance.onFinishAreaEntered += OnFinishAreaEntered;
        CoreGameSignals.Instance.onMiniGameAreaEntered += OnMiniGameEntered;
        CoreGameSignals.Instance.onReset += OnReset;
    }
    

    private void OnInputDragged(HorizontalInputParams inputparams)
    {
        movementController.UpdateInputParams(inputparams);
    }
    

    private void OnFinishAreaEntered()
    {
        CoreGameSignals.Instance.onLevelSuccessful?.Invoke();
       
        //mini game yazılmalı
    }

    private void OnMiniGameEntered()
    {
        movementController.MiniGameMovement();
        
        meshController.PlayConfetti();
        
    }


    private void OnStageAreaSuccessful(byte value)
    {
        StageValue = (byte)++value;
        movementController.IsReadyToPlay(true);
        meshController.ScaleUpPlayer();
        meshController.PlayConfetti();
        meshController.ShowUpText();
    }
    
    private void OnReset()
    {
        StageValue = 0;
        movementController.OnReset();
        physicsController.OnReset();
        meshController.OnReset();
    }
    
    private void UnSubscribeEvents()
    {
        InputSignals.instance.onInputTaken -= () => movementController.IsReadyToMove(true);
        InputSignals.instance.onInputReleased -= () =>  movementController.IsReadyToMove(false);
        InputSignals.instance.onInputDragged -= OnInputDragged;
        UISignals.instance.onPlay -= () => movementController.IsReadyToPlay(true);
        CoreGameSignals.Instance.onLevelSuccessful -= () => movementController.IsReadyToPlay(false);
        CoreGameSignals.Instance.onLevelFailed -=() => movementController.IsReadyToPlay(false);
        CoreGameSignals.Instance.onStageAreaEntered -= ()=>movementController.IsReadyToPlay(false);
        CoreGameSignals.Instance.onStageAreaSuccessful -= OnStageAreaSuccessful;
        CoreGameSignals.Instance.onFinishAreaEntered -= OnFinishAreaEntered;
        CoreGameSignals.Instance.onReset -= OnReset;
    }

    private void OnDisable()
    {
        UnSubscribeEvents();
    }
}
