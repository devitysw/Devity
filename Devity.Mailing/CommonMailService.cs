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
    /// on every call (the provider it builds is discarded after this one send); callers that just
    /// want to validate credentials can call this with a minimal DevityEmail and treat a thrown
    /// exception as "invalid". Callers sending more than one e-mail through the same account back to
    /// back should build their own <see cref="IEmailService"/> once (<c>new EmailService(new
    /// MailKitProvider(mailKitOptions))</c>) and reuse it via the <see cref="SendEmailAsync(DevityEmail, IEmailService)"/>
    /// overload instead - a fresh connect+authenticate per e-mail looks like a compromised-account
    /// login pattern to some mailbox providers' abuse detection, regardless of how the sends are paced.
    /// </summary>
    /// <param name="emailData">An e-mail in the data format.</param>
    /// <param name="mailKitOptions">The mail server/account to send through, in place of the configured one.</param>
    protected async Task SendEmailAsync(DevityEmail emailData, MailKitOptions mailKitOptions)
    {
        using var provider = new MailKitProvider(mailKitOptions);
        await SendEmailAsync(emailData, new EmailService(provider));
    }

    /// <summary>
    /// Triggers a multipart/alternative send (HTML + a plain-text fallback) using the mail
    /// service configured at startup - for recipients/clients that don't render HTML.
    /// </summary>
    /// <param name="emailData">An e-mail in the data format. Its Template is used as the HTML body.</param>
    /// <param name="plainTextMessage">The plain-text alternative body.</param>
    /// <param name="extraHeaders">Additional raw message headers to set (e.g. List-Unsubscribe), keyed by header name.</param>
    protected Task SendMultipartEmailAsync(DevityEmail emailData, string plainTextMessage, IDictionary<string, string>? extraHeaders = null) =>
        SendMultipartEmailAsync(emailData, plainTextMessage, _emailService, extraHeaders);

    /// <summary>
    /// Triggers a multipart/alternative send through a different mail server/account than the one
    /// configured at startup - e.g. a per-tenant SMTP account instead of the app's own. Connects and
    /// authenticates on every call (the provider it builds is discarded after this one send). Callers
    /// sending a batch through the same account back to back should build their own
    /// <see cref="IEmailService"/> once and reuse it via the
    /// <see cref="SendMultipartEmailAsync(DevityEmail, string, IEmailService, IDictionary{string, string}?)"/>
    /// overload instead - see that overload's remarks.
    /// </summary>
    /// <param name="emailData">An e-mail in the data format. Its Template is used as the HTML body.</param>
    /// <param name="plainTextMessage">The plain-text alternative body.</param>
    /// <param name="mailKitOptions">The mail server/account to send through, in place of the configured one.</param>
    /// <param name="extraHeaders">Additional raw message headers to set (e.g. List-Unsubscribe), keyed by header name.</param>
    protected async Task SendMultipartEmailAsync(
        DevityEmail emailData,
        string plainTextMessage,
        MailKitOptions mailKitOptions,
        IDictionary<string, string>? extraHeaders = null
    )
    {
        using var provider = new MailKitProvider(mailKitOptions);
        await SendMultipartEmailAsync(emailData, plainTextMessage, new EmailService(provider), extraHeaders);
    }

    /// <summary>
    /// Triggers a multipart/alternative send through an already-built <see cref="IEmailService"/>
    /// (e.g. <c>new EmailService(new MailKitProvider(mailKitOptions))</c>) instead of one built fresh
    /// for this single call. Build that <see cref="IEmailService"/> once per SMTP account and reuse it
    /// across a batch of sends through the same account - MailKitProvider keeps its underlying SMTP
    /// connection open and reuses it (reconnecting only if it actually drops) rather than
    /// connecting/authenticating separately for every e-mail, since a mailbox seeing many independent
    /// automated logins in a short span can get flagged/locked by the provider's own account-security
    /// system, distinct from and in addition to spam/content filtering. Dispose the provider (it
    /// implements <see cref="IDisposable"/>) once the batch is done to close the connection cleanly.
    /// </summary>
    /// <param name="emailData">An e-mail in the data format. Its Template is used as the HTML body.</param>
    /// <param name="plainTextMessage">The plain-text alternative body.</param>
    /// <param name="emailService">An <see cref="IEmailService"/> built once and reused across the batch.</param>
    /// <param name="extraHeaders">Additional raw message headers to set (e.g. List-Unsubscribe), keyed by header name.</param>
    protected Task SendMultipartEmailAsync(
        DevityEmail emailData,
        string plainTextMessage,
        IEmailService emailService,
        IDictionary<string, string>? extraHeaders = null
    ) => SendMultipartEmailAsyncCore(emailData, plainTextMessage, emailService, extraHeaders);

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

    private async Task SendMultipartEmailAsyncCore(
        DevityEmail emailData,
        string plainTextMessage,
        IEmailService emailService,
        IDictionary<string, string>? extraHeaders
    )
    {
        await emailService.SendMultipartAsync(
            emailData.EmailAddress,
            _subjectFormat.Replace(TITLE_KEY, emailData.SubjectMessage),
            emailData.Template.PopulateTemplate(),
            plainTextMessage,
            emailData.Attachments.ToArray(),
            extraHeaders: extraHeaders
        );
    }
}
