using System.Threading.Tasks;

public interface IValueProvider<T>
{
    T GetValue();
}
