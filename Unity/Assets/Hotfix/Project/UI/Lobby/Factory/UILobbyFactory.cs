using ETModel;
using UnityEngine;

namespace ETHotfix
{
    [UIFactory(UIType.UILobby)]
    public class UILobbyFactory : AUIFactoty
    {
        public override void AddComponet(UI ui)
        {
            ui.AddComponent<UILobbyComponent>();
            ETModel.Game.Scene.GetComponent<FontComponent>().ChangeAllFont(ui.GameObject);
        }
        public override void Remove(string type)
        {
            base.Remove(type);
        }
    }
}
