using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDeformerPivot : MonoBehaviour {

    protected void OnDrawGizmosSelected()
    {
        //UIDeformerController deformerController = new UIDeformerController();
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(transform.position, new Vector3(0.01f, 100000000, 0.01f));
    }
}
