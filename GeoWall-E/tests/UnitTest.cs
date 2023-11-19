namespace tests;
public class EvaluatorTests
{
    [Fact]
    public void Evaluate_Should_Return_EmptyList_When_Root_Is_Empty()
    {
        // Arrange
        var evaluator = new Evaluator(new List<Node>());

        // Act
        var result = evaluator.Evaluate();

        // Assert
        Assert.Empty(result);
    }
}