using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _force = 1200.0f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Fire(Vector3 pos, Quaternion rot)
    {
        transform.SetPositionAndRotation(pos, rot);
        
        // 물리 속성 초기화
        _rb.linearVelocity = _rb.angularVelocity = Vector3.zero;
        _rb.rotation = rot;
        
        // 뉴튼(N)
        _rb.AddRelativeForce(Vector3.forward * _force);
    }
}
