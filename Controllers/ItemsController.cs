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

        [HttpGet]   // url: ~/api/items/
        public async Task<IActionResult> Get()
        {
            var items = await _consumer.GetPublishedItems();
            return Ok(items);
        }

        [HttpGet]
        [Route("No1")]      // url: ~/api/items/No1/
        public async Task<IActionResult> GetNo1()
        {
            var items = await _consumer.GetPublishedItems();
            // #1: Find all of the items published between April 2, 2020 and June 30, 2021, and the units they are associated with.
            DateTime dStart = new DateTime(2020, 4, 2);
            DateTime dEnd = new DateTime(2021, 7, 1);
            var items1 = ItemConsumer.GetItemsWithinPeriod(items, dStart, dEnd);  // list1: #1  (count: 76)
            return Ok(items1);
        }

        [HttpGet]
        [Route("No2")]      // url: ~/api/items/No2/
        public async Task<IActionResult> GetNo2()
        {
            var items = await _consumer.GetPublishedItems();
            // #1: Find all of the items published between April 2, 2020 and June 30, 2021, and the units they are associated with.
            DateTime dStart = new DateTime(2020, 4, 2);
            DateTime dEnd = new DateTime(2021, 7, 1);
            var items1 = ItemConsumer.GetItemsWithinPeriod(items, dStart, dEnd);  // list1: #1  (count: 76)
            // #2: present a count of the number of items per unit, published during that time frame.
            var items2 = ItemConsumer.GetItemsPerUnit(items1);
            return Ok(items2);
        }

        [HttpGet]
        [Route("File")]      // url: ~/api/items/file/
        public async Task<IActionResult> GetFile()
        {
            var items = await _consumer.GetItemsFromFile();
            // #1: Find all of the items published between April 2, 2020 and June 30, 2021, and the units they are associated with.
            DateTime dStart = new DateTime(2020, 4, 2);
            DateTime dEnd = new DateTime(2021, 7, 1);
            var items1 = ItemConsumer.GetItemsWithinPeriod(items, dStart, dEnd);  // list1: #1  (count: 76)
            // #2: present a count of the number of items per unit, published during that time frame.
            var items2 = ItemConsumer.GetItemsPerUnit(items1);
            return Ok(items2);
        }

    }
}
