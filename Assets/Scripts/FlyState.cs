using System.Collections;
using UnityEngine;


public class FlyState : IBirdState
{
    public void Enter(BirdBehaviour bird) 
    {
        Debug.Log($"{bird.name} Enters : {nameof(FlyState)}");

        bird.BehaviourCoroutine = bird.StartCoroutine(
            bird.FlyAround(null)
        );
    }
    
    public void Update(BirdBehaviour bird) 
    {
        var target = bird.FindPerchingSpot();
        if (target != bird.PreTarget && !target.IsOccupied)
        {
            bird.CurTarget = target;
            bird.CurTarget.Occupy();
            bird.TransitState(new ApproachState());
        }
    }
    
    public void Exit(BirdBehaviour bird) 
    {
        Debug.Log($"{bird.name} Exits : {nameof(FlyState)}");

        bird.StopCoroutine(bird.BehaviourCoroutine);
        bird.BehaviourCoroutine = null;
    }

    public void OnDone(BirdBehaviour bird) 
    {
        
    }
}
