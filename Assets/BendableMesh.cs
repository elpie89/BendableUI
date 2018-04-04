using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BendableMesh : MonoBehaviour, IMeshModifier
{
    public int subdivisionLevel = 0;

    public void ModifyMesh(Mesh mesh)
    {
        throw new System.NotImplementedException();
    }

    public void ModifyMesh(VertexHelper verts)
    {
        for (int sub = 0; sub < subdivisionLevel; sub++)
        {
            List<UIVertex> stream = new List<UIVertex>();
            verts.GetUIVertexStream(stream);

            UIVertex[] streamArray = stream.ToArray();

            List<UIVertex> triangles = new List<UIVertex>();

            for (int i = 0; i < stream.Count; i = i + 3)
            {
                triangles.AddRange(SubdivedTringle(streamArray[i], streamArray[i + 1], streamArray[i + 2]));
            }

            verts.Clear();
            verts.AddUIVertexTriangleStream(new List<UIVertex>(triangles));//, indices);
        }
    }

    private List<UIVertex> SubdivedTringle(UIVertex a, UIVertex b, UIVertex c)
    {
        List<UIVertex> subdTriangles = new List<UIVertex>();

        UIVertex d = new UIVertex();
        InitVertex(ref d, a, b);

        UIVertex e = new UIVertex();
        InitVertex(ref e, b, c);


        UIVertex f = new UIVertex();
        InitVertex(ref f, c, a);

        subdTriangles.Add(a); subdTriangles.Add(d); subdTriangles.Add(f);
        subdTriangles.Add(d); subdTriangles.Add(b); subdTriangles.Add(e);
        subdTriangles.Add(f); subdTriangles.Add(d); subdTriangles.Add(e);
        subdTriangles.Add(f); subdTriangles.Add(e); subdTriangles.Add(c);

        return subdTriangles;

    }

    private void InitVertex(ref UIVertex c , UIVertex a, UIVertex b)
    {
        c.position = (a.position + b.position) * 0.5f;
        c.color = Color32.Lerp(a.color, b.color, 0.5f);
        c.uv0 = (a.uv0 + b.uv0) * 0.5f;
        c.uv1 = (a.uv1 + b.uv1) * 0.5f;
        c.uv2 = (a.uv2 + b.uv2) * 0.5f;
        c.uv3 = (a.uv3 + b.uv3) * 0.5f;
        c.normal = (a.normal + b.normal) * 0.5f;
        c.tangent = (a.normal + b.normal) * 0.5f;
    }
}
