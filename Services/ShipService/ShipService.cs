using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace battleship_server.Services.ShipService
{
    public class ShipService : IShipService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dataContext;

        public ShipService(IMapper mapper, DataContext dataContext)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<GetShipDto>>> AddShip(AddShipDto newShip)
        {
            // New ServiceResponse
            var serviceResponse = new ServiceResponse<List<GetShipDto>>();
            // Map the newShip
            var ship = _mapper.Map<Ship>(newShip);
            // Add the newShip to the context
            _dataContext.Ships.Add(ship);
            // Async save changes to the db
            await _dataContext.SaveChangesAsync();

            // Retrieve all the Ships to return
            serviceResponse.Data = await _dataContext.Ships.Select(ship => _mapper.Map<GetShipDto>(ship)).ToListAsync();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetShipDto>>> DeleteShip(int id)
        {
            // New ServiceResponse
            var serviceResponse = new ServiceResponse<List<GetShipDto>>();

            try
            {
                var ship = await _dataContext.Ships.FirstOrDefaultAsync(ship => ship.ID == id);
                if (ship is null)
                {
                    throw new Exception($"Cannot find ship with ID {id}");
                }

                // Remove
                _dataContext.Ships.Remove(ship);

                // Save changes
                await _dataContext.SaveChangesAsync();

                serviceResponse.Data = await _dataContext.Ships.Select(item => _mapper.Map<GetShipDto>(item)).ToListAsync();
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
            // New serviceResponse object
            var serviceResponse = new ServiceResponse<List<GetShipDto>>();
            
            // Query from DB
            var dbShip = await _dataContext.Ships.ToListAsync();

            serviceResponse.Data = dbShip.Select(ship => _mapper.Map<GetShipDto>(ship)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetShipDto>> GetShipById(int id)
        {
            // New ServiceResponse
            var serviceResponse = new ServiceResponse<GetShipDto>();

            // Query from DB
            var dbShip = await _dataContext.Ships.FirstOrDefaultAsync(ship => ship.ID == id);

            // Using AutoMapper to map the ship to GetShipDtos
            serviceResponse.Data = _mapper.Map<GetShipDto>(dbShip);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetShipDto>> UpdateShip(UpdateShipDto newShip)
        {
            // New ServiceResponse
            var serviceResponse = new ServiceResponse<GetShipDto>();

            try
            {
                // Look for the ship with the id
                var ship = await _dataContext.Ships.FirstOrDefaultAsync(ship => ship.ID == newShip.ID);
                if (ship is null)
                {
                    throw new Exception($"Cannot find ship with ID {newShip.ID}");
                }

                ship.Name = newShip.Name;
                ship.Size = newShip.Size;
                ship.Rarity = newShip.Rarity;

                // Save the changes to the DB
                await _dataContext.SaveChangesAsync();
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