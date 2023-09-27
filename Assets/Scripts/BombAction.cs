using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAction : MonoBehaviour
{
    public GameObject bombEffect;
    public float bombdamage = 100f; //��ź ������

    //Ʈ���� ó�� �Լ� ����
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            // Ʈ���ſ� ������ ������Ʈ�� 'Enemy' ���̾ ���� ���
            Enemy target = other.gameObject.GetComponent<Enemy>();

            if (target != null)
            {
                // Enemy ��ũ��Ʈ�� �ִ� ��� �������� �����ϴ�.
                target.TakeDamage(bombdamage);
            }
        }

        // ����Ʈ ������ ����
        GameObject eff = Instantiate(bombEffect);
        // ����Ʈ ������ ��ġ ����
        eff.transform.position = transform.position;
        // �ڱ� ������Ʈ�� ����
        Destroy(gameObject);
    }

    /*
    //�浹ü ó�� �Լ� ����
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            // �浹�� ������Ʈ�� 'Enemy' ���̾ ���� ���
            Enemy target = collision.gameObject.GetComponent<Enemy>();

            if (target != null)
            {
                // Enemy ��ũ��Ʈ�� �ִ� ��� �������� �����ϴ�.
                target.TakeDamage(bombdamage);
            }
        }

        //����Ʈ ������ ����
        GameObject eff = Instantiate(bombEffect);
        //����Ʈ ������ ��ġ ����
        eff.transform.position = transform.position;
        //�ڱ� ������Ʈ�� ����
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
