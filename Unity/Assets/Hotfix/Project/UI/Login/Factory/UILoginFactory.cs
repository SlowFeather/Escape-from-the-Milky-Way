using ETModel;
using UnityEngine;

namespace ETHotfix
{
    [UIFactory(UIType.UILogin)]
    public class UILoginFactory : AUIFactoty
    {
        public override void AddComponet(UI ui)
        {
            ui.AddComponent<UILoginComponent>();
            
            ETModel.Game.Scene.GetComponent<FontComponent>().ChangeAllFont(ui.GameObject);
        }
        public override void Remove(string type)
        {
            base.Remove(type);
        }
    }
}
