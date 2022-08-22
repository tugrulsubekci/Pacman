using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passage : MonoBehaviour
{
    [SerializeField] Vector3 spawnPos;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.transform.position =spawnPos;
    }
}
