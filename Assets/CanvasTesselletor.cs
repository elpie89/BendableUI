using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CanvasTesselletor : MonoBehaviour,IMeshModifier {
    public void ModifyMesh(Mesh mesh)
    {
        throw new System.NotImplementedException();
    }

    public int subdivision = 0;
    
    public void ModifyMesh(VertexHelper verts)
    {
        List<UIVertex> vList = new List<UIVertex>();
        verts.GetUIVertexStream(vList);
        verts.Clear();

        List<int> indices = new List<int>();
        List<UIVertex> vertices = new List<UIVertex>();

        for (int i = 0; i < vList.Count; i = i+2)
        {
            TringleSubdivision(vList[0], vList[1], vList[2],ref indices, ref vertices);
        }       

        verts.AddUIVertexStream(vList, indices.ToList());
    }


    public void TringleSubdivision(UIVertex a, UIVertex b, UIVertex c, ref List<int> indices, ref List<UIVertex> vertices)
    {
        vertices.Add(a);
        vertices.Add(b);
        vertices.Add(c);

        UIVertex d = new UIVertex();
        d.position = (a.position + b.position) / 2;
        vertices.Add(d);

        UIVertex e = new UIVertex();
        e.position = (b.position + c.position) / 2;
        vertices.Add(e);

        UIVertex f = new UIVertex();
        f.position = (c.position + a.position) / 2;
        vertices.Add(f);



    }

}
