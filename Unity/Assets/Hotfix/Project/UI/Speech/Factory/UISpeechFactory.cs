using ETModel;
using UnityEngine;

namespace ETHotfix
{
    [UIFactory(UIType.UISpeech)]
    public class UISpeechFactory : AUIFactoty
    {
        public override void AddComponet(UI ui)
        {
            ui.AddComponent<UISpeechComponent>();
            //ETModel.Game.Scene.GetComponent<FontComponent>().ChangeAllFont(ui.GameObject);
        }
        public override void Remove(string type)
        {
            base.Remove(type);
        }
    }
}
