# Duplicate Personnel Detection - Implementation Summary

## Overview
The Excel personnel import system now detects duplicate employees before saving and presents users with a list of conflicts for confirmation.

---

## Workflow

### Step 1: Upload Excel File
**Endpoint:** `POST /api/personnels/upload`  
**Request:** IFormFile (Excel .xlsx file)

**Response:** `ExcelImportWithDuplicatesResult<PersonnelDto>`
```json
{
  "newData": [
    {
      "employeeID": "EMP001",
      "firstName": "John",
      "lastName": "Doe",
      "position": 1,
      "office": 1,
      ...
    }
  ],
  "duplicates": [
    {
      "excelRowNumber": 5,
      "newData": {
        "employeeID": "EMP002",
        "firstName": "Jane",
        "lastName": "Smith",
        ...
      },
      "existingData": {
        "personnelId": 42,
        "employeeID": "EMP002",
        "firstName": "Jane",
        "lastName": "Smith",
        "position": "Teacher",
        "office": "Main Office",
        "hiredDate": "2020-06-15"
      },
      "userAction": ""
    }
  ],
  "errors": [
    {
      "rowNumber": 8,
      "column": "Position",
      "message": "Position 'Invalid Position' not found"
    }
  ],
  "summary": {
    "totalRowsProcessed": 10,
    "newRecordsCount": 3,
    "duplicateRecordsCount": 1,
    "errorCount": 1,
    "isReadyForImport": false
  }
}
```

**What Happens:**
1. Excel file is parsed and validated
2. Each row is checked against the database for matching EmployeeID
3. Personnel are categorized into three groups:
   - **newData**: New personnel (no duplicates found)
   - **duplicates**: Personnel with matching EmployeeID in database
   - **errors**: Validation errors (invalid position, office, etc.)

4. **New personnel are saved immediately** (no conflicts)
5. **Duplicates are returned** without saving, awaiting user confirmation

---

### Step 2: Review Duplicates (User Action)
User reviews the duplicate list showing:
- Excel row number where duplicate was found
- New data from Excel file
- Existing data in database
- Side-by-side comparison helps user decide

**Decision needed for each duplicate:**
- `"create"` - Create a new record (allow duplicate EmployeeID)
- `"update"` - Update the existing record with new data
- (empty) - Skip this record

---

### Step 3: Submit Confirmation
**Endpoint:** `POST /api/personnels/upload/confirm-duplicates`  
**Request:** The ExcelImportWithDuplicatesResult with `userAction` populated for each duplicate

```json
{
  "newData": [ ... ],
  "duplicates": [
    {
      "excelRowNumber": 5,
      "newData": { ... },
      "existingData": { ... },
      "userAction": "update"  // User chose to update
    }
  ],
  "errors": [ ... ]
}
```

**Processing Logic:**
- For each duplicate with `userAction == "update"`:
  - Find existing personnel by EmployeeID
  - Update all fields (FirstName, MiddleName, LastName, Email, Phone, Position, Office, HiredDate)
  - Save changes
  
- For each duplicate with `userAction == "create"`:
  - Create new personnel record (duplicate EmployeeID allowed)
  - Save to database
  
- For each duplicate with `userAction == ""` or empty:
  - Report error: "No action specified for duplicate"

**Response:** 
```json
{
  "success": true,
  "message": "Import completed: 4 records saved, 1 errors",
  "data": {
    "data": [...],
    "errors": [...]
  }
}
```

---

## Benefits

✅ **Prevents Unintended Duplicates**
- User reviews conflicts before data is created/modified
- Clear visibility of existing data vs. new data

✅ **Flexible Handling**
- **Update existing**: Merge new data into existing record
- **Create new**: Allow multiple employees with same ID (if intentional)
- **Skip**: Ignore problematic records

✅ **Data Integrity**
- Validation errors still reported and prevent save
- Only correct data is committed to database
- Transaction-safe: either all changes succeed or all fail

