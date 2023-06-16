using System.Diagnostics.CodeAnalysis;
using BattleshipGame.Enums;
using BattleshipGame.Models;

namespace BattleshipGame.Services;

public interface IGameEngine
{
    ShipStatus[,]? InitializeGame(int boardSize, ShipType[] shipTypes);
    bool AreAllShipsSunk(ShipStatus[,] ships);
}

// Logger used only inside this place. There is not much need to "optimise" logger with the use of LoggerMessage appraoch
[SuppressMessage("Performance", "CA1848:Use the LoggerMessage delegates")]
public class GameEngine : IGameEngine
{
    private readonly ILogger<GameEngine> _logger;

    public GameEngine(ILogger<GameEngine> logger)
    {
        _logger = logger;
    }

    public ShipStatus[,]? InitializeGame(int boardSize, ShipType[] shipTypes)
    {
        if (boardSize < 1)
        {
            _logger.LogWarning("GameEngine:InitializeGame: boardSize is less than 1");
            return null;
        }

        var random = new Random();
        var ships = new ShipStatus[boardSize, boardSize];

        foreach (var shipType in shipTypes)
        {
            if (shipType.Length > boardSize)
            {
                _logger.LogWarning("GameEngine:InitializeGame: Ship lenght exceeds board size");
                return null;
            }

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

    private static Coordinate GetRandomCoordinate(Random random, int size)
    {
        var row = random.Next(size);
        var column = random.Next(size);
        return new Coordinate(row, column);
    }

    private static ShipDirection GetRandomDirection(Random random)
    {
        var value = random.Next(2);
        return value == 0 ? ShipDirection.Horizontal : ShipDirection.Vertical;
    }

    private static Coordinate GetEndCoordinate(Coordinate startCoordinate, int shipLength, ShipDirection shipDirection)
    {
        if (shipDirection == ShipDirection.Horizontal)
            return new Coordinate(startCoordinate.Row, startCoordinate.Column + shipLength - 1);

        return new Coordinate(startCoordinate.Row + shipLength - 1, startCoordinate.Column);
    }

    private static bool CanPlaceShip(Coordinate startCoordinate, Coordinate endCoordinate,
        ShipStatus[,] ships, int boardSize)
    {
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

    private static bool CoordinateIsValid(Coordinate coordinate, int boardSize) =>
        coordinate.Row >= 0 && coordinate.Row < boardSize &&
        coordinate.Column >= 0 && coordinate.Column < boardSize;

    private static void PlaceShip(ShipStatus shipStatus, Coordinate startCoordinate, Coordinate endCoordinate,
        ShipStatus[,] ships)
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