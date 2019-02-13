using DG.Tweening;
using ETModel;

using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UiLobbyComponentAwakeSystem : AwakeSystem<UILobbyComponent>
    {
        public override void Awake(UILobbyComponent self)
        {
            self.uilobby = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            self.Awake();
        }
    }
    [ObjectSystem]
    public class UiLobbyComponentUpdateSystem : UpdateSystem<UILobbyComponent>
    {
        public override void Update(UILobbyComponent self)
        {
            self.Update();
        }
    }
    public class UILobbyComponent : Component
    {
        public ReferenceCollector uilobby;

        Text TestText;
        Button TestButton;
        Text TestDescription;
        Text TestName;

        private int KunNum = 0;
        private int KunLevel = 1;

        LevelConfig LevelConfig;
        public void Awake()
        {
            //获取到1级的表
            LevelConfig = (LevelConfig)Game.Scene.GetComponent<ConfigComponent>().Get(typeof(LevelConfig), 1);

            TestText = uilobby.Get<GameObject>("Text").GetComponent<Text>();
            TestDescription = uilobby.Get<GameObject>("DescriptionText").GetComponent<Text>();
            TestName = uilobby.Get<GameObject>("NameText").GetComponent<Text>();
            TestButton = uilobby.Get<GameObject>("Button").GetComponent<Button>();
            TestButton.onClick.Add(GetExpFunction);
            TestText.text = $"当前鲲的经验：{KunNum} \n当前鲲的等级：{KunLevel}";
            TestDescription.text = LevelConfig.Description;
            TestName.text = LevelConfig.Name;
            //Log.Debug($"{LevelConfig.Name} : {LevelConfig.Description}");
        }

        public void Update()
        {

        }

        public void GetExpFunction()
        {
            KunNum++;
            if (KunNum>GetConfig(KunLevel).Num)
            {
                KunNum = 0;
                KunLevel ++;
                LevelUp();
            }
            TestText.text = $"当前鲲的经验：{KunNum} \n当前鲲的等级：{KunLevel}";

            Game.Scene.GetComponent<AudioComponent>().Play("Click");
        }
        /// <summary>
        /// 升级后触发的方法
        /// </summary>
        private void LevelUp()
        {
            TestDescription.text = GetConfig(KunLevel).Description;
            TestName.text = GetConfig(KunLevel).Name;
        }
        /// <summary>
        /// 通过ID来获取提升到该等级所需要的经验
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private LevelConfig GetConfig(int num)
        {
            return (LevelConfig)Game.Scene.GetComponent<ConfigComponent>().Get(typeof(LevelConfig), num);
        }


    }
}
