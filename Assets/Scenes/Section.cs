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

    private void Do()
    {
        var triangles = mesh.GetTriangles(0);
        var vertices = mesh.vertices;
        
        var lessIndices = new List<int>();
        var lessPoints = new List<Vector3>();
        var greaterIndices = new List<int>();
        var greaterPoints = new List<Vector3>();
        for (var i = 0; i < triangles.Length; i += 3)
        {
            lessIndices.Clear();
            lessPoints.Clear();
            greaterIndices.Clear();
            greaterPoints.Clear();
            for (var j = 0; j < 3; j++)
            {
                var index = i + j;
                var localPos = transform.InverseTransformPoint(objTransform.TransformPoint(vertices[index]));
                if (localPos.z < 0)
                {
                    lessIndices.Add(index);
                    lessPoints.Add(localPos);
                }
                else
                {
                    greaterIndices.Add(index);
                    greaterPoints.Add(localPos);
                }
            }

            if (lessIndices.Count > 0 && greaterIndices.Count > 0)
            {
                // Плоскость сечения пересекает этот треугольник
                //todo: вычисляю линию пересечения
            }
        }
    }
}