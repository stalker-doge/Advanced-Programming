using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

public class TerrainGeneration : EditorWindow
{
    private int _width=100, _height=100, _octaves = 8, _seed, _seedOffset = 100,_cellSize = 1,_heightAmplfier = 5,_levelOfDetailCollision=1, _levelOfDetailTerrain = 1;
    private float _scale = 100, _lacunarity = 1, _persistence = 1, _scalar;
    private Color _col1, _col2, _col3, _col4, _col5;//Colours for the terrain
    private GameObject _terrain;
    private Texture2D tex;

    [MenuItem("Tools/Procedural Generation")]
    public static void ShowWindow()
    {
        GetWindow(typeof(TerrainGeneration));
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {

        EditorGUILayout.Space();
        GUILayout.Label("Configure Terrain", EditorStyles.largeLabel);
        EditorGUILayout.Space();
        _width = EditorGUILayout.IntField("Terrain Width", _width);
        _height = EditorGUILayout.IntField("Terrain Width", _height);
        _cellSize = EditorGUILayout.IntField("Cell Size", _cellSize);
        _scale = EditorGUILayout.FloatField("Noise Scale", _scale);
        _octaves = EditorGUILayout.IntField("Octaves", _octaves);
        _lacunarity = EditorGUILayout.FloatField("Terrain Intensity", _lacunarity);
        _persistence = EditorGUILayout.FloatField("Height Variance", _persistence);
        _seed = EditorGUILayout.IntField("Seed", _seed);
        _seedOffset = EditorGUILayout.IntField("Seed Offset", _seedOffset);
        _heightAmplfier = EditorGUILayout.IntField("Height Amplifier", _heightAmplfier);
        _levelOfDetailCollision = EditorGUILayout.IntField("Level of Detail-Collision", _levelOfDetailCollision);
        _levelOfDetailTerrain = EditorGUILayout.IntField("Level of Detail-Terrain", _levelOfDetailTerrain);
        _col1 = EditorGUILayout.ColorField("Water Colour", _col1);
        _col2 = EditorGUILayout.ColorField("Valley Colour", _col2);
        _col3 = EditorGUILayout.ColorField("Hills Colour", _col3);
        _col4 = EditorGUILayout.ColorField("Mountain Colour", _col4);
        _col5 = EditorGUILayout.ColorField("Mountain Top Colour", _col5);
        if (GUILayout.Button("Create Terrain")) 
        {
            if(!_terrain)
            {
                _terrain=CreateMeshTemplate();
            }
            float[,] noiseMap = Perlin.Instance.NoiseGeneration(_width,_height,_seed, _seedOffset,_persistence,_octaves,_lacunarity,_scale);
            Mesh mesh = MeshGeneration.Instance.GenerateMesh(_width,_height,_cellSize, noiseMap,_heightAmplfier, _levelOfDetailTerrain);
            Mesh colliderMesh = MeshGeneration.Instance.GenerateSimplifiedMesh(_width,_height,_cellSize,noiseMap,_heightAmplfier, _levelOfDetailCollision);
            //tex = new Texture2D(_width,_height, TextureFormat.ARGB32, false);
            tex = Perlin.Instance.DrawNoise(noiseMap,_col1,_col2,_col3,_col4,_col5);
            _terrain.GetComponent<MeshFilter>().mesh = mesh;
            _terrain.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = tex;
            _terrain.GetComponent<MeshCollider>().sharedMesh = colliderMesh;
            
        }
        if (GUILayout.Button("Toggle Wireframe Collision"))
        {
            //toggles between showing the collider only and the mesh
            if (_terrain.GetComponent<MeshRenderer>().enabled)
            {
                _terrain.GetComponent<MeshRenderer>().enabled = false;
            }
            else
            {
                _terrain.GetComponent<MeshRenderer>().enabled = true;
            }
        }
    }

    private GameObject CreateMeshTemplate()
    {
        GameObject obj = new GameObject();
        obj.AddComponent<MeshFilter>();
        obj.AddComponent<MeshRenderer>();
        obj.AddComponent<MeshCollider>();
        Material myMaterial = (Material)Resources.Load("Noise");
        obj.GetComponent<MeshRenderer>().material = myMaterial;
        return obj;
    }

}
