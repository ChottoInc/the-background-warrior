using System.Collections;
using UnityEngine;

public class FishingRockSpawner : MonoBehaviour
{
    [Header("Rock Prefab")]
    [SerializeField] GameObject[] _rockPrefabs;

    [Header("Spawn Settings")]
    [SerializeField] float _spawnInterval = 1.5f;
    [SerializeField] float _spawnIntervalVariance = 0.5f;
    [SerializeField] float _moveSpeedMin = 3f;
    [SerializeField] float _moveSpeedMax = 3f;

    [Header("Spawn Position")]
    [SerializeField] float _spawnX = 10f;
    [SerializeField] float _minY = -3f;
    [SerializeField] float _maxY = 3f;

    [Header("Despawn")]
    [SerializeField] float _despawnX = -10f;



    public IEnumerator CoSpawnLoop(float timer)
    {
        yield return new WaitForSeconds(timer);

        while (true)
        {
            SpawnRock();

            float wait = _spawnInterval + Random.Range(-_spawnIntervalVariance, _spawnIntervalVariance);
            yield return new WaitForSeconds(Mathf.Max(0.1f, wait));
        }
    }

    private void SpawnRock()
    {
        if (_rockPrefabs == null || _rockPrefabs.Length == 0) return;

        GameObject prefab = _rockPrefabs[Random.Range(0, _rockPrefabs.Length)];
        float spawnY = Random.Range(_minY, _maxY);

        GameObject rock = Instantiate(prefab, new Vector3(_spawnX, spawnY, 0), Quaternion.identity);
        rock.SetActive(true);

        FishingRock rockScript = rock.GetComponent<FishingRock>();
        if (rockScript == null)
        {
            rockScript = rock.AddComponent<FishingRock>();
        }

        float randSpeed = Random.Range(_moveSpeedMin, _moveSpeedMax);
        rockScript.Initialize(randSpeed, _despawnX);
    }
}
