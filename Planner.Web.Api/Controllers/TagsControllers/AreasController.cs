using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Planner.Data;
using Planner.Data.Entities;
using Planner.Web.Api.Models;

namespace Planner.Web.Api.Controllers.TagsControllers
{
    [Authorize(Roles = "user")]
    [Route("api/tags/[controller]")]
    [ApiController]
    public class AreasController : ControllerBase
    {
        private readonly PlannerContext _db;
        private readonly UserManager<Account> _userManager;
        private readonly ILogger<AreasController> _logger;
        private readonly IMapper _mapper;

        public AreasController(ILogger<AreasController> logger, UserManager<Account> userManager, PlannerContext db, IMapper mapper)
        {
            _db = db;
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<AreaTagModel>> GetAllAsync(CancellationToken ct = default)
        {
            return await _db.Areas.Where(x => x.Account.UserName == User.Identity!.Name)
                .ProjectTo<AreaTagModel>(_mapper.ConfigurationProvider).ToArrayAsync(ct);
        }

        [HttpGet("{id}")]
        [ActionName("GetAsync")]
        public async Task<ActionResult<AreaTagModel>> GetAsync(long id, CancellationToken ct = default)
        {
            var entity = await _db.Areas.Include(x => x.Account)
                .FirstOrDefaultAsync(x => x.Id == id, ct);

            if (entity is null || entity.Account.UserName != User.Identity!.Name)
                return Forbid();
        
            return _mapper.Map<AreaTagModel>(entity);
        }
        
        [HttpPost]
        public async Task<ActionResult<AreaTagModel>> AddAsync(AreaTagModel model, CancellationToken ct = default)
        {
            var entity = _mapper.Map<Area>(model);
            entity.Id = 0;
            entity.Account = await _userManager.FindByNameAsync(User.Identity!.Name);
            
            _db.Areas.Add(entity);
            await _db.SaveChangesAsync(ct);
        
            model.Id = entity.Id;
            return CreatedAtAction(nameof(GetAsync), new { id = model.Id }, model);
        }
        
        [HttpPut]
        public async Task<ActionResult<AreaTagModel>> UpdateAsync(AreaTagModel model, CancellationToken ct = default)
        {
            var entity = await _db.Areas.Include(x => x.Account)
                .FirstOrDefaultAsync(x => x.Id == model.Id, ct);
        
            if (entity is null || entity.Account.UserName != User.Identity!.Name)
                return await AddAsync(model, ct);
        
            entity.Name = model.Name;
            entity.Color = model.Color is not null ? _mapper.Map<Color>(model.Color) : null;
            
            _db.Areas.Update(entity);
            await _db.SaveChangesAsync(ct);
            
            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(long id, CancellationToken ct = default)
        {
            var entity = await _db.Areas.Include(x => x.Account)
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        
            if (entity is null || entity.Account.UserName != User.Identity!.Name)
                return NoContent();
        
            _db.Areas.Remove(entity);
            await _db.SaveChangesAsync(ct);
            return NoContent();
        }
    }
}
