using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigTest : InteractiveObjects
{
    [Header("Definiowane dynamicznie")]
    private bool _isViewLeft = false;

    public GameObject visibleTarget;
    void Update()
    {
        if (visibleTarget.transform.position.x < transform.position.x && _isViewLeft == false)
        {
            Flip();
        }
        else if (visibleTarget.transform.position.x > transform.position.x && _isViewLeft == true)
        {
            Flip();
        }
    }

    protected virtual void Flip()
    {
        _isViewLeft = !_isViewLeft;
        Vector3 _setFlip = new Vector3((_isViewLeft ? 1: -1), 1, 1);
        this.transform.localScale = _setFlip;
    }
}
