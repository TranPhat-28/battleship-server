using System.Text.Json.Serialization;

namespace battleship_server.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ShipRarity
    {
        Common,
        Rare,
        Legendary
    }
}