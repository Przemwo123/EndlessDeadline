using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EnemyDefinition
{
    [SerializeField]
    private bool moveOn = true;

    //------ Nadpisana funkcja EnemyFixedUpdate w skrypcie EnemyDefinition ------
    protected override void EnemyFixedUpdate()
    {
        switch (_currentState)
        {
            case State.Moving:
                if(moveOn) UpdateMovingState();
                FindVisibleTargets();
                break;
            case State.Dead:
                UpdateDeadState();
                break;
            case State.InAir:
                UpdateInAir();
                break;
        }
    }
}