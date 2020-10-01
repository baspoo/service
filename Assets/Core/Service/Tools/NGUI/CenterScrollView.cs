using UnityEngine;
public class CenterScrollView : MonoBehaviour
{
	public bool isCanClick;
	void OnClick ()
	{
		if(isCanClick)
			MovetoCenter ();
	}

    public void MovetoCenter()
    {
        UICenterOnChild center = NGUITools.FindInParents<UICenterOnChild>(gameObject);
        UIPanel panel = NGUITools.FindInParents<UIPanel>(gameObject);

        if (center != null)
        {
            if (center.enabled)
                center.CenterOn(transform);
        }
        else if (panel != null && panel.clipping != UIDrawCall.Clipping.None)
        {
            UIScrollView sv = panel.GetComponent<UIScrollView>();
            Vector3 offset = -panel.cachedTransform.InverseTransformPoint(transform.position);
            if (!sv.canMoveHorizontally) offset.x = panel.cachedTransform.localPosition.x;
            if (!sv.canMoveVertically) offset.y = panel.cachedTransform.localPosition.y;
            SpringPanel.Begin(panel.cachedGameObject, offset, 6f);
        }
    }
}
