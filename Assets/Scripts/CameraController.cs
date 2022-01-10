using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject Player;
    public float PlayerTopYBoundary = 20f;
    public float PlayerBottomYBoundary = 7f;
    
    public float MoveSpeed = 9f;

    public float DefaultTargetXPos = 16.15f;
    public float DefaultTargetZPos = -10f;
    public float TargetTopYPos = 24f;
    public float TargetMidYPos = 12.5f;
    public float TargetBottomYPos = 1f;
    private Vector3 _targetPos;

    private void Start()
    {
        if (Player == null)
        {
            Player = transform.parent.gameObject;
        }        

        _targetPos = new Vector3(DefaultTargetXPos, TargetBottomYPos, DefaultTargetZPos);
    }

    private void LateUpdate()
    {
        SetTargetPos();

        transform.position = Vector3.MoveTowards(gameObject.transform.position, _targetPos, MoveSpeed * Time.deltaTime);
    }

    private void SetTargetPos()
    {
        if (Player.transform.position.y > PlayerTopYBoundary)
        {
            _targetPos.y = TargetTopYPos;
        }
        else if (Player.transform.position.y <= PlayerTopYBoundary && Player.transform.position.y >= PlayerBottomYBoundary)
        {
            _targetPos.y = TargetMidYPos;
        }
        else if (Player.transform.position.y < PlayerBottomYBoundary)
        {
            _targetPos.y = TargetBottomYPos;
        }
    }
}