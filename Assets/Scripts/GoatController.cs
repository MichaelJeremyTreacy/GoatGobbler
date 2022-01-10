using Random = UnityEngine.Random;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoatController : MonoBehaviour
{
    public float MoveSpeed = 5f;

    public float XPosLimit = 32f;
    public float YPosLimit = 28f;
    private Vector2 _targetPos;

    private void Start()
    {
        float xPos = Random.Range(0, XPosLimit);
        float yPos = Random.Range(0, YPosLimit);

        _targetPos = new Vector2(xPos, yPos);
    }

    private void FixedUpdate()
    {
        if (!GameManager.s_Instance.WeekIsSettingUp)
        {
            if (Vector2.Distance(gameObject.transform.position, _targetPos) <= 2)
            {
                GetNewTarget();
            }

            gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, _targetPos, MoveSpeed * Time.deltaTime);

            SetScale();
        }
    }

    private void GetNewTarget()
    {
        _targetPos.x = Random.Range(0, XPosLimit);
        _targetPos.y = Random.Range(0, YPosLimit);
    }

    private void SetScale()
    {
        Vector2 newScale = gameObject.transform.localScale;

        if (gameObject.transform.position.x > _targetPos.x)
        {
            newScale.x = 1f;
        }
        else if (gameObject.transform.position.x < _targetPos.x)
        {
            newScale.x = -1f;
        }

        gameObject.transform.localScale = newScale;
    }
}
