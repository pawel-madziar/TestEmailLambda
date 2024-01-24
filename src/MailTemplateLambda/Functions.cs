using Amazon.Lambda.Core;
using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.APIGateway;
using MailTemplateLambda.LambdaMailService;
using System.Net.Http.Headers;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace MailTemplateLambda;

/// <summary>
/// A collection of sample Lambda functions that provide a REST api for doing simple math calculations. 
/// </summary>
public class Functions
{
    private readonly IMailService _mailService;

    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <remarks>
    /// The <see cref="ICalculatorService"/> implementation that we
    /// instantiated in <see cref="Startup"/> will be injected here.
    /// 
    /// As an alternative, a dependency could be injected into each 
    /// Lambda function handler via the [FromServices] attribute.
    /// </remarks>
    public Functions(IMailService mailService)
    {
        _mailService = mailService;
    }

    [LambdaFunction()]
    [HttpApi(LambdaHttpMethod.Put,"{mailSettings}")]
    public bool SendMail(string mailSettings, ILambdaContext context)
    {
        bool ret = true;
        // implement
        try
        {
            context.Logger.LogWarning($"Input: {mailSettings}");
         //_mailService.SendMail(mailSettings);
        }
        catch(Exception ex)
        {
            context.Logger.LogError($"Mail sending failed: {ex.Message}");
            ret = false;
        }

        context.Logger.LogInformation($"Mail sent to {mailSettings}");
        return ret;
    }
}