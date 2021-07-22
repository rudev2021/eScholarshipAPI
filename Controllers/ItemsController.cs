using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eScholarshipAPI_Client.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eScholarshipAPI_Client.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly ItemConsumer _consumer;
        public ItemsController(ItemConsumer consumer)
        {
            _consumer = consumer;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var items = await _consumer.GetPublishedItems();
            return Ok(items);
        }
    }
}
