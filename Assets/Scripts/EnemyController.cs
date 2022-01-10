using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject Player;
    
    public float MoveSpeed = 5f;

    private Animator _animator;

    private void Start()
    {
        _animator = gameObject.GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (!GameManager.s_Instance.WeekIsSettingUp)
        {
            gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, Player.transform.position, MoveSpeed * Time.deltaTime);

            SetScale();
        }
    }

    private void SetScale()
    {
        Vector2 newScale = gameObject.transform.localScale;

        if (gameObject.transform.position.x > Player.transform.position.x)
        {
            newScale.x = -1f;
        }
        else if (gameObject.transform.position.x < Player.transform.position.x)
        {
            newScale.x = 1f;
        }

        gameObject.transform.localScale = newScale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Foliage" || other.tag == "Player")
        {
            Attack();
        }
    }

    private void Attack()
    {
        gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, gameObject.transform.position, MoveSpeed * Time.deltaTime);

        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Enemy1WalkState"))
        {
            _animator.SetTrigger("EnemyAttackTrigger");
        }
    }
}
