using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BendableMesh : MonoBehaviour, IMeshModifier
{
    public enum BendType
    {
        Sphere,
        CylinderX,
        CylinderY
    }

    public BendType typeOfBend;

    public int subdivisionLevel = 0;
    public Transform bendPivot;
    private RectTransform rectTransform;
    private Image img;

    public float curvatureK;
    float xMin;
    float xMax;
    float yMin;
    float yMax;

    private void Start()
    {
        img = GetComponent<Image>();
    }

    public void ModifyMesh(Mesh mesh)
    {
        throw new System.NotImplementedException();
    }

    public void ModifyMesh(VertexHelper verts)
    {
        rectTransform = GetComponent<RectTransform>();
       
        if (subdivisionLevel < 1)
        {
            return;
        }
        else
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

            if (typeOfBend == BendType.CylinderY)
            {
                BendAlongY(verts);
            }
            if (typeOfBend == BendType.CylinderX)
            {
                BendAlongX(verts);
            }
            if (typeOfBend == BendType.Sphere)
            {
                BendAlongX(verts);
                BendAlongY(verts);
                //BendAlongXY(verts);
            }
        }

       
    }

    private void Update()
    {
        if (img != null)
        {
            img.SetAllDirty();
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

    private void BendAlongY(VertexHelper vh)
    {
        xMin = bendPivot.position.x - (rectTransform.rect.width / 2);
        xMax = bendPivot.position.x + (rectTransform.rect.width / 2);
        List<UIVertex> vertices = new List<UIVertex>();

        vh.GetUIVertexStream(vertices);
        for (int i = 0; i < vh.currentIndexCount; i++)
        {
            UIVertex v = new UIVertex();
            vh.PopulateUIVertex(ref v, i);

            float bendRange = 0;
            float x = v.position.x;
            float xo = bendPivot.position.x;


            if (x <= xMin)
            {
                bendRange = xMin;
            }
            else if (x > xMin && x < xMax)
            {
                bendRange = x;
            }
            else if (x >= xMax)
            {
                bendRange = xMax;
            }
            else
            {
                print("there is something wrong here");
            }

            float curvFactor = curvatureK / rectTransform.rect.width;

            float tetaAngle = curvFactor * (bendRange - xo);
            float cos = Mathf.Cos(tetaAngle);
            float sin = Mathf.Sin(tetaAngle);
            //print(string.Format("yBend for {0} is {1}, end teta is {2}", g.name, yBend, tetaAngle));

            float Xnew = 0;
            float Znew = 0;
            float oldZ = v.position.z;

            if (x >= xMin && x <= xMax)
            {
                Xnew = -sin * (oldZ - 1 / curvFactor) + xo;
                Znew = cos * (oldZ - 1 / curvFactor) + 1 / curvFactor;
            }
            else if (x < xMin)
            {
                Xnew = -sin * (oldZ - 1 / curvFactor) + xo + cos * (x - xMin);
                Znew = cos * (oldZ - 1 / curvFactor) + 1 / curvFactor + sin * (x - xMin);
            }
            else if (x > xMax)
            {
                Xnew = -sin * (oldZ - 1 / curvFactor) + xo + cos * (x - xMax);
                Znew = cos * (oldZ - 1 / curvFactor) + 1 / curvFactor + sin * (x - xMax);
            }
            else
            {
                print("there is something wrong here 2");
            }

            v.position.x = Xnew;
            v.position.z = Znew;

            vh.SetUIVertex(v, i);

        }



    }

    private void BendAlongX(VertexHelper vh)
    {
        xMin = bendPivot.position.y - (rectTransform.rect.height / 2);
        xMax = bendPivot.position.y + (rectTransform.rect.height / 2);
        List<UIVertex> vertices = new List<UIVertex>();

        vh.GetUIVertexStream(vertices);
        for (int i = 0; i < vh.currentIndexCount; i++)
        {
            UIVertex v = new UIVertex();
            vh.PopulateUIVertex(ref v, i);

            float bendRange = 0;
            float x = v.position.y;
            float xo = bendPivot.position.y;


            if (x <= xMin)
            {
                bendRange = xMin;
            }
            else if (x > xMin && x < xMax)
            {
                bendRange = x;
            }
            else if (x >= xMax)
            {
                bendRange = xMax;
            }
            else
            {
                print("there is something wrong here");
            }

            float curvFactor = curvatureK / rectTransform.rect.height;

            float tetaAngle = curvFactor * (bendRange - xo);
            float cos = Mathf.Cos(tetaAngle);
            float sin = Mathf.Sin(tetaAngle);
            //print(string.Format("yBend for {0} is {1}, end teta is {2}", g.name, yBend, tetaAngle));

            float Ynew = 0;
            float Znew = 0;
            float oldZ = v.position.z;

            if (x >= xMin && x <= xMax)
            {
                Ynew = -sin * (oldZ - 1 / curvFactor) + xo;
                Znew = cos * (oldZ - 1 / curvFactor) + 1 / curvFactor;
            }
            else if (x < xMin)
            {
                Ynew = -sin * (oldZ - 1 / curvFactor) + xo + cos * (x - xMin);
                Znew = cos * (oldZ - 1 / curvFactor) + 1 / curvFactor + sin * (x - xMin);
            }
            else if (x > xMax)
            {
                Ynew = -sin * (oldZ - 1 / curvFactor) + xo + cos * (x - xMax);
                Znew = cos * (oldZ - 1 / curvFactor) + 1 / curvFactor + sin * (x - xMax);
            }
            else
            {
                print("there is something wrong here 2");
            }

            v.position.y = Ynew;
            v.position.z = Znew;

            vh.SetUIVertex(v, i);

        }



    }

    private void BendAlongXY(VertexHelper vh)
    {
        yMin = bendPivot.position.y - (rectTransform.rect.height / 2);
        yMax = bendPivot.position.y + (rectTransform.rect.height / 2);
        xMin = bendPivot.position.x - (rectTransform.rect.width / 2);
        xMax = bendPivot.position.x + (rectTransform.rect.width / 2);

        List<UIVertex> vertices = new List<UIVertex>();

        vh.GetUIVertexStream(vertices);
        for (int i = 0; i < vh.currentIndexCount; i++)
        {
            UIVertex v = new UIVertex();
            vh.PopulateUIVertex(ref v, i);

            float bendRangeY = 0;
            float y = v.position.y;
            float yo = bendPivot.position.y;
            float bendRangeX = 0;
            float x = v.position.x;
            float xo = bendPivot.position.x;


            if (x <= xMin)
            {
                bendRangeX = xMin;
            }
            else if (x > xMin && x < xMax)
            {
                bendRangeX = x;
            }
            else if (x >= xMax)
            {
                bendRangeX = xMax;
            }
            else
            {
                print("there is something wrong here");
            }


            if (y <= yMin)
            {
                bendRangeY = yMin;
            }
            else if (y > yMin && y < yMax)
            {
                bendRangeY = y;
            }
            else if (y >= yMax)
            {
                bendRangeY = yMax;
            }
            else
            {
                print("there is something wrong here");
            }

            float curvFactor = curvatureK / rectTransform.rect.height;

            float tetaAngle = curvFactor * (bendRangeY - yo);
            float cos = Mathf.Cos(tetaAngle);
            float sin = Mathf.Sin(tetaAngle);

            float alphaAngle = curvFactor * (bendRangeX - xo);
            float cosAlpha = Mathf.Cos(alphaAngle);
            float sinAlpha = Mathf.Sin(alphaAngle);
            //print(string.Format("yBend for {0} is {1}, end teta is {2}", g.name, yBend, tetaAngle));

            float Xnew = 0;
            float Ynew = 0;
            float Znew = 0;
            float oldZ = v.position.z;

            if (y >= yMin && y <= yMax)
            {
                Ynew = -sin * (oldZ - 1 / curvFactor) + yo;
                Znew = cos * (oldZ - 1 / curvFactor) + 1 / curvFactor;
            }
            else if (y < yMin)
            {
                Ynew = -sin * (oldZ - 1 / curvFactor) + yo + cos * (y - yMin);
                Znew = cos * (oldZ - 1 / curvFactor) + 1 / curvFactor + sin * (y - yMin);
            }
            else if (y > yMax)
            {
                Ynew = -sin * (oldZ - 1 / curvFactor) + yo + cos * (y - yMax);
                Znew = cos * (oldZ - 1 / curvFactor) + 1 / curvFactor + sin * (y - yMax);
            }
            else
            {
                print("there is something wrong here 2");
            }


            if (x >= xMin && x <= xMax)
            {
                Xnew = -sinAlpha * (oldZ - 1 / curvFactor) + xo;
                Znew = cosAlpha * (oldZ - 1 / curvFactor) + 1 / curvFactor;
            }
            else if (x < xMax)
            {
                Xnew = -sinAlpha * (oldZ - 1 / curvFactor) + xo + cosAlpha * (x - xMin);
                Znew = cosAlpha * (oldZ - 1 / curvFactor) + 1 / curvFactor + sinAlpha * (x - xMin);
            }
            else if (x > xMax)
            {
                Xnew = -sinAlpha * (oldZ - 1 / curvFactor) + xo + cosAlpha * (x - xMax);
                Znew = cosAlpha * (oldZ - 1 / curvFactor) + 1 / curvFactor + sinAlpha * (x - xMax);
            }
            else
            {
                print("there is something wrong here 2");
            }




            v.position.x = Xnew;
            v.position.y = Ynew;
            v.position.z = Znew;

            vh.SetUIVertex(v, i);

        }



    }
}
