using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace battleship_server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShipController : ControllerBase
    {
        private readonly IShipService _shipService;

        public ShipController(IShipService shipService)
        {
            _shipService = shipService;
        }

        // API
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<GetShipDto>>>> GetAllShip()
        {
            return Ok(await _shipService.GetAllShip());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetShipDto>>> GetShipById(int id)
        {
            return Ok(await _shipService.GetShipById(id));
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<GetShipDto>>>> AddShip(AddShipDto newShip)
        {
            return Ok(await _shipService.AddShip(newShip));
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<GetShipDto>>> UpdateShip(UpdateShipDto newShip)
        {
            var response = await _shipService.UpdateShip(newShip);

            if (response.Data is null)
            {
                return NotFound(response);
            }
            else
            {
                return Ok(response);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<List<GetShipDto>>>> DeleteShip(int id)
        {
            var response = await _shipService.DeleteShip(id);

            if (response.Data is null)
            {
                return NotFound(response);
            }
            else
            {
                return Ok(response);
            }
        }
    }
}