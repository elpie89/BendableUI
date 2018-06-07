using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class CanvasDeformer : MonoBehaviour
{

    private GraphicTasselletor[] childs;
    public int subdivisionLevel = 0;

    public enum BendType
    {
        Sphere,
        CylinderX,
        CylinderY
    }

    public BendType typeOfBend;

    public Transform bendPivot;
    public RectTransform rectTransform;
    private Image img;

    public float curvatureK;
    

    protected void Start()
    {
        img = GetComponent<Image>();
    }

    protected void Update()
    {
        if (img != null)
        {
            img.SetAllDirty();
        }

    }
    

    protected void OnValidate()
    {
        Debug.Log("validate");
        childs = GetComponentsInChildren<GraphicTasselletor>();
        foreach (GraphicTasselletor tasselletor in childs)
        {
            tasselletor.BendPivot = bendPivot;
            tasselletor.RectTransform = rectTransform;
            tasselletor.SubdivisionLevel = subdivisionLevel;
            tasselletor.CurvatureK = curvatureK;
            //tasselletor.GetComponent<Image>().Rebuild(CanvasUpdate.MaxUpdateValue);
            tasselletor.Dirty = true;
        }
    }
    
}
