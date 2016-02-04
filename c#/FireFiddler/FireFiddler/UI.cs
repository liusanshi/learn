using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Fiddler;
using FireFiddler.Component;

namespace FireFiddler
{
    public partial class UI : UserControl
    {
        PacketManger PManger = PacketManger.PManger;

        public UI()
        {
            InitializeComponent();            
        }

        public void Notify(Session session)
        {
            if (session != null && cb_disabled.Checked)
            {
                //Logger log = new Logger(false);
                //log.LogString(session.RequestMethod);
                Render(session);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            PManger.PHPersistence = new HttpHeaderToMemory(); //保存在内存里面
            if (!PManger.GetRules().Any()) //没有规则的时候
            {
                PManger.AddRule(new HostRule { Disabled = true, Host = "iyouxi.vip.qq.com" });
            }
            PManger.HttpHeaderProcess = new HttpHeaderRemove(); //添加移除http头的代码

            cb_disabled.Checked = PManger.Disabled;

            base.OnLoad(e);
        }

        private void cb_disabled_CheckedChanged(object sender, EventArgs e)
        {
            PManger.Disabled = cb_disabled.Checked;
            PManger.SaveRule();
        }

        private void Render(Session session)
        {
            tv_view.Nodes.Clear(); //先清除
            PManger.GetPacket(session).Render(tv_view.Nodes);
        }
    }
}
