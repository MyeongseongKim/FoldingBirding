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
        var spot = bird.FindPerchingSpot();
        if (spot != bird.PreSpot && !spot.IsOccupied)
        {
            spot.Occupy();
            bird.CurSpot = spot;
            bird.Target = spot.transform;

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

    public void HandlePalmUpSelected(BirdBehaviour bird) 
    {
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
