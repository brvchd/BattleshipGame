using BattleshipGame.Enums;
using BattleshipGame.Models;
using FluentAssertions;

namespace BattleshipGame.Tests;

public class GameEngineTests : IClassFixture<GameEngineFixture>
{
    private readonly GameEngineFixture _gameEngineFixture;

    public GameEngineTests(GameEngineFixture gameEngineFixture)
    {
        _gameEngineFixture = gameEngineFixture;
    }

    [Fact]
    public void InitializeGame_BoardsSize10_ShouldReturn_ShipsArrayOfLength100()
    {
        // Arrange
        ShipType[] shipTypes =
        {
            new(Name: "Battleship", Length: 5, Quantity: 1, ShipStatus: ShipStatus.Ship),
            new(Name: "Destroyer", Length: 4, Quantity: 2, ShipStatus: ShipStatus.Ship)
        };

        // Act
        var ships = _gameEngineFixture.GameEngine.InitializeGame(10, shipTypes);

        // Assert
        ships.Should().NotBeNull();
        ships!.Length.Should().Be(100);
    }

    [Fact]
    public void InitializeGame_BoardsSize10_ShouldReturn_ArrayWith13ShipCells()
    {
        //Arrange 
        var shipsCount = 0;
        ShipType[] shipTypes =
        {
            new(Name: "Battleship", Length: 5, Quantity: 1, ShipStatus: ShipStatus.Ship),
            new(Name: "Destroyer", Length: 4, Quantity: 2, ShipStatus: ShipStatus.Ship)
        };

        // Act
        var ships = _gameEngineFixture.GameEngine.InitializeGame(10, shipTypes);

        // Assert
        ships.Should().NotBeNull();

        foreach (var ship in ships!)
        {
            if (ship == ShipStatus.Ship)
                shipsCount++;
        }

        shipsCount.Should().Be(13);
    }

    [Fact]
    public void InitializeGame_BoardsSize0_ShouldReturn_Null()
    {
        //Arrange 
        ShipType[] shipTypes =
        {
            new(Name: "Battleship", Length: 5, Quantity: 1, ShipStatus: ShipStatus.Ship),
            new(Name: "Destroyer", Length: 4, Quantity: 2, ShipStatus: ShipStatus.Ship)
        };

        // Act
        var ships = _gameEngineFixture.GameEngine.InitializeGame(0, shipTypes);

        // Assert
        ships.Should().BeNull();
    }

    [Fact]
    public void InitializeGame_BoardsSize5_ShipLengthExceedsBoardSize_ShouldReturn_Null()
    {
        //Arrange 
        ShipType[] shipTypes = { new(Name: "Battleship", Length: 6, Quantity: 1, ShipStatus: ShipStatus.Ship) };

        // Act
        var ships = _gameEngineFixture.GameEngine.InitializeGame(5, shipTypes);

        // Assert
        ships.Should().BeNull();
    }

    [Fact]
    public void AreAllShipsSunk_BoardIsEmpty__ShouldReturn_True()
    {
        //Arrange 
        var emptyBoard = new ShipStatus[0, 0];

        // Act
        var ships = _gameEngineFixture.GameEngine.AreAllShipsSunk(emptyBoard);

        // Assert
        ships.Should().BeTrue();
    }
    
    [Fact]
    public void AreAllShipsSunk_BoardContainsAliveShips__ShouldReturn_False()
    {
        // Arrange 
        var emptyArray = new ShipStatus[3, 3];
        for (var i = 0; i < 3; i++)
        {
            emptyArray[1,i] = ShipStatus.Ship;
        }
        
        // Act
        var ships = _gameEngineFixture.GameEngine.AreAllShipsSunk(emptyArray);

        // Assert
        ships.Should().BeFalse();
    }
    
    [Fact]
    public void AreAllShipsSunk_AllShipsHitOnBoard__ShouldReturn_True()
    {
        // Arrange 
        var emptyArray = new ShipStatus[3, 3];
        for (var i = 0; i < 3; i++)
        {
            emptyArray[1,i] = ShipStatus.Hit;
        }
        
        // Act
        var ships = _gameEngineFixture.GameEngine.AreAllShipsSunk(emptyArray);

        // Assert
        ships.Should().BeTrue();
    }
}