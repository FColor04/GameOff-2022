using System.Threading.Tasks;

namespace Firearms.Actions
{
    public interface IValueProvider<T>
    {
        T GetValue();
    }
}