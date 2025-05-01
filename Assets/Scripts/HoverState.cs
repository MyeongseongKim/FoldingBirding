using UnityEngine;

public class HoverState : IBirdState
{
    public void Enter(BirdBehaviour bird) 
    {
        Debug.Log($"{bird.name} Enters : {nameof(HoverState)}");

        bird.BehaviourCoroutine = bird.StartCoroutine(
            bird.HoverAround(null)
        );
    }
    
    public void Update(BirdBehaviour bird) 
    {

    }
    
    public void Exit(BirdBehaviour bird) 
    {
        Debug.Log($"{bird.name} Exits : {nameof(HoverState)}");

        bird.BehaviourCoroutine = null;
    }

    public void OnDone(BirdBehaviour bird) 
    {

    }

    public void HandlePalmUpSelected(BirdBehaviour bird) 
    {

    }

    public void HandlePalmUpUnselected(BirdBehaviour bird) 
    {

    }

    public void HandlePerchSelected(BirdBehaviour bird) 
    {
        bird.Target = UserSpots.Instance.GetPerchTarget();

        bird.TransitState(new ApproachState());
    }

    public void HandlePerchUnselected(BirdBehaviour bird)
    {
        
    }
 }
