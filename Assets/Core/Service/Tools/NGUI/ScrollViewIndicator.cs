using System.Collections.Generic;
using UnityEngine;

public class ScrollViewIndicator : MonoBehaviour
{
    public UICenterOnChild centerOnChild;
    public GameObject indicatorPrefab;

    private readonly List<GameObject> indicators = new List<GameObject>();
    private GameObject currentCenteredObject;

    private void Start()
    {
        int count = centerOnChild.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            GameObject indicator = Instantiate(indicatorPrefab, transform);
            indicators.Add(indicator);
        }

        UIGrid grid = GetComponent<UIGrid>();
        if (grid != null)
        {
            grid.repositionNow = true;
        }

        OnCenter(centerOnChild.centeredObject);

        centerOnChild.onCenter += OnCenter;
    }

    public void OnCenter(GameObject centeredObject)
    {

        if (!gameObject.activeSelf)
            return;

        if (currentCenteredObject != null)
        {
            int currentIndex = currentCenteredObject.transform.GetSiblingIndex();
            UITweener currentTweener = indicators[currentIndex].GetComponent<UITweener>();
            if (currentTweener != null)
            {
                currentTweener.PlayReverse();
            }
        }

        int index = centeredObject != null ? centeredObject.transform.GetSiblingIndex() : 0;
        currentCenteredObject = centeredObject;

        UITweener tweener = indicators[index].GetComponent<UITweener>();
        if (tweener != null)
        {
            tweener.PlayForward();
        }
    }
}