namespace MailTemplateLambda.LambdaMailService;

using System.Runtime.CompilerServices;
using Amazon;
using Amazon.Lambda.Core;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;

public class MailService : IMailService
{
    public async Task<bool> SendMail(MailSettings mailSettings, ILambdaLogger logger)
    {
        bool ret = true;
        // ImageReplacer imageReplacer = new ImageReplacer(mailSettings.body, logger);
        // string body = await imageReplacer.replaceImages();
        string body = mailSettings.body;
        using (var client = new AmazonSimpleEmailServiceClient(RegionEndpoint.EUWest2))
        {
            var sendRequest = new SendEmailRequest
            {
                Source = mailSettings.from,
                Destination = new Destination { ToAddresses = new List<string> { mailSettings.to } },
                Message = new Message
                {
                    Subject = new Content(mailSettings.subject),
                    Body = new Body
                    {
                        Html = new Content { Charset = "UTF-8", Data = body }
                    }
                }
            };
            try
            {
                await client.SendEmailAsync(sendRequest);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                if (ex.InnerException != null)
                    logger.LogError(ex.InnerException.Message);
                if (ex.StackTrace != null)
                    logger.LogError(ex.StackTrace);
                ret = false;
            }
        }

        return ret;

    }
}