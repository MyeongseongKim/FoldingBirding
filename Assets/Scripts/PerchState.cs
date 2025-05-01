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
        bird.transform.position = bird.Target.position;
        bird.transform.rotation = bird.Target.rotation;
    }
    
    public void Exit(BirdBehaviour bird) 
    {
        Debug.Log($"{bird.name} Exits : {nameof(PerchState)}");

        bird.BehaviourCoroutine = null;
    }

    public void OnDone(BirdBehaviour bird) 
    {
        if (bird.CurSpot == null)
            return;

        bird.CurSpot.Vacate();
        bird.PreSpot = bird.CurSpot;
        bird.CurSpot = null;
        bird.Target = null;
        bird.Direction = bird.transform.forward;

        bird.Animator.SetTrigger("IdleToFly");
        bird.TransitState(new FlyState());
    }

    public void HandlePalmUpSelected(BirdBehaviour bird) 
    {
        bird.CurSpot.Vacate();
        bird.PreSpot = bird.CurSpot;
        bird.CurSpot = null;
        bird.Target = null;
        bird.Direction = bird.transform.forward;

        bird.Target = UserSpots.Instance.GetHoverTarget();

        bird.Animator.SetTrigger("IdleToFly");
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
        if (bird.CurSpot != null)
            return;

        bird.CurSpot = null;
        bird.CurSpot = null;
        bird.Target = null;
        bird.Direction = bird.transform.forward;

        bird.Animator.SetTrigger("IdleToFly");
        bird.TransitState(new FlyState());
    }
}
