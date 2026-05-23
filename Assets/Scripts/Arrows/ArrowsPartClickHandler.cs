using UnityEngine;
using UnityEngine.EventSystems;

public class ArrowPartClickHandler : MonoBehaviour
{
    private ArrowController arrowController;

    public void Initialize(ArrowController controller)
    {
        arrowController = controller;
    }

    private void OnMouseDown()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (arrowController != null)
        {
            arrowController.OnClicked();
        }
    }
}