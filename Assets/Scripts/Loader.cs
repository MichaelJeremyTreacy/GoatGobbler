using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    public GameObject GameManagerObject;

    private void Awake()
    {
        if (GameManager.s_Instance == null)
        {
            Instantiate(GameManagerObject);
        }
    }
}