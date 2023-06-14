using BattleshipGame.Enums;
using BattleshipGame.Models;

namespace BattleshipGame.Helpers;

public static class ShipTypes
{
    public static ShipType[] Types { get; } = {
        new(Name: "Battleship", Length: 5, Quantity: 1, ShipStatus: ShipStatus.Ship),
        new(Name: "Destroyer", Length: 4, Quantity: 2, ShipStatus: ShipStatus.Ship)
    };
}