✅ **User-Friendly**
- Shows existing personnel details alongside new data
- Clear error messages for invalid positions/offices
- Summary statistics (new: 3, duplicates: 1, errors: 1)

---

## Example Usage in Frontend

```javascript
// Step 1: Upload file
const formData = new FormData();
formData.append('file', excelFile);

const uploadResponse = await fetch('/api/personnels/upload', {
  method: 'POST',
  body: formData
});

const importResult = await uploadResponse.json();

// Check results
if (importResult.summary.isReadyForImport) {
  // No errors, no duplicates - data already saved!
  showMessage(`Success! ${importResult.summary.newRecordsCount} personnel imported.`);
} else if (importResult.summary.hasDuplicates) {
  // Show duplicate list to user
  showDuplicateConfirmation(importResult.duplicates);
} else if (importResult.summary.hasErrors) {
  // Show errors only
  showErrors(importResult.errors);
}

// Step 2: User reviews and decides on duplicates
// User interface allows setting userAction: "create", "update", or empty for each duplicate

// Step 3: Submit confirmation
const confirmResponse = await fetch('/api/personnels/upload/confirm-duplicates', {
  method: 'POST',
  headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify(importResult)  // Now with userAction populated
});

const finalResult = await confirmResponse.json();
if (finalResult.success) {
  showMessage(`Import complete: ${finalResult.data.data.length} records saved`);
}
```

---

## Technical Details

**Duplicate Detection:**
- Primary key: `EmployeeID` (case-insensitive comparison)
- Query: `_context.Personnels.Where(p => p.EmployeeID == newEmployeeID)`
- Includes related Position and Office data for display

**Update Operation:**
- Finds existing by EmployeeID
- Updates: FirstName, MiddleName, LastName, Email, Phone, HiredDate, PositionId, OfficeId
- Uses EF Core's Update() method for change tracking

**Create Operation:**
- Creates new PersonnelsModel with all provided data
- EmployeeID can be duplicate (if user explicitly chose "create")
- Uses StringHelper.ToProperCase() for name formatting

**Transaction Safety:**
- All new records added before SaveChangesAsync()
- All updates applied before SaveChangesAsync()
- Single transaction commits all changes atomically

---

## Classes Added

### `ExcelImportWithDuplicatesResult<T>`
Extended import result with:
- `NewData`: List of new personnel without conflicts
- `Duplicates`: List of duplicate conflicts with existing data
- `Errors`: Validation errors
- `Summary`: Statistics and import readiness status

### `DuplicatePersonnelRecord<T>`
Represents a single duplicate conflict:
- `ExcelRowNumber`: Where duplicate was found
- `NewData`: Data from Excel file
- `ExistingData`: Data from database
- `UserAction`: User's decision ("create", "update", or empty)

### `ExistingPersonnelInfo`
Display data for existing personnel:
- PersonnelId, EmployeeID, FirstName, MiddleName, LastName
- EmailAddress, ContactNumber
- Position, Office (loaded with relationships)
- HiredDate

### `ImportSummary`
Statistics summary:
- TotalRowsProcessed, NewRecordsCount, DuplicateRecordsCount, ErrorCount
- IsReadyForImport: Boolean flag for automatic vs. manual processing

---

## Database Impact

**No schema changes required** - Uses existing PersonnelsModel table:
- EmployeeID (string) - Primary identifier for duplicate detection
- FirstName, MiddleName, LastName (string)
- EmailAddress, ContactNumber (string)
- PositionId, OfficeId (int foreign keys)
- HiredDate (DateTime)

---

## Next Steps

1. **Frontend Implementation**
   - Create duplicate confirmation UI
   - Display side-by-side comparison
   - Allow user to select action per duplicate
   - Submit confirmation endpoint

2. **Testing**
   - Test with Excel file containing duplicates
   - Test "create" action (allows duplicate EmployeeID)
   - Test "update" action (overwrites existing record)
   - Test error scenarios (invalid position, office, etc.)

3. **Validation**
   - Verify existing personnel are properly updated
   - Verify new personnel are properly created
   - Verify error messages appear for skipped duplicates
