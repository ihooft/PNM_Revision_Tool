PNM Revision Tool split source files

Files:
- Commands.cs: AutoCAD command entry point and CommandClass assembly attribute
- frmMain.cs: WinForms code-behind (keep your existing frmMain.Designer.cs and .resx)
- RevisionFormValues.cs: form input model
- SheetEntry.cs: sheet model
- SheetProcessingResult.cs: per-sheet result model
- ProcessingSummary.cs: batch summary model
- SheetSetProcessor.cs: sheet set and DWG processing

Important:
1. Remove the original combined Main.cs from the project, or set Build Action to None, to avoid duplicate class definitions.
2. Add all seven .cs files to the same project.
3. Keep the existing frmMain.Designer.cs and frmMain.resx unchanged.
4. SheetSetProcessor now receives UpdateProgress and LogMessage callbacks instead of a frmMain reference.
5. No drawing-processing behavior was intentionally changed.
