using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SumoJunction : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected MeshRenderer meshRenderer = null;
    [SerializeField] protected MeshFilter meshFilter = null;

    protected string junctionId = null;
    protected List<Vector2> shape = new List<Vector2>();
    protected List<Vector2> polygon = new List<Vector2>();

    protected Vector2 position = new Vector2();

    public void Init(JunctionInitData initData) {
        this.meshRenderer.material = initData.material;

        this.gameObject.name = "Junction_ID: " + initData.junctionId;

        this.junctionId = initData.junctionId;
        this.position = new Vector2(
            (float)initData.position[0],
            (float)initData.position[1]
        );
        this.transform.position = new Vector3(this.position.x, 0, this.position.y);

        foreach (List<double> vertexPos in initData.shape) {
            Vector2 vertex = new Vector2(
                (float)vertexPos[0] - this.position.x,
                (float)vertexPos[1] - this.position.y
            );

            this.shape.Add(vertex);
        }
        this.polygon = this.GetLargestConvexPolygon(new List<Vector2>(this.shape));

        this.meshFilter.mesh = this.BuildMesh(this.polygon);
    }

    protected Mesh BuildMesh(List<Vector2> polygon) {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[polygon.Count];
        int[] tris = new int[3 * (polygon.Count - 2)];
        Vector3[] normals = new Vector3[polygon.Count];
        Vector2[] uv = new Vector2[polygon.Count];

        vertices[0] = new Vector3(polygon[0].x, 0, polygon[0].y);
        vertices[1] = new Vector3(polygon[1].x, 0, polygon[1].y);
        normals[0] = Vector3.up;
        normals[1] = Vector3.up;
        uv[0] = Vector3.zero;
        uv[1] = Vector3.zero;

        for (int i = 2; i < this.polygon.Count; i++) {
            vertices[i] = new Vector3(
                polygon[i].x,  
                0, 
                polygon[i].y
            );

            normals[i] = Vector3.up;
            uv[i] = Vector3.zero;

            tris[(3 * (i - 1)) - 3] = 0;
            tris[(3 * (i - 1)) - 2] = i;
            tris[(3 * (i - 1)) - 1] = i - 1;
        }

        mesh.vertices = vertices;
        mesh.triangles = tris;
        mesh.normals = normals;
        mesh.uv = uv;
        return mesh;
    }

    protected List<Vector2> GetLargestConvexPolygon(List<Vector2> vertices) {
        List<Vector2> maxHull = new List<Vector2>();

        this.SortListByAngle(vertices);

        maxHull.Add(vertices[0]);
        maxHull.Add(vertices[1]);
        maxHull.Add(vertices[2]);

        for(int i = 3; i < vertices.Count; i++) {
            Vector2 candidate = vertices[i];

            while (
                !this.IsLeftTurn(
                    maxHull[maxHull.Count - 2],
                    maxHull[maxHull.Count - 1],
                    candidate
                )
            ) {
                maxHull.RemoveAt(maxHull.Count - 1);
            }

            maxHull.Add(candidate);
        }

        return maxHull;
    }

    protected bool IsLeftTurn(Vector2 a, Vector2 b, Vector2 c) {
        float checkAngle = Vector2.SignedAngle(
            a - b,
            c - b
        );

        return checkAngle < 0;
    }

    protected void SortListByAngle(List<Vector2> vertices) {
        int originIndex = this.GetIndexOfSmallestYCoord(vertices);
        this.SwapIndices(0, originIndex, vertices);

        Vector2 origin = vertices[0];

        int examineIndex = 0;
        for (int i = 2; i < vertices.Count; i++) {
            examineIndex = i;
            Vector2 examineVertex = vertices[i];
            float examineAngle = Vector2.Angle(examineVertex - origin, Vector2.right);
            
            int j = i - 1;

            while(j != 0) {
                Vector2 currentVertex = vertices[j];
                float currentAngle = Vector2.Angle(currentVertex - origin, Vector2.right);

                if (examineAngle < currentAngle) {
                    this.SwapIndices(examineIndex, j, vertices);
                    examineIndex = j;
                }

                j--;
            }
        }


        string test = "[ ";

        for (int i = 0; i < vertices.Count; i++) {
            Vector2 currentVertex = vertices[i];
            float currentAngle = Vector2.Angle(currentVertex - origin, Vector2.right);

            test += vertices[i].ToString() + ": " + $"{currentAngle}" +"    ";
        }

        examineIndex = 1;
        while (examineIndex < vertices.Count - 1) {
            Vector2 examineVertex = vertices[examineIndex];
            Vector2 nextVertex = vertices[examineIndex + 1];

            float examineAngle = Vector2.Angle(examineVertex - origin, Vector2.right);
            float nextAngle = Vector2.Angle(nextVertex - origin, Vector2.right);

            if (examineAngle == nextAngle) {

                if(examineVertex.magnitude > nextVertex.magnitude) {
                    vertices.RemoveAt(examineIndex + 1);
                } else {
                    vertices.RemoveAt(examineIndex);
                }

            } else {
                examineIndex++;
            }

        }
    }

    protected void SwapIndices(int index1, int index2, List<Vector2> vertices) {
        Vector2 temp = vertices[index1];
        vertices[index1] = vertices[index2];
        vertices[index2] = temp;
    }

    protected int GetIndexOfSmallestYCoord(List<Vector2> vertices) {
        int smallestIndex = 0;

        for(int i = 1; i < vertices.Count; i++) {
            Vector2 smallest = vertices[smallestIndex];
            Vector2 vertex = vertices[i];

            if(vertex.y < smallest.y) {
                smallestIndex = i;
            } else if(vertex.y == smallest.y) {
                if (vertex.x < smallest.x) {
                    smallestIndex = i;
                }
            }
        }

        return smallestIndex;
    }

    
}
