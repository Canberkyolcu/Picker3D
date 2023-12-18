
using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Transform levelHolder;
    [SerializeField] private byte totalLevelCount;


    private OnLevelLoaderCommand _levelLoaderCommand;
    private OnLevelDestoryerCommand _levelDestroyerCommand;

    private short _currentLevel;
    private LevelData _levelData;

    private void Awake()
    {
        _levelData = GetLevelData();
        _currentLevel = GetActiveLevel();

        Init();
    }

    private void Init()
    {
        _levelLoaderCommand = new OnLevelLoaderCommand(levelHolder);
        _levelDestroyerCommand = new OnLevelDestoryerCommand(levelHolder);
    }

    private byte GetActiveLevel()
    {
      return (byte) _currentLevel;
    }

    private LevelData GetLevelData()
    {
        return Resources.Load<CD_Level>("Data/CD_Level").Levels[_currentLevel];
    }

    private void OnEnable()
    {
        SubscribeEvenets();
    }


    private void SubscribeEvenets()
    {
        CoreGameSignals.Instance.onLevelInitialize += _levelLoaderCommand.Execute;
        CoreGameSignals.Instance.onClearActiveLevel += _levelDestroyerCommand.Execute;
        CoreGameSignals.Instance.onGetLevelValue += OnGetLevelValue;
        CoreGameSignals.Instance.onNextLevel += OnNextLevel;
        CoreGameSignals.Instance.onRestartLevel += OnRestartLevel;
    }

    private byte OnGetLevelValue()
    {
        return (byte)_currentLevel;
    }

    private void UnSubscribeEvenets()
    {
        CoreGameSignals.Instance.onLevelInitialize -= _levelLoaderCommand.Execute;
        CoreGameSignals.Instance.onClearActiveLevel -= _levelDestroyerCommand.Execute;
        CoreGameSignals.Instance.onGetLevelValue -= OnGetLevelValue;
        CoreGameSignals.Instance.onNextLevel -= OnNextLevel;
        CoreGameSignals.Instance.onRestartLevel -= OnRestartLevel;
    }

    private void OnDisable()
    {
        UnSubscribeEvenets();
    }

    private void Start()
    {
        CoreGameSignals.Instance.onLevelInitialize?.Invoke((byte)(_currentLevel % totalLevelCount));
    }

    private void OnNextLevel()
    {
        _currentLevel++;
        CoreGameSignals.Instance.onClearActiveLevel?.Invoke();
        CoreGameSignals.Instance.onReset?.Invoke();
        CoreGameSignals.Instance.onLevelInitialize?.Invoke((byte)(_currentLevel % totalLevelCount));
    }

    private void OnRestartLevel()
    {
        CoreGameSignals.Instance.onClearActiveLevel?.Invoke();
        CoreGameSignals.Instance.onReset?.Invoke();
        CoreGameSignals.Instance.onLevelInitialize?.Invoke((byte)(_currentLevel % totalLevelCount));
    }

}

