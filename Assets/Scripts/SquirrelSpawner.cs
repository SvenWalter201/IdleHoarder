using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelSpawner : MonoBehaviour
{
    [SerializeField] SquirrelBehaviour squirrelPrefab;
    [SerializeField] public Transform[] spawnPoints = new Transform[4];

    public SquirrelBehaviour[] spawnedSquirrels = new SquirrelBehaviour[4];

    void Start()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            var sq = Instantiate(squirrelPrefab, spawnPoints[i].position, Quaternion.identity);
            sq.transform.RotateAround(sq.transform.position, Vector3.up, Random.Range(0, 360));
            spawnedSquirrels[i] = sq;
        }
    }

    public void Despawn()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            spawnedSquirrels[i].Destroy();
        }
    }
}
