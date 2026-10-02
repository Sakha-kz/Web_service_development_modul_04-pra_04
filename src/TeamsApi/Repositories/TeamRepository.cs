using Microsoft.EntityFrameworkCore;
using TeamsApi.Data;
using TeamsApi.Models;

namespace TeamsApi.Repositories;

public class TeamRepository : ITeamRepository
{
    private readonly AppDbContext _context;

    public TeamRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Team>> GetAllAsync()
    {
        return await _context.Teams.AsNoTracking().ToListAsync();
    }

    public async Task<Team?> GetAsync(int id)
    {
        return await _context.Teams.FindAsync(id);
    }

    public async Task CreateAsync(Team team)
    {
        await _context.Teams.AddAsync(team);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Team team)
    {
        _context.Teams.Update(team);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var team = await _context.Teams.FindAsync(id);
        if (team != null)
        {
            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Team>> GetByCityAsync(string city)
    {
        return await _context.Teams
            .AsNoTracking()
            .Where(t => t.City.ToLower() == city.ToLower())
            .ToListAsync();
    }
}
