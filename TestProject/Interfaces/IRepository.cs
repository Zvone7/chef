using LanguageExt.Common;
using Work.Database;

namespace Work.Interfaces
{

    public interface IRepository<T, K>
    {
        Task<Result<UserDto>> CreateAsync(T obj, CancellationToken cancellationToken);
        Task<Result<UserDto>> ReadByIdAsync(K id, CancellationToken cancellationToken);
        Task<Result<IEnumerable<UserDto>>> ReadAllAsync(CancellationToken cancellationToken);
        Task<Result<Boolean>> UpdateAsync(T obj, CancellationToken cancellationToken);
        Task<Result<Boolean>> RemoveByIdAsync(K id, CancellationToken cancellationToken);
    }
}