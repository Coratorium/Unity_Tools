using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PerlinNoiseTerrain : MonoBehaviour
{
    [Header("Values To Play with:")]
    public int meshWidth = 10;
    public int meshHeight = 10;
    [Range(0,1)]
    public float perlinValue1 = 0.053f;
    [Range(0, 1)]
    public float perlinValue2 =0.5f;
    [Range(0, 50)]
    public float oszilate1 = 16.6f;
    [Range(0, 50)]
    public float oszilate2 = 27f;

    [Header("Move the Mesh:")]
    public bool moveMesh = false;
    public float meshSpeed = 1;

    [Header("Variable Display:")]
    public float perlinX;
    public float perlinY;

    private Vector3[] vertices;
    private int[] triangles;
    private Mesh ground;

    private float oldPerlin1;
    private float oldPerlin2;
    private float oldOsz1;
    private float oldOsz2;

    public float minY = 0;
    public float maxY = 0;

    private Color[] colorVertexes;

    [Header("Choose a name:")]
    public string fileName = "unnamed";
    [Tooltip("If folder Name is unnamed it will be saved directly into Assets")]
    public string folderName = "unnamed";

    void Start()
    {
        ground = new Mesh();
        GetComponent<MeshFilter>().mesh = ground;
        createGround();
        refreshMesh();
    }
    
    private void createGround()
    {
        //verticies
        int xAmount = meshWidth + 1;
        int zAmount = meshHeight + 1;
        vertices = new Vector3[xAmount * zAmount];
        int vertCounter = 0;
        colorVertexes = new Color[vertices.Length];
        for (int x=0; x<xAmount; x++)
        {
            for (int z = 0; z < zAmount; z++)
            {
                float y = Mathf.PerlinNoise((x * perlinValue1)+perlinX, (z * perlinValue1)+perlinY) * oszilate1;
                y = y + ((Mathf.PerlinNoise((x * perlinValue2) + perlinX, (z * perlinValue2) + perlinY) /3) * oszilate2);

                if (y > maxY)
                {
                    maxY = y;
                }
                if (y < minY)
                {
                    minY = y;
                }
                if (y < 10)
                {
                    y = 10;
                    colorVertexes[vertCounter] = Color.gray;
                }
                else
                {
                    colorVertexes[vertCounter] = Color.yellow;
                }

                y = y - 0.5f;

                vertices[vertCounter] = new Vector3(x,y,z);
                vertCounter++;
                
            }
        }
        //triangles = new int[meshHeight*meshWidth*3];
        triangles = new int[meshHeight*meshWidth*6];
        int sqCounter = 0;
        int verts = 0;
        for (int x=0; x<meshWidth; x++)
        {
            for (int z = 0; z < meshHeight; z++)
            {
                triangles[0 + sqCounter] = verts + 0;
                triangles[1 + sqCounter] = verts + 1;
                triangles[2 + sqCounter] = verts + meshHeight + 1;

                triangles[3 + sqCounter] = verts + meshHeight + 1;
                triangles[4 + sqCounter] = verts + 1;
                triangles[5 + sqCounter] = verts + meshHeight + 2;
                sqCounter += 6;
                verts++;
            }
            verts++;
        }
    }
    void refreshMesh()
    {
        ground.Clear();
        ground.vertices = vertices;
        ground.triangles = triangles;
        ground.colors = colorVertexes;
        ground.RecalculateNormals();
    }
    private void Update()
    {
        if (oldOsz1 != oszilate1 || oldOsz1 != oszilate1|| oldPerlin1 != perlinValue1 || oldPerlin2 != perlinValue2)
        {
            
            oldOsz1 = oszilate1;
            oldOsz2 = oszilate2;
            oldPerlin1 = perlinValue1;
            oldPerlin2 = perlinValue2;
        }
        createGround();
        refreshMesh();
        if (moveMesh)
        {
            perlinX += (Time.deltaTime / 10) * meshSpeed;
            perlinY += (Time.deltaTime / 7) * meshSpeed;
        }

    }
    private void OnDrawGizmos()
    {
        /*
        if (vertices != null)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                Gizmos.DrawSphere(vertices[i], 0.1f);
            }
        }
        */
        
    }
    public void saveAsFile()
    {
        var savePath = "Assets/";
        if (!folderName.Equals("unnamed"))
        {
            savePath += folderName + "/";
        }
        savePath +=  fileName + ".asset";
        Debug.Log("Saved Mesh to:" + savePath);
        AssetDatabase.CreateAsset(ground, savePath);
    }
}
