using DG.Tweening;
using ETModel;
using I2.Loc;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UiLoginComponentAwakeSystem : AwakeSystem<UILoginComponent>
    {
        public override void Awake(UILoginComponent self)
        {
            self.uilogin = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            self.Awake();
        }
    }
    [ObjectSystem]
    public class UiLoginComponentUpdateSystem : UpdateSystem<UILoginComponent>
    {
        public override void Update(UILoginComponent self)
        {
            self.Update();
        }
    }
    public class UILoginComponent : Component
    {
        public ReferenceCollector uilogin;

        Button LoginButton;
        //在加三个测试按钮

        InputField AccountInput;
        InputField PasswordInput;

        GameObject cam;

        public void Awake()
        {

            AccountInput = uilogin.Get<GameObject>("Account").GetComponent<InputField>();
            PasswordInput = uilogin.Get<GameObject>("Password").GetComponent<InputField>();

            LoginButton = uilogin.Get<GameObject>("LoginBtn").GetComponent<Button>();
            LoginButton.onClick.Add(LoginFunction);

            cam=Camera.main.gameObject;
            Game.Scene.GetComponent<AudioComponent>().Play("BGM");
            //AudioController.Play("BGM");

            Unit unit = UnitFactory.Create(1);
            
            unit.Position = new Vector3(0, 0, 0);


            //发个邮件测试下
            this.GetParent<UI>().AddComponent<MailComponent>();
            //this.GetParent<UI>().GetComponent<MailComponent>().SendMail("SlowFeather", "测试邮件标题", "测试邮件内容", "961020217@qq.com", () => { Debug.Log("成功"); }, () => { Debug.Log("失败"); });
            //改变语言
            Game.Scene.GetComponent<LanguageComponent>().SetLanguage(LanguageType.China); 
        }

        public void Update()
        {
    //        cam.transform.localRotation =
    //Quaternion.AngleAxis(Time.time * 30.0f, Vector3.up) *
    //Quaternion.AngleAxis(Mathf.Sin(Time.time * 0.37f) * 80.0f, Vector3.right);
        }
        /// <summary>
        /// 登陆方法
        /// </summary>
        public async void LoginFunction()
        {
            string account = AccountInput.text;
            string password = PasswordInput.text;
            Log.Debug($"点击了登陆，账号 {account}密码 {password}");

            //把这个界面慢慢缩小
            uilogin.gameObject.transform.DOScale(0, 0.35f).SetEase(Ease.InBack).OnComplete(() => {
                //条游戏
                //Game.Scene.GetComponent<UIComponent>().CreateUI(UIType.UIStripMain);
                //游戏大厅
                Game.Scene.GetComponent<UIComponent>().CreateUI(UIType.UILobby); 
                //移除UI
                Game.Scene.GetComponent<UIComponent>().Remove(UIType.UILogin);
            });
        }

        public override void Dispose()
        {
            this.GetParent<UI>().RemoveComponent<MailComponent>();
            base.Dispose();
        }
    }
}
