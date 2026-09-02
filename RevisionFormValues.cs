namespace PNM_Revision_Tool
{
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

        public bool IsEmpty()
        {
            return string.IsNullOrWhiteSpace(RevisionNumber) &&
                   string.IsNullOrWhiteSpace(Date) &&
                   string.IsNullOrWhiteSpace(DrafterInitials) &&
                   string.IsNullOrWhiteSpace(Description1) &&
                   string.IsNullOrWhiteSpace(Description2) &&
                   string.IsNullOrWhiteSpace(Description3) &&
                   string.IsNullOrWhiteSpace(CheckedInitials) &&
                   string.IsNullOrWhiteSpace(OkayedInitials) &&
                   string.IsNullOrWhiteSpace(ApprovedInitials) &&
                   string.IsNullOrWhiteSpace(StatusStamp);
        }
    }
}
