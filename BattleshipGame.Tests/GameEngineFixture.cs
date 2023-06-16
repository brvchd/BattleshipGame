using BattleshipGame.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace BattleshipGame.Tests;

// ReSharper disable once ClassNeverInstantiated.Global
public class GameEngineFixture
{
    public IGameEngine GameEngine { get; private set; }
    
    public GameEngineFixture()
    {
        var mockLogger = new Mock<ILogger<GameEngine>>();
        var logger = mockLogger.Object;
        GameEngine = new GameEngine(logger);
    }
    
}