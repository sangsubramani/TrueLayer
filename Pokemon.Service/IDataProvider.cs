using System.Threading.Tasks;

namespace Pokemon.Service
{
    public interface IDataProvider
    {
        Task<Persistency.Pokemon> GetData();
    }
}
