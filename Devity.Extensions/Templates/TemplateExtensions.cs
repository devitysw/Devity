namespace Devity.Extensions.Templates;

public static class TemplateHelper
{
    public static string PopulateTemplate(this DevityTemplate template)
    {
        if (!File.Exists(template.BodyPath))
            throw new FileNotFoundException("Could not find template file for populating.");

        string html = File.ReadAllText(template.BodyPath);

        foreach (var condition in template.ConditionMap)
        {
            if (!html.Contains(condition.Key))
                throw new FormatException($"{condition.Key} was missing in the template.");

            if (condition.Value)
            {
                html = html.Replace(condition.Key, string.Empty);
                continue;
            }

            var startPoint = html.IndexOf(condition.Key);
            var lastKey = html.LastIndexOf(condition.Key);

            if (startPoint == lastKey)
                throw new FormatException(
                    $"The condition key was only found once in the template."
                );

            var endPoint = lastKey + condition.Key.Length;

            html = html.Remove(startPoint, endPoint - startPoint);
        }

        foreach (var loop in template.LoopMap)
        {
            if (!html.Contains(loop.Key))
                throw new FormatException($"{loop.Key} was missing in the template.");

            if (loop.Key.Length == 0)
                throw new FormatException($"The loop key was empty.");

            var startPoint = html.IndexOf(loop.Key);
            var lastKey = html.LastIndexOf(loop.Key);

            if (startPoint == lastKey)
                throw new FormatException($"The loop key was only found once in the template.");

            var endPoint = lastKey + loop.Key.Length;

            string partToReplace = html[startPoint..endPoint];

            html = html.Remove(startPoint, endPoint - startPoint);

            foreach (var obj in loop.Value.Objects)
            {
                var target = partToReplace;

                foreach (var key in loop.Value.KeyMap)
                {
                    try
                    {
                        target = target.Replace(
                            key.Key,
                            key.Value.Compile().DynamicInvoke(obj).ToString()
                        );
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(
                            $"Failed to replace value of key {key.Key}. Underlying exception: {ex}"
                        );
                    }
                }

                html = html.Insert(startPoint, target);
                startPoint += target.Length;
            }

            html = html.Replace(loop.Key, string.Empty);
        }

        foreach (var dataPoint in template.KeyMap)
            html = html.Replace(dataPoint.Key, dataPoint.Value);

        return html;
    }
}
