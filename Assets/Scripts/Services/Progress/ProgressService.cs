using System;
using System.Collections.Generic;
using System.Text;
using OdinSerializer;
using UnityEngine;

namespace Services.Progress
{
  [Serializable]
  public class ProgressService : IProgressService
  {
    private const string ProgressKey = "Progress";

    private Stack<GameProgress> _gameProgress;

    public void Load()
    {
      string value = PlayerPrefs.GetString(ProgressKey);
      byte[] bytes = Encoding.ASCII.GetBytes(value);
      Stack<GameProgress> gameProgress = 
        SerializationUtility.DeserializeValue<Stack<GameProgress>>(bytes, DataFormat.JSON);
      _gameProgress = gameProgress?.Count >= 1 ? gameProgress : new Stack<GameProgress>();
    }
    
    public void Push(GameProgress gameProgress)
    {
      _gameProgress.Push(gameProgress);
      SaveProgress();
    }

    public GameProgress Pop()
    {
      if (_gameProgress.Count <= 1)
        return _gameProgress.Peek();
      GameProgress currentGameProgress = _gameProgress.Pop();
      SaveProgress();
      return currentGameProgress;
    }

    public GameProgress Get()
    {
      return _gameProgress.Count >= 1 ? _gameProgress.Peek() : null;
    }

    public void Clear()
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