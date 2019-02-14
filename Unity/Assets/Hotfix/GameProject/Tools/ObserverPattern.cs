using System.Collections.Generic;

namespace ETHotfix
{
    public class ObserverPattern<K, V, E> : Singleton<ObserverPattern<K, V, E>>
    {
        public override void Init()
        {
            base.Init();
            dic = new Dictionary<K, List<OnActinHandlerOneParm>>();
        }
        public override void Dispose()
        {
            base.Dispose();
            dic.Clear();
        }

        /// <summary>
        /// 方便给观察者传递参数
        /// </summary>
        /// <param name="value">这个可传递的</param>
        public delegate void OnActinHandlerOneParm(V value, E response2);
        //委托字典
        private Dictionary<K, List<OnActinHandlerOneParm>> dic;

        /// <summary>
        /// 添加监听 [给监听者使用，想监听了就添加，不需要监听的就不用管]
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="handler"></param>
        public void AddEventListener(K key, OnActinHandlerOneParm handler)
        {
            if (dic.ContainsKey(key))
            {
                dic[key].Add(handler);
            }
            else
            {
                List<OnActinHandlerOneParm> lsHandler = new List<OnActinHandlerOneParm>();
                lsHandler.Add(handler);
                dic[key] = lsHandler;
            }
        }
        /// <summary>
        /// 移除监听  [给监听者使用，如果有监听某条消息必须有移除，否则死人还能收到消息]
        /// </summary>
        /// <param name="protoID"></param>
        /// <param name="handler"></param>
        public void RemoveMsgEventListener(K key, OnActinHandlerOneParm handler)
        {
            if (dic.ContainsKey(key))
            {
                List<OnActinHandlerOneParm> lsHandler = dic[key];
                if (lsHandler.Count > 0)
                {
                    for (int i = 0; i < lsHandler.Count; i++)
                    {
                        if (lsHandler[i] == handler)
                        {
                            lsHandler.RemoveAt(i);
                            if (lsHandler.Count == 0)
                            {
                                dic.Remove(key);
                                return;
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 派发消息 【给发布者使用,不监听的人我就不发给他，谁监听谁能收到】
        /// </summary>
        /// <param name="opcdoe"></param>
        /// <param name="arg"></param>
        public void Dispatch(K key, V response, E response2)
        {

            if (dic.ContainsKey(key))
            {//先根据id将list拿到
                List<OnActinHandlerOneParm> lsHandler = dic[key];
                if (lsHandler != null && lsHandler.Count > 0)
                {
                    ////this.LogInfo(dic.Count + "派发消息当前编号池子的大小" + lsHandler.Count);
                    for (int i = 0; i < lsHandler.Count; i++)
                    {
                        //判断空执行
                        if (lsHandler[i] != null)
                        {
                            lsHandler[i](response, response2);
                        }
                    }
                }
            }
        }

        ///  零GC 遍历字典集合
        /// for (int count = 0; count < myDictionary.Count; count++) 
        /// { var element = myDictionary.ElementAt(count);
        /// var Key = element.Key; 
        /// var Value = element.Value;
        /// Debug.Log(Key + "  " + Value);
        /// }
        /// 
        /// 或者这样  来代替foreach 实现零GC的遍历字典
        /// var enumerator = dic.GetEnumerator();
        ///  while (enumerator.MoveNext())
        ///{
        /// Debug.Log("while " + enumerator.Current.Key + " " + enumerator.Current.Value);
        ///}
    }


    public class ObserverPattern<K, V> : Singleton<ObserverPattern<K, V>>
    {
        public override void Init()
        {
            base.Init();
            dic = new Dictionary<K, List<OnActinHandlerOneParm>>();
        }
        public override void Dispose()
        {
            base.Dispose();
            dic.Clear();
        }

        /// <summary>
        /// 方便给观察者传递参数
        /// </summary>
        /// <param name="value">这个可传递的</param>
        public delegate void OnActinHandlerOneParm(V value);
        //委托字典
        private Dictionary<K, List<OnActinHandlerOneParm>> dic;

        /// <summary>
        /// 添加监听 [给监听者使用，想监听了就添加，不需要监听的就不用管]
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="handler"></param>
        public void AddEventListener(K key, OnActinHandlerOneParm handler)
        {
            if (dic.ContainsKey(key))
            {
                dic[key].Add(handler);
            }
            else
            {
                List<OnActinHandlerOneParm> lsHandler = new List<OnActinHandlerOneParm>();
                lsHandler.Add(handler);
                dic[key] = lsHandler;
            }
        }
        /// <summary>
        /// 移除监听  [给监听者使用，如果有监听某条消息必须有移除，否则死人还能收到消息]
        /// </summary>
        /// <param name="protoID"></param>
        /// <param name="handler"></param>
        public void RemoveMsgEventListener(K key, OnActinHandlerOneParm handler)
        {
            if (dic.ContainsKey(key))
            {
                List<OnActinHandlerOneParm> lsHandler = dic[key];
                if (lsHandler.Count > 0)
                {
                    for (int i = 0; i < lsHandler.Count; i++)
                    {
                        if (lsHandler[i] == handler)
                        {
                            lsHandler.RemoveAt(i);
                            if (lsHandler.Count == 0)
                            {
                                dic.Remove(key);
                                return;
                            }
                        }
                    }
                }

            }
        }
        /// <summary>
        /// 派发消息 【给发布者使用,不监听的人我就不发给他，谁监听谁能收到】
        /// </summary>
        /// <param name="opcdoe"></param>
        /// <param name="arg"></param>
        public void Dispatch(K key, V response)
        {

            if (dic.ContainsKey(key))
            {//先根据id将list拿到
                List<OnActinHandlerOneParm> lsHandler = dic[key];
                if (lsHandler != null && lsHandler.Count > 0)
                {
                    ////this.LogInfo(dic.Count + "派发消息当前编号池子的大小" + lsHandler.Count);
                    for (int i = 0; i < lsHandler.Count; i++)
                    {
                        //判断空执行
                        if (lsHandler[i] != null)
                        {
                            lsHandler[i](response);
                        }
                    }
                }
            }
        }

        ///  零GC 遍历字典集合
        /// for (int count = 0; count < myDictionary.Count; count++) 
        /// { var element = myDictionary.ElementAt(count);
        /// var Key = element.Key; 
        /// var Value = element.Value;
        /// Debug.Log(Key + "  " + Value);
        /// }
        /// 
        /// 或者这样  来代替foreach 实现零GC的遍历字典
        /// var enumerator = dic.GetEnumerator();
        ///  while (enumerator.MoveNext())
        ///{
        /// Debug.Log("while " + enumerator.Current.Key + " " + enumerator.Current.Value);
        ///}
    }


    public class ObserverPattern<K> : Singleton<ObserverPattern<K>>
    {
        public override void Init()
        {
            base.Init();
            dic = new Dictionary<K, List<OnActinHandlerVoid>>();
        }
        public override void Dispose()
        {
            base.Dispose();
            dic.Clear();
        }

        /// <summary>
        /// 方便给观察者传递参数
        /// </summary>
        /// <param name="value">这个可传递的</param>
        public delegate void OnActinHandlerVoid();
        //委托字典
        private Dictionary<K, List<OnActinHandlerVoid>> dic;

        /// <summary>
        /// 添加监听 [给监听者使用，想监听了就添加，不需要监听的就不用管]
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="handler"></param>
        public void AddEventListener(K key, OnActinHandlerVoid handler)
        {
            if (dic.ContainsKey(key))
            {
                dic[key].Add(handler);
            }
            else
            {
                List<OnActinHandlerVoid> lsHandler = new List<OnActinHandlerVoid>();
                lsHandler.Add(handler);
                dic[key] = lsHandler;
            }
        }
        /// <summary>
        /// 移除监听  [给监听者使用，如果有监听某条消息必须有移除，否则死人还能收到消息]
        /// </summary>
        /// <param name="protoID"></param>
        /// <param name="handler"></param>
        public void RemoveMsgEventListener(K key, OnActinHandlerVoid handler)
        {
            if (dic.ContainsKey(key))
            {
                List<OnActinHandlerVoid> lsHandler = dic[key];
                if (lsHandler.Count > 0)
                {
                    for (int i = 0; i < lsHandler.Count; i++)
                    {
                        if (lsHandler[i] == handler)
                        {
                            lsHandler.RemoveAt(i);
                            if (lsHandler.Count == 0)
                            {
                                dic.Remove(key);
                                return;
                            }
                        }
                    }
                }

            }
        }
        /// <summary>
        /// 派发消息 【给发布者使用,不监听的人我就不发给他，谁监听谁能收到】
        /// </summary>
        /// <param name="opcdoe"></param>
        /// <param name="arg"></param>
        public void Dispatch(K key)
        {

            if (dic.ContainsKey(key))
            {//先根据id将list拿到
                List<OnActinHandlerVoid> lsHandler = dic[key];
                if (lsHandler != null && lsHandler.Count > 0)
                {
                    ////this.LogInfo(dic.Count + "派发消息当前编号池子的大小" + lsHandler.Count);
                    for (int i = 0; i < lsHandler.Count; i++)
                    {
                        //判断空执行
                        if (lsHandler[i] != null)
                        {
                            lsHandler[i]();
                        }
                    }
                }
            }
        }

        ///  零GC 遍历字典集合
        /// for (int count = 0; count < myDictionary.Count; count++) 
        /// { var element = myDictionary.ElementAt(count);
        /// var Key = element.Key; 
        /// var Value = element.Value;
        /// Debug.Log(Key + "  " + Value);
        /// }
        /// 
        /// 或者这样  来代替foreach 实现零GC的遍历字典
        /// var enumerator = dic.GetEnumerator();
        ///  while (enumerator.MoveNext())
        ///{
        /// Debug.Log("while " + enumerator.Current.Key + " " + enumerator.Current.Value);
        ///}
    }
}
