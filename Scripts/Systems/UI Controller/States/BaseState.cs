namespace Nato.StateMachine
{
    public interface IState<T>
    {
        void OnStart(T manager);
        void OnTick();
        void OnEnd();
    }

    public abstract class BaseState<T> : IState<T>
    {
        protected T Manager;

        public virtual void OnStart(T manager)
        {
            Manager = manager;
        }

        public virtual void OnTick()
        {
        }

        public virtual void OnEnd()
        {
        }
    }
}