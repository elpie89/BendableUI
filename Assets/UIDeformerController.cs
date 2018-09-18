using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class UIDeformerController : MonoBehaviour
{
    [SerializeField]
    private UIDeformer.BendMode bendType;

    [SerializeField]
    [Range(0, 4)]
    private int subdivisionLevel = 0;

    [SerializeField]
    private Transform bendPivot;

    [SerializeField]
    private RectTransform rectTransform;

    [SerializeField]
    private float curvatureK;

    private List<UIDeformer> uiDeformerList;
    private UIDeformerPivot deformerPivot;


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
        CreatePivot();
        AddRequiredComponent();        
    }

    private void AddRequiredComponent()
    {
        Debug.Log("component initialization");
        uiDeformerList = new List<UIDeformer>();

        Transform[] children = GetComponentsInChildren<Transform>(true);
        foreach (Transform g in children)
        {
            if (g.gameObject.Equals(this.gameObject) || g.gameObject.Equals(deformerPivot.gameObject))
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

    private void CreatePivot()
    {
        deformerPivot = GetComponentInChildren<UIDeformerPivot>();
        if (deformerPivot == null)
        {
            GameObject pivot = new GameObject("DeformerPivot");
            pivot.transform.SetParent(this.transform);
            deformerPivot = pivot.AddComponent<UIDeformerPivot>();
        }
        bendPivot = deformerPivot.transform;
    }
    

    protected void OnValidate()
    {
        foreach (UIDeformer deformer in uiDeformerList)
        {
            deformer.BendType = bendType;
            deformer.SubdivisionLevel = subdivisionLevel;
            deformer.BendPivot = bendPivot;
            deformer.RectTransform = rectTransform;
            deformer.CurvatureK = curvatureK;
            deformer.Dirty = true;
        }
    }

}
