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
    
    protected void OnEnable()
    {
        //should be moved to onAttach
        UIDeformerPivot deformerPivot = GetComponentInChildren<UIDeformerPivot>();
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
        UIDeformer[] childs = GetComponentsInChildren<UIDeformer>();
        foreach (UIDeformer deformer in childs)
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
