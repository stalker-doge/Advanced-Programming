using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] objects;
    [SerializeField] float spawnRadius;
    [SerializeField] int spawnCount;
    [SerializeField] float spawnDelay;
    [SerializeField] float spawnVariation;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnObjects());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SpawnObjects()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 spawnPosition = transform.position + Random.insideUnitSphere * spawnRadius;
            //randomly select an object from the array
            GameObject randomObject = objects[Random.Range(0, objects.Length)];
            //shoots a raycast from the spawn position to the ground
            RaycastHit hit;
            if (Physics.Raycast(spawnPosition, Vector3.down, out hit))
            {
                //if the raycast hits the ground, spawn the object slightly above the ground
                Instantiate(randomObject, hit.point + Vector3.up * 0.5f, Quaternion.identity);
            }
            //randomises the spawn delay slightly
            yield return new WaitForSeconds(spawnDelay + Random.Range(-spawnVariation, spawnVariation));
        }
    }
}
