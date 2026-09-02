using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Versioning;

namespace PNM_Revision_Tool
{
    [SupportedOSPlatform("windows")]
    public partial class frmMain : Form
    {

        public void UpdateProgress(
            int current,
            int total,
            string sheetName)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(
                    () => UpdateProgress(
                        current,
                        total,
                        sheetName)));

                return;
            }

            prgStatus.Maximum = total;
            prgStatus.Value =
                Math.Min(current, total);

            int percent =
                (int)((double)current / total * 100.0);

            lblStatus.Text =
                $"{percent}%  ({current}/{total})  " +
                $"{sheetName}";

            System.Windows.Forms.Application.DoEvents();
        }

        public void LogMessage(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(
                    () => LogMessage(message)));
                return;
            }

            txtLog.AppendText(
                $"[{DateTime.Now:HH:mm:ss}] {message}" +
                Environment.NewLine);

            txtLog.SelectionStart =
                txtLog.TextLength;

            txtLog.ScrollToCaret();

            System.Windows.Forms.Application.DoEvents();
        }

        private static readonly string[] StampOptions =
        {
            "",
            "PRELIMINARY",
            "PRELIMINARY-NOT FOR CONSTRUCTION",
            "CONCEPTUAL-NOT FOR CONSTRUCTION",
            "ISSUED FOR STANDARDS USAGE",
            "ISSUED FOR REVIEW",
            "ISSUED FOR 30% REVIEW",
            "ISSUED FOR 60% REVIEW",
            "ISSUED FOR 90% REVIEW",
            "ISSUED FOR FABRICATION",
            "ISSUED FOR CONSTRUCTION",
            "ISSUED FOR MATERIAL PROCUREMENT",
            "ISSUED FOR PERMITTING",
            "FOR REFERENCE ONLY",
            "FOR REFERENCE ONLY-NOT FOR CONSTRUCTION",
            "FOR BIDDING PURPOSES ONLY",
            "FOR PLATE CUTTING ONLY",
            "NOT FOR CONSTRUCTION",
            "HOLD FOR VENDOR DRAWINGS",
            "HOLD",
            "REMOVAL",
            "AS BUILT"
         };

        public frmMain()
        {
            InitializeComponent();
            cbxStamp.Items.Clear();
            cbxStamp.Items.AddRange(StampOptions);
            cbxStamp.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void cmbCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void cmbApplyShtSet_Click(object sender, EventArgs e)
        {
            using OpenFileDialog dialog =
                new OpenFileDialog
                {
                    Title =
                        "Select AutoCAD Sheet Set",

                    Filter =
                        "AutoCAD Sheet Set Files (*.dst)|*.dst",

                    DefaultExt = "dst",
                    AddExtension = true,
                    CheckFileExists = true,
                    CheckPathExists = true,
                    Multiselect = false
                };

            if (dialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            RevisionFormValues values = new RevisionFormValues
            {
                RevisionNumber =
                    txbRevNumber.Text,

                Date =
                    txbDate.Text,

                DrafterInitials =
                    txbDrafterInit.Text,

                Description1 =
                    txbDesc1.Text,

                Description2 =
                    txbDesc2.Text,

                Description3 =
                    txbDesc3.Text,

                CheckedInitials =
                    txbCHKinit.Text,

                OkayedInitials =
                    txbOKDinit.Text,

                ApprovedInitials =
                    txbAPPinit.Text,

                StatusStamp =
                    cbxStamp.Text
            };

            cmbApplyShtSet.Enabled = false;
            UseWaitCursor = true;

            try
            {
                txtLog.Clear();

                ProcessingSummary summary =
                    SheetSetProcessor.Process(
                        dialog.FileName,
                        values,
                        UpdateProgress,
                        LogMessage);

                MessageBox.Show(
                    this,
                    BuildSummaryMessage(summary),
                    "PNM Revision Tool",
                    MessageBoxButtons.OK,
                    summary.FailedSheets == 0
                        ? MessageBoxIcon.Information
                        : MessageBoxIcon.Warning);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(
                    this,
                    ex.Message,
                    "PNM Revision Tool",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                UseWaitCursor = false;
                cmbApplyShtSet.Enabled = true;
            }
        }

        private static string BuildSummaryMessage(ProcessingSummary summary)
        {
            StringBuilder message =
                new StringBuilder();

            message.AppendLine(
                "Sheet set processing is complete.");
            message.AppendLine();

            message.AppendLine(
                $"Sheets processed: " +
                $"{summary.ProcessedSheets}");

            message.AppendLine(
                $"Sheets failed: " +
                $"{summary.FailedSheets}");

            message.AppendLine(
                $"Sheets skipped: " +
                $"{summary.SkippedSheets}");

            if (summary.RevisionBlocksNotFound > 0)
            {
                message.AppendLine(
                    $"Sheets without REV BLOCK: " +
                    $"{summary.RevisionBlocksNotFound}");
            }

            // Detailed list of sheets missing REV BLOCK removed to reduce
            // verbosity in the summary message. The count is still shown
            // above if any were not found.

            if (summary.SkippedDrawings.Count > 0)
            {
                message.AppendLine();
                message.AppendLine(
                    "Documents skipped because " +
                    "they are open in AutoCAD:");

                foreach (string drawingFile
                         in summary.SkippedDrawings
                             .Distinct(
                                 StringComparer
                                     .OrdinalIgnoreCase)
                             .OrderBy(
                                 fileName => fileName,
                                 StringComparer
                                     .OrdinalIgnoreCase))
                {
                    message.AppendLine(
                        drawingFile);
                }
            }

            return message.ToString();
        }

    }
}
