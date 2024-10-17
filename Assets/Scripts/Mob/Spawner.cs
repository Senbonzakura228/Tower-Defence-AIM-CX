using System;
using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Monster monsterPrefab;
    [SerializeField] private float spawnInterval = 3;
    [SerializeField] private Transform monsterTargetDestination;

    private Coroutine _spawnRoutine;

    private void Start()
    {
        _spawnRoutine = StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        while (true)
        {
            CreateMonster();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void CreateMonster()
    {
        var monster = Instantiate(monsterPrefab, transform);
        monster.Init(monsterTargetDestination);
    }

    private void OnDestroy()
    {
        StopCoroutine(_spawnRoutine);
    }
}