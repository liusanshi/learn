using System;
using System.Collections.Generic;
using System.Web;

namespace XDHelper.Common
{
    /// <summary>
    /// token管理单元的 管理者
    /// </summary>
    public class TokenManager
    {
        Dictionary<string, Token> _tokens = new Dictionary<string, Token>(32);

        /// <summary>
        /// code的生命周期 (天)
        /// </summary>
        public double CodeLifeTime { get; private set; }

        /// <summary>
        /// 获取token
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string GetToken(string code)
        {
            Token token;
            if (_tokens.TryGetValue(code, out token))
            {
                return token.token;
            }
            else
            {
                token = new Token(code, DateTime.Now);
                token.LifeTime = AppConfig.Token_LiftTime;
                if (!_tokens.ContainsKey(code))
                    _tokens.Add(code, token);
                return token.token;
            }
        }

        /// <summary>
        /// 回收Token
        /// </summary>
        public void Recycle()
        {
            List<string> codes = new List<string>(_tokens.Count / 3);
            foreach (var item in _tokens)
            {
                if (!hasExpiry(item.Value))
                {
                    codes.Add(item.Key);
                }
            }
            foreach (var item in codes)
            {
                _tokens.Remove(item);
            }
        }

        /// <summary>
        /// 判断Code是否有效期
        /// </summary>
        /// <param name="hour"></param>
        /// <returns></returns>
        private bool hasExpiry(Token token)
        {
            return token.StartTime.AddHours(CodeLifeTime) > DateTime.Now;
        }
    }
}