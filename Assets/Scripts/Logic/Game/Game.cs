using System.Threading.Tasks;
using Logic.Board;
using Logic.Points;
using Services.Ads;
using Services.Progress;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace Logic.Game
{
  public class Game : IGame
  {
    private GameObject _selectedFruit;
    private bool _isFruitMoving;

    private IBoard _board;
    private IPoints _points;
    private IProgressService _progressService;
    private IAdsService _adsService;

    [Inject]
    public void Constructor(
      IBoard board,
      IPoints points,
      IProgressService progressService,
      IAdsService adsService)
    {
      _board = board;
      _points = points;
      _progressService = progressService;
      _adsService = adsService;
    }

    public void StartGame()
    {
      GameProgress currentGameProgress = _progressService.Get();
      if (currentGameProgress.IsUnityNull())
      {
        StartNewGame();
      }
      else
      {
        ResumeGame(currentGameProgress);
      }
    }

    public void StartNewGame()
    {
      _progressService.Clear();
      _points.SetPoints(0);
      _board.ResetBoard();
    }

    public void SelectFruit(GameObject fruit)
    {
      if (_isFruitMoving)
        return;

      _selectedFruit = fruit;
    }

    public void SelectCell(GameObject targetCell)
    {
      if (_selectedFruit == null || _isFruitMoving)
        return;

      _isFruitMoving = _board.TryMakeMove(_selectedFruit, targetCell, () =>
      {
        _isFruitMoving = false;
        _selectedFruit = null;
      });
    }

    public async Task<bool> TryUndoMove()
    {
      if (_isFruitMoving)
        return false;

      bool shown = await _adsService.TryShowRewarded();

      if (!shown)
      {
        return false;
      }

      _progressService.Pop();

      GameProgress previousGameProgress = _progressService.Get();

      await _board.SetCells(previousGameProgress.Cells);
      _points.SetPoints(previousGameProgress.Points);

      return true;
    }

    private void ResumeGame(GameProgress currentGameProgress)
    {
      _board.SetCells(currentGameProgress.Cells);
      _points.SetPoints(currentGameProgress.Points);
    }
  }
}