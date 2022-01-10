using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoliageController : MonoBehaviour
{
    private Animator _animator;

    private void Start()
    {
        _animator = gameObject.GetComponent<Animator>();
    }

    public void TakeHit()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Foliage1AllGreenSwayState"))
        {
            _animator.SetTrigger("FoliageSomeGreenSwayTrigger");
        }
        else if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Foliage1SomeGreenSwayState"))
        {
            _animator.SetTrigger("FoliageNoGreenSwayTrigger");
        }
        else if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Foliage1NoGreenSwayState"))
        {
            gameObject.SetActive(false);
        }
    }
}