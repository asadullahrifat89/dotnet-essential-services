﻿using Blob.Domain.Repositories.Interfaces;
using FluentValidation;

namespace Blob.Application.Commands.Validators
{
    public class UploadBlobFileCommandValidator : AbstractValidator<UploadBlobFileCommand>
    {
        private readonly IBlobFileRepository _blobFileRepository;

        public UploadBlobFileCommandValidator(IBlobFileRepository blobFileRepository)
        {
            _blobFileRepository = blobFileRepository;

            RuleFor(x => x.FormFile).NotNull();
            RuleFor(x => x).Must(x => x.FormFile.Length > 0).When(x => x.FormFile != null);
            RuleFor(x => x.FormFile.Name).NotNull().NotEmpty().When(x => x.FormFile != null);
            RuleFor(x => x.FormFile.ContentType).NotNull().When(x => x.FormFile != null);
        }
    }
}
