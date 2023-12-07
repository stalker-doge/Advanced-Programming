using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distribution : MonoBehaviour
{
    List<Vector2> _positions = new List<Vector2>();
    [SerializeField] GameObject orb;
    [SerializeField] int width=100;
    [SerializeField] int height=100;
    [SerializeField] int iterations = 500;
    [SerializeField] int candidateCheckCount = 30;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Generate());
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
        Vector2[] newPos= new Vector2[iterations];
        for (int i = 0; i < iterations; i++)
        {
            Vector2 bestCandidate=Vector2.zero;
            float bestDistance = 0;
            for (int j = 0; j < candidateCheckCount; j++)
            {
                Vector2 a = GetRandomPoint(width, height);
                Vector2 closestPoint=FindClosestSample(a);
                float dist=Distance(closestPoint, bestCandidate);

                if(dist > bestDistance)
                {
                    bestDistance = dist;
                    bestCandidate = a;
                }
            }
            Instantiate(orb,new Vector3(bestCandidate.x,0,bestCandidate.y),Quaternion.identity,this.transform);
           _positions.Add(bestCandidate);
            yield return new WaitForSeconds(0.01f);
        }
    }

    float Distance (Vector2 candidate, Vector2 sample)
    {
        float dist;
        dist = Mathf.Sqrt((candidate.x - sample.x) * (candidate.x - sample.x) + (candidate.y - sample.y) * (candidate.y - sample.y));
        return dist;
    }

    Vector2 FindClosestSample(Vector2 candidate)
    {
        float currentClosest = float.MaxValue;
        Vector2 closestSample=Vector2.zero;
        for(int i=0;i< _positions.Count;i++)
        {
            float distance = Distance(candidate, _positions[i]);
            if(distance < currentClosest)
            {
                currentClosest = distance;
                closestSample = candidate;
            }

        }
        return closestSample;
    }
}
