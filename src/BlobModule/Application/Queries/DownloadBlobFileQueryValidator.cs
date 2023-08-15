using BaseModule.Infrastructure.Extensions;
using BlobModule.Domain.Repositories.Interfaces;
using FluentValidation;

namespace BlobModule.Application.Queries
{
    public class DownloadBlobFileQueryValidator : AbstractValidator<DownloadBlobFileQuery>
    {
        private readonly IBlobFileRepository _blobFileRepository;

        public DownloadBlobFileQueryValidator(IBlobFileRepository blobFileRepository)
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
