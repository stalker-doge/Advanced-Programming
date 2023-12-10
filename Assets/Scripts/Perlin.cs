using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Perlin : MonoBehaviour
{

    [SerializeField] public float scale;
    [SerializeField] public float lacunarity;
    [SerializeField] public float persistance;
    [SerializeField] public int octaves;
    [SerializeField] public int seed;
    [SerializeField] public int seedOffset;
    [SerializeField] float maxNoiseHeight;
    [SerializeField] float minNoiseHeight;
    [SerializeField] Renderer textureRenderer;
    [SerializeField] Renderer testRenderer;

    private static Perlin _Instance;
    public static Perlin Instance
    {
        //checks if there is an instance of the class, if there is not it creates one
        get
        {
            if (!_Instance)
            {
                _Instance = new GameObject().AddComponent<Perlin>();
            }
            return _Instance;
        }
    }
    float[,] noiseMap;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Awake()
    {
        if(_Instance != null && _Instance !=this)
        {
            Destroy(_Instance);
        }
        else
        {
            _Instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private float[,] NoiseGeneration(int width, int height,int seed, int seedOffset)
    {
        float[,] t= new float[width, height];

        float seedValue = GenerateSeedValue(seed, seedOffset);
        float maxHeight = maxNoiseHeight;
        float minHeight = minNoiseHeight;
        for (int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                float amplitude = 1;
                float frequency = 1;
                float maxValue = 0;
                float value = 0;
                for (int o = 0; o < octaves; o++)
                {
                    float xSample = i * scale * frequency;
                    xSample += seedValue;

                    float ysample = j * scale * frequency;
                    ysample += seedValue;
                    float perlinValue = Mathf.PerlinNoise(xSample, ysample * frequency) *amplitude;

                    value += perlinValue;
                    maxValue += amplitude;
                    amplitude *= persistance;
                    frequency *= lacunarity;


                    if (value > maxHeight)
                    { 
                         maxHeight = value;
                    }
                    if (value < minHeight)
                    {
                        minHeight = value;
                    }


                }
                t[i, j] = value;

            }
        }

        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {

                t[i, j] = Mathf.InverseLerp(minHeight, maxHeight, t[i, j]);
            }
        }
        return t;
    }
    public float[,] NoiseGeneration(int width, int height, int _seed, int _seedOffset,float _persistance, int _octaves, float _lacunarity,float _scale)
    {
        seed = _seed;
        seedOffset = _seedOffset;
        persistance = _persistance;
        octaves = _octaves;
        lacunarity = _lacunarity;
        scale = _scale;

        float[,] t = new float[width, height];

        float seedValue = GenerateSeedValue(seed, seedOffset);
        float maxHeight = maxNoiseHeight;
        float minHeight = minNoiseHeight;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                float amplitude = 1;
                float frequency = 1;
                float maxValue = 0;
                float value = 0;
                for (int o = 0; o < octaves; o++)
                {
                    float xSample = i * scale * frequency;
                    xSample += seedValue;

                    float ysample = j * scale * frequency;
                    ysample += seedValue;
                    float perlinValue = Mathf.PerlinNoise(xSample, ysample * frequency) * amplitude;

                    value += perlinValue;
                    maxValue += amplitude;
                    amplitude *= persistance;
                    frequency *= lacunarity;


                    if (value > maxHeight)
                    {
                        maxHeight = value;
                    }
                    if (value < minHeight)
                    {
                        minHeight = value;
                    }


                }
                t[i, j] = value;

            }
        }

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {

                t[i, j] = Mathf.InverseLerp(minHeight, maxHeight, t[i, j]);
            }
        }
        return t;
    }

    public void Generate()
    {
        noiseMap = NoiseGeneration(25, 25,seed,seedOffset);
        DrawNoise(noiseMap);
    }

    public void Generate(int width, int height)
    {
        noiseMap = NoiseGeneration(width, height, seed, seedOffset);
        DrawNoise(noiseMap);
    }

    public float[,] GetNoiseMap()
    {
        return noiseMap;
    }

    public Texture2D DrawNoise(float[,] noiseMap)
    {
        int mapWidth, mapHeight;
        mapWidth = noiseMap.GetLength(0);
        mapHeight = noiseMap.GetLength(1);
        Texture2D tex= new Texture2D(mapWidth, mapHeight);

        Color[] colourMap= new Color[mapWidth*mapHeight];

        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                colourMap[i * mapWidth + j] = Color.Lerp(Color.black, Color.white, noiseMap[i, j]);
            }
        }

        ColourMap(ref colourMap,noiseMap);
        tex.SetPixels(colourMap);
        tex.Apply();
        tex.filterMode = FilterMode.Point;
        tex.wrapMode = TextureWrapMode.Clamp;
        return tex;
    }
    public Texture2D DrawNoise(float[,] noiseMap, Color Mountain, Color MountainTop, Color Hill, Color Valley, Color Water)
    {
        int mapWidth, mapHeight;
        mapWidth = noiseMap.GetLength(0);
        mapHeight = noiseMap.GetLength(1);
        Texture2D tex = new Texture2D(mapWidth, mapHeight);

        Color[] colourMap = new Color[mapWidth * mapHeight];

        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                colourMap[i * mapWidth + j] = Color.Lerp(Color.black, Color.white, noiseMap[i, j]);
            }
        }

        ColourMap(ref colourMap,noiseMap,Water,Valley,Hill,Mountain,MountainTop);
        tex.SetPixels(colourMap);
        tex.Apply();
        tex.filterMode = FilterMode.Point;
        tex.wrapMode = TextureWrapMode.Clamp;
        return tex;
    }

    float GenerateSeedValue(int seed, int sample)
    {
        Random.InitState(seed);
        return Random.Range(-10000, 10000) + sample;
    }


    void ColourMap(ref Color[] colourArray, float[,] noiseMap)
    {
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);
        for (int i = 0;i < width;i++)
        {
            for(int j = 0;j < height;j++)
            {
                if (noiseMap[i, j] <= 0.35f)
                {
                    colourArray[i*width+j] = Color.blue;
                }
                else if (noiseMap[i, j] <= 0.40f)
                {
                    colourArray[i*width+j] = Color.yellow;
                }
                else if (noiseMap[i, j] <= 0.60f)
                {
                    colourArray[i*width+j] = Color.green;
                }
                else if (noiseMap[i, j] <= 0.85f)
                {
                    colourArray[i * width + j] = Color.gray;
                }
                else if (noiseMap[i,j]<= 1f)
                {
                    colourArray[i * width + j] = Color.white;
                }
            }
        }
    }
    void ColourMap(ref Color[] colourArray, float[,] noiseMap, Color Water, Color Valley, Color Hill, Color Mountain, Color MountainTop)
    {
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (noiseMap[i, j] <= 0.35f)
                {
                    colourArray[i * width + j] = Water;
                }
                else if (noiseMap[i, j] <= 0.40f)
                {
                    colourArray[i * width + j] = Valley;
                }
                else if (noiseMap[i, j] <= 0.60f)
                {
                    colourArray[i * width + j] = Hill;
                }
                else if (noiseMap[i, j] <= 0.85f)
                {
                    colourArray[i * width + j] = Mountain;
                }
                else if (noiseMap[i, j] <= 1f)
                {
                    colourArray[i * width + j] = MountainTop;
                }
            }
        }
    }

}
