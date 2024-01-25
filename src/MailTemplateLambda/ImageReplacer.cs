using System.Text.RegularExpressions;
using Amazon.Lambda.Core;

namespace MailTemplateLambda;

public class ImageReplacer
{

    private readonly string html;
    private readonly ILambdaLogger logger;
    public ImageReplacer(string html, ILambdaLogger logger)
    {
        this.html = html;
        this.logger = logger;
    }

    private Regex pngrx = new Regex(@"\<img.*src=(('|"")http[^('|"")]+.png('|""))[^>]*>");
    private Regex jpgrx = new Regex(@"\<img.*src=(('|"")http[^('|"")]+.jpe?g('|""))[^>]*>");

    public async Task<string> replaceImages()
    {
        var nhtml = html;
        var pngmatches = pngrx.Matches(nhtml);
        foreach (Match match in pngmatches)
        {
            var urlx = match.Groups[1].Value;
            var url = urlx.Trim("\"'".ToCharArray());
            var newUrl = await getImageData(url, "image/png");
            if (!string.IsNullOrEmpty(newUrl))
            {
                nhtml = nhtml.Replace(url, newUrl);
            }
        }
        var jpgmatches = jpgrx.Matches(nhtml);
        foreach (Match match in jpgmatches)
        {
            var urlx = match.Groups[1].Value;
            var url = urlx.Trim("\"'".ToCharArray());
            var newUrl = await getImageData(url, "image/jpeg");
            if (!string.IsNullOrEmpty(newUrl))
            {
                nhtml = nhtml.Replace(url, newUrl);
            }
        }
        return nhtml;
    }

    private async Task<string?> getImageData(string url, string contenttype)
    {
        string? ret = null;
        try
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                var image = await response.Content.ReadAsByteArrayAsync();
                var base64 = Convert.ToBase64String(image);
                ret = $"data:{contenttype};base64,{base64}";
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            if (ex.StackTrace != null) logger.LogError(ex.StackTrace);
            if (ex.InnerException != null) logger.LogError(ex.InnerException.Message);
            if (ex.InnerException != null && ex.InnerException.StackTrace != null) logger.LogError(ex.InnerException.StackTrace);
            ret = null;
        }
        return ret;
    }
}