using UnityEngine;

public class PerchState : IBirdState
{
    public void Enter(BirdBehaviour bird) 
    {
        Debug.Log($"{bird.name} Enters : {nameof(PerchState)}");

        bird.BehaviourCoroutine = bird.StartCoroutine(
            bird.HangAround((bird) => OnDone(bird))
        );
    }
    
    public void Update(BirdBehaviour bird) 
    {

    }
    
    public void Exit(BirdBehaviour bird) 
    {
        Debug.Log($"{bird.name} Exits : {nameof(PerchState)}");

        bird.BehaviourCoroutine = null;
    }

    public void OnDone(BirdBehaviour bird) 
    {
        bird.PreTarget = bird.CurTarget;
        bird.CurTarget.Vacate();
        bird.CurTarget = null;

        bird.Direction = bird.transform.forward;

        bird.Animator.SetTrigger("IdleToFly");
        bird.TransitState(new FlyState());
    }
}
