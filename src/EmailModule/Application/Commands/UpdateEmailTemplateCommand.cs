namespace EmailModule.Application.Commands
{
    public class UpdateEmailTemplateCommand : CreateEmailTemplateCommand
    {
        public string TemplateId { get; set; } = string.Empty;
    }
}
