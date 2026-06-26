using FluentValidation;

namespace FunctionTemplate.Application.Handlers.Commands.CreateSample
{
	public class CreateSampleCommandValidator : AbstractValidator<CreateSampleCommand>
	{
		public CreateSampleCommandValidator()
		{
			RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
			RuleFor(x => x.Description).MaximumLength(500);
		}
	}
}
