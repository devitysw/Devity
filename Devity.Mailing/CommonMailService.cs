using Devity.Extensions.Templates;
using Devity.NETCore.MailKit.Core;

namespace Devity.Mailing;

public abstract class CommonMailService
{
    private readonly IEmailService _emailService;

    protected const string TITLE_KEY = "-TITLE-";

    private string _subjectFormat;

    /// <summary>
    /// Constructs a new CommonMailService.
    /// </summary>
    /// <param name="mailService">Reference to IEmailService from MailKit.</param>
    /// <param name="subjectFormat">The format of how the e-mail subject should be laid out. Use the TITLE_KEY constant for dynamically inputting title.</param>
    public CommonMailService(IEmailService mailService, string subjectFormat)
    {
        _emailService = mailService;

        if (!subjectFormat.Contains(TITLE_KEY))
            throw new Exception(
                $"The subject format argument is missing it's dynamic parameter {TITLE_KEY}. Read constructor documentation for more information."
            );

        _subjectFormat = subjectFormat;
    }

    /// <summary>
    /// Triggers an e-mail send.
    /// </summary>
    /// <param name="emailData">An e-mail in the data format.</param>
    protected async Task SendEmailAsync(DevityEmail emailData)
    {
        await _emailService.SendAsync(
            emailData.EmailAddress,
            _subjectFormat.Replace(TITLE_KEY, emailData.SubjectMessage),
            emailData.Template.PopulateTemplate(),
            emailData.Attachments.ToArray(),
            true
        );
    }
}
