using System.IO;

namespace M.A.R.S.Core.Application.Interfaces
{
    public interface IActorContext<T> where T : IActor
    {
        T Actor { get; }
        TextReader In { get; }
        TextWriter Out { get; }
    }
}