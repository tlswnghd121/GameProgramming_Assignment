using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAction : MonoBehaviour
{
    public GameObject bombEffect;
    public float bombdamage = 100f; //폭탄 데미지

    //트리거 처리 함수 구현
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            // 트리거에 진입한 오브젝트가 'Enemy' 레이어를 가진 경우
            Enemy target = other.gameObject.GetComponent<Enemy>();

            if (target != null)
            {
                // Enemy 스크립트가 있는 경우 데미지를 입힙니다.
                target.TakeDamage(bombdamage);
            }
        }

        // 이펙트 프리팹 생성
        GameObject eff = Instantiate(bombEffect);
        // 이펙트 프리팹 위치 설정
        eff.transform.position = transform.position;
        // 자기 오브젝트를 제거
        Destroy(gameObject);
    }

    /*
    //충돌체 처리 함수 구현
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            // 충돌한 오브젝트가 'Enemy' 레이어를 가진 경우
            Enemy target = collision.gameObject.GetComponent<Enemy>();

            if (target != null)
            {
                // Enemy 스크립트가 있는 경우 데미지를 입힙니다.
                target.TakeDamage(bombdamage);
            }
        }

        //이펙트 프리팹 생성
        GameObject eff = Instantiate(bombEffect);
        //이펙트 프리팹 위치 설정
        eff.transform.position = transform.position;
        //자기 오브젝트를 제거
        Destroy(gameObject);

    }
    */
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
