using System.Collections.Generic;
using UnityEngine;

namespace Streameme.UI
{
    /// <summary>
    /// OnSelect/Deselectの管理
    /// </summary>
    public class SelectedManager : MonoBehaviour
    {
        /// <summary>
        /// 選択中のオブジェクト一覧
        /// </summary>
        public static List<ButtonColorTransmitter> Selected = new();

        /// <summary>
        /// 新たに選択したオブジェクトのセット・既存のオブジェクトのチェック
        /// </summary>
        /// <param name="next"></param>
        public static void SetNewSelected(ButtonColorTransmitter next)
        {
            // Listの後ろの方がposZの値が大きいことが保証されているため、後ろから処理
            for (var i = Selected.Count - 1; i >= 0; i--)
            {
                if (next.posZ <= Selected[i].posZ)
                {
                    Selected[i].OnCustomDeselect();
                    Selected.RemoveAt(i);
                }
                else
                {
                    break;
                }
            }

            Selected.Add(next);
        }

        /// <summary>
        /// 指定したオブジェクトが選択リスト内に存在しているか
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsSelected(ButtonColorTransmitter obj)
        {
            foreach (var selected in Selected)
            {
                if (ReferenceEquals(obj, selected)) return true;
            }

            return false;
        }

        /// <summary>
        /// 全て選択解除
        /// </summary>
        public static void AllDeselect()
        {
            for (var i = Selected.Count - 1; i >= 0; i--)
            {
                Selected[i].OnCustomDeselect();
                Selected.RemoveAt(i);
            }
        }
    }
}