using Autodesk.AutoCAD.Runtime;
using AcAp = Autodesk.AutoCAD.ApplicationServices.Application;

[assembly: CommandClass(typeof(PNM_Revision_Tool.Commands))]

namespace PNM_Revision_Tool
{
    public sealed class Commands
    {
        [CommandMethod(
            "PNMREVISIONTOOL",
            CommandFlags.Session)]
        public void ShowRevisionTool()
        {
            using frmMain form =
                new frmMain();

            AcAp.ShowModalDialog(form);
        }
    }
}
