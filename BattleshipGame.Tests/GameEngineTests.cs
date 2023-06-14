using BattleshipGame.Enums;
using BattleshipGame.Services;
using FluentAssertions;
using FluentAssertions.Execution;

namespace BattleshipGame.Tests;

public class GameEngineTests
{
    [Fact]
    public void GetRandomCoordinate_ShouldReturnProperCoordinate()
    {
        var gameEngine = new GameEngine();
        var random = new Random();
        var boardSize = 10;
        
        var coordinate = gameEngine.GetRandomCoordinate(random, boardSize);

        using (new AssertionScope())
        {
            coordinate.Should().NotBeNull();
            coordinate.Row.Should().BeInRange(0, 10);
            coordinate.Column.Should().BeInRange(0, 10);
        }
    }

    [Fact]
    public void GetRandomDirection_ShouldReturnHorizontalOrVertical()
    {
        var random = new Random();
        var gameEngine = new GameEngine();

        var direction = gameEngine.GetRandomDirection(random);
        
        direction.Should().BeOneOf(ShipDirection.Horizontal, ShipDirection.Vertical);
    }
    
}