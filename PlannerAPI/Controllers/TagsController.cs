using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlannerAPI.Database;


namespace PlannerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TagsController : ControllerBase
{
    private readonly PlannerContext _db;
    private readonly ILogger<TagsController> _logger;
    private readonly IMapper _mapper;

    public TagsController(ILogger<TagsController> logger, PlannerContext db, IMapper mapper)
    {
        _db = db;
        _logger = logger;
        _mapper = mapper;
    }

    // [HttpGet]
    // public async Task<IEnumerable<TagModel>> GetAllAsync()
    // {
    //     long id = 1;
    //     var account = await _db.Accounts.FindAsync(id);
    //
    //     return _mapper.Map<ICollection<Tag>, TagModel[]>(account.Tags);
    // }
    
    // [HttpGet]
    // public async Task<IEnumerable<TagModel>> GetAllLabelsAsync()
    // {
    //     long id = 1;
    //     var account = await _db.Accounts.FindAsync(id);
    //
    //     return _mapper.Map<ICollection<Tag>, TagModel[]>(account.Tags);
    // }
    
    // [HttpGet]
    // [Route("/{id}")]
    // public async Task<ActionModel<TagModel>> GetAsync(long id)
    // {
    //     long userId = 1;
    //     var account = await _db.Accounts.FindAsync(id);
    //
    //    
    // }
    
    // [HttpPost]
    // public async Task<ActionResult<TagModel>> AddAsync(TagModel tagModel, CancellationToken ct)
    // {
        // long id = 1;
        // var account = await _db.Accounts.FindAsync(new object[] {id}, ct);
        //
        // // validation
        // var isTagExist = await _db.Tags.FirstOrDefaultAsync(x => x.AccountId == account.Id && x.Name == tagModel.Name);
        // var isAreaExist = await _db.Areas.FirstOrDefaultAsync(x => x.AccountId == account.Id && x.Name == tagModel.Name);
        // var isContactExist = await _db.Contacts.FirstOrDefaultAsync(x => x.AccountId == account.Id && x.Name == tagModel.Name);
        //
        // if (isTagExist is not null || isAreaExist is not null || isContactExist is not null)
        //     return BadRequest("имя занято");
        //
        // switch (tagModel.Type)
        // {
        //     case TagTypes.LABEL:
        //         account.Tags.Add(new Tag
        //         {
        //             Name = tagModel.Name,
        //             ColorId = tagModel.Color is not null ? (int) tagModel.Color : null,
        //         });
        //         break;
        //     case TagTypes.AREA:
        //         account.Areas.Add(new Area
        //         {
        //             Name = tagModel.Name,
        //             ColorId = tagModel.Color is not null ? (int) tagModel.Color : null,
        //         });
        //         break;
        //     case TagTypes.CONTACT:
        //         account.Contacts.Add(new Contact
        //         {
        //             Name = tagModel.Name,
        //             ColorId = tagModel.Color is not null ? (int) tagModel.Color : null,
        //         });
        //         break;
        //     default:
        //         return BadRequest();
        // }
        //
        // await _db.SaveChangesAsync(ct);
        //
        // return Ok();

        // return CreatedAtAction(nameof(GetAsync), new { id = user.Id }, user);
    // }
}