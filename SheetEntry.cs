namespace PNM_Revision_Tool
{
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
}
