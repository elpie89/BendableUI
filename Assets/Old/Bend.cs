using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bend : MonoBehaviour {

    public GameObject[] pointList;
    public Transform bendPivot;
    public Dictionary<GameObject,Vector3> positions;

    public float curvatureK;
    public float bendOffset;
    float zMin;
    float zMax;

    void Start () {
        
        positions = new Dictionary<GameObject, Vector3>();

        foreach (GameObject g in pointList)
        {
            positions.Add(g, g.transform.position);
        }
    }

    void Update ()
    {
        zMin = bendPivot.position.z - bendOffset;
        zMax = bendPivot.position.z + bendOffset;
        foreach (GameObject g in pointList)
        {
            float yBend = 0;
            Vector3 pos;
            positions.TryGetValue(g, out pos);
            float z = pos.z;
            float zo = bendPivot.position.z;


            if (z <= zMin)
            {
                yBend = zMin;
            }
            else if (z > zMin && z < zMax)
            {
                yBend = z;
            }
            else if (z >= zMax)
            {
                yBend = zMax;
            }
            else
            {
                print("there is something wrong here");
            }

            float tetaAngle = curvatureK * (yBend - zo);
            float cos = Mathf.Cos(tetaAngle);
            float sin = Mathf.Sin(tetaAngle);
            print(string.Format("yBend for {0} is {1}, end teta is {2}", g.name, yBend, tetaAngle));

            float newZ = 0;
            float newX = 0;
            float oldX = pos.x;

            if (z >= zMin && z <= zMax)
            {
                newZ = -sin * (oldX - 1 / curvatureK) + zo;
                newX = cos * (oldX - 1 / curvatureK) + 1 / curvatureK;
            }
            else if (z < zMin)
            {
                newZ = -sin * (oldX - 1 / curvatureK) + zo + cos * (z - zMin);
                newX = cos * (oldX - 1 / curvatureK) + 1 / curvatureK + sin * (z - zMin);
            }
            else if (z > zMax)
            {
                newZ = -sin * (oldX - 1 / curvatureK) + zo + cos * (z - zMax);
                newX = cos * (oldX - 1 / curvatureK) + 1 / curvatureK + sin * (z - zMax);
            }
            else
            {
                print("there is something wrong here 2");
            }

            g.transform.position = new Vector3(newX, pos.y, newZ);

        }
    }
	
}
