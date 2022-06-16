using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Planner.Data;
using Planner.Data.Entities;
using Planner.Web.Api.Models;

namespace Planner.Web.Api.Controllers
{
    [Authorize(Roles = "user")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly PlannerContext _db;
        private readonly UserManager<Account> _userManager;
        private readonly ILogger<ProjectsController> _logger;
        private readonly IMapper _mapper;

        public ProjectsController(ILogger<ProjectsController> logger, UserManager<Account> userManager, PlannerContext db, IMapper mapper)
        {
            _db = db;
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<ProjectModel>> GetAllAsync(CancellationToken ct = default)
        {
            return await _db.Projects.Where(x => x.Account.UserName == User.Identity!.Name)
                .ProjectTo<ProjectModel>(_mapper.ConfigurationProvider).ToArrayAsync(ct);
        }

        [HttpGet("{id}")]
        [ActionName("GetAsync")]
        public async Task<ActionResult<ProjectModel>> GetAsync(long id, CancellationToken ct = default)
        {
            var entity = await _db.Projects
                .Include(x => x.Account)
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        
            if (entity is null || entity.Account.UserName != User.Identity!.Name)
                return Forbid();
        
            return _mapper.Map<ProjectModel>(entity);
        }
        
        [HttpPost]
        public async Task<ActionResult<ProjectModel>> AddAsync(ProjectModel model, CancellationToken ct = default)
        {
            var entity = _mapper.Map<Project>(model);
            entity.Id = default;
            entity.CreatedDate = DateTime.Now;
            entity.Account = await _userManager.FindByNameAsync(User.Identity!.Name);

            _db.Projects.Add(entity);
            await _db.SaveChangesAsync(ct);
        
            model.Id = entity.Id;
            return CreatedAtAction(nameof(GetAsync), new { id = model.Id }, model);
        }
        
        [HttpPut]
        public async Task<ActionResult<ProjectModel>> UpdateAsync(ProjectModel model, CancellationToken ct = default)
        {
            var entity = await _db.Projects.AsNoTracking().Include(x => x.Account)
                .FirstOrDefaultAsync(x => x.Id == model.Id, ct);
        
            if (entity is null || entity.Account.UserName != User.Identity!.Name)
                return await AddAsync(model, ct);
            
            var newEntity = _mapper.Map<Project>(model);
            _db.Update(newEntity);
            newEntity.CreatedDate = entity.CreatedDate;
            newEntity.Account = await _userManager.FindByNameAsync(User.Identity!.Name);
            
            await _db.SaveChangesAsync(ct);
            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(long id, [FromQuery]bool deleteAllAction, CancellationToken ct = default)
        {
            var entity = await _db.Projects.Include(x => x.Account)
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        
            if (entity is null || entity.Account.UserName != User.Identity!.Name)
                return NoContent();
        
            if (deleteAllAction)
                _db.Actions.RemoveRange(_db.Actions.Where(x => x.ProjectId == id));
            
            _db.Projects.Remove(entity);
            await _db.SaveChangesAsync(ct);
            return NoContent();
        }
    }
}
