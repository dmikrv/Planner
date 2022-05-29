using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlannerAPI.Database;
using PlannerAPI.Database.Entities;
using PlannerAPI.Models;

namespace PlannerAPI.Controllers.TagsControllers
{
    [Route("api/tags/[controller]")]
    [ApiController]
    public class LabelsController : ControllerBase
    {
        private readonly PlannerContext _db;
        private readonly ILogger<LabelsController> _logger;
        private readonly IMapper _mapper;

        public LabelsController(ILogger<LabelsController> logger, PlannerContext db, IMapper mapper)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<LabelTagModel>> GetAllAsync(CancellationToken ct)
        {
            const long accountId = 1;

            return await _db.Tags.Where(x => x.AccountId == accountId)
                .ProjectTo<LabelTagModel>(_mapper.ConfigurationProvider).ToArrayAsync(ct);
        }
        
        [HttpGet("{id}")]
        [ActionName("GetAsync")]
        public async Task<ActionResult<LabelTagModel>> GetAsync(long id, CancellationToken ct)
        {
            const long accountId = 1;

            var entity = await _db.Tags.FirstOrDefaultAsync(x => x.Id == id, ct);

            if (entity is null || entity.AccountId != accountId)
                return BadRequest();// Forbid();

            return _mapper.Map<LabelTagModel>(entity);
        }

        [HttpPost]
        public async Task<ActionResult<LabelTagModel>> AddAsync(LabelTagModel model, CancellationToken ct)
        {
            const long accountId = 1;

            var entity = _mapper.Map<Tag>(model);
            entity.AccountId = accountId;
            _db.Tags.Add(entity);
            await _db.SaveChangesAsync(ct);

            model.Id = entity.Id;
            return CreatedAtAction(nameof(GetAsync), new { id = model.Id }, model);
        }
        
        [HttpPut]
        public async Task<ActionResult<LabelTagModel>> UpdateAsync(LabelTagModel model, CancellationToken ct)
        {
            const long accountId = 1;
            
            var entity = await _db.Tags.FirstOrDefaultAsync(x => x.Id == model.Id, ct);

            if (entity is null)
            {
                model.Id = 0;
                return await AddAsync(model, ct);
            }
            
            if (entity.AccountId != accountId)
                return BadRequest();

            entity.Name = model.Name;
            entity.Color = _mapper.Map<Color>(model.Color);
            
            _db.Tags.Update(entity);
            await _db.SaveChangesAsync(ct);
            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(long id, CancellationToken ct)
        {
            const long accountId = 1;

            var entity = await _db.Tags.FirstOrDefaultAsync(x => x.Id == id, ct);

            if (entity is null)
                return NoContent();

            _db.Tags.Remove(entity);
            await _db.SaveChangesAsync(ct);
            return NoContent();
        }
    }
}
