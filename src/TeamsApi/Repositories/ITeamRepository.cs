using TeamsApi.Models;

namespace TeamsApi.Repositories;

public interface ITeamRepository
{
    Task<IEnumerable<Team>> GetAllAsync();
    Task<Team?> GetAsync(int id);
    Task CreateAsync(Team team);
    Task UpdateAsync(Team team);
    Task DeleteAsync(int id);
    Task<IEnumerable<Team>> GetByCityAsync(string city);
}
