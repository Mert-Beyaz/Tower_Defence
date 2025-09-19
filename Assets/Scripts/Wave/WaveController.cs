using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    public static WaveController Instance;

    [Header("Path")]
    [SerializeField] private List<Path> pathList = new();

    [Header("Wave")]
    [SerializeField] private List<Wave> waves = new();
    [SerializeField] private int waveCounter = 0;
    [SerializeField] private float spawnTime = 0.5f;

    private List<Monster> _monsterList = new();

    public List<Monster> MonsterList { get => _monsterList; set => _monsterList = value; }
    public int WaveCounter { get => waveCounter;}

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        Reset();
        StartCoroutine(StartWave());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (waveCounter >= waves.Count)
            {
                return;
            }
            waveCounter++;
            StartCoroutine(StartWave(false));
        }
    }

    private IEnumerator StartWave(bool addToWave = true)
    {
        yield return null;
        if (waveCounter >= waves.Count)
        {
            yield break;
        }

        UIManager.Instance.SetWaveText();
        int thisWaveIndex = waveCounter;
        foreach (EnemyAndAmount wave in waves[thisWaveIndex].EnemyAndAmounts)
        {
            for (int i = 0; i < wave.Amount; i++)
            {
                var item = PoolManager.Instance.GetObject(wave.EnemyType);
                var monster = item.GetComponent<Monster>();
                _monsterList.Add(monster);
                monster.Init(pathList[UnityEngine.Random.Range(0, pathList.Count)].PointList);
                yield return Helper.GetWait(spawnTime);

            }
        }
        if (addToWave)
        {
            waveCounter++;
        }
    }

    public void CheckWave()
    {
        if (_monsterList.Count <= 0)
        {
            StartCoroutine(StartWave());
        }

        if (waveCounter >= waves.Count && _monsterList.Count <= 0)
        {
            GameManager.Instance.Win();
        }
    }

    private void Reset()
    {
        waveCounter = 0;
        foreach (var item in _monsterList)
        {
            PoolManager.Instance.ReturnObject(item.gameObject);
        }
        _monsterList.Clear();
    }
}

[Serializable]
public class Wave
{
    public List<EnemyAndAmount> EnemyAndAmounts = new();
}

[Serializable]
public class EnemyAndAmount
{
    public PoolType EnemyType;
    public int Amount;
}

[Serializable]
public class Path
{
    public List<Transform> PointList;
}

