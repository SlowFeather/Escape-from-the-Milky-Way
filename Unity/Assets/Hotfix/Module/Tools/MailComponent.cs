using ETModel;
using System;
using System.Net;
using System.Net.Mail;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class MailComponentAwakeSystem : AwakeSystem<MailComponent>
    {
        public override void Awake(MailComponent self)
        {
            self.Awake();
        }
    }


    /// <summary>
    /// 管理所有邮件 目前仅限163邮箱，并且需开通服务
    /// 通过配置MailConfig来设置自己的邮件密钥
    /// </summary>
    public class MailComponent : Component
    {
        /// <summary>
        /// 我的邮件账号
        /// </summary>
        string MyMailAddress = "";
        /// <summary>
        /// 我的邮件授权码
        /// </summary>
        string MyMailAuthorizationCode = "";
        /// <summary>
        /// 醒目标题，或者叫大写发件人
        /// </summary>
        string StrongHeadlines = "";
        /// <summary>
        /// 邮件标题
        /// </summary>
        string MailTitle = "";
        /// <summary>
        /// 邮件内容
        /// </summary>
        string MailContent = "";

        /// <summary>
        /// 发送成功事件
        /// </summary>
        public Action SendSuccessAction;
        /// <summary>
        /// 发送失败事件
        /// </summary>
        public Action SendFieldAction;
        public void Awake()
        {
            //下面两个从配置中读取，于是就实现了热更
            MailConfig MailConfig = (MailConfig)Game.Scene.GetComponent<ConfigComponent>().Get(typeof(MailConfig), 1);
            MyMailAddress = MailConfig.MailAddress;
            MyMailAuthorizationCode = MailConfig.MailAuthorizationCode;
            //SendMail("SlowFeather", "测试邮件标题", "测试邮件内容", "961020217@qq.com", () => { Debug.Log("1"); }, () => { Debug.Log("2"); });
        }
        /// <summary>
        /// 外界调用发送邮件函数,暂不支持携带附件
        /// </summary>
        /// <param name="strongheadlines">醒目标题，大写发件人</param>
        /// <param name="mailtitle">邮件标题</param>
        /// <param name="mailcontent">邮件内容</param>
        /// <param name="mailAddress">对方邮件地址</param>
        /// <param name="successaction">发送成功触发</param>
        /// <param name="fieldaction">发送失败触发</param>
        public void SendMail(string strongheadlines, string mailtitle, string mailcontent, string mailAddress, Action successaction=null, Action fieldaction=null)
        {
            StrongHeadlines = strongheadlines;
            MailTitle = mailtitle;
            MailContent = mailcontent;
            SendSuccessAction = successaction;
            SendFieldAction = fieldaction;
            SendMailTo(mailAddress);
        }
        #region Inner
        void SendMailTo(string mailAddresss, string picUrl = "")
        {
            //var sucRes = new SuccessRes(mailAddresss, picUrl);
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(MyMailAddress, StrongHeadlines); mail.To.Add(mailAddresss);
                    //给自己抄送一份，否则会报错
                    mail.From = new MailAddress(MyMailAddress, StrongHeadlines); mail.To.Add(MyMailAddress);
                    //邮件的标题
                    mail.Subject = MailTitle;
                    //邮件的内容
                    mail.Body = MailContent;
                    //邮件的附件
                    if (picUrl != "")
                    {
                        mail.Attachments.Add(new Attachment(picUrl));
                    }
                    //发送邮件的服务器               
                    SmtpClient mailServer = new SmtpClient("smtp.163.com")
                    {
                        //端口
                        Port = 25,
                        Credentials = new NetworkCredential(MyMailAddress, MyMailAuthorizationCode) as ICredentialsByHost,
                        EnableSsl = true
                    };
                    ServicePointManager.ServerCertificateValidationCallback += (e, t, f, g) =>
                    {
                        return true;
                    };
                    mailServer.Send(mail);
                    SendSuccessAction?.Invoke();
                    Log.Debug("发送邮件成功");
                }
            }
            catch (Exception e)
            {
                SendFieldAction?.Invoke();
                Log.Error("发送邮件失败:" + e.Message);
            }
        }
        #endregion
        public override void Dispose()
        {
            MyMailAddress = null;
            MyMailAuthorizationCode = null;
            StrongHeadlines = null;
            MailTitle = null;
            MailContent = null;
            base.Dispose();
        }
    }
}



