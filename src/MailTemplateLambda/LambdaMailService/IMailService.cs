using Amazon.Lambda.Core;

namespace MailTemplateLambda.LambdaMailService;
public interface IMailService
{
    Task<bool> SendMail(MailSettings mailSettings, ILambdaLogger logger);
}
