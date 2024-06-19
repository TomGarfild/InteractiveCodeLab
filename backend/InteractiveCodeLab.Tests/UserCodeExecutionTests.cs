using InteractiveCodeLab.Application.Visualizations;
using InteractiveCodeLab.Application.Visualizations.AlgorithmVisualizationStrategies;
using InteractiveCodeLab.Domain.Models;
using InteractiveCodeLab.Infrastructure.Models;
using InteractiveCodeLab.Infrastructure.Repositories;
using InteractiveCodeLab.Infrastructure.Repositories.Specifications;
using NSubstitute;

namespace InteractiveCodeLab.UnitTests;

public class UserCodeExecutionTests
{
    private readonly IRepository<CompiledUserCodeData> _compiledCodeRepository;
    private readonly IAlgorithmSimulationFactory _factory;
    private readonly CreateVisualizationCommandHandler _handler;

    public UserCodeExecutionTests()
    {
        _compiledCodeRepository = Substitute.For<IRepository<CompiledUserCodeData>>();
        _factory = Substitute.For<IAlgorithmSimulationFactory>();
        _handler = new CreateVisualizationCommandHandler(_compiledCodeRepository, _factory);
    }

    [Fact]
    public async Task Handle_ShouldReturnNull_WhenNoCompiledCodeIsFound()
    {
        // Arrange
        var command =
            new CreateVisualizationCommand(new UserCodeKey("userid", "insert-sort", "java"), new[] { 1, 2, 3 });
        _compiledCodeRepository.GetOne(Arg.Any<CompiledCodeByAlgorithmIdAndLanguageSpecification>()).Returns((CompiledUserCodeData)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_ShouldCallGetStrategyWithCorrectAlgorithmId()
    {
        // Arrange
        var command = new CreateVisualizationCommand(new UserCodeKey("userid", "insert-sort", "java"), new[] { 1, 2, 3 });
        var compiledUserCodeData = new CompiledUserCodeData();
        _compiledCodeRepository.GetOne(Arg.Any<CompiledCodeByAlgorithmIdAndLanguageSpecification>()).Returns(compiledUserCodeData);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _factory.Received(1).GetStrategy("algo1");
    }

    [Fact]
    public async Task Handle_ShouldReturnVisualizationSteps_WhenSimulationSucceeds()
    {
        // Arrange
        var command = new CreateVisualizationCommand(new UserCodeKey("userid", "insert-sort", "java"), new[] { 1, 2, 3 });
        var compiledUserCodeData = new CompiledUserCodeData();
        var strategy = Substitute.For<IAlgorithmSimulationStrategy>();
        var visualizationSteps = new List<VisualizationStep> { new VisualizationStep() };
        strategy.Simulate(Arg.Any<Step[]>(), Arg.Any<Dictionary<string, int[]>>()).Returns(visualizationSteps);
        _compiledCodeRepository.GetOne(Arg.Any<CompiledCodeByAlgorithmIdAndLanguageSpecification>()).Returns(compiledUserCodeData);
        _factory.GetStrategy("algo1").Returns(strategy);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(visualizationSteps, result);
    }

    [Fact]
    public async Task Handle_ShouldPassCorrectDataToSimulateMethod()
    {
        // Arrange
        var command = new CreateVisualizationCommand(new UserCodeKey("userid", "insert-sort", "java"), new[] { 1, 2, 3 });
        var compiledUserCodeData = new CompiledUserCodeData();
        var compiledUserCode = new CompiledUserCode("id", new UserCodeKey("userid", "insert-sort", "java"), new Step[] {});
        var strategy = Substitute.For<IAlgorithmSimulationStrategy>();
        _compiledCodeRepository.GetOne(Arg.Any<CompiledCodeByAlgorithmIdAndLanguageSpecification>()).Returns(compiledUserCodeData);
        _factory.GetStrategy("algo1").Returns(strategy);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        strategy.Received(1).Simulate(compiledUserCode.Steps, Arg.Is<Dictionary<string, int[]>>(dict => dict["arr"] == command.DataSet));
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoStepsProvided()
    {
        // Arrange
        var command = new CreateVisualizationCommand(new UserCodeKey("userid", "insert-sort", "java"), new[] { 1, 2, 3 });
        var compiledUserCodeData = new CompiledUserCodeData();
        var strategy = Substitute.For<IAlgorithmSimulationStrategy>();
        var emptySteps = new List<VisualizationStep>();
        strategy.Simulate(Arg.Any<Step[]>(), Arg.Any<Dictionary<string, int[]>>()).Returns(emptySteps);
        _compiledCodeRepository.GetOne(Arg.Any<CompiledCodeByAlgorithmIdAndLanguageSpecification>()).Returns(compiledUserCodeData);
        _factory.GetStrategy("algo1").Returns(strategy);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task Handle_ShouldReturnStepsInCorrectOrder()
    {
        // Arrange
        var command = new CreateVisualizationCommand(new UserCodeKey("userid", "insert-sort", "java"), new[] { 1, 2, 3 });
        var compiledUserCodeData = new CompiledUserCodeData();
        var strategy = Substitute.For<IAlgorithmSimulationStrategy>();
        var steps = new List<VisualizationStep> 
        { 
        };
        strategy.Simulate(Arg.Any<Step[]>(), Arg.Any<Dictionary<string, int[]>>()).Returns(steps);
        _compiledCodeRepository.GetOne(Arg.Any<CompiledCodeByAlgorithmIdAndLanguageSpecification>()).Returns(compiledUserCodeData);
        _factory.GetStrategy("algo1").Returns(strategy);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(steps, result);
    }

    [Fact]
    public async Task Handle_ShouldPassCorrectAlgorithmIdToGetStrategy()
    {
        // Arrange
        var command = new CreateVisualizationCommand(new UserCodeKey("userid", "insert-sort", "java"), new[] { 1, 2, 3 });
        var compiledUserCodeData = new CompiledUserCodeData();
        _compiledCodeRepository.GetOne(Arg.Any<CompiledCodeByAlgorithmIdAndLanguageSpecification>()).Returns(compiledUserCodeData);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _factory.Received(1).GetStrategy(compiledUserCodeData.AlgorithmId);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenCompiledUserCodeDataIsNull()
    {
        // Arrange
        var command = new CreateVisualizationCommand(new UserCodeKey("userid", "insert-sort", "java"), new[] { 1, 2, 3 });
        _compiledCodeRepository.GetOne(Arg.Any<CompiledCodeByAlgorithmIdAndLanguageSpecification>()).Returns((CompiledUserCodeData)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenGetStrategyReturnsNull()
    {
        // Arrange
        var command = new CreateVisualizationCommand(new UserCodeKey("userid", "insert-sort", "java"), new[] { 1, 2, 3 });
        var compiledUserCodeData = new CompiledUserCodeData();
        _compiledCodeRepository.GetOne(Arg.Any<CompiledCodeByAlgorithmIdAndLanguageSpecification>()).Returns(compiledUserCodeData);
        _factory.GetStrategy(Arg.Any<string>()).Returns((IAlgorithmSimulationStrategy)null);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldReturnStepsFromSimulation_WhenCalledWithValidCommand()
    {
        // Arrange
        var command = new CreateVisualizationCommand(new UserCodeKey("userid", "insert-sort", "java"), new[] { 1, 2, 3 });
        var compiledUserCodeData = new CompiledUserCodeData();
        var strategy = Substitute.For<IAlgorithmSimulationStrategy>();
        var visualizationSteps = new List<VisualizationStep> { new VisualizationStep() };
        strategy.Simulate(Arg.Any<Step[]>(), Arg.Any<Dictionary<string, int[]>>()).Returns(visualizationSteps);
        _compiledCodeRepository.GetOne(Arg.Any<CompiledCodeByAlgorithmIdAndLanguageSpecification>()).Returns(compiledUserCodeData);
        _factory.GetStrategy("algo1").Returns(strategy);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(visualizationSteps, result);
    }
}
