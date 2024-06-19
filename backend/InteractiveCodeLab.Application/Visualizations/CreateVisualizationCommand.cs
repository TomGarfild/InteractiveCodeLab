using InteractiveCodeLab.Domain.Models;
using MediatR;

namespace InteractiveCodeLab.Application.Visualizations;

public record CreateVisualizationCommand(UserCodeKey Key, int[] DataSet) : IRequest<List<VisualizationStep>?>;