using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace battleship_server.Services.ShipService
{
    public interface IShipService
    {
        Task<ServiceResponse<List<GetShipDto>>> GetAllShip();
        Task<ServiceResponse<GetShipDto>> GetShipById(int id);
        Task<ServiceResponse<List<GetShipDto>>> AddShip(AddShipDto newShip);
        Task<ServiceResponse<GetShipDto>> UpdateShip(UpdateShipDto newShip);
        Task<ServiceResponse<List<GetShipDto>>> DeleteShip(int id);
    }
}