using UnityEngine.UI;

namespace Logic.Points
{
  public interface IPoints
  {
    void SetPoints(int newPoints);
    int GetPoints();
    void AddPoints(int addedPoints);
    void SetUIText(Text uiText);
  }
}