using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float followHeight = 7f;
    public float followDistance = 6f;
    public float followHeightSpeed = 0.9f;

    private Transform player;

    private float targetHeight;
    private float currentHeight;
    private float currentRotation;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        targetHeight = player.position.y + followHeight;

        currentRotation = transform.eulerAngles.y; //kamera y acisinda donecek

        currentHeight = Mathf.Lerp(transform.position.y, targetHeight, followHeightSpeed * Time.deltaTime);

        Quaternion euler = Quaternion.Euler(0f, currentRotation, 0f);

        Vector3 targetPosition = player.position - (euler * Vector3.forward) * followDistance;
        targetPosition.y = currentHeight;

        transform.position = targetPosition;
        transform.LookAt(player);
    }
}
