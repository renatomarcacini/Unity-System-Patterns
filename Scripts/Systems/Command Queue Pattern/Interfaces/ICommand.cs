using System.Collections;

namespace Nato.Command
{
    public interface ICommand
    {
        IEnumerator Execute();
    }
}