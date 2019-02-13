using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;
using I2.Loc;
using UnityEngine;

namespace ETHotfix
{
	[ObjectSystem]
	public class LanguageComponentAwakeSystem : AwakeSystem<LanguageComponent>
	{
		public override void Awake(LanguageComponent self)
		{
			self.Awake();
		}
	}

	/// <summary>
	/// 管理所有语言
	/// </summary>
	public class LanguageComponent: Component
	{
		public void Awake()
		{
            //从包里读取出组件，保证每次都是最新的文字
            //ETModel.Game.Scene.GetComponent<ResourcesComponent>().LoadBundle("languagesource.unity3d");
            ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
            resourcesComponent.LoadBundle("languagesource.unity3d");
            GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset("languagesource.unity3d", "LanguageSource");
            GameObject.Instantiate(bundleGameObject);
        }
        /// <summary>
        /// 设置当前语言
        /// </summary>
        /// <param name="type">所选的语言</param>
        public void SetLanguage(LanguageType type)
        {
            //Log.Debug(type.ToString());
            LocalizationManager.CurrentLanguage = type.ToString();
        }
	}
}