using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
	[ObjectSystem]
	public class UiComponentAwakeSystem : AwakeSystem<UIComponent>
	{
		public override void Awake(UIComponent self)
		{
			self.Awake();
		}
	}

	[ObjectSystem]
	public class UiComponentLoadSystem : LoadSystem<UIComponent>
	{
		public override void Load(UIComponent self)
		{
			self.Load();
		}
	}

	/// <summary>
	/// 管理所有UI
	/// </summary>
	public class UIComponent: Component
	{
		private GameObject Root;
		private readonly Dictionary<string, IUIFactory> UiTypes = new Dictionary<string, IUIFactory>();
		private readonly Dictionary<string, UI> uis = new Dictionary<string, UI>();

		public override void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}

			base.Dispose();

			foreach (string type in uis.Keys.ToArray())
			{
				UI ui;
				if (!uis.TryGetValue(type, out ui))
				{
					continue;
				}
				uis.Remove(type);
				ui.Dispose();
			}

			this.UiTypes.Clear();
			this.uis.Clear();
		}

		public void Awake()
		{
			this.Root = GameObject.Find("Global/UI/");
			this.Load();
		}

		public void Load()
		{
			UiTypes.Clear();
            
			List<Type> types = Game.EventSystem.GetTypes();

			foreach (Type type in types)
			{
				object[] attrs = type.GetCustomAttributes(typeof (UIFactoryAttribute), false);
				if (attrs.Length == 0)
				{
					continue;
				}

				UIFactoryAttribute attribute = attrs[0] as UIFactoryAttribute;
				if (UiTypes.ContainsKey(attribute.Type))
				{
                    Log.Debug($"已经存在同类UI Factory: {attribute.Type}");
					throw new Exception($"已经存在同类UI Factory: {attribute.Type}");
				}
				object o = Activator.CreateInstance(type);
				IUIFactory factory = o as IUIFactory;
				if (factory == null)
				{
					Log.Error($"{o.GetType().FullName} 没有继承 IUIFactory");
					continue;
				}
				this.UiTypes.Add(attribute.Type, factory);
			}
		}

		//public UI Create(string type)
		//{
		//	try
		//	{
		//		UI ui = UiTypes[type].Create(this.GetParent<Scene>(), type, Root);
  //              uis.Add(type, ui);

		//		// 设置canvas
		//		string cavasName = ui.GameObject.GetComponent<CanvasConfig>().CanvasName;
		//		ui.GameObject.transform.SetParent(this.Root.Get<GameObject>(cavasName).transform, false);
		//		return ui;
		//	}
		//	catch (Exception e)
		//	{
		//		throw new Exception($"{type} UI 错误: {e}");
		//	}
		//}
        public UI CreateUI(string type)
        {
            try
            {
                //等待从子类 应用层 根据AB包创建出来的预设体绑定的UI
                UI ui = UiTypes[type].Create(this.GetParent<Scene>(), type, Root);
                //缓存到UIComponent组件里，给外界频繁调用
                uis.Add(type, ui);
                // 设置canvas
                string cavasName = ui.GameObject.GetComponent<CanvasConfig>().CanvasName;
                ui.GameObject.transform.SetParent(this.Root.Get<GameObject>(cavasName).transform, false);


                //更换到这里去添加组件
                UiTypes[type].AddComponet(ui);
                //Log.Debug($"[异步创建UI完成]：ui组件：{ui.Name} 绑定对象：{ui.GameObject.name}");
                return ui;
            }
            catch (Exception e)
            {
                throw new Exception($"[异步创建UI错误]{type} UI 错误: {e}");
            }
        }
        public void Add(string type, UI ui)
		{
			this.uis.Add(type, ui);
		}

		public void Remove(string type)
		{
			UI ui;
			if (!uis.TryGetValue(type, out ui))
			{
				return;
			}
            UiTypes[type].Remove(type);
            uis.Remove(type);
			ui.Dispose();
		}

		public void RemoveAll()
		{
			foreach (string type in this.uis.Keys.ToArray())
			{
				UI ui;
				if (!this.uis.TryGetValue(type, out ui))
				{
					continue;
                }
                this.uis.Remove(type);
				ui.Dispose();
			}
		}

		public UI Get(string type)
		{
			UI ui;
			this.uis.TryGetValue(type, out ui);
			return ui;
		}

		public List<string> GetUITypeList()
		{
			return new List<string>(this.uis.Keys);
		}

        /// <summary>
        /// 统一显示UI有各种弹出动画，当然也可以不使用这些，自己另行修改
        /// 不写事件可直接出现
        /// </summary>
        /// <param name="type"></param>
        /// <param name="action"></param>
        /// <param name="enterTime"></param>
        /// <param name="enterEase"></param>
        /// <returns></returns>
        public UI ShowUI(string type, Action action = null, float enterTime = 0.32f, Ease enterEase = Ease.OutBack)
        {
            UI ui = Get(type);
            //这里判断是否为空，为空的话创建出一个，创建出的就是显示的
            if (ui == null)
            {
                ui = CreateUI(type);
            }
            else
            {
                if (ui.GameObject.activeInHierarchy)
                {
                    ui.GameObject.transform.localScale = Vector3.one;
                }
                else
                {
                    ui.GameObject.SetActive(true); //这样导致Deactivate/Activate的性能消耗GC很大
                    ui.GameObject.transform.localScale = Vector3.one;
                }
            }
            if (action != null)
            {
                ShowAnimation(ui, () => { action.Invoke(); }, enterTime, enterEase);
            }
            return ui;
        }
        /// <summary>
        /// 播放UI进入动画
        /// </summary>
        /// <param name="ui"></param>
        /// <param name="callback"></param>
        /// <param name="time"></param>
        /// <param name="ease"></param>
        void ShowAnimation(UI ui, TweenCallback callback, float time, Ease ease)
        {
            ui.GameObject.transform.localScale = Vector3.zero;
            ui.GameObject.transform.DOScale(Vector3.one, time).SetEase(ease).OnComplete(callback);
        }
        /// <summary>
        /// 统一隐藏UI，个人认为直接隐藏最好，当然也可以自行调整或者手动键入动画
        /// 不写事件即可直接隐藏
        /// </summary>
        /// <param name="type"></param>
        /// <param name="action"></param>
        /// <param name="backTime"></param>
        /// <param name="backEase"></param>
        public void HideUI(string type, Action action = null, float backTime = 0.25f, Ease backEase = Ease.InBack)
        {
            UI ui = Get(type);
            if (ui != null)
            {
                if (action != null)
                {
                    BackAnimation(ui, () => {
                        action.Invoke();
                    }, backTime, backEase);
                }
                else
                {
                    ui.GameObject.transform.localScale = Vector3.zero;
                }
            }
        }
        /// <summary>
        /// UI退出动画
        /// </summary>
        /// <param name="ui"></param>
        /// <param name="callback"></param>
        /// <param name="time"></param>
        /// <param name="ease"></param>
        void BackAnimation(UI ui, TweenCallback callback, float time, Ease ease)
        {
            //Test
            ui.GameObject.transform.localScale = Vector3.one;
            ui.GameObject.transform.DOScale(Vector3.zero, time).SetEase(ease).OnComplete(callback);
        }
    }
}