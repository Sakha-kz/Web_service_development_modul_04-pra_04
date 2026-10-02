using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TeamsApi.Models;
using TeamsApi.Repositories;

namespace TeamsApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeamController : ControllerBase
{
    private readonly ITeamRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<TeamController> _logger;

    public TeamController(ITeamRepository repository, IMapper mapper, ILogger<TeamController> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    // GET: /api/team
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("Getting all teams");
        var teams = await _repository.GetAllAsync();
        var dtos = _mapper.Map<IEnumerable<TeamDto>>(teams);
        return Ok(ReturnResult<IEnumerable<TeamDto>>.Success(dtos));
    }

    // GET: /api/team/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("Getting team with ID {TeamId}", id);
        var team = await _repository.GetAsync(id);
        if (team == null)
        {
            _logger.LogWarning("Team with ID {TeamId} not found", id);
            return NotFound(ReturnResult<TeamDto>.Failure($"Команда с ID {id} не найдена."));
        }

        var dto = _mapper.Map<TeamDto>(team);
        return Ok(ReturnResult<TeamDto>.Success(dto));
    }

    // POST: /api/team
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TeamDto dto)
    {
        if (dto == null || string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.City))
        {
            return BadRequest(ReturnResult<TeamDto>.Failure("Название команды и город являются обязательными."));
        }

        var team = _mapper.Map<Team>(dto);
        await _repository.CreateAsync(team);

        var createdDto = _mapper.Map<TeamDto>(team);
        _logger.LogInformation("Team {TeamName} created with ID {TeamId}", team.Name, team.Id);

        return CreatedAtAction(nameof(GetById), new { id = createdDto.Id }, ReturnResult<TeamDto>.Success(createdDto));
    }

    // PUT: /api/team/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] TeamDto dto)
    {
        if (dto == null || string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.City))
        {
            return BadRequest(ReturnResult<TeamDto>.Failure("Некорректные данные для обновления команды."));
        }

        var existing = await _repository.GetAsync(id);
        if (existing == null)
        {
            _logger.LogWarning("Team with ID {TeamId} not found for update", id);
            return NotFound(ReturnResult<TeamDto>.Failure($"Команда с ID {id} не найдена."));
        }

        _mapper.Map(dto, existing);
        existing.Id = id; // гарантируем сохранение ID

        await _repository.UpdateAsync(existing);
        var updatedDto = _mapper.Map<TeamDto>(existing);

        _logger.LogInformation("Team with ID {TeamId} successfully updated", id);
        return Ok(ReturnResult<TeamDto>.Success(updatedDto));
    }

    // DELETE: /api/team/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existing = await _repository.GetAsync(id);
        if (existing == null)
        {
            _logger.LogWarning("Team with ID {TeamId} not found for deletion", id);
            return NotFound(ReturnResult<bool>.Failure($"Команда с ID {id} не найдена."));
        }

        await _repository.DeleteAsync(id);
        _logger.LogInformation("Team with ID {TeamId} successfully deleted", id);

        return Ok(ReturnResult<bool>.Success(true));
    }

    // GET: /api/team/city/{city} (Самостоятельное задание: поиск по городу)
    [HttpGet("city/{city}")]
    public async Task<IActionResult> GetByCity(string city)
    {
        _logger.LogInformation("Searching teams by city: {City}", city);
        var teams = await _repository.GetByCityAsync(city);
        var dtos = _mapper.Map<IEnumerable<TeamDto>>(teams);

        return Ok(ReturnResult<IEnumerable<TeamDto>>.Success(dtos));
    }
}
