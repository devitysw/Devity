using Devity.Extensions.Templates;
using Devity.NETCore.MailKit;
using Devity.NETCore.MailKit.Core;
using Devity.NETCore.MailKit.Infrastructure.Internal;

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
    /// Triggers an e-mail send using the mail service configured at startup.
    /// </summary>
    /// <param name="emailData">An e-mail in the data format.</param>
    protected Task SendEmailAsync(DevityEmail emailData) => SendEmailAsync(emailData, _emailService);

    /// <summary>
    /// Triggers an e-mail send through a different mail server/account than the one configured at
    /// startup - e.g. a per-tenant SMTP account instead of the app's own. Connects and authenticates
    /// on every call; callers that just want to validate credentials can call this with a minimal
    /// DevityEmail and treat a thrown exception as "invalid".
    /// </summary>
    /// <param name="emailData">An e-mail in the data format.</param>
    /// <param name="mailKitOptions">The mail server/account to send through, in place of the configured one.</param>
    protected Task SendEmailAsync(DevityEmail emailData, MailKitOptions mailKitOptions) =>
        SendEmailAsync(emailData, new EmailService(new MailKitProvider(mailKitOptions)));

    private async Task SendEmailAsync(DevityEmail emailData, IEmailService emailService)
    {
        await emailService.SendAsync(
            emailData.EmailAddress,
            _subjectFormat.Replace(TITLE_KEY, emailData.SubjectMessage),
            emailData.Template.PopulateTemplate(),
            emailData.Attachments.ToArray(),
            true
        );
    }
}
