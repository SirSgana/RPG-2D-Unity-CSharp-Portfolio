public class StateMachine
{
    public EntityState currentState { get; private set; } //Riferimento/Incapsulamento allo stato attuale dallo script EntityState
    public bool canChangeState;
    public void Initialize(EntityState startState)
    {
        canChangeState = true;
        currentState = startState; //Imposta lo stato iniziale ma non lo attiva
        currentState.Enter();     //Chiama il metodo Enter dello stato iniziale
    }

    public void ChangeState(EntityState newState)
    {
        if (canChangeState == false)
            return;

        currentState.Exit();      //Chiama il metodo Exit dello stato attuale
        currentState = newState;  //Aggiorna lo stato attuale al nuovo stato
        currentState.Enter();     //Chiama il metodo Enter del nuovo stato
    }

    public void UpdateActiveState()
    {
        currentState.Update();
    }

    public void SwitchOffStateMachine() => canChangeState = false;
}
