using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class BirdsonDistribution : MonoBehaviour
{
    List<Vector2> _positions = new List<Vector2>();
    [SerializeField] GameObject orb;
    [SerializeField] int width = 100;
    [SerializeField] int height = 100;
    [SerializeField] int radius = 10;
    int radiusSquared;
    [SerializeField] int maxIterations = 30;
    [SerializeField] float cellSize;
    [SerializeField] float secondsToWait;
    private Vector2[,] grid;
    private Rect rect;
    private List<Vector2> activeSamples = new List<Vector2>();
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Generate());
    }

    public BirdsonDistribution(int radius, int width, int height)
    {
        radiusSquared= radius*radius;
        cellSize = radius/Mathf.Sqrt(2);
        grid = new Vector2[Mathf.CeilToInt(width/cellSize),Mathf.CeilToInt(height/cellSize)];
        rect = new Rect(0,0, width, height);
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    Vector2 GetRandomPoint(int width, int height)
    {
        return new Vector2(Random.Range(0, width), Random.Range(0, height));
    }

    IEnumerator Generate()
    {
        BirdsonDistribution sampler = new BirdsonDistribution(radius,width,height);
        foreach(Vector2 sample in sampler.FindSamples())
        {
            Instantiate(orb, new Vector3(sample.x, 0, sample.y), Quaternion.identity,this.transform);
            yield return new WaitForSeconds(secondsToWait);
        }
    }


    private struct GridPos
    {
        public int x;
        public int y;

        public GridPos(Vector2 sample, float cellSize)
        {
            x=(int)(sample.x/cellSize);
            y = (int)(sample.y / cellSize);
        }
    }


    bool IsFar(Vector2 candidate)
    {
        GridPos pos = new GridPos(candidate, cellSize);
        int xmin = Mathf.Max(pos.x - 2, 0);
        int ymin = Mathf.Max(pos.y - 2, 0);
        int xmax = Mathf.Min(pos.x + 2, grid.GetLength(0) - 1);
        int ymax = Mathf.Min(pos.y + 2, grid.GetLength(1) - 1);

        for (int y = ymin; y <= ymax; y++)
        {
            for (int x = xmin; x <= xmax; x++)
            {
                Vector2 s = grid[x, y];
                if (s != Vector2.zero)
                {
                    Vector2 d = s - candidate;
                    if (d.x * d.x + d.y * d.y < radiusSquared) return false;
                }
            }
        }

        return true;

    }

    IEnumerable<Vector2> FindSamples()
    {
      yield return AddSample(new Vector2(Random.value * rect.width, Random.value * rect.height));

        while (activeSamples.Count > 0)
        {
            int i =(int) Random.value* activeSamples.Count;
            Vector2 sample = activeSamples[i];
            bool found = false;
            for (int j = 0; j < maxIterations; j++)
            {
                float angle = 2 * Mathf.PI * Random.value;
                float r = Mathf.Sqrt(Random.value * 3 * radiusSquared + radiusSquared);
                Vector2 candidate = sample + r * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                if (rect.Contains(candidate) && IsFar(candidate))
                {
                    found = true;
                    yield return AddSample(candidate);
                    break;
                }
            }

            if(!found)
            {
                activeSamples[i]= activeSamples[activeSamples.Count-1];
                activeSamples.RemoveAt(activeSamples.Count-1);
            }
        }
    }


    Vector2 AddSample (Vector2 sample)
    {
        activeSamples.Add(sample);
        GridPos pos = new GridPos(sample, cellSize);
        grid[pos.x, pos.y] = sample;
        return sample;
    }

}
