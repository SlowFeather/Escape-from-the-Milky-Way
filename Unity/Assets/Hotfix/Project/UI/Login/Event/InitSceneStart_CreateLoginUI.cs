using ETModel;

namespace ETHotfix
{
	[Event(EventIdType.InitSceneStart)]
	public class InitSceneStart_CreateLoginUI: AEvent
	{
		public override void Run()
		{
			UI login = Game.Scene.GetComponent<UIComponent>().CreateUI(UIType.UILogin);
            UI speech = Game.Scene.GetComponent<UIComponent>().CreateUI(UIType.UISpeech);

        }
    }
}
