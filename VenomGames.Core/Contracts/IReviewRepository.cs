using VenomGames.Infrastructure.Data.Models;

namespace VenomGames.Core.Contracts
{
    public interface IReviewRepository : IRepository<Review>
    {
        Task<IEnumerable<Review>> GetReviewsByGameIdAsync(int gameId);
    }
}
