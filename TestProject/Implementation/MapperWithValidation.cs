using AutoMapper;
using LanguageExt.Common;
using Work.Interfaces;

namespace Work.Implementation;

public class MapperWithValidation<TDto, TVm> : IMapperWithValidation<TDto, TVm>
{
    private readonly IMapper _mapper_;
    private readonly ILogger<MapperWithValidation<TDto, TVm>> _logger_;
    public MapperWithValidation(
        IMapper mapper,
        ILogger<MapperWithValidation<TDto, TVm>> logger)
    {
        _mapper_ = mapper;
        _logger_ = logger;
    }

    public Result<TDto> MapVmToDto(TVm vm)
    {
        try
        {
            var mapped = _mapper_.Map<TDto>(vm);
            return new Result<TDto>(mapped);
        }
        catch (Exception e)
        {
            _logger_.LogError($"Exception mapping.", e);
            return new Result<TDto>(e);
        }
    }
    public Result<TVm> MapDtoToVm(TDto dto)
    {
        try
        {
            var mapped = _mapper_.Map<TVm>(dto);
            return new Result<TVm>(mapped);
        }
        catch (Exception e)
        {
            _logger_.LogError($"Exception mapping.", e);
            return new Result<TVm>(e);
        }
    }
}