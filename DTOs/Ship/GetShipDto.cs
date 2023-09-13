using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace battleship_server.DTOs.Ship
{
    public class GetShipDto
    {
        public int ID { get; set; }
        public string Name { get; set; } = "Basic Ship";
        public ShipSize Size { get; set; }
        public ShipRarity Rarity { get; set; }
    }
}