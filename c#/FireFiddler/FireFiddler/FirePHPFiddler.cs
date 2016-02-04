using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fiddler;
using System.Windows.Forms;


namespace FireFiddler
{
    public class FirePHPFiddler : IAutoTamper /*, Fiddler.IResponseInspector2*/
    {
        /// <summary>
        /// 选项卡
        /// </summary>
        TabPage tabPage;
        /// <summary>
        /// 界面
        /// </summary>
        UI ui;

        /// <summary>
        /// 
        /// </summary>
        public FirePHPFiddler()
        {
            tabPage = new TabPage("FirePHP");
            ui = new UI();
        }

        public void OnBeforeUnload()
        {

        }

        public void OnLoad()
        {
            //将用户控件添加到选项卡中
            this.tabPage.Controls.Add(this.ui);
            //为选项卡添加icon图标，这里使用Fiddler 自带的
            this.tabPage.ImageIndex = (int)Fiddler.SessionIcons.JSON;
            //将tabTage选项卡添加到Fidder UI的Tab 页集合中
            FiddlerApplication.UI.tabsViews.TabPages.Add(this.tabPage);

            FiddlerApplication.UI.lvSessions.ItemSelectionChanged += lvSessions_ItemSelectionChanged;
        }

        void lvSessions_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                ui.Notify(FiddlerApplication.UI.GetFirstSelectedSession());
            }
        }

        public void AutoTamperRequestAfter(Session oSession)
        {
            //throw new NotImplementedException();
        }

        public void AutoTamperRequestBefore(Session oSession)
        {
            //throw new NotImplementedException();
        }

        public void AutoTamperResponseAfter(Session oSession)
        {
            //throw new NotImplementedException();
        }

        public void AutoTamperResponseBefore(Session oSession)
        {
            PacketManger.PManger.FilterSession(oSession);
        }

        public void OnBeforeReturningError(Session oSession)
        {
            //throw new NotImplementedException();
        }
    }
}
