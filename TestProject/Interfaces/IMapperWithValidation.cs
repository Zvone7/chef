using LanguageExt.Common;

namespace Work.Interfaces;

public interface IMapperWithValidation<TDto, TVm>
{
    public Result<TDto> MapVmToDto(TVm vm);
    public Result<TVm> MapDtoToVm(TDto dto);
}