namespace Devity.Mailing;

public class DevityEmail
{
    public string EmailAddress { get; }
    public string SubjectMessage { get; }
    public string BodyPath { get; }
    public Dictionary<string, string> KeyMap { get; } = new();
    public Dictionary<string, dynamic> LoopMap { get; } = new();
    public List<string> Attachments { get; } = new();

    public DevityEmail(string emailAddress, string subjectMessage, string bodyPath)
    {
        EmailAddress = emailAddress;
        SubjectMessage = subjectMessage;
        BodyPath = bodyPath;
    }

    public DevityEmail AddKey(string key, string value)
    {
        KeyMap.Add(key, value);
        return this;
    }

    public DevityEmail AddLoop(string key, dynamic loop)
    {
        LoopMap.Add(key, loop);
        return this;
    }

    public DevityEmail AddAttachment(string attachment)
    {
        Attachments.Add(attachment);
        return this;
    }
}
