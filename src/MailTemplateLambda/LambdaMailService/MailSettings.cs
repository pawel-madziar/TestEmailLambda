using Amazon.Lambda.Core;

namespace MailTemplateLambda.LambdaMailService;
public class MailSettings {
    public string from { get; set; }
    public string to { get; set; }
    public string subject { get; set; }
    public string body { get; set; }
}
