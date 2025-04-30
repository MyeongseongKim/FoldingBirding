using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class ApproachState : IBirdState
{
    public void Enter(BirdBehaviour bird) 
    {
        Debug.Log($"{bird.name} Enters : {nameof(ApproachState)}");

        bird.BehaviourCoroutine = bird.StartCoroutine(
            bird.ApproachToTarget((bird) => OnDone(bird))
        );
    }
    
    public void Update(BirdBehaviour bird) 
    {

    }
    
    public void Exit(BirdBehaviour bird) 
    {
        Debug.Log($"{bird.name} Exits : {nameof(ApproachState)}");

        bird.StopCoroutine(bird.BehaviourCoroutine);
    }

    public void OnDone(BirdBehaviour bird) 
    {
        bird.Animator.SetTrigger("FlyToIdle");
        bird.TransitState(new PerchState());
    }
}
