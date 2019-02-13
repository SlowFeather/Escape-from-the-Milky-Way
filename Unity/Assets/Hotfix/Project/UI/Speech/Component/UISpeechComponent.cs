using DG.Tweening;
using ETModel;

using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Wit.BaiduAip.Speech;
using System.Collections;
using System.Collections.Generic;
using System;

namespace ETHotfix
{
    [ObjectSystem]
    public class UiSpeechComponentAwakeSystem : AwakeSystem<UISpeechComponent>
    {
        public override void Awake(UISpeechComponent self)
        {
            self.uispeech = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            self.Awake();
        }
    }
    [ObjectSystem]
    public class UiSpeechComponentUpdateSystem : UpdateSystem<UISpeechComponent>
    {
        public override void Update(UISpeechComponent self)
        {
            self.Update();
        }
    }
    /// <summary>
    /// 那这里我就需要重写一下了，发送的协程我得放到非热更层，然后进行操作
    /// </summary>
    public class UISpeechComponent : Component
    {
        public ReferenceCollector uispeech;
        //这两个东西也应该从配置文件读取
        public string APIKey = "aaZ13FrI9SarWliIFC95ImbG";
        public string SecretKey = "BqqhnqL32C1ZgVfOALcwr66vn1zrLkrX";

        /// <summary>
        /// 语音控制按钮
        /// </summary>
        Button CtrlButton;

        /// <summary>
        /// 是否正在讲话
        /// 是的话为true
        /// </summary>
        public bool StartSpeech = false;
        //录音
        private AudioClip clipRecord;
        //合成后
        private AudioSource audioSource;
        //解析
        private Asr asr;
        //合成
        private Tts tts;

        EventTrigger trigger;
        EventTrigger.Entry entry1;
        EventTrigger.Entry entry2;
        EventTrigger.Entry entry3;
        public void Awake()
        {
            CtrlButton = uispeech.Get<GameObject>("CtrlBtn").GetComponent<Button>();
            //CtrlButton.onClick.Add(() => { Debug.Log("1111"); });
            //初始化事件
            trigger = CtrlButton.gameObject.GetComponent<EventTrigger>();
            entry1 = new EventTrigger.Entry();
            entry2 = new EventTrigger.Entry();
            entry3 = new EventTrigger.Entry();

            entry1.eventID = EventTriggerType.PointerEnter;
            entry2.eventID = EventTriggerType.PointerDown;
            entry3.eventID = EventTriggerType.PointerExit;
            //这里是分别绑定
            //绑定鼠标进入事件
            entry1.callback = new EventTrigger.TriggerEvent();
            entry1.callback.AddListener(M);
            trigger.triggers.Add(entry1);
            //绑定鼠标点击事件
            entry2.callback = new EventTrigger.TriggerEvent();
            entry2.callback.AddListener(N);
            trigger.triggers.Add(entry2);
            //绑定鼠标离开事件
            entry3.callback = new EventTrigger.TriggerEvent();
            entry3.callback.AddListener(F);
            trigger.triggers.Add(entry3);

            //初始化百度Api工具
            asr = new Asr(APIKey, SecretKey);
            tts = new Tts(APIKey, SecretKey);
        }

        private void M(BaseEventData pointData)
        {
            Debug.Log("鼠标进入了!");
        }
        private void N(BaseEventData pointData)
        {
            Debug.Log("鼠标点击了!");
            //这里可以开始录音
            //开启麦克风录音
            clipRecord = Microphone.Start(null, false, 30, 16000);
            //15s后强制停止
            entry2.callback.RemoveListener(N);
            StartSpeech = true;

        }
        private void F(BaseEventData pointData)
        {
            Debug.Log("鼠标滑出了!");

            StopRecording();
        }
        private void StopRecording()
        {
            //此时判断是否处于说话状态，如果是说话状态则将按钮复位，并上传说话内容
            if (StartSpeech)
            {
                //上传内容
                Log.Debug("麦克风停止录音");
                Microphone.End(null);
                //转换格式
                var data = Asr.ConvertAudioClipToPCM16(clipRecord);
                asr.Recognize(data, s =>
                {
                    Log.Debug("进来了");
                    if (s.result == null && s.result.Length < 0)
                    {
                        Log.Debug("结果为空，表示麦克风未识别到声音");
                        //提示有问题，复位
                        entry2.callback.AddListener(N);
                        StartSpeech = false;
                    }
                    else
                    {
                        //有结果，发送给机器人进行语音回复
                        tts.Synthesis(s.result[0], r =>
                        {
                            if (r.Success)
                            {
                                //正常播放
                                Log.Debug("合成成功，正在播放,声音有几秒：" + audioSource.clip.length);
                                audioSource.clip = r.clip;
                                audioSource.Play();
                                //复位
                                entry2.callback.AddListener(N);
                                StartSpeech = false;
                            }
                            else
                            {
                                //这是出问题了
                                Debug.Log(s.err_msg);
                                //提示有问题，复位
                                entry2.callback.AddListener(N);
                                StartSpeech = false;
                            }
                        });
                    }

                });
            }
        }


        public void Update()
        {

        }
    }
}
