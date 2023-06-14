using BattleshipGame.Enums;

namespace BattleshipGame.Models;

public record ShipType(string Name, int Length, int Quantity, CellStatus CellStatus);