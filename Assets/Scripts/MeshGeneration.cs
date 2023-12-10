using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class MeshGeneration : MonoBehaviour
{

    [SerializeField] MeshFilter meshFilter;
    [SerializeField] MeshFilter testMesh;
    [SerializeField] int width;
    [SerializeField] int height;
    [SerializeField] int heightAmplifier;
    [SerializeField] int cellSize;
    float[,] noiseMap;
    [SerializeField] int levelOfDetail=1;
    [SerializeField] MeshCollider meshCollider;
    private static MeshGeneration _Instance;
    public static MeshGeneration Instance
    { 
        get
        {
            if (!_Instance)
            {
                _Instance = new GameObject().AddComponent<MeshGeneration>();
            }
            return _Instance;
        }
    }
    // Start is called before the first frame update
    void Start()
    { 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        if (_Instance != null && _Instance != this)
        {
            Destroy(Instance);
        }
        else
        {
            _Instance = this;
        }
    }


    public void GenerateMesh()
    {
        Mesh mesh = new Mesh();
        Vector3[] verts = new Vector3[width*height];
        Vector2[] uvs=new Vector2[width*height];
        int[] tris =new int[(width-1)*(height-1)*6];
        float[,] noiseMap = Perlin.Instance.GetNoiseMap();
        int vertexIndex, triangleIndex;
        vertexIndex = 0;
        triangleIndex = 0;

        for(int i=0; i<width; i++)
        {
            for (int j=0; j<height; j++)
            {
                verts[vertexIndex].x = -width / 2 + i * cellSize;
                verts[vertexIndex].z = -height / 2 + j * cellSize;
                verts[vertexIndex].y = noiseMap[i,j]*heightAmplifier;
                float uvWidth, uvHeight;
                uvWidth = (float)i / width;
                uvHeight=(float)j/height;
                uvs[vertexIndex] = new Vector2(uvWidth,uvHeight);

                if(i<width-1&&j<height-1)
                {
                    
                    tris[triangleIndex] = vertexIndex;
                    tris[triangleIndex + 1] = vertexIndex + 1;
                    tris[triangleIndex + 2] = vertexIndex + (width);

                    tris[triangleIndex + 3] = vertexIndex + (width);
                    tris[triangleIndex + 4] = vertexIndex + 1;
                    tris[triangleIndex + 5] = vertexIndex + (width) + 1;

                    triangleIndex = triangleIndex + 6;

                }
                vertexIndex++;


            }
        }
        Mesh simplifiedMesh=new Mesh();
        simplifiedMesh=GenerateSimplifiedMesh(noiseMap);

        mesh.vertices = verts;
        mesh.uv = uvs;
        mesh.triangles = tris;
        mesh.RecalculateNormals();
        testMesh.sharedMesh = mesh;
        testMesh.transform.position = new Vector3(0, 0, 0);
        if (!testMesh.gameObject.activeInHierarchy)
        {
            meshFilter.sharedMesh = mesh;
            meshCollider.sharedMesh=simplifiedMesh;
        }
    }

    public Mesh GenerateMesh(int _width, int _height, int _cellSize, float[,] noiseMap, int _heightAmplifier)
    {
        width=_width;
        height=_height;
        cellSize=_cellSize;
        heightAmplifier=_heightAmplifier;
        Mesh mesh = new Mesh();
        Vector3[] verts = new Vector3[width * height];
        Vector2[] uvs = new Vector2[width * height];
        int[] tris = new int[(width - 1) * (height - 1) * 6];
        int vertexIndex, triangleIndex;
        vertexIndex = 0;
        triangleIndex = 0;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                verts[vertexIndex].x = -width / 2 + i * cellSize;
                verts[vertexIndex].z = -height / 2 + j * cellSize;
                verts[vertexIndex].y = noiseMap[i, j] * heightAmplifier;
                float uvWidth, uvHeight;
                uvWidth = (float)i / width;
                uvHeight = (float)j / height;
                uvs[vertexIndex] = new Vector2(uvWidth, uvHeight);

                if (i < width - 1 && j < height - 1)
                {

                    tris[triangleIndex] = vertexIndex;
                    tris[triangleIndex + 1] = vertexIndex + 1;
                    tris[triangleIndex + 2] = vertexIndex + (width);

                    tris[triangleIndex + 3] = vertexIndex + (width);
                    tris[triangleIndex + 4] = vertexIndex + 1;
                    tris[triangleIndex + 5] = vertexIndex + (width) + 1;

                    triangleIndex = triangleIndex + 6;

                }
                vertexIndex++;


            }
        }

        mesh.vertices = verts;
        mesh.uv = uvs;
        mesh.triangles = tris;
        mesh.RecalculateNormals();
        return mesh;
    }

    public void SetHeight(int playerHeight)
    {
        height = playerHeight;
    }

    public void SetWidth(int playerWidth)
    { 
        width = playerWidth;
    }

    public void SetCellSize(int playerCellSize)
    {
        cellSize = playerCellSize;
    }

    Mesh GenerateSimplifiedMesh(float[,] mapData)
    {
        Mesh mesh = new Mesh();
        Vector3[] verts = new Vector3[width * height];
        int[] tris = new int[(width - 1) * (height - 1) * 6];

        int vertexIndex, triangleIndex;
        vertexIndex = 0;
        triangleIndex = 0;
        while ((width - 1) % levelOfDetail > 0)
        {
            levelOfDetail--;
        }
        int vertsPerLine = ((width - 1) / levelOfDetail) + 1;
        for (int i = 0; i < width; i+=levelOfDetail)
        {
            for (int j = 0; j < height; j += levelOfDetail)
            {
                verts[vertexIndex].x = -width / 2 + i * cellSize;
                verts[vertexIndex].z = -height / 2 + j * cellSize;
                verts[vertexIndex].y = mapData[i, j] * heightAmplifier;

                if (i < width - 1 && j < height - 1)
                {
                    

                    tris[triangleIndex] = vertexIndex;
                    tris[triangleIndex + 1] = vertexIndex + 1;
                    tris[triangleIndex + 2] = vertexIndex + (vertsPerLine);

                    tris[triangleIndex + 3] = vertexIndex + (vertsPerLine);
                    tris[triangleIndex + 4] = vertexIndex + 1;
                    tris[triangleIndex + 5] = vertexIndex + (vertsPerLine) + 1;

                    triangleIndex = triangleIndex + 6;

                }
                vertexIndex++;


            }
        }
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.RecalculateNormals();
        return mesh;
    }
    public Mesh GenerateSimplifiedMesh(int _width, int _height, int _cellSize, float[,] mapData, int _heightAmplifier, int _levelOfDetail)
    {
        width = _width;
        height = _height;
        cellSize = _cellSize;
        heightAmplifier = _heightAmplifier;
        levelOfDetail = _levelOfDetail;
        noiseMap = mapData;
        Mesh mesh = new Mesh();
        Vector3[] verts = new Vector3[width * height];
        int[] tris = new int[(width - 1) * (height - 1) * 6];

        int vertexIndex, triangleIndex;
        vertexIndex = 0;
        triangleIndex = 0;
        while ((width - 1) % levelOfDetail > 0)
        {
            levelOfDetail--;
        }
        int vertsPerLine = ((width - 1) / levelOfDetail) + 1;
        for (int i = 0; i < width; i += levelOfDetail)
        {
            for (int j = 0; j < height; j += levelOfDetail)
            {
                verts[vertexIndex].x = -width / 2 + i * cellSize;
                verts[vertexIndex].z = -height / 2 + j * cellSize;
                verts[vertexIndex].y = mapData[i, j] * heightAmplifier;

                if (i < width - 1 && j < height - 1)
                {


                    tris[triangleIndex] = vertexIndex;
                    tris[triangleIndex + 1] = vertexIndex + 1;
                    tris[triangleIndex + 2] = vertexIndex + (vertsPerLine);

                    tris[triangleIndex + 3] = vertexIndex + (vertsPerLine);
                    tris[triangleIndex + 4] = vertexIndex + 1;
                    tris[triangleIndex + 5] = vertexIndex + (vertsPerLine) + 1;

                    triangleIndex = triangleIndex + 6;

                }
                vertexIndex++;


            }
        }
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.RecalculateNormals();
        return mesh;
    }
}
