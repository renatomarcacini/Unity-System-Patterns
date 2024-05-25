namespace Nato.StateMachine
{
    public class StateMachine<T>
    {
        public T _manager { get; private set; }
        public IState<T> CurrentState { get; private set; }
        public StateMachine(T manager)
        {
            _manager = manager;
        }

        public TState TransitionTo<TState>() where TState : IState<T>, new()
        {
            CurrentState?.OnEnd();
            TState newState = new TState();
            CurrentState = newState;
            CurrentState.OnStart(_manager);
            return newState;
        }


        public void TransitionTo<TState>(TState state) where TState : IState<T>, new()
        {
            CurrentState?.OnEnd();
            CurrentState = state;
            CurrentState.OnStart(_manager);
        }


        public void OnTick()
        {
            CurrentState?.OnTick();
        }
    }
}