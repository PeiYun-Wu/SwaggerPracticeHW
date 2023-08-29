using System;
using System.Net.Mail;

namespace CommonModule
{
    /// <summary>
    /// SMTP Mail共用
    /// </summary>
    public static class SmtpMailHelper
    {
        /// <summary>
        /// 以SMTP寄送信件
        /// </summary>
        /// <param name="mailFrom">寄件者</param>
        /// <param name="mailTo">收件者</param>
        /// <param name="mailSubject">信件主旨</param>
        /// <param name="mailBody">信件內容</param>
        /// <param name="attachFile">附加檔案</param>
        /// <param name="isBodyHtml">信件內容是否為Html</param>
        public static void SendSmtpMail(string mailFrom, string mailTo, string mailSubject, string mailBody, Attachment attachFile, bool isBodyHtml = false)
        {
            SendSmtpMail(mailFrom, new string[] { mailTo }, mailSubject, mailBody, attachFile, isBodyHtml);
        }

        /// <summary>
        /// 以SMTP寄送信件(多個收件者)
        /// </summary>
        /// <param name="mailFrom">寄件者</param>
        /// <param name="mailToList">收件者(多個收件者)</param>
        /// <param name="mailSubject">信件主旨</param>
        /// <param name="mailBody">信件內容</param>
        /// <param name="attachFile">附加檔案</param>
        /// <param name="isBodyHtml">信件內容是否為Html</param>
        public static void SendSmtpMail(string mailFrom, string[] mailToList, string mailSubject, string mailBody, Attachment attachFile, bool isBodyHtml = false)
        {
            try
            {
                string smtpHostIP = CommUtility.GetConfigWhenFailGetBaseConfig<string>("SmtpHostIP");
                int smtpPort = CommUtility.GetConfigWhenFailGetBaseConfig("SmtpPort", 25);
                SmtpClient client = new SmtpClient(smtpHostIP, smtpPort);

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(mailFrom);
                foreach (var item in mailToList)
                {
                    if (!string.IsNullOrWhiteSpace(item))
                        mail.To.Add(item.Trim());
                }

                if (attachFile != null)
                    mail.Attachments.Add(attachFile);

                mail.Subject = mailSubject;
                mail.IsBodyHtml = isBodyHtml;
                mail.Body = mailBody;
                client.Send(mail);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(LogLevel.Error, "SmtpMailError", ex);
            }
        }
    }
}