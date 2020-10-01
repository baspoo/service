using UnityEngine;

public class ScrollViewController : MonoBehaviour
{
    public UICenterOnChild centerOnChild;

    private Transform currentCenteredObject;

    private void Start()
    {
        OnCenter(centerOnChild.centeredObject);

        centerOnChild.onCenter += OnCenter;
    }

    public void SwipeLeft()
    {
        CenterOn(-1);
    }

    public void SwipeRight()
    {
        CenterOn(1);
    }

    private void CenterOn(int offset)
    {
        int index = currentCenteredObject.GetSiblingIndex();
        int count = centerOnChild.transform.childCount;
        Transform newCenteredObject = centerOnChild.transform.GetChild((index + offset + count) % count);
        centerOnChild.CenterOn(newCenteredObject);
    }

    private void OnCenter(GameObject centeredObject)
    {
        currentCenteredObject = centeredObject?.transform;
    }
}