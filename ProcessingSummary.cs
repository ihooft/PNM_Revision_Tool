using System.Collections.Generic;

namespace PNM_Revision_Tool
{
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
}
