using System;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.AllServer)]
    public class C2G_LoginHandler : AMRpcHandler<C2G_ClientLoginRequest, G2C_ClientLoginResponse>
    {
        protected override async void Run(Session session, C2G_ClientLoginRequest message, Action<G2C_ClientLoginResponse> reply)
        {
            //new 一个回复
            G2C_ClientLoginResponse response = new G2C_ClientLoginResponse();
            try
            {
                //查询
                Log.Debug($"收到 Username:{message.Account} Password:{message.Password}");
                response.Error = 0;
                
                DBProxyComponent DBPComponent= Game.Scene.GetComponent<DBProxyComponent>();
                Account account=ComponentFactory.Create<Account>();

                account.UserAccount = message.Account;
                account.UserPassword = message.Password;
                await DBPComponent.Save(account, false);
                //DBPComponent.Query<Account>("")

                response.Message = "注册成功";
                reply(response);
            }
            catch (Exception e)
            {
                response.Error = -1;
                ReplyError(response, e, reply);
            }
            


        }
    }
}
