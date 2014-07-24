using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LL.Framework.Web.MVC
{
    public static class ValidationExtensions
    {
        /// <summary>
        /// 是否含有指定key的验证信息
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Boolean HasValidationMessage(this HTMLHelper helper, string key)
        {
            return helper.ViewData.ModelState.ContainsKey(key);
        }
        /// <summary>
        /// 获取验证信息
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string ValidationMessage(this HTMLHelper helper, string key)
        {
            return ValidationAllMessage(helper, key).FirstOrDefault() ?? string.Empty;
        }
        /// <summary>
        /// 获取所有的验证信息
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static IEnumerable<string> ValidationAllMessage(this HTMLHelper helper, string key)
        {
            ModelState state;
            if (helper.ViewData.ModelState.TryGetValue(key, out state) && state != null)
            {
                string msg;
                foreach (var item in state.Errors)
                {
                    msg = item.AllErrorMsg;
                    if (string.IsNullOrEmpty(msg))
                        yield return msg;
                }
            }
            yield break;
        }
    }
}
