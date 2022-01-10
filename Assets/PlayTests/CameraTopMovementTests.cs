using System.Collections;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CameraTopMovementTests
{
    private GameObject _backgroundGameObject;

    public float _playerTopYBoundary = 20f;
    public float _playerBottomYBoundary = 7f;

    public float _targetTopYPos = 24f;
    public float _targetMidYPos = 12.5f;
    public float _targetBottomYPos = 1f;

    [OneTimeSetUp]
    public void BackgroundSetUp()
    {
        Debug.Log("Setting up Background");

        _backgroundGameObject = GameObject.Instantiate(Resources.Load("TestBackground")) as GameObject;

        Assert.IsNotNull(_backgroundGameObject);
    }

    [UnityTest]
    public IEnumerator SetTargetPos_PlayerGreaterThanTopYBound_CameraEqualToTargetTopY()
    {
        Debug.Log("SetTargetPos_PlayerGreaterThanTopYBound_CameraEqualToTargetTopY");

        GameObject greaterPlayerGameObject = GameObject.Instantiate(Resources.Load("TestPlayer")) as GameObject;
        GameObject greaterCameraGameObject = greaterPlayerGameObject.transform.Find("TestTopCamera").gameObject;

        greaterPlayerGameObject.transform.position = new Vector3(0.0f, _playerTopYBoundary + 1f, 0.0f);

        yield return new WaitForSeconds(1f);

        Assert.AreEqual(_targetTopYPos, greaterCameraGameObject.transform.position.y);
    }

    [UnityTest]
    public IEnumerator SetTargetPos_PlayerBetweenTopYBoundAndBottomYBound_CameraEqualToTargetMidY()
    {
        Debug.Log("SetTargetPos_PlayerBetweenTopYBoundAndBottomYBound_CameraEqualToTargetMidY");

        GameObject betweenPlayerGameObject = GameObject.Instantiate(Resources.Load("TestPlayer")) as GameObject;
        GameObject betweenCameraGameObject = betweenPlayerGameObject.transform.Find("TestTopCamera").gameObject;

        float middle = (_playerTopYBoundary + _playerBottomYBoundary) / 2f;
        betweenPlayerGameObject.transform.position = new Vector3(0.0f, middle, 0.0f);

        yield return new WaitForSeconds(1f);

        Assert.AreEqual(_targetMidYPos, betweenCameraGameObject.transform.position.y);
    }

    [UnityTest]
    public IEnumerator SetTargetPos_PlayerLessThanBottomYBound_CameraEqualToTargetBottomY()
    {
        Debug.Log("SetTargetPos_PlayerLessThanBottomYBound_CameraEqualToTargetBottomY");

        GameObject lessPlayerGameObject = GameObject.Instantiate(Resources.Load("TestPlayer")) as GameObject;
        GameObject lessCameraGameObject = lessPlayerGameObject.transform.Find("TestTopCamera").gameObject;

        lessPlayerGameObject.transform.position = new Vector3(0.0f, _playerBottomYBoundary - 1f, 0.0f);

        yield return new WaitForSeconds(1f);

        Assert.AreEqual(_targetBottomYPos, lessCameraGameObject.transform.position.y);
    }
}
