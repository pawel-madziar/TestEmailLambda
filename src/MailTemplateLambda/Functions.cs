using Amazon.Lambda.Core;
using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.APIGateway;
using MailTemplateLambda.LambdaMailService;
using System.Net.Http.Headers;
using System.Text.Json;


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
    [HttpApi(LambdaHttpMethod.Put, "{infox}")]
    public async Task<bool> SendMail(string infox, [FromBody] string mailSettings, ILambdaContext context)
    {
        bool ret = true;
        // implement
        try
        {
            context.Logger.LogWarning($"mailSettings: {mailSettings}");
            context.Logger.LogWarning($"infox: {infox}");

            MailSettings settings = JsonSerializer.Deserialize<MailSettings>(mailSettings);

            ret = await _mailService.SendMail(settings, context.Logger);
            context.Logger.LogInformation($"Mail sent to {settings.to}");
        }
        catch (Exception ex)
        {
            context.Logger.LogError($"Mail sending failed: {ex.Message}");
            ret = false;
        }

        return ret;
    }
}