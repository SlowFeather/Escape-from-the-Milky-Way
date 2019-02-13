using ETModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETHotfix
{
    [ObjectSystem]
    public class AudioComponentAwakeSystem : AwakeSystem<AudioComponent>
    {
        public override void Awake(AudioComponent self)
        {
            self.Awake();
        }
    }
    public class AudioComponent: Component
    {
        public void Awake()
        {
        }
        /// <summary>
        /// 播放一个音效
        /// </summary>
        /// <param name="audioName"></param>
        public void Play(string audioName)
        {
            AudioController.Play(audioName);
        }
        /// <summary>
        /// 停止一个音效的播放
        /// </summary>
        /// <param name="audioName"></param>
        public void Stop(string audioName)
        {
            AudioController.Stop(audioName);
        }
        /// <summary>
        /// 停止所有音效
        /// </summary>
        public void StopAll()
        {
            AudioController.StopAll();
        }
        /// <summary>
        /// 设置全局音量
        /// </summary>
        /// <param name="volume"></param>
        public void SetGlobalVolume(float volume)
        {
            AudioController.SetGlobalVolume(volume);
        }
        /// <summary>
        /// 设置音乐的音量
        /// </summary>
        /// <param name="musicVolume"></param>
        public void SetMusicVolume(float musicVolume)
        {
            AudioController.SetCategoryVolume("Music", musicVolume);
        }
        /// <summary>
        /// 设置音效的音量
        /// </summary>
        /// <param name="soundVolume"></param>
        public void SetSoundVolume(float soundVolume)
        {
            AudioController.SetCategoryVolume("Audio", soundVolume);
        }
    }
}
