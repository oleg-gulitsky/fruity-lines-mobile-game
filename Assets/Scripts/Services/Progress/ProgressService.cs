using System;
using System.Collections.Generic;
using System.Text;
using GameLogic;
using OdinSerializer;
using UnityEngine;

namespace Services.Progress
{
  [Serializable]
  public class ProgressService : IProgressService
  {
    private const string ProgressKey = "Progress";

    private Stack<GameState> _gameProgress;

    public void LoadProgress()
    {
      string value = PlayerPrefs.GetString(ProgressKey);
      byte[] bytes = Encoding.ASCII.GetBytes(value);
      Stack<GameState> gameProgress = SerializationUtility.DeserializeValue<Stack<GameState>>(bytes, DataFormat.JSON);
      _gameProgress = gameProgress?.Count >= 1 ? gameProgress : new Stack<GameState>();
    }
    
    public void UpdateGameProgress(FruitType[,] cells, int points)
    {
      _gameProgress.Push(new GameState((FruitType[,]) cells.Clone(), points));
      SaveProgress();
    }

    public GameState PopGameState()
    {
      if (_gameProgress.Count <= 1)
        return _gameProgress.Peek();
      GameState currentGameState = _gameProgress.Pop();
      SaveProgress();
      return currentGameState;
    }

    public GameState GetGameState()
    {
      return _gameProgress.Count >= 1 ? _gameProgress.Peek() : null;
    }

    public void ClearGameProgress()
    {
      _gameProgress.Clear();
    }

    private void SaveProgress()
    {
      byte[] bytes = SerializationUtility.SerializeValue(_gameProgress, DataFormat.JSON);
      string value = Encoding.ASCII.GetString(bytes);
      PlayerPrefs.SetString(ProgressKey, value);
    }
  }
}