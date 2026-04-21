using System;
using System.Collections.Generic;

#nullable enable

namespace InventorySystemDepEd.Shared.Excel.Imports
{
    /// <summary>
    /// Extended import result that includes duplicate detection
    /// Allows users to confirm whether to update existing personnel or create new ones
    /// </summary>
    public class ExcelImportWithDuplicatesResult<T>
    {
        /// <summary>
        /// New personnel records without duplicates
        /// </summary>
        public List<T> NewData { get; set; } = new();

        /// <summary>
        /// Personnel records that match existing database entries (duplicates)
        /// </summary>
        public List<DuplicatePersonnelRecord<T>> Duplicates { get; set; } = new();

        /// <summary>
        /// Validation errors from Excel processing
        /// </summary>
        public List<ExcelRowError> Errors { get; set; } = new();

        /// <summary>
        /// Indicates if there are any validation errors
        /// </summary>
        public bool HasErrors => Errors.Any();

        /// <summary>
        /// Indicates if there are duplicate records requiring user confirmation
        /// </summary>
        public bool HasDuplicates => Duplicates.Any();

        /// <summary>
        /// Total summary of import results
        /// </summary>
        public ImportSummary Summary => new()
        {
            TotalRowsProcessed = NewData.Count + Duplicates.Count + Errors.Count,
            NewRecordsCount = NewData.Count,
            DuplicateRecordsCount = Duplicates.Count,
            ErrorCount = Errors.Count,
            IsReadyForImport = !HasErrors && !HasDuplicates
        };
    }

    /// <summary>
    /// Represents a duplicate personnel record with matching existing record details
    /// </summary>
    public class DuplicatePersonnelRecord<T>
    {
        /// <summary>
        /// Excel row number where the duplicate was found
        /// </summary>
        public int ExcelRowNumber { get; set; }

        /// <summary>
        /// The new personnel data from Excel file
        /// </summary>
        public T? NewData { get; set; }

        /// <summary>
        /// The existing personnel record in the database
        /// </summary>
        public ExistingPersonnelInfo? ExistingData { get; set; }

        /// <summary>
        /// User's action: "create" (create new) or "update" (update existing)
        /// Used when user confirms duplicate handling
        /// </summary>
        public string UserAction { get; set; } = ""; // "create" or "update"
    }

    /// <summary>
    /// Summary information of an existing personnel record
    /// </summary>
    public class ExistingPersonnelInfo
    {
        public int PersonnelId { get; set; }
        public string? EmployeeID { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? EmailAddress { get; set; }
        public string? ContactNumber { get; set; }
        public string? Position { get; set; }
        public string? Office { get; set; }
        public DateTime? HiredDate { get; set; }
    }

    /// <summary>
    /// Summary statistics of the import operation
    /// </summary>
    public class ImportSummary
    {
        public int TotalRowsProcessed { get; set; }
        public int NewRecordsCount { get; set; }
        public int DuplicateRecordsCount { get; set; }
        public int ErrorCount { get; set; }

        /// <summary>
        /// True if import is ready to proceed (no errors and no duplicates, or duplicates confirmed)
        /// </summary>
        public bool IsReadyForImport { get; set; }
    }
}
