using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons
{
  [RequireComponent(typeof(Button))]
  public abstract class ButtonBase : MonoBehaviour
  {
    private Button _button;

    private void Awake()
    {
      _button = GetComponent<Button>();
      _button.onClick.AddListener(OnClickHandler);
    }
    
    private void OnDestroy()
    {
      _button.onClick.RemoveListener(OnClickHandler);
    }

    protected abstract void OnClickHandler();
  }
}