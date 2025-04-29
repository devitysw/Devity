using Devity.Extensions.Templates;

namespace Devity.Mailing;

public class DevityEmail
{
    public string EmailAddress { get; }
    public string SubjectMessage { get; }
    public string BodyPath { get; }
    public DevityTemplate Template { get; }
    public List<string> Attachments { get; } = new();

    public DevityEmail(
        string emailAddress,
        string subjectMessage,
        string bodyPath,
        DevityTemplate template
    )
    {
        EmailAddress = emailAddress;
        SubjectMessage = subjectMessage;
        BodyPath = bodyPath;
        Template = template;
    }

    public DevityEmail AddAttachment(string attachment)
    {
        Attachments.Add(attachment);
        return this;
    }
}
