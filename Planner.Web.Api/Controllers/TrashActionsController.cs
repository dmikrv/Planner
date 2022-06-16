using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Planner.Data;
using Planner.Web.Api.Models;

namespace Planner.Web.Api.Controllers
{
    [Authorize(Roles = "user")]
    [Route("api/[controller]")]
    [ApiController]
    public class TrashActionsController : ControllerBase
    {
        private readonly PlannerContext _db;
        private readonly ILogger<TrashActionsController> _logger;
        private readonly IMapper _mapper;

        public TrashActionsController(ILogger<TrashActionsController> logger,  PlannerContext db, IMapper mapper)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<TrashActionModel>> GetAllAsync(CancellationToken ct = default)
        {
            return await _db.TrashActions.Where(x => x.Account.UserName == User.Identity!.Name)
                .ProjectTo<TrashActionModel>(_mapper.ConfigurationProvider).ToArrayAsync(ct);
        }
        
        [HttpPut]
        public async Task<ActionResult<TrashActionModel>> UpdateAsync(TrashActionModel model, CancellationToken ct = default)
        {
            var entity = await _db.TrashActions.Include(x => x.Account)
                .FirstOrDefaultAsync(x => x.Id == model.Id, ct);

            if (entity is null || entity.Account.UserName != User.Identity!.Name)
                return Forbid();

            entity.Name = model.Text;

            _db.TrashActions.Update(entity);
            await _db.SaveChangesAsync(ct);
            
            return NoContent();
        }
        
        [HttpDelete]
        public async Task<ActionResult> DeleteAllAsync(CancellationToken ct = default)
        {
            var entities = _db.TrashActions.Where(x => x.Account.UserName == User.Identity!.Name);
            _db.TrashActions.RemoveRange(entities);
            await _db.SaveChangesAsync(ct);
            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(long id, CancellationToken ct = default)
        {
            var entity = await _db.TrashActions.Include(x => x.Account).FirstOrDefaultAsync(x => x.Id == id, ct);
            
            if (entity is null || entity.Account.UserName != User.Identity!.Name)
                return NoContent();
            
            _db.TrashActions.Remove(entity);
            await _db.SaveChangesAsync(ct);
            return NoContent();
        }
    }
}
