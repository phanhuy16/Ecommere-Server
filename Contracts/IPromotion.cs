
using Server.Entities;
using Server.Utilities.Response;

namespace Server.Contracts;
public interface IPromotion
{
    Task<Response<Promotion>> AddPromotion(Promotion promo);
    Task<Response<List<Promotion>>> GetAllPromotion();
    Task<Response<Promotion>> UpdatePromotion(Promotion promo, Guid Id);
    Task<Response<Promotion>> DeletePromotion(Guid Id);
}
