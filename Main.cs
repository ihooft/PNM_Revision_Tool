using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;

using AcAp =
    Autodesk.AutoCAD.ApplicationServices.Application;

using ACSMCOMPONENTS25Lib;
using System.Runtime.Versioning;

[assembly: CommandClass(typeof(PNM_Revision_Tool.Commands))]
   

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
                        this);

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

            if (summary.MissingRevisionBlocks.Count > 0)
            {
                message.AppendLine();
                message.AppendLine(
                    "Sheets without REV BLOCK:");

                foreach (string item in
                         summary.MissingRevisionBlocks
                             .OrderBy(x => x))
                {
                    message.AppendLine(item);
                }
            }

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

    internal sealed class RevisionFormValues
    {
        public string RevisionNumber
        {
            get;
            init;
        } = string.Empty;

        public string Date
        {
            get;
            init;
        } = string.Empty;

        public string DrafterInitials
        {
            get;
            init;
        } = string.Empty;

        public string Description1
        {
            get;
            init;
        } = string.Empty;

        public string Description2
        {
            get;
            init;
        } = string.Empty;

        public string Description3
        {
            get;
            init;
        } = string.Empty;

        public string CheckedInitials
        {
            get;
            init;
        } = string.Empty;

        public string OkayedInitials
        {
            get;
            init;
        } = string.Empty;

        public string ApprovedInitials
        {
            get;
            init;
        } = string.Empty;

        public string StatusStamp
        {
            get;
            init;
        } = string.Empty;
    }

    internal sealed class SheetEntry
    {
        public string SheetTitle
        {
            get;
            init;
        } = string.Empty;

        public string DrawingFile
        {
            get;
            init;
        } = string.Empty;

        public string LayoutName
        {
            get;
            init;
        } = string.Empty;
    }

    internal sealed class SheetProcessingResult
    {
        public bool WasProcessed
        {
            get;
            init;
        }

        public bool RevisionBlockFound
        {
            get;
            init;
        }
    }

    internal sealed class ProcessingSummary
    {
        public int ProcessedSheets
        {
            get;
            set;
        }

        public int FailedSheets
        {
            get;
            set;
        }

        public int SkippedSheets
        {
            get;
            set;
        }

        public int RevisionBlocksNotFound
        {
            get;
            set;
        }

        public List<string> SkippedDrawings
        {
            get;
        } = new List<string>();

        public List<string> MissingRevisionBlocks
        {
            get;
        } = new List<string>();
    }

    internal static class SheetSetProcessor
    {
        private static readonly string[] RevBlockNames =
        {
            "REV BLOCK"
        };

        private static readonly string[] StatusStampNames =
        {
            "STATUS STAMP",
            "Status Stamp-Dynamic"
        };

        private static readonly string[] PlotAttNames =
        {
            "PLOTATT",
            "TITLEBLOCK",
            "SECTIONATT",
            "FNDATT",
            "TBBLATTS",
            "OVGND_ATT",
            "GNDATT",
            "OVCND_ATT",
            "CNDATT"
        };

        private static bool IsDrawingOpen(string drawingFile)
        {
            if (string.IsNullOrWhiteSpace(
                    drawingFile))
            {
                return false;
            }

            string targetPath =
                Path.GetFullPath(
                    drawingFile);

            foreach (Document document
                     in AcAp.DocumentManager)
            {
                if (string.Equals(
                        Path.GetFullPath(
                            document.Name),
                        targetPath,
                        StringComparison
                            .OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        public static ProcessingSummary Process(
            string dstFileName,
            RevisionFormValues values,
            frmMain form)
        {
            if (string.IsNullOrWhiteSpace(dstFileName))
            {
                throw new ArgumentException(
                    "A sheet set file was not provided.",
                    nameof(dstFileName));
            }

            if (!File.Exists(dstFileName))
            {
                throw new FileNotFoundException(
                    "The selected sheet set file was not found.",
                    dstFileName);
            }

            List<SheetEntry> sheets =
                GetSheets(dstFileName);

            int totalSheets =
                sheets.Count;

            int currentSheet = 0;

            form.UpdateProgress(
                0,
                totalSheets,
                "Starting...");

            if (sheets.Count == 0)
            {
                throw new InvalidOperationException(
                    "No valid sheets were found in the selected " +
                    "sheet set.");
            }

            ProcessingSummary summary =
                new ProcessingSummary();

            /*
             * Do not deduplicate drawing filenames.
             *
             * Every sheet set entry is processed separately.
             * If multiple sheets reference layouts in the same
             * drawing, that drawing is read, modified, and saved
             * separately for each sheet.
             */
            HashSet<string> warnedOpenDrawings = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (SheetEntry sheet in sheets)
            {
                currentSheet++;

                form.UpdateProgress(
                    currentSheet,
                    totalSheets,
                    sheet.SheetTitle);

                if (IsDrawingOpen(
                        sheet.DrawingFile))
                {
                    summary.SkippedSheets++;

                    summary.SkippedDrawings.Add(
                        sheet.DrawingFile);

                    if (warnedOpenDrawings.Add(
                            sheet.DrawingFile))
                    {
                        MessageBox.Show(
                            "The following drawing is " +
                            "currently open in AutoCAD " +
                            "and will be skipped:" +
                            Environment.NewLine +
                            Environment.NewLine +
                            sheet.DrawingFile,
                            "PNM Revision Tool",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }

                    System.Windows.Forms.Application.DoEvents();
                    continue;
                }

                SheetProcessingResult result =
                    ProcessSheet(
                        sheet,
                        values,
                        form);

                if (result.WasProcessed)
                {
                    summary.ProcessedSheets++;

                    if (!result.RevisionBlockFound)
                    {
                        summary.RevisionBlocksNotFound++;

                        summary.MissingRevisionBlocks.Add(
                            $"{sheet.SheetTitle}" +
                            $"({Path.GetFileName(sheet.DrawingFile)})");
                    }
                }
                else
                {
                    summary.FailedSheets++;
                }

                System.Windows.Forms.Application.DoEvents();
            }

            form.UpdateProgress(
                totalSheets,
                totalSheets,
                "Complete");

            return summary;
        }

        private static List<SheetEntry> GetSheets(
            string dstFileName)
        {
            IAcSmSheetSetMgr sheetSetManager = null;
            AcSmDatabase sheetSetDatabase = null;

            List<SheetEntry> sheets =
                new List<SheetEntry>();

            try
            {
                sheetSetManager =
                    new AcSmSheetSetMgr();

                /*
                 * OpenDatabase may be generated as returning
                 * IAcSmDatabase while Close requires AcSmDatabase.
                 * The concrete cast matches the AutoCAD 2026 COM
                 * interop signature reported by Visual Studio.
                 */
                sheetSetDatabase =
                    sheetSetManager.OpenDatabase(
                        dstFileName,
                        false) as AcSmDatabase;

                if (sheetSetDatabase == null)
                {
                    throw new InvalidOperationException(
                        "AutoCAD could not open the selected " +
                        "sheet set.");
                }

                IAcSmSheetSet sheetSet =
                    sheetSetDatabase.GetSheetSet();

                if (sheetSet == null)
                {
                    throw new InvalidOperationException(
                        "The selected DST does not contain a " +
                        "valid sheet set.");
                }

                IAcSmEnumComponent sheetEnumerator =
                    sheetSet.GetSheetEnumerator();

                if (sheetEnumerator == null)
                {
                    throw new InvalidOperationException(
                        "AutoCAD could not enumerate the sheets " +
                        "in the selected sheet set.");
                }

                IAcSmComponent component;

                while ((component =
                        sheetEnumerator.Next()) != null)
                {
                    IAcSmSheet sheet =
                        component as IAcSmSheet;

                    if (sheet == null)
                    {
                        continue;
                    }

                    IAcSmAcDbLayoutReference
                        layoutReference =
                            sheet.GetLayout()
                            as IAcSmAcDbLayoutReference;

                    if (layoutReference == null)
                    {
                        continue;
                    }

                    string drawingFile =
                        layoutReference.GetFileName();

                    string layoutName =
                        layoutReference.GetName();

                    string sheetTitle =
                        sheet.GetTitle();

                    if (string.IsNullOrWhiteSpace(
                            drawingFile))
                    {
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(
                            layoutName))
                    {
                        continue;
                    }

                    drawingFile =
                        ResolveDrawingPath(
                            drawingFile,
                            dstFileName);

                    sheets.Add(
                        new SheetEntry
                        {
                            SheetTitle =
                                string.IsNullOrWhiteSpace(
                                    sheetTitle)
                                    ? layoutName
                                    : sheetTitle.Trim(),

                            DrawingFile =
                                drawingFile,

                            LayoutName =
                                layoutName.Trim()
                        });
                }

                return sheets;
            }
            finally
            {
                if (sheetSetManager != null &&
                    sheetSetDatabase != null)
                {
                    try
                    {
                        sheetSetManager.Close(
                            sheetSetDatabase);
                    }
                    catch
                    {
                        /*
                         * Do not replace an earlier exception with
                         * a COM cleanup exception.
                         */
                    }
                }

                ReleaseComObject(
                    sheetSetDatabase);

                ReleaseComObject(
                    sheetSetManager);
            }
        }

        private static string ResolveDrawingPath(
            string drawingFile,
            string dstFileName)
        {
            drawingFile =
                Environment.ExpandEnvironmentVariables(
                    drawingFile.Trim());

            if (Path.IsPathRooted(drawingFile))
            {
                return Path.GetFullPath(
                    drawingFile);
            }

            string sheetSetDirectory =
                Path.GetDirectoryName(
                    dstFileName) ??
                string.Empty;

            return Path.GetFullPath(
                Path.Combine(
                    sheetSetDirectory,
                    drawingFile));
        }

        private static SheetProcessingResult ProcessSheet(
            SheetEntry sheet,
            RevisionFormValues values,
            frmMain form)
        {
            if (!File.Exists(sheet.DrawingFile))
            {
                ShowSheetWarning(
                    "Drawing file not found.",
                    sheet);

                return new SheetProcessingResult
                {
                    WasProcessed = false,
                    RevisionBlockFound = false
                };
            }

            try
            {
                bool revisionBlockFound;

                /*
                 * This is an external or side database.
                 *
                 * The drawing is not opened in the AutoCAD document
                 * window, so DocumentManager.Open, MDI activation,
                 * DocumentLock, and CloseAndSave are not required.
                 */
                //using (Database database =
                //       new Database(false, true))
                //{
                //    database.ReadDwgFile(
                //        sheet.DrawingFile,
                //        FileOpenMode
                //            .OpenForReadAndWriteNoShare,
                //        true,
                //        string.Empty);

                //    /*
                //     * ReadDwgFile can use deferred loading.
                //     * CloseInput forces any remaining drawing data
                //     * to be loaded before overwriting the source.
                //     */
                //    database.CloseInput(true);

                //    revisionBlockFound =
                //        UpdateSheetLayout(
                //            database,
                //            sheet,
                //            values);

                //    /*
                //     * Preserve the drawing's original DWG version
                //     * instead of automatically upgrading it.
                //     */
                //    database.SaveAs(
                //        sheet.DrawingFile,
                //        database.OriginalFileVersion);
                //}
                using (Database database = new Database(false, true))
                {
                    database.ReadDwgFile(
                        sheet.DrawingFile,
                        FileOpenMode.OpenForReadAndWriteNoShare,
                        true,
                        string.Empty);

                    database.CloseInput(true);

                    Database previousDb =
                        HostApplicationServices.WorkingDatabase;

                    try
                    {
                        HostApplicationServices.WorkingDatabase =
                            database;

                        revisionBlockFound =
                            UpdateSheetLayout(
                                database,
                                sheet,
                                values,
                                form);
                    }
                    finally
                    {
                        HostApplicationServices.WorkingDatabase =
                            previousDb;
                    }

                    database.SaveAs(
                        sheet.DrawingFile,
                        database.OriginalFileVersion);
                }

                return new SheetProcessingResult
                {
                    WasProcessed = true,
                    RevisionBlockFound =
                        revisionBlockFound
                };
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(
                    BuildSheetMessage(
                        "Error processing sheet:" +
                        Environment.NewLine +
                        Environment.NewLine +
                        ex.Message,
                        sheet),
                    "PNM Revision Tool",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return new SheetProcessingResult
                {
                    WasProcessed = false,
                    RevisionBlockFound = false
                };
            }
        }

        private static bool UpdateSheetLayout(
            Database database,
            SheetEntry sheet,
            RevisionFormValues values,
            frmMain form)
        {
            Database previouseDb = HostApplicationServices.WorkingDatabase;

            try
            {
                HostApplicationServices.WorkingDatabase = database;

                bool revisionBlockFound;

                using (Transaction transaction =
                       database.TransactionManager
                           .StartTransaction())
                {
                    ObjectId layoutBlockTableRecordId =
                        GetLayoutBlockTableRecordId(
                            database,
                            transaction,
                            sheet.LayoutName);

                    List<ObjectId> revisionBlocks =
                        FindBlockReferencesInLayout(
                            layoutBlockTableRecordId,
                            transaction,
                            RevBlockNames);

                    revisionBlockFound =
                        revisionBlocks.Count > 0;

                    if (!revisionBlockFound)
                    {
                        form.LogMessage(
                            $"REV BLOCK not found: " +
                            $"{sheet.SheetTitle}");
                    }
                    else
                    {
                        /*
                         * Only one REV BLOCK is updated.
                         * It is the one with the greatest insertion
                         * point X value on this sheet's layout.
                         */
                        ObjectId rightmostRevisionBlock =
                            revisionBlocks
                                .OrderByDescending(
                                    blockId =>
                                        GetBlockXPosition(
                                            blockId,
                                            transaction))
                                .First();

                        Dictionary<string, string>
                            revisionAttributes =
                                new Dictionary<string, string>(
                                    StringComparer
                                        .OrdinalIgnoreCase)
                                {
                                    ["REV_#"] =
                                        values.RevisionNumber,

                                    ["DATE"] =
                                        values.Date,

                                    ["BY"] =
                                        values.DrafterInitials,

                                    ["LINE-1"] =
                                        values.Description1,

                                    ["LINE-2"] =
                                        values.Description2,

                                    ["LINE-3"] =
                                        values.Description3,

                                    ["CHK'D"] =
                                        values.CheckedInitials,

                                    ["OK'D"] =
                                        values.OkayedInitials,

                                    ["APP'D"] =
                                        values.ApprovedInitials
                                };

                        UpdateAttributes(
                            rightmostRevisionBlock,
                            revisionAttributes,
                            transaction);
                    }

                    if (!string.IsNullOrWhiteSpace(
                            values.StatusStamp))
                    {
                        List<ObjectId> statusStampBlocks =
                            FindBlockReferencesInLayout(
                                layoutBlockTableRecordId,
                                transaction,
                                StatusStampNames);

                        foreach (ObjectId statusStampId
                                 in statusStampBlocks)
                        {
                            SetDynamicProperty(
                                statusStampId,
                                "Visibility1",
                                values.StatusStamp,
                                transaction);
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(
                            values.RevisionNumber))
                    {
                        List<ObjectId> plotAttBlocks =
                            FindBlockReferencesInLayout(
                                layoutBlockTableRecordId,
                                transaction,
                                PlotAttNames);


                        Dictionary<string, string>
                            plotAttributes =
                                new Dictionary<string, string>(
                                    StringComparer
                                        .OrdinalIgnoreCase)
                                {
                                    ["PLOTREV#"] =
                                        values.RevisionNumber
                                };

                        foreach (ObjectId plotAttId
                                 in plotAttBlocks)
                        {
                            UpdateAttributes(
                                plotAttId,
                                plotAttributes,
                                transaction);
                        }
                    }

                    transaction.Commit();
                }
                
                return revisionBlockFound;
            }

            finally
            {
                HostApplicationServices.WorkingDatabase = previouseDb;
            }

            
        }

        private static ObjectId
            GetLayoutBlockTableRecordId(
                Database database,
                Transaction transaction,
                string layoutName)
        {
            DBDictionary layoutDictionary =
                (DBDictionary)transaction.GetObject(
                    database.LayoutDictionaryId,
                    OpenMode.ForRead);

            foreach (DBDictionaryEntry entry
                     in layoutDictionary)
            {
                if (!string.Equals(
                        entry.Key,
                        layoutName,
                        StringComparison
                            .OrdinalIgnoreCase))
                {
                    continue;
                }

                Layout layout =
                    (Layout)transaction.GetObject(
                        entry.Value,
                        OpenMode.ForRead);

                return layout.BlockTableRecordId;
            }

            throw new InvalidOperationException(
                $"Layout \"{layoutName}\" was not found " +
                "in the drawing.");
        }

        private static List<ObjectId>
    FindBlockReferencesInLayout(
        ObjectId layoutBlockTableRecordId,
        Transaction transaction,
        IEnumerable<string> blockNames)
        {
            HashSet<string> validNames =
                new HashSet<string>(
                    blockNames,
                    StringComparer.OrdinalIgnoreCase);

            List<ObjectId> matchingBlocks =
                new List<ObjectId>();

            BlockTableRecord layoutRecord =
                (BlockTableRecord)transaction.GetObject(
                    layoutBlockTableRecordId,
                    OpenMode.ForRead);

            foreach (ObjectId entityId in layoutRecord)
            {
                if (transaction.GetObject(
                        entityId,
                        OpenMode.ForRead)
                    is not BlockReference blockReference)
                {
                    continue;
                }

                string effectiveName =
                    GetEffectiveBlockName(
                        blockReference,
                        transaction);

                if (validNames.Contains(effectiveName))
                {
                    matchingBlocks.Add(entityId);
                }
            }

            return matchingBlocks;
        }

        private static string GetEffectiveBlockName(
            BlockReference blockReference,
            Transaction transaction)
        {
            ObjectId blockDefinitionId =
                blockReference.IsDynamicBlock
                    ? blockReference
                        .DynamicBlockTableRecord
                    : blockReference
                        .BlockTableRecord;

            BlockTableRecord blockDefinition =
                (BlockTableRecord)transaction.GetObject(
                    blockDefinitionId,
                    OpenMode.ForRead);

            return blockDefinition.Name;
        }

        private static double GetBlockXPosition(
            ObjectId blockId,
            Transaction transaction)
        {
            BlockReference blockReference =
                (BlockReference)transaction.GetObject(
                    blockId,
                    OpenMode.ForRead);

            return blockReference.Position.X;
        }

        private static void UpdateAttributes(
            ObjectId blockId,
            IReadOnlyDictionary<string, string>
                values,
            Transaction transaction)
        {
            BlockReference blockReference =
                (BlockReference)transaction.GetObject(
                    blockId,
                    OpenMode.ForRead);

            foreach (ObjectId attributeId
                     in blockReference
                         .AttributeCollection)
            {
                AttributeReference attribute =
                    transaction.GetObject(
                        attributeId,
                        OpenMode.ForRead)
                    as AttributeReference;

                if (attribute == null)
                {
                    continue;
                }

                string attributeTag =
                    attribute.Tag?.Trim() ??
                    string.Empty;

                if (!values.TryGetValue(
                        attributeTag,
                        out string newValue))
                {
                    continue;
                }

                /*
                 * An empty form field leaves the existing drawing
                 * attribute unchanged.
                 */
                if (string.IsNullOrWhiteSpace(
                        newValue))
                {
                    continue;
                }

                attribute.UpgradeOpen();
                attribute.TextString = newValue;
                attribute.AdjustAlignment(attribute.Database);
            }
        }

        private static void SetDynamicProperty(
            ObjectId blockId,
            string propertyName,
            string requestedValue,
            Transaction transaction)
        {
            if (string.IsNullOrWhiteSpace(
                    requestedValue))
            {
                return;
            }

            BlockReference blockReference =
                (BlockReference)transaction.GetObject(
                    blockId,
                    OpenMode.ForRead);

            if (!blockReference.IsDynamicBlock)
            {
                return;
            }

            foreach (
                DynamicBlockReferenceProperty property
                in blockReference
                    .DynamicBlockReferencePropertyCollection)
            {
                if (!string.Equals(
                        property.PropertyName,
                        propertyName,
                        StringComparison
                            .OrdinalIgnoreCase))
                {
                    continue;
                }

                if (property.ReadOnly)
                {
                    return;
                }

                object[] allowedValues =
                    property.GetAllowedValues();

                /*
                 * Some dynamic properties may not return an allowed
                 * values list. In that case, attempt to assign the
                 * requested string directly.
                 */
                if (allowedValues == null ||
                    allowedValues.Length == 0)
                {
                    property.Value =
                        requestedValue;

                    return;
                }

                object matchingValue =
                    allowedValues.FirstOrDefault(
                        allowedValue =>
                            string.Equals(
                                Convert.ToString(
                                    allowedValue),
                                requestedValue,
                                StringComparison
                                    .OrdinalIgnoreCase));

                if (matchingValue == null)
                {
                    string validValues =
                        string.Join(
                            ", ",
                            allowedValues.Select(
                                value =>
                                    Convert.ToString(
                                        value)));

                    throw new InvalidOperationException(
                        $"\"{requestedValue}\" is not a valid " +
                        $"value for dynamic property " +
                        $"\"{propertyName}\"." +
                        Environment.NewLine +
                        $"Valid values: {validValues}");
                }

                property.Value =
                    matchingValue;

                return;
            }
        }

        private static void ShowSheetWarning(
            string warning,
            SheetEntry sheet)
        {
            MessageBox.Show(
                BuildSheetMessage(
                    warning,
                    sheet),
                "PNM Revision Tool",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        private static string BuildSheetMessage(
            string message,
            SheetEntry sheet)
        {
            return
                message +
                Environment.NewLine +
                Environment.NewLine +
                $"Sheet: {sheet.SheetTitle}" +
                Environment.NewLine +
                $"Layout: {sheet.LayoutName}" +
                Environment.NewLine +
                $"Drawing: {sheet.DrawingFile}";
        }

        private static void ReleaseComObject(
            object comObject)
        {
            if (comObject == null)
            {
                return;
            }

            if (!Marshal.IsComObject(comObject))
            {
                return;
            }

            try
            {
                Marshal.FinalReleaseComObject(
                    comObject);
            }
            catch
            {
                /*
                 * COM cleanup must not hide an earlier processing
                 * exception.
                 */
            }
        }
    }

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