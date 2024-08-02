using dummyWebApi2.Models.Data;
using dummyWebApi2.Models.SharedDictionary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dummyWebApi2.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CustomItemController : ControllerBase
    {
        private readonly List<CustomItem> _customItems = new()
        {
            new(1, "test"),
            new(2, "another test"),
            new(3, "admin test")
        };

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<CustomItem[]>> GetAll()
        {
            var items = await Task.FromResult(_customItems);
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<CustomItem>> GetID(int id)
        {
            var item = await Task.FromResult(_customItems.FirstOrDefault(x => x.ID == id));
            if (item == null)
                return NotFound();
            else
                return Ok(item);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> CreateNew([FromBody]CustomItem item)
        {
            if (string.IsNullOrWhiteSpace(item.Name))
                return BadRequest();
            else
            {
                await Task.FromResult(() =>
                {
                    var nextMaxID = _customItems.Max(x => x.ID) + 1;
                    item.ID = nextMaxID;
                    _customItems.Add(item);
                });
                
                return Ok();
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = Policies.AppAdmin)]
        public async Task<ActionResult> DeleteID(int id)
        {
            var item = await Task.FromResult(_customItems.FirstOrDefault(x => x.ID == id));
            if (item == null)
            {
                return NotFound();
            }
            else
            {
                _customItems.Remove(item);
                return Ok();
            }
        }
    }
}
