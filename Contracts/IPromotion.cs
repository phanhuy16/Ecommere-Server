
using Server.Entities;
using Server.Utilities.Response;

namespace Server.Contracts;
public interface IPromotion
{
    Task<Response<Promotion>> AddPromotion(Promotion promo);
}
