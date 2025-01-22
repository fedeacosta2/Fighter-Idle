using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private float translationSpeed = 5f; // Adjust speed as needed
    [SerializeField] private GameObject character;
    private Rigidbody characterRigidbody;

    private void Start()
    {
        characterRigidbody = character.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (player.moveBoat)
        {
            character.transform.SetParent(transform);
            // Translate the boat in the z-axis
            transform.Translate(Vector3.down * (translationSpeed * Time.deltaTime));
            
        }
    }
}
