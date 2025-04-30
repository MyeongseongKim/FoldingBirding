using UnityEngine;

public class NullState : IBirdState
{
    public void Enter(BirdBehaviour bird) 
    {
        Debug.Log($"{bird.name} Enters : {nameof(NullState)}");
    }
    
    public void Update(BirdBehaviour bird) 
    {
        bird.Animator.SetTrigger("ToFly");
        bird.TransitState(new FlyState());
    }
    
    public void Exit(BirdBehaviour bird) 
    {
        Debug.Log($"{bird.name} Exits : {nameof(NullState)}");

        bird.BehaviourCoroutine = null;
    }

    public void OnDone(BirdBehaviour bird) 
    {
        bird.Animator.SetTrigger("FlyToIdle");
        bird.TransitState(new PerchState());
    }
}
