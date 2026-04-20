using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _force = 1200.0f;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();

        // 뉴튼(N)
        _rb.AddRelativeForce(Vector3.forward * _force);
        
        // GetComponent<Rigidbody>().AddRelativeForce(Vector3.up * _force);
    }
}
