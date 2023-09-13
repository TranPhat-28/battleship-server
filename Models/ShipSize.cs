using System.Text.Json.Serialization;

namespace battleship_server.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ShipSize
    {
        S=2,
        M,
        L,
        XL
    }
}