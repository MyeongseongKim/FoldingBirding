public interface IBirdState
{
    void Enter(BirdBehaviour bird);
    void Update(BirdBehaviour bird);
    void Exit(BirdBehaviour bird);

    void OnDone(BirdBehaviour bird);

    void HandlePalmUpSelected(BirdBehaviour bird);
    void HandlePalmUpUnselected(BirdBehaviour bird);
    void HandlePerchSelected(BirdBehaviour bird);
    void HandlePerchUnselected(BirdBehaviour bird);
}