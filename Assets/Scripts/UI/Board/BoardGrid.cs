using Logic.Board;
using UnityEngine;
using Zenject;

namespace UI.Board
{
  public class BoardGrid : MonoBehaviour
  {
    private IBoard _board;

    [Inject]
    public void Construct(IBoard board)
    {
      _board = board;
    }

    private void Awake()
    {
      _board.SetBoardView(gameObject);
    }
  }
}