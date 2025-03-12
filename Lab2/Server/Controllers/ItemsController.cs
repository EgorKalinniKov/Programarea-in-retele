using Microsoft.AspNetCore.Mvc;
using Server.Models;
using System.Collections.Generic;
using System.Linq;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private static List<Item> Items = new List<Item>
        {
            new Item { Id = 1, Name = "Item1", Description = "Description1" },
            new Item { Id = 2, Name = "Item2", Description = "Description2" }
        };

        [HttpGet]
        public ActionResult<IEnumerable<Item>> Get()
        {
            return Ok(Items);
        }

        [HttpGet("{id}")]
        public ActionResult<Item> Get(int id)
        {
            var item = Items.FirstOrDefault(i => i.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost]
        public ActionResult<Item> Post([FromBody] Item item)
        {
            Items.Add(item);
            return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Item item)
        {
            var existingItem = Items.FirstOrDefault(i => i.Id == id);
            if (existingItem == null)
            {
                return NotFound();
            }
            existingItem.Name = item.Name;
            existingItem.Description = item.Description;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var item = Items.FirstOrDefault(i => i.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            Items.Remove(item);
            return NoContent();
        }
    }
}