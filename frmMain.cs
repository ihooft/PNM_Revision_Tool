using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Versioning;

namespace PNM_Revision_Tool
{
    [SupportedOSPlatform("windows")]
    public partial class frmMain : Form
    {
        private string _currentSheetSetFile = string.Empty;

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

        //private void cmbCancel_Click(object sender, EventArgs e)
        //{
        //    DialogResult = DialogResult.Cancel;
        //    Close();
        //}

        private void cmbApplyShtSet_Click(object sender, EventArgs e)
        {
            // Check if a sheet set file has been loaded
            if (string.IsNullOrWhiteSpace(_currentSheetSetFile))
            {
                MessageBox.Show(
                    this,
                    "Please open a sheet set file first using the 'Open Sheet Set' button.",
                    "PNM Revision Tool",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            // Get the checked sheets from the TreeView
            List<SheetEntry> selectedSheets = GetCheckedSheets();

            if (selectedSheets.Count == 0)
            {
                MessageBox.Show(
                    this,
                    "No sheets selected. Please select at least one sheet to process.",
                    "PNM Revision Tool",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

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

            // If the user did not enter any values, do not proceed with
            // opening or processing drawings to avoid unnecessary work.
            if (values.IsEmpty())
            {
                MessageBox.Show(
                    this,
                    "No revision values were entered. Nothing to do.",
                    "PNM Revision Tool",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            cmbApplyShtSet.Enabled = false;
            UseWaitCursor = true;

            try
            {
                txtLog.Clear();

                ProcessingSummary summary =
                    SheetSetProcessor.ProcessSelectedSheets(
                        _currentSheetSetFile,
                        selectedSheets,
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

            // The count of sheets missing REV BLOCK is intentionally omitted
            // from the summary message to reduce verbosity. Details (if any)
            // are available in the txt log.

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

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void cmbOpenSS_Click(object sender, EventArgs e)
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

            try
            {
                UseWaitCursor = true;
                cmbOpenSS.Enabled = false;

                _currentSheetSetFile = dialog.FileName;

                // Get the sheet hierarchy from the sheet set file
                List<SheetSetNode> sheetNodes = 
                    SheetSetProcessor.GetSheetSetHierarchy(
                        _currentSheetSetFile);

                // Populate the TreeView
                trvSheets.Nodes.Clear();
                PopulateTreeView(trvSheets.Nodes, sheetNodes);

                LogMessage(
                    $"Loaded sheet set: {Path.GetFileName(_currentSheetSetFile)} " +
                    $"({sheetNodes.Count} sheets)");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    this,
                    ex.Message,
                    "PNM Revision Tool",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                _currentSheetSetFile = string.Empty;
                trvSheets.Nodes.Clear();
            }
            finally
            {
                UseWaitCursor = false;
                cmbOpenSS.Enabled = true;
            }
        }

        private void PopulateTreeView(TreeNodeCollection nodeCollection, List<SheetSetNode> sheetNodes)
        {
            foreach (SheetSetNode node in sheetNodes)
            {
                TreeNode treeNode = new TreeNode
                {
                    Text = node.Name,
                    Tag = node.Sheet,
                    Checked = false
                };

                if (node.Children.Count > 0)
                {
                    PopulateTreeView(treeNode.Nodes, node.Children);
                }

                nodeCollection.Add(treeNode);
            }
        }

        private void cmbSelectAll_Click(object sender, EventArgs e)
        {
            SetAllNodesChecked(trvSheets.Nodes, true);
        }

        private void SetAllNodesChecked(TreeNodeCollection nodes, bool isChecked)
        {
            foreach (TreeNode node in nodes)
            {
                node.Checked = isChecked;

                if (node.Nodes.Count > 0)
                {
                    SetAllNodesChecked(node.Nodes, isChecked);
                }
            }
        }

        private void cmbSelectNone_Click(object sender, EventArgs e)
        {
            SetAllNodesChecked(trvSheets.Nodes, false);
        }

        private void trvSheets_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private List<SheetEntry> GetCheckedSheets()
        {
            List<SheetEntry> checkedSheets = new List<SheetEntry>();
            CollectCheckedSheets(trvSheets.Nodes, checkedSheets);
            return checkedSheets;
        }

        private void CollectCheckedSheets(TreeNodeCollection nodes, List<SheetEntry> checkedSheets)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Checked && node.Tag is SheetEntry sheet)
                {
                    checkedSheets.Add(sheet);
                }

                if (node.Nodes.Count > 0)
                {
                    CollectCheckedSheets(node.Nodes, checkedSheets);
                }
            }
        }
    }
}
