using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MetroFramework.Controls;
using SubPointSolutions.Shelly.Desktop.Controls;
using SubPointSolutions.Shelly.Desktop.Definitions.UI;
using SubPointSolutions.Shelly.Desktop.Events.StartPage;
using SubPointSolutions.Shelly.Desktop.Plugins;

namespace MetaPack.Client.Desktop.Impl.Views
{
    public partial class StartPageViewControl : ShUserControlBase
    {
        public StartPageViewControl()
        {
            InitializeComponent();

            InitControls();
            InitEvents();
            InitLinks();
        }

        private void InitControls()
        {
            pStart.FlowDirection = FlowDirection.TopDown;
            pHelp.FlowDirection = FlowDirection.TopDown;

            pStart.BackColor = Color.White;
            pHelp.BackColor = Color.White;
        }

        private void InitLinks()
        {
            InitStartLinks();
            InitHelpLinks();
        }

        private void InitHelpLinks()
        {
            foreach (var link in GetActionLinks("help"))
                AddLink(pHelp, link);
        }

        private void InitStartLinks()
        {
            foreach (var link in GetActionLinks("start"))
                AddLink(pStart, link);
        }

        private void InitEvents()
        {
            ReceiveEvent<ShOnAppStartPageLoadCompletedEvent>(e =>
            {
                if (e.EventType == StartPageEventType.AddLink)
                {
                    if (e.Item.Location == "help")
                        AddLink(pHelp, e.Item);

                    if (e.Item.Location == "start")
                        AddLink(pStart, e.Item);
                }
            });
        }



        private void AddStartLink(ShAppStartPageItemDefinition def)
        {

        }

        private void AddHelpLink(ShAppStartPageItemDefinition def)
        {
            AddLink(pHelp, def);
        }

        private void AddLink(Panel panel, ShAppStartPageItemDefinition def)
        {
            var link = new MetroLink();

            link.Text = def.Title;
            link.TextAlign = ContentAlignment.MiddleLeft;
            link.Width = pStart.Width;

            if (def.Click != null)
                link.Click += def.Click;

            panel.Controls.Add(link);
        }
    }
}
