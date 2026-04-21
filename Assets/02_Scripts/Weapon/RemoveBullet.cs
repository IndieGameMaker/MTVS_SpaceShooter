using UnityEngine;

public class RemoveBullet : MonoBehaviour
{
    private const string TAG_BULLET = "BULLET";
    
    // 스파크 파티클 프리팹 저장할 변수
    [SerializeField] private GameObject _sparkEffect;
    
    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.CompareTag(TAG_BULLET))
        {
            // 충돌 정보를 추출
            ContactPoint cp = coll.GetContact(0);
            // 충돌 좌표
            Vector3 point = cp.point;
            // 법선 벡터
            Vector3 normal = -1 * cp.normal;
            
            // 법선 벡터가 바로 보는 각도 산출 (Quaternion)
            Quaternion rot = Quaternion.LookRotation(normal);
            
            // 스파크 생성
            GameObject spark = Instantiate(_sparkEffect, point, rot);
            Destroy(spark, 0.5f);
            
            // Destroy(coll.gameObject); // 충돌한 게임오브젝트
            // Pool로 환원
            BulletPool.Instance.Return(coll.gameObject.GetComponent<Bullet>());

            // Destroy(this); // 스크립트가 삭제
            // Destroy(this.gameObject); // 벽 삭제
        }
    }
}
