using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private GameObject ArrowObject;
    [SerializeField] private GameObject MissionTarget;
    private float rotationSpeed = 10f;

    private void Update()
    {
        var MissionTargetPosition = MissionTarget.transform.position;
        var Position = transform.position;
        var Direction = (MissionTargetPosition - Position).normalized;
        var targetRotation = Quaternion.LookRotation(Direction);
        var newRotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        transform.rotation = newRotation;
    }
}
