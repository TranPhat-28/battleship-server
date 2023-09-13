using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace battleship_server.Services.ShipService
{
    public class ShipService : IShipService
    {
        private static List<Ship> demoShipList = new List<Ship> {
            new Ship {
                ID = 0,
                Name = "Basic Ship A",
                Size = ShipSize.S,
                Rarity = ShipRarity.Common,
            },
            new Ship {
                ID = 1,
                Name = "Basic Ship B",
                Size = ShipSize.M,
                Rarity = ShipRarity.Common,
            },
            new Ship {
                ID = 2,
                Name = "Basic Ship C",
                Size = ShipSize.L,
                Rarity = ShipRarity.Common,
            },
            new Ship {
                ID = 3,
                Name = "Basic Ship D",
                Size = ShipSize.XL,
                Rarity = ShipRarity.Common,
            },
        };
        private readonly IMapper _mapper;

        public ShipService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<GetShipDto>>> AddShip(AddShipDto newShip)
        {
            var serviceResponse = new ServiceResponse<List<GetShipDto>>();
            demoShipList.Add(_mapper.Map<Ship>(newShip));
            serviceResponse.Data = demoShipList.Select(ship => _mapper.Map<GetShipDto>(ship)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetShipDto>>> DeleteShip(int id)
        {
            // New ServiceResponse
            var serviceResponse = new ServiceResponse<List<GetShipDto>>();

            try
            {
                var ship = demoShipList.FirstOrDefault(ship => ship.ID == id);
                if (ship is null)
                {
                    throw new Exception($"Cannot find ship with ID {id}");
                }

                demoShipList.Remove(ship);

                serviceResponse.Data = demoShipList.Select(item => _mapper.Map<GetShipDto>(item)).ToList();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetShipDto>>> GetAllShip()
        {
            var serviceResponse = new ServiceResponse<List<GetShipDto>>();
            serviceResponse.Data = demoShipList.Select(ship => _mapper.Map<GetShipDto>(ship)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetShipDto>> GetShipById(int id)
        {
            // New ServiceResponse
            var serviceResponse = new ServiceResponse<GetShipDto>();

            // Using AutoMapper to map the ship to GetShipDtos
            var ship = demoShipList.FirstOrDefault(ship => ship.ID == id);
            serviceResponse.Data = _mapper.Map<GetShipDto>(ship);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetShipDto>> UpdateShip(UpdateShipDto newShip)
        {
            // New ServiceResponse
            var serviceResponse = new ServiceResponse<GetShipDto>();

            try
            {
                var ship = demoShipList.FirstOrDefault(ship => ship.ID == newShip.ID);
                if (ship is null)
                {
                    throw new Exception($"Cannot find ship with ID {newShip.ID}");
                }

                ship.Name = newShip.Name;
                ship.Size = newShip.Size;
                ship.Rarity = newShip.Rarity;

                serviceResponse.Data = _mapper.Map<GetShipDto>(ship);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }
    }
}