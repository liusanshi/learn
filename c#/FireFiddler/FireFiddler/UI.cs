using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Fiddler;

namespace FireFiddler
{
    public partial class UI : UserControl
    {
        public UI()
        {
            InitializeComponent();            
        }

        public void Notify(Session session)
        {
            if (session != null)
            {
                //Logger log = new Logger(false);
                //log.LogString(session.RequestMethod);

                //MessageBox.Show(string.Join("\r\n", session.ResponseHeaders.Select(p => p.Name + ":" + p.Value).ToArray()));

                Render(session);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        private void cb_disabled_CheckedChanged(object sender, EventArgs e)
        {
            PacketManger.PManger.Disabled = cb_disabled.Checked;
        }

        private void Render(Session session)
        {
            new Packet(session.url, session.ResponseHeaders).Render(tv_view.Nodes);
        }
    }
}
