using UnityEngine;

public class MyGizmos : MonoBehaviour
{
    [SerializeField] private Color _color = Color.green;
    [Range(1f, 5f)] 
    [SerializeField] private float _radius = 1f;

    private void OnDrawGizmos()
    {
        // 기즈모 색상 설정
        Gizmos.color = _color;
        // Sphere 생성
        Gizmos.DrawSphere(transform.position, _radius);
    }
}
