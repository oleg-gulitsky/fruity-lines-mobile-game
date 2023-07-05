using UnityEngine.UI;

namespace Logic.Points
{
  public class Points : IPoints
  {
    private int _points;
    private Text _uiText;
    
    public void SetPoints(int newPoints)
    {
      _points = newPoints;
      _uiText.text = _points.ToString();
    }

    public int GetPoints() => _points;
    
    public void AddPoints(int addedPoints)
    {
      SetPoints(_points + addedPoints);
    }

    public void SetUIText(Text uiText)
    {
      _uiText = uiText;
    }
  }
}