/****************************************************************
*   作者：Morain
*   创建时间：2018/3/18 17:15:03
*   描述说明：
*****************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
  public static class ClipboardHelper
  {
    public static string ClipBoard
    {
      get
      {
        return GUIUtility.systemCopyBuffer;
      }

      set
      {

        GUIUtility.systemCopyBuffer = value;
      }
    }
  }
}
