using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace battleship_server.DTOs.Ship
{
    public class AddShipUserDto
    {
        public int UserId { get; set; }
        public int ShipId { get; set; }
    }
}