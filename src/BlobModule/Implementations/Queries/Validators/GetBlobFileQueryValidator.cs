using FluentValidation;
using BlobModule.Declarations.Repositories;
using BaseModule.Extensions;
using BlobModule.Declarations.Queries;

namespace BlobModule.Implementations.Queries.Validators
{
    public class GetBlobFileQueryValidator : AbstractValidator<GetBlobFileQuery>
    {
        private readonly IBlobFileRepository _blobFileRepository;

        public GetBlobFileQueryValidator(IBlobFileRepository blobFileRepository)
        {
            _blobFileRepository = blobFileRepository;

            RuleFor(x => x.FileId).NotNull().NotEmpty();
            RuleFor(x => x.FileId).MustAsync(BeAnExistingBlobFile).WithMessage("File doesn't exist.").When(x => !x.FileId.IsNullOrBlank());
        }

        private async Task<bool> BeAnExistingBlobFile(string fileId, CancellationToken token)
        {
            return await _blobFileRepository.BeAnExistingBlobFile(fileId);
        }
    }
}
