﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using ETModel;
using NLog;

namespace App
{
	internal static class Program
	{
		private static void Main(string[] args)
		{
			// 异步方法全部会回掉到主线程
			SynchronizationContext.SetSynchronizationContext(OneThreadSynchronizationContext.Instance);
			
			try
			{			
				Game.EventSystem.Add(DLLType.Model, typeof(Game).Assembly);
				Game.EventSystem.Add(DLLType.Hotfix, DllHelper.GetHotfixAssembly());

				Options options = Game.Scene.AddComponent<OptionComponent, string[]>(args).Options;
				StartConfig startConfig = Game.Scene.AddComponent<StartConfigComponent, string, int>(options.Config, options.AppId).StartConfig;

				if (!options.AppType.Is(startConfig.AppType))
				{
					Log.Error("命令行参数apptype与配置不一致");
					return;
				}

				IdGenerater.AppId = options.AppId;

				LogManager.Configuration.Variables["appType"] = startConfig.AppType.ToString();
				LogManager.Configuration.Variables["appId"] = startConfig.AppId.ToString();
				LogManager.Configuration.Variables["appTypeFormat"] = $"{startConfig.AppType,-8}";
				LogManager.Configuration.Variables["appIdFormat"] = $"{startConfig.AppId:D3}";

				Log.Info($"server start........................ {startConfig.AppId} {startConfig.AppType}");

				Game.Scene.AddComponent<OpcodeTypeComponent>();
				Game.Scene.AddComponent<MessageDispatherComponent>();

				// 根据不同的AppType添加不同的组件
				OuterConfig outerConfig = startConfig.GetComponent<OuterConfig>();
				InnerConfig innerConfig = startConfig.GetComponent<InnerConfig>();
				ClientConfig clientConfig = startConfig.GetComponent<ClientConfig>();
				
				switch (startConfig.AppType)
				{
					case AppType.Manager:
						Game.Scene.AddComponent<AppManagerComponent>();
						Game.Scene.AddComponent<NetInnerComponent, string>(innerConfig.Address);
						Game.Scene.AddComponent<NetOuterComponent, string>(outerConfig.Address);
						break;
					case AppType.Realm:
						Game.Scene.AddComponent<ActorMessageDispatherComponent>();
						Game.Scene.AddComponent<NetInnerComponent, string>(innerConfig.Address);
						Game.Scene.AddComponent<NetOuterComponent, string>(outerConfig.Address);
						Game.Scene.AddComponent<LocationProxyComponent>();
						Game.Scene.AddComponent<RealmGateAddressComponent>();
						break;
					case AppType.Gate:
						Game.Scene.AddComponent<PlayerComponent>();
						Game.Scene.AddComponent<ActorMessageDispatherComponent>();
						Game.Scene.AddComponent<NetInnerComponent, string>(innerConfig.Address);
						Game.Scene.AddComponent<NetOuterComponent, string>(outerConfig.Address);
						Game.Scene.AddComponent<LocationProxyComponent>();
						Game.Scene.AddComponent<ActorMessageSenderComponent>();
						Game.Scene.AddComponent<ActorLocationSenderComponent>();
						Game.Scene.AddComponent<GateSessionKeyComponent>();
						break;
					case AppType.Location:
						Game.Scene.AddComponent<NetInnerComponent, string>(innerConfig.Address);
						Game.Scene.AddComponent<LocationComponent>();
						break;
					case AppType.Map:
						Game.Scene.AddComponent<NetInnerComponent, string>(innerConfig.Address);
						Game.Scene.AddComponent<UnitComponent>();
						Game.Scene.AddComponent<LocationProxyComponent>();
						Game.Scene.AddComponent<ActorMessageSenderComponent>();
						Game.Scene.AddComponent<ActorLocationSenderComponent>();
						Game.Scene.AddComponent<ActorMessageDispatherComponent>();
						Game.Scene.AddComponent<ServerFrameComponent>();
						break;
					case AppType.AllServer:
						Game.Scene.AddComponent<ActorMessageSenderComponent>();
						Game.Scene.AddComponent<ActorLocationSenderComponent>();
						Game.Scene.AddComponent<PlayerComponent>();
						Game.Scene.AddComponent<UnitComponent>();
                        //数据库管理组件 管理数据库链接地址，数据库名称等
						Game.Scene.AddComponent<DBComponent>();
                        //数据库调用组件 调用DB数据库组件 增删改查
						Game.Scene.AddComponent<DBProxyComponent>();
                        //数据库缓存 当数据反复使用中，把这个数据放在内存中，不再向数据库读取
						Game.Scene.AddComponent<DBCacheComponent>();
						Game.Scene.AddComponent<LocationComponent>();
						Game.Scene.AddComponent<ActorMessageDispatherComponent>();
						Game.Scene.AddComponent<NetInnerComponent, string>(innerConfig.Address);
						Game.Scene.AddComponent<NetOuterComponent, string>(outerConfig.Address);
						Game.Scene.AddComponent<LocationProxyComponent>();
						Game.Scene.AddComponent<AppManagerComponent>();
						Game.Scene.AddComponent<RealmGateAddressComponent>();
						Game.Scene.AddComponent<GateSessionKeyComponent>();
						Game.Scene.AddComponent<ConfigComponent>();
						Game.Scene.AddComponent<ServerFrameComponent>();
						// Game.Scene.AddComponent<HttpComponent>();
						break;
					case AppType.Benchmark:
						Game.Scene.AddComponent<NetOuterComponent>();
						Game.Scene.AddComponent<BenchmarkComponent, string>(clientConfig.Address);
						break;
					case AppType.BenchmarkWebsocketServer:
						Game.Scene.AddComponent<NetOuterComponent, string>(outerConfig.Address);
						break;
					case AppType.BenchmarkWebsocketClient:
						Game.Scene.AddComponent<NetOuterComponent>();
						Game.Scene.AddComponent<WebSocketBenchmarkComponent, string>(clientConfig.Address);
						break;
					default:
						throw new Exception($"命令行参数没有设置正确的AppType: {startConfig.AppType}");
				}

				while (true)
				{
					try
					{
						Thread.Sleep(1);
						OneThreadSynchronizationContext.Instance.Update();
						Game.EventSystem.Update();
					}
					catch (Exception e)
					{
						Log.Error(e);
					}
				}
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
		}
	}
}