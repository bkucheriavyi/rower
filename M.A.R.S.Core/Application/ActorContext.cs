using System.IO;
using M.A.R.S.Core.Application.Interfaces;

namespace M.A.R.S.Core.Application
{
    public class ActorContext<T> : IActorContext<T> where T: IActor
    {
        public T Actor { get; private set; }
        public TextReader In { get; private set; }
        public TextWriter Out { get; private set; }

        public ActorContext(T actor, TextReader input, TextWriter output)
        {
            Actor = actor;
            In = input;
            Out = output;
        }
    }
}