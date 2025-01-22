using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public HealthBar HealthBarEnemy;
    //[SerializeField] private LightingManager SpawnTime;
    // Start is called before the first frame update
    [SerializeField] private PlayerController _Player;
    public Transform playerPosition;
    private NavMeshAgent agent;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = playerPosition.position;
    }

    public void EnemyDamaged()
    {
        HealthBarEnemy.sliderValue -= 0.20f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("cagaste wachin");
            _Player.PlayerDamaged();
        }
    }
}
