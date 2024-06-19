using InteractiveCodeLab.Domain.Models;
using MediatR;

namespace InteractiveCodeLab.Application.Visualizations;

public record GetDataSetCommand(string AlgorithmId, VisualizationRegime Regime, int[]? CustomDataSet = default): IRequest<int[]>;