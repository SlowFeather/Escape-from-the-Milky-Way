using ETModel;
using System;
using UnityEngine;

namespace ETHotfix
{
	public interface IUIFactory
	{
		UI Create(Scene scene, string type, GameObject parent);
		void Remove(string type);
        void AddComponet(UI ui);
    }

    public abstract class AUIFactoty : IUIFactory
    {
        string LOG_TAG = "AUIFactoty";
        //public virtual SubGameType currentSubGameType { get; }
        ResourcesComponent resourcesComponent;

        public UI Create(Scene scene, string type, GameObject parent)
        {
            try
            {
                //this.LogInfo($"当前已经Load出来这个：{type}的Ab包，因为子类继承了AUIFactoty，而AUIFactoty实现了IUIFactory的Create方法，所以子类看不到Create就创建过了");
                //if (currentSubGameType != SubGameType.Hall)
                //{
                //    this.LogInfo($"当前开始从子游戏类目：{currentSubGameType}下面加载AB资源包");
                //}
                //获取资源组件
                resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
                //加载AB包
                resourcesComponent.LoadBundle($"{type}.unity3d");
                //this.LogInfo("[AB包异步加载完毕]====>"+ $"{type}.unity3d");
                //通过对应的AB包得到对应的预制体,
                GameObject bundlePrefab = (GameObject)resourcesComponent.GetAsset($"{type}.unity3d", $"{type}");


                //通过AB包加载预制体 
                GameObject login = UnityEngine.Object.Instantiate(bundlePrefab); //这个时候已经clone出来了 其实已经走了awake了
                //this.LogInfo("[预设体加载完毕]====>" + $"{login.name}");
                //第二次加载时直接从缓存里加载，由于加载速度比从磁盘快，实例化出来就awake开始执行了，执行完毕后，才王下面执行
                //设置层级
                login.layer = LayerMask.NameToLayer(LayerNames.UI);
                //创建ui对象  [使用工厂将UI 和游戏对象绑定]
                UI ui = ComponentFactory.Create<UI, GameObject>(login);


                //暂时调整一下执行顺序，等绑定上组件后，再执行实例化
                //给ui对象挂载组件
                //AddComponet(ui);
                //this.LogInfo($"[从{type}.unity3d -> {bundlePrefab.name}异步结束 并绑定UI组件{ui.Name}完毕]");
                return ui;
            }
            catch (Exception e)
            {
                Debug.Log("我的错" + e.ToStr());
                return null;
            }
        }

        public virtual void Remove(string type)
        {
            ETModel.Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle($"{type}.unity3d");
            //this.LogInfo($"[释放AB包]:->SubGameType:[{currentSubGameType}] -> UIType:[{type}]");
        }

        public abstract void AddComponet(UI ui);
    }
}