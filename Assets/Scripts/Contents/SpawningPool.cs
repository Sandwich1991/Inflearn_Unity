using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawningPool : MonoBehaviour
{
    [SerializeField] private int _monsterCount = 0;
    
    [SerializeField] private int _reserveCount = 0;
    
    [SerializeField] private int _keepMonsterCount = 0;
    
    [SerializeField] private Vector3 _spawnPos;
    
    [SerializeField] private float _spawnRadius = 15f;
    
    [SerializeField] private float _spwanTime = 5f;

    public void AddMonsterCount(int value) { _monsterCount += value; }
    public void KeepMonsterCount(int count) { _keepMonsterCount += count; }
    
    
    void Start()
    {
        Managers.Game.OnSpawnEvent -= AddMonsterCount;
        Managers.Game.OnSpawnEvent += AddMonsterCount;
    }

    
    void Update()
    {
        while (_reserveCount + _monsterCount < _keepMonsterCount)
        {
            StartCoroutine("ReserveSpawn");
        }


    }

    IEnumerator ReserveSpawn()
    {
        _reserveCount++;
        yield return new WaitForSeconds(Random.Range(0, _spwanTime));
        GameObject obj = Managers.Game.Spawn(Define.WorldObject.Monster, "Zombie");
        NavMeshAgent nma = obj.GetOrAddComponent<NavMeshAgent>();
        
        Vector3 randPos;
        while (true)
        {
            Vector3 randDir = Random.insideUnitSphere * Random.Range(0, _spawnRadius);
            randDir.y = 0;
            randPos = _spawnPos + randDir;
            
            // 갈 수 있나
            NavMeshPath path = new NavMeshPath();
            if (nma.CalculatePath(randPos, path))
                break;

        }

        obj.transform.position = randPos;
        _reserveCount--;
    }
}
