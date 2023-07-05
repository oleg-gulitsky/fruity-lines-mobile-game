using Logic.Points;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
  public class Points : MonoBehaviour
  {
    [SerializeField] private Text text;

    private IPoints _points;

    [Inject]
    private void Constructor(IPoints points)
    {
      _points = points;
    }

    private void Awake()
    {
      _points.SetUIText(text);
    }
  }
}