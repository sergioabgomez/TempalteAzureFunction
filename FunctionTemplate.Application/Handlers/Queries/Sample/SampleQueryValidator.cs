using FluentValidation;

namespace FunctionTemplate.Application.Handlers.Queries.Sample
{
	public class SampleQueryValidator : AbstractValidator<SampleQuery>
	{
		public SampleQueryValidator()
		{
			RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
		}
	}
}
