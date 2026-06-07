using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreAPI.Models;
using StoreAppService;
using StoreModels;
using System.Security.Cryptography.X509Certificates;

namespace StoreAPI.Controllers
{
    [Route("api/stores")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly StoreService _storeService;

        public StoreController()
        {
            _storeService = new StoreService();
        }
        [HttpGet]
        public ActionResult<IEnumerable<Store>> GetAllStores()
        {
            return Ok(_storeService.ViewStores());
        }
        [HttpGet("{id:guid}")]
        public ActionResult<Store> GetStoreById(Guid id)
        {
            var store = _storeService.GetStore(id);
            if (store == null)
                return NotFound($"Store with ID {id} not found.");

            return Ok(store);
        }
        [HttpPost]
        public IActionResult CreateStore([FromBody]Models.StoreModel store)
        {
            if (store == null)
            {
                return BadRequest("Store data is required");
            }

            var newStore = new StoreModels.Store
            {
                StoreId = Guid.NewGuid(),
                Name = store.Name,
                Location = store.Location,
                Profit = store.Profit,
                Expenses = store.Expenses,
                Employees = store.Employees,
                Products = store.Products
            };
             _storeService.AddStore(newStore);
            return CreatedAtAction(nameof(GetStoreById), new { id = newStore.StoreId }, newStore);



        }
        [HttpPut("{id:guid}")]
        public IActionResult UpdateStore(Guid id, [FromBody] StoreModel store)
        {
            if (store == null)
                return BadRequest("Store data is required.");

            var updated = new Store
            {
                StoreId = id,
                Name = store.Name,
                Location = store.Location,
                Profit = store.Profit,
                Expenses = store.Expenses,
                Employees = store.Employees,
                Products = store.Products
            };

            bool success = _storeService.UpdateStore(updated);

            if (!success)
                return NotFound($"Store with ID {id} not found.");

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public IActionResult DeleteStore(Guid id)
        {
            bool success = _storeService.DeleteStore(id);

            if (!success)
                return NotFound($"Store with ID {id} not found.");

            return NoContent();
        }


    }

}
