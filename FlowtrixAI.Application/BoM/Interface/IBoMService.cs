using FlowtrixAI.Application.BoM.Dtos;

namespace FlowtrixAI.Application.BoM.Interface;

public interface IBoMService
{
    Task<bool> AddBoMForProduct(AddBoMdto dto);
}
