using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(Canvas))]
public class UIDeformerController : MonoBehaviour
{
    [SerializeField]
    private UIDeformer.BendMode bendType;

    [SerializeField]
    [Range(0, 4)]
    private int subdivisionLevel = 0;

    [SerializeField]
    [Range(-1,1)]
    private float bendPivot = 0;

    [SerializeField]
    private float curvatureK;

    private List<UIDeformer> uiDeformerList;
    private Canvas canvas;
    private Bounds canvasBound;
    private RectTransform rectTransform;

    protected void Awake()
    {
#if UNITY_EDITOR
        Initialize();        
#endif
    }

    protected void Reset()
    {
        Initialize();
    }

    private void Initialize()
    {
        canvas = GetComponent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        AddRequiredComponent();        
    }

    private void AddRequiredComponent()
    {
        uiDeformerList = new List<UIDeformer>();

        Transform[] children = GetComponentsInChildren<Transform>(true);
        foreach (Transform g in children)
        {
            if (g.gameObject.Equals(this.gameObject))
            {
                continue;
            }

            UIDeformer deformer = g.GetComponent<UIDeformer>();
            if (deformer != null)
            {
                DestroyImmediate(deformer);
            }
            deformer = g.gameObject.AddComponent<UIDeformer>();
            uiDeformerList.Add(deformer);
        }
    }

    protected Bounds CalculateBounds(RectTransform transform, float uiScaleFactor)
    {
        Bounds bounds = new Bounds(transform.position, new Vector3(transform.rect.width, transform.rect.height, 0.0f) * uiScaleFactor);
        RectTransform[] children = GetComponentsInChildren<RectTransform>(true);
        if (transform.childCount > 0)
        {
            foreach (RectTransform child in children)
            {
                Bounds childBounds = new Bounds(child.position, new Vector3(child.rect.width, child.rect.height, 0.0f) * uiScaleFactor);
                bounds.Encapsulate(childBounds);
            }
        }

        return bounds;
    }


    protected void OnValidate()
    {
        foreach (UIDeformer deformer in uiDeformerList)
        {
            deformer.BendType = bendType;
            deformer.SubdivisionLevel = subdivisionLevel;
            deformer.BendPivot = bendPivot;
            deformer.CanvasCenter = transform.position;
            deformer.RectTransform = rectTransform;
            deformer.CurvatureK = curvatureK;
            deformer.Dirty = true;
        }
    }

}
