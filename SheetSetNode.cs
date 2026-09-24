namespace PNM_Revision_Tool
{
    /// <summary>
    /// Represents a node in the sheet set hierarchy, which can be either a subset or a sheet.
    /// </summary>
    internal sealed class SheetSetNode
    {
        /// <summary>
        /// Gets the display name of this node (subset name or sheet title).
        /// </summary>
        public string Name { get; init; } = string.Empty;

        /// <summary>
        /// Gets the associated SheetEntry if this node represents a sheet (leaf node).
        /// </summary>
        public SheetEntry? Sheet { get; init; }

        /// <summary>
        /// Gets the child nodes (subsets or sheets).
        /// </summary>
        public List<SheetSetNode> Children { get; init; } = new List<SheetSetNode>();

        /// <summary>
        /// Gets a value indicating whether this node is a sheet (as opposed to a subset).
        /// </summary>
        public bool IsSheet => Sheet != null;
    }
}
