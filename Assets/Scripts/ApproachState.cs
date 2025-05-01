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
            bird.ApproachTo((bird) => OnDone(bird))
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

    public void HandlePalmUpSelected(BirdBehaviour bird) 
    {
        bird.CurSpot.Vacate();
        bird.PreSpot = bird.CurSpot;
        bird.CurSpot = null;
        bird.Target = null;
        bird.Direction = bird.transform.forward;

        bird.Target = UserSpots.Instance.GetHoverTarget();
        
        bird.TransitState(new HoverState());
    }

    public void HandlePalmUpUnselected(BirdBehaviour bird) 
    {

    }

    public void HandlePerchSelected(BirdBehaviour bird) 
    {

    }

    public void HandlePerchUnselected(BirdBehaviour bird)
    {
        
    }
}
