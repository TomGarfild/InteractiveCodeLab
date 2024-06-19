namespace InteractiveCodeLab.Models.Dto;

public record AlgorithmDto(string Id, string Title, string Description, string SelectedLanguage, string Code, int[] Test1, int[] Test2, int[] Test3);

public record AlgorithmPreviewDto(string Id, string Title, string Description);
public record AlgorithmCodeDto(string SelectedLanguage, string Code);