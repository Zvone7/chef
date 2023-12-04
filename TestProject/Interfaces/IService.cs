using LanguageExt.Common;
using Work.ApiModels;

namespace Work.Interfaces;

public interface IService<T, K>
{
    Task<Result<UserVm>> CreateAsync(T obj, CancellationToken cancellationToken);
    Task<Result<UserVm>> ReadByIdAsync(K id, CancellationToken cancellationToken);
    Task<Result<List<UserVm>>> ReadAllAsync(CancellationToken cancellationToken);
    Task<Result<Boolean>> UpdateAsync(T obj, CancellationToken cancellationToken);
    Task<Result<Boolean>> RemoveByIdAsync(K id, CancellationToken cancellationToken);
}