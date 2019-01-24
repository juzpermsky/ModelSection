using System.Collections.Generic;
using UnityEngine;

public class Section : MonoBehaviour
{
    public Transform objTransform;
    private Mesh mesh;

    private void Start()
    {
        /*
        // save the parent GO-s pos+rot
        Vector3 position = objTransform.position;
        Quaternion rotation = objTransform.rotation;

        // move to the origin for combining
        objTransform.position = Vector3.zero;
        objTransform.rotation = Quaternion.identity;
        */
        var meshFilter = objTransform.GetComponent<MeshFilter>();
        /*
        var combiner = new List<CombineInstance>();

        for (var i = 0; i < meshFilter.sharedMesh.subMeshCount; i++)
        {
            var ci = new CombineInstance();
            ci.mesh = meshFilter.sharedMesh;
            ci.subMeshIndex = i;
            ci.transform = meshFilter.transform.localToWorldMatrix;
            combiner.Add(ci);
        }

        objTransform.gameObject.SetActive(false);
        objTransform.GetComponent<MeshFilter>().mesh = new Mesh();
        objTransform.GetComponent<MeshFilter>().mesh.CombineMeshes(combiner.ToArray(),true,true);
        objTransform.gameObject.SetActive(true);
        */


        mesh = objTransform.GetComponent<MeshFilter>().mesh;
        Debug.Log(mesh.subMeshCount);
        Debug.Log(mesh.GetTriangles(0).Length);
        Debug.Log(mesh.GetTriangles(1).Length);
        Debug.Log(mesh.GetTriangles(2).Length);
        Debug.Log(mesh.GetTriangles(3).Length);
        Debug.Log(mesh.GetTriangles(4).Length);
        Debug.Log(mesh.GetTriangles(5).Length);

        objTransform.gameObject.SetActive(false);
        mesh.SetTriangles(mesh.triangles, 0);
        mesh.subMeshCount = 1;
        objTransform.gameObject.SetActive(true);

        Debug.Log(mesh.subMeshCount);
        Debug.Log(mesh.GetTriangles(0).Length);
        /*
        // restore the parent GO-s pos+rot
        transform.position = position;
        transform.rotation = rotation;
        */
    }

    private void Update()
    {
        Do();
    }


    private void Do()
    {
        var triangles = mesh.GetTriangles(0);
        var vertices = mesh.vertices;

        var lessIndices = new List<int>();
        var lessPoints = new List<Vector3>();
        var greaterIndices = new List<int>();
        var greaterPoints = new List<Vector3>();
        for (var i = 0; i < triangles.Length-3; i += 3)
        {
            // идем по треугольникам меша
            lessIndices.Clear();
            lessPoints.Clear();
            greaterIndices.Clear();
            greaterPoints.Clear();
            for (var j = 0; j < 3; j++)
            {
                // смотрим вершины треугольника
                // определяем индекс вершины
                var index = triangles[i + j];
                // положение вершины в системе координат плоскости сечения 
                var localPos = transform.InverseTransformPoint(objTransform.TransformPoint(vertices[index]));
                if (localPos.z < 0)
                {
                    // запоминаем вершины, которые ниже плоскости сечения
                    lessIndices.Add(index);
                    lessPoints.Add(localPos);
                }
                else
                {
                    // запоминаем вершины, которые выше плоскости сечения
                    greaterIndices.Add(index);
                    greaterPoints.Add(localPos);
                }
            }

            if (lessIndices.Count > 0 && greaterIndices.Count > 0)
            {
                // Плоскость сечения пересекает этот треугольник
                var x1 = 0f;
                var x2 = 0f;
                var y1 = 0f;
                var y2 = 0f;

                if (lessIndices.Count == 1)
                {
                    // одна вершина снизу, две сверху
                    x1 = lessPoints[0].x - lessPoints[0].z * (greaterPoints[0].x - lessPoints[0].x) /
                         (greaterPoints[0].z - lessPoints[0].z);
                    x2 = lessPoints[0].x - lessPoints[0].z * (greaterPoints[1].x - lessPoints[0].x) /
                         (greaterPoints[1].z - lessPoints[0].z);
                    y1 = lessPoints[0].y - lessPoints[0].z * (greaterPoints[0].y - lessPoints[0].y) /
                         (greaterPoints[0].z - lessPoints[0].z);
                    y2 = lessPoints[0].y - lessPoints[0].z * (greaterPoints[1].y - lessPoints[0].y) /
                         (greaterPoints[1].z - lessPoints[0].z);
                }
                else
                {
                    // одна вершина сверху, две снизу
                    x1 = greaterPoints[0].x - greaterPoints[0].z * (lessPoints[0].x - greaterPoints[0].x) /
                         (lessPoints[0].z - greaterPoints[0].z);
                    x2 = greaterPoints[0].x - greaterPoints[0].z * (lessPoints[1].x - greaterPoints[0].x) /
                         (lessPoints[1].z - greaterPoints[0].z);
                    y1 = greaterPoints[0].y - greaterPoints[0].z * (lessPoints[0].y - greaterPoints[0].y) /
                         (lessPoints[0].z - greaterPoints[0].z);
                    y2 = greaterPoints[0].y - greaterPoints[0].z * (lessPoints[1].y - greaterPoints[0].y) /
                         (lessPoints[1].z - greaterPoints[0].z);
                }
                // Рисуем линию пересечения
                Debug.DrawLine(transform.TransformPoint(x1,y1,0),transform.TransformPoint(x2,y2,0),Color.green );
            }
        }
    }
}