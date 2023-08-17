using NETCore.MailKit.Core;

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
            throw new Exception($"The subject format argument is missing it's dynamic parameter {TITLE_KEY}. Read constructor documentation for more information.");

        _subjectFormat = subjectFormat;
    }

    /// <summary>
    /// Triggers an e-mail send.
    /// </summary>
    /// <param name="emailData">An e-mail in the data format.</param>
    protected async Task SendEmailAsync(DevityEmail emailData)
    {
        if (!File.Exists(emailData.BodyPath))
            throw new FileNotFoundException("Could not find e-mail body file when sending e-mail.");

        string body = File.ReadAllText(emailData.BodyPath);

        foreach (var loop in emailData.LoopMap)
        {
            if (!body.Contains(loop.Key))
                throw new FormatException($"{TITLE_KEY} was missing in the e-mail template.");

            if (loop.Key.Length == 0)
                throw new FormatException($"The loop key was empty.");

            var startPoint = body.IndexOf(loop.Key);
            var lastKey = body.LastIndexOf(loop.Key);

            if (startPoint == lastKey)
                throw new FormatException($"The loop key was only found once in the e-mail template.");

            var endPoint = lastKey + loop.Key.Length;

            string partToReplace = body[startPoint..endPoint];

            body = body.Remove(startPoint, endPoint - startPoint);

            foreach (var obj in loop.Value.Objects)
            {
                var target = partToReplace;

                foreach (var key in loop.Value.KeyMap)
                {
                    try
                    {
                        target = target.Replace(key.Key, key.Value.Compile().DynamicInvoke(obj).ToString());
                    }
                    catch(Exception ex)
                    {
                        throw new Exception($"Failed to replace value of key {key.Key}. Underlying exception: {ex}");
                    }
                }

                body = body.Insert(startPoint, target);
                startPoint += target.Length;
            }

            body = body.Replace(loop.Key, string.Empty);
        }

        foreach (var dataPoint in emailData.KeyMap)
            body = body.Replace(dataPoint.Key, dataPoint.Value);

        await _emailService.SendAsync(emailData.EmailAddress, _subjectFormat.Replace(TITLE_KEY, emailData.SubjectMessage), body, true);
    }
}
