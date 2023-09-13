using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace battleship_server
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Create AutoMapper profiles here
            CreateMap<Ship, GetShipDto>();
            CreateMap<AddShipDto, Ship>();
            CreateMap<UpdateShipDto, Ship>();
        }
    }
}