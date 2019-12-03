namespace M.A.R.S.Core.Application.Interfaces
{
    public interface IAction<T> where T : IActor
    {
        void Execute(IActorContext<T> context);
    }
}
