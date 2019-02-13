using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    [ObjectSystem]
    public class FontComponentAwakeSystem : AwakeSystem<FontComponent>
    {
        public override void Awake(FontComponent self)
        {
            self.Awake();
        }
    }


    /// <summary>
    /// 管理所有字体
    /// </summary>
    public class FontComponent : Component
    {
        private GameObject Root;

        private string NormalFontName = "Ex_GameFontYaHei";
        private string BoldFontName = "Ex_GameFont";
        private string ItalicFontName = "Ex_CustomFont_1";

        private Font NormalFont;
        private Font BoldFont;
        private Font ItalicFont;
        public void Awake()
        {
            //从Res里加载默认的三种字体，这三种字体不参与打包，默认，加粗，倾斜
            NormalFont = Resources.Load<Font>($"Basic/Font/{NormalFontName}");
            BoldFont = Resources.Load<Font>($"Basic/Font/{BoldFontName}");
            ItalicFont = Resources.Load<Font>($"Basic/Font/{ItalicFontName}");
            Log.Debug("字体组件加载完毕");
        }
        /// <summary>
        /// 改变一个物体下所有的字体
        /// 若selectNormalFont为null或者不写，则使用默认字体（非系统默认字体）
        /// selectBoldFont和selectItalicFont若为空则规则同上
        /// </summary>
        /// <param name="root"></param>
        /// <param name="selectNormalFont"></param>
        /// <param name="selectBoldFont"></param>
        /// <param name="selectItalicFont"></param>
        public void ChangeAllFont(GameObject root, Font selectNormalFont = null,Font selectBoldFont=null,Font selectItalicFont=null)
        {
            Font font_Normal;
            Font font_Bold;
            Font font_Italic;
            if (selectNormalFont == null)
            {
                font_Normal = NormalFont;
            }
            else
            {
                font_Normal = selectNormalFont;
            }
            if (selectBoldFont == null)
            {
                font_Bold = BoldFont;
            }
            else
            {
                font_Bold = selectBoldFont;
            }
            if (selectItalicFont == null)
            {
                font_Italic = ItalicFont;
            }
            else
            {
                font_Italic = selectItalicFont;
            }
            //遍历所有根物体下的物体
            Text[] allBtns = root.gameObject.GetComponentsInChildren<Text>(true);
            for (int i = 0; i < allBtns.Length; i++)
            {
                if (allBtns[i].fontStyle == FontStyle.Normal)
                {
                    allBtns[i].font = font_Normal;
                    goto END;
                }
                if (allBtns[i].fontStyle == FontStyle.Bold)
                {
                    allBtns[i].font = font_Bold;
                    goto END;
                }
                if (allBtns[i].fontStyle == FontStyle.Italic)
                {
                    allBtns[i].font = font_Italic;
                    goto END;
                }
            END:;
            }
        }
        /// <summary>
        /// 改变一个物体的字体
        /// </summary>
        /// <param name="root"></param>
        /// <param name="selectFont"></param>
        public void ChangeOneFont(Text root,Font selectFont)
        {
            if (selectFont==null)
            {
                selectFont = NormalFont;
            }
            root.font = selectFont;
        }
    }
}



