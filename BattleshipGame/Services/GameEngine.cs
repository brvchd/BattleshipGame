using BattleshipGame.Enums;
using BattleshipGame.Helpers;
using BattleshipGame.Models;

namespace BattleshipGame.Services;

public interface IGameEngine
{
    ShipStatus[,] InitializeGame(int boardSize);
    Coordinate GetRandomCoordinate(Random random, int size);
    ShipDirection GetRandomDirection(Random random);
    Coordinate GetEndCoordinate(Coordinate startCoordinate, int shipLength, ShipDirection shipDirection);
    bool AreAllShipsSunk(ShipStatus[,] ships);
    void PlaceShip(ShipStatus shipStatus, Coordinate startCoordinate, Coordinate endCoordinate, ShipStatus[,] ships);
    bool CoordinateIsValid(Coordinate coordinate, int boardSize);

    bool CanPlaceShip(Coordinate startCoordinate, Coordinate endCoordinate, ShipStatus[,] ships,
        int boardSize);
}

public class GameEngine : IGameEngine
{
    public ShipStatus[,] InitializeGame(int boardSize)
    {
        var random = new Random();
        var ships = new ShipStatus[boardSize, boardSize];

        foreach (var shipType in ShipTypes.Types)
        {
            for (var j = 0; j < shipType.Quantity; j++)
            {
                while (true)
                {
                    var startCoordinate = GetRandomCoordinate(random, boardSize);
                    var direction = GetRandomDirection(random);
                    var endCoordinate = GetEndCoordinate(startCoordinate, shipType.Length, direction);

                    if (CanPlaceShip(startCoordinate, endCoordinate, ships, boardSize))
                    {
                        PlaceShip(shipType.ShipStatus, startCoordinate, endCoordinate, ships);
                        break;
                    }
                }
            }
        }

        return ships;
    }

    public Coordinate GetRandomCoordinate(Random random, int size)
    {
        var row = random.Next(size);
        var column = random.Next(size);
        return new Coordinate(row, column);
    }

    public ShipDirection GetRandomDirection(Random random)
    {
        var value = random.Next(2);
        return value == 0 ? ShipDirection.Horizontal : ShipDirection.Vertical;
    }

    public Coordinate GetEndCoordinate(Coordinate startCoordinate, int shipLength, ShipDirection shipDirection)
    {
        if (shipDirection == ShipDirection.Horizontal)
            return new Coordinate(startCoordinate.Row, startCoordinate.Column + shipLength - 1);

        return new Coordinate(startCoordinate.Row + shipLength - 1, startCoordinate.Column);
    }

    public bool AreAllShipsSunk(ShipStatus[,] ships)
    {
        if (ships.Length == 0)
            return true;

        foreach (var ship in ships)
        {
            if (ship is ShipStatus.Ship)
                return false;
        }

        return true;
    }

    public bool CanPlaceShip(Coordinate startCoordinate, Coordinate endCoordinate,
        ShipStatus[,] ships, int boardSize)
    {
        if (ships.Length == 0 || boardSize < 1)
            return false;

        if (!CoordinateIsValid(startCoordinate, boardSize) || !CoordinateIsValid(endCoordinate, boardSize))
            return false;

        for (var row = startCoordinate.Row; row <= endCoordinate.Row; row++)
        {
            for (var column = startCoordinate.Column; column <= endCoordinate.Column; column++)
            {
                if (ships[row, column] != ShipStatus.None)
                    return false;
            }
        }

        return true;
    }

    public bool CoordinateIsValid(Coordinate coordinate, int boardSize) =>
        coordinate.Row >= 0 && coordinate.Row < boardSize &&
        coordinate.Column >= 0 && coordinate.Column < boardSize;

    public void PlaceShip(ShipStatus shipStatus, Coordinate startCoordinate, Coordinate endCoordinate, ShipStatus[,] ships)
    {
        for (var row = startCoordinate.Row; row <= endCoordinate.Row; row++)
        {
            for (var column = startCoordinate.Column; column <= endCoordinate.Column; column++)
            {
                ships[row, column] = shipStatus;
            }
        }
    }
}