using Devity.Extensions.Templates;

namespace Devity.Mailing;

public class DevityEmail
{
    public string EmailAddress { get; }
    public string SubjectMessage { get; }
    public DevityTemplate Template { get; }
    public List<string> Attachments { get; } = new();

    public DevityEmail(string emailAddress, string subjectMessage, DevityTemplate template)
    {
        EmailAddress = emailAddress;
        SubjectMessage = subjectMessage;
        Template = template;
    }

    public DevityEmail AddAttachment(string attachment)
    {
        Attachments.Add(attachment);
        return this;
    }
}
