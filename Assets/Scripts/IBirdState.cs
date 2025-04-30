public interface IBirdState
{
    void Enter(BirdBehaviour bird);
    void Update(BirdBehaviour bird);
    void Exit(BirdBehaviour bird);
    void OnDone(BirdBehaviour bird);
}