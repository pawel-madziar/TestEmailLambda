namespace MailTemplateLambda.LambdaMailService;
public interface IMailService {
    void SendMail(MailSettings mailSettings);
}
