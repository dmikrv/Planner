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
    public class LabelsController : ControllerBase
    {
        private readonly PlannerContext _db;
        private readonly UserManager<Account> _userManager;
        private readonly ILogger<LabelsController> _logger;
        private readonly IMapper _mapper;

        public LabelsController(ILogger<LabelsController> logger, UserManager<Account> userManager, PlannerContext db, IMapper mapper)
        {
            _db = db;
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<LabelTagModel>> GetAllAsync(CancellationToken ct = default)
        {
            return await _db.Tags.Where(x => x.Account.UserName == User.Identity!.Name)
                .ProjectTo<LabelTagModel>(_mapper.ConfigurationProvider).ToArrayAsync(ct);
        }

        [HttpGet("{id}")]
        [ActionName("GetAsync")]
        public async Task<ActionResult<LabelTagModel>> GetAsync(long id, CancellationToken ct = default)
        {
            var entity = await _db.Tags.Include(x => x.Account)
                .FirstOrDefaultAsync(x => x.Id == id, ct);

            if (entity is null || entity.Account.UserName != User.Identity!.Name)
                return Forbid();
        
            return _mapper.Map<LabelTagModel>(entity);
        }
        
        [HttpPost]
        public async Task<ActionResult<LabelTagModel>> AddAsync(LabelTagModel model, CancellationToken ct = default)
        {
            var entity = _mapper.Map<Tag>(model);
            entity.Id = 0;
            entity.Account = await _userManager.FindByNameAsync(User.Identity!.Name);
            
            _db.Tags.Add(entity);
            await _db.SaveChangesAsync(ct);
        
            model.Id = entity.Id;
            return CreatedAtAction(nameof(GetAsync), new { id = model.Id }, model);
        }
        
        [HttpPut]
        public async Task<ActionResult<LabelTagModel>> UpdateAsync(LabelTagModel model, CancellationToken ct = default)
        {
            var entity = await _db.Tags.Include(x => x.Account)
                .FirstOrDefaultAsync(x => x.Id == model.Id, ct);
        
            if (entity is null || entity.Account.UserName != User.Identity!.Name)
                return await AddAsync(model, ct);

            entity.Name = model.Name;
            entity.Color = model.Color is not null ? _mapper.Map<Color>(model.Color) : null;
            
            _db.Tags.Update(entity);
            await _db.SaveChangesAsync(ct);
            
            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(long id, CancellationToken ct = default)
        {
            var entity = await _db.Tags.Include(x => x.Account)
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        
            if (entity is null || entity.Account.UserName != User.Identity!.Name)
                return NoContent();
        
            _db.Tags.Remove(entity);
            await _db.SaveChangesAsync(ct);
            return NoContent();
        }
    }
}
