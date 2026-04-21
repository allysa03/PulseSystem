# Duplicate Personnel Detection - API Response Examples

## Scenario 1: No Duplicates, No Errors
✅ Import succeeds immediately, all data saved

**Request:** Upload Excel with 5 new personnel  
**Response Status:** 200 OK

```json
{
  "newData": [
    {
      "personnelId": 0,
      "employeeID": "EMP001",
      "firstName": "John",
      "middleName": "Mark",
      "lastName": "Doe",
      "emailAddress": "john.doe@deped.gov.ph",
      "contactNumber": "09171234567",
      "positionId": 1,
      "officeId": 2,
      "hiredDate": "2023-01-15T00:00:00Z"
    },
    {
      "employeeID": "EMP002",
      "firstName": "Jane",
      "lastName": "Smith",
      ...
    }
    // 3 more new personnel
  ],
  "duplicates": [],
  "errors": [],
  "summary": {
    "totalRowsProcessed": 5,
    "newRecordsCount": 5,
    "duplicateRecordsCount": 0,
    "errorCount": 0,
    "isReadyForImport": true
  }
}
```

**Next Action:** Success message, show summary to user

---

## Scenario 2: Found Duplicates, No Errors
⏸️ Import paused, awaiting user confirmation on duplicates

**Request:** Upload Excel with 3 new + 2 duplicates  
**Response Status:** 200 OK

```json
{
  "newData": [
    {
      "employeeID": "EMP001",
      "firstName": "John",
      "lastName": "Doe",
      "positionId": 1,
      "officeId": 2,
      ...
    },
    {
      "employeeID": "EMP002",
      "firstName": "Jane",
      "lastName": "Smith",
      ...
    },
    {
      "employeeID": "EMP003",
      "firstName": "Bob",
      "lastName": "Johnson",
      ...
    }
  ],
  "duplicates": [
    {
      "excelRowNumber": 6,
      "newData": {
        "employeeID": "EMP004",
        "firstName": "Alice",
        "middleName": "Marie",
        "lastName": "Brown",
        "emailAddress": "alice.brown@deped.gov.ph",
        "contactNumber": "09172223333",
        "positionId": 2,
        "officeId": 1,
        "hiredDate": "2024-01-10T00:00:00Z"
      },
      "existingData": {
        "personnelId": 42,
        "employeeID": "EMP004",
        "firstName": "Alice",
        "middleName": "M",
        "lastName": "Brown",
        "emailAddress": "alice.brown@old.deped.gov.ph",
        "contactNumber": "09179999999",
        "position": "Teacher",
        "office": "Main Office",
        "hiredDate": "2020-05-20T00:00:00Z"
      },
      "userAction": ""  // Empty - awaiting user decision
    },
    {
      "excelRowNumber": 7,
      "newData": {
        "employeeID": "EMP005",
        "firstName": "Charlie",
        "lastName": "Wilson",
        "positionId": 3,
        "officeId": 2,
        ...
      },
      "existingData": {
        "personnelId": 89,
        "employeeID": "EMP005",
        "firstName": "Charles",
        "lastName": "Wilson",
        "position": "Principal",
        "office": "Regional Office",
        ...
      },
      "userAction": ""
    }
  ],
  "errors": [],
  "summary": {
    "totalRowsProcessed": 5,
    "newRecordsCount": 3,
    "duplicateRecordsCount": 2,
    "errorCount": 0,
    "isReadyForImport": false
  }
}
```

**Next Action:** 
1. Show duplicates confirmation dialog to user
2. For each duplicate, show side-by-side comparison
3. Allow user to select "Create New" or "Update Existing"
4. Submit confirmation with userAction populated

---

## Scenario 3: Errors Found (No Duplicates)
❌ Import failed, show error list to user

**Request:** Upload Excel with invalid Position/Office values  
**Response Status:** 200 OK

```json
{
  "newData": [
    {
      "employeeID": "EMP001",
      "firstName": "John",
      ...
    }
  ],
  "duplicates": [],
  "errors": [
    {
      "rowNumber": 3,
      "column": "Position",
      "message": "Position 'InvalidPosition' not found"
    },
    {
      "rowNumber": 4,
      "column": "Office",
      "message": "Office 'InvalidOffice' not found"
    },
    {
      "rowNumber": 5,
      "column": "EmailAddress",
      "message": "Email must end with @deped.gov.ph"
    },
    {
      "rowNumber": 6,
      "column": "EmployeeID",
      "message": "Employee ID is required"
    }
  ],
  "summary": {
    "totalRowsProcessed": 6,
    "newRecordsCount": 1,
    "duplicateRecordsCount": 0,
    "errorCount": 5,
    "isReadyForImport": false
  }
}
```

**Next Action:** 
1. Show error messages to user
2. Highlight problematic rows
3. User fixes Excel file and re-uploads
4. Note: New personnel (EMP001) were NOT saved because there are errors

---

## Scenario 4: Both Duplicates and Errors
⚠️ Mixed results - some data saved, some awaiting confirmation, some errors

**Request:** Upload Excel with 4 records (1 new, 1 duplicate, 2 errors)  
**Response Status:** 200 OK

```json
{
  "newData": [
    {
      "employeeID": "EMP001",
      "firstName": "John",
      "lastName": "Doe",
      ...
    }
  ],
  "duplicates": [
    {
      "excelRowNumber": 3,
      "newData": {
        "employeeID": "EMP002",
        "firstName": "Jane",
        ...
      },
      "existingData": {
        "personnelId": 42,
        "employeeID": "EMP002",
        ...
      },
      "userAction": ""
    }
  ],
  "errors": [
    {
      "rowNumber": 4,
      "column": "Position",
      "message": "Position 'Coach' not found"
    },
    {
      "rowNumber": 5,
      "column": "HiredDate",
      "message": "Invalid Hire Date format (use YYYY-MM-DD)"
    }
  ],
  "summary": {
    "totalRowsProcessed": 4,
    "newRecordsCount": 1,
    "duplicateRecordsCount": 1,
    "errorCount": 2,
    "isReadyForImport": false
  }
}
```

**Next Action:**
1. New data (EMP001) is saved immediately ✅
2. Show duplicate confirmation dialog for EMP002
3. Show error messages for rows 4-5
4. User fixes errors, re-uploads the problematic rows
5. User confirms duplicate action for EMP002

---

## Confirmation Request Example

After user reviews duplicates, sends confirmation with actions selected:

**Endpoint:** POST `/api/personnels/upload/confirm-duplicates`

```json
{
  "newData": [
    {
      "employeeID": "EMP001",
      "firstName": "John",
      "lastName": "Doe",
      ...
    }
  ],
  "duplicates": [
    {
      "excelRowNumber": 6,
      "newData": {
        "employeeID": "EMP004",
        "firstName": "Alice",
        ...
      },
      "existingData": {
        "personnelId": 42,
        ...
      },
      "userAction": "update"  // User chose to UPDATE
    },
    {
      "excelRowNumber": 7,
      "newData": {
        "employeeID": "EMP005",
        "firstName": "Charlie",
        ...
      },
      "existingData": {
        "personnelId": 89,
        ...
      },
      "userAction": "create"  // User chose to CREATE
    }
  ],
  "errors": []
}
```

**Response:** 200 OK
```json
{
  "success": true,
  "message": "Import completed: 3 records saved, 0 errors",
  "data": {
    "data": [
      // All 3 saved records (1 new + 1 updated + 1 created)
    ],
    "errors": []
  }
}
```

---

## Frontend Decision Logic

```javascript
function handleImportResponse(result) {
  // Check immediate save success
  if (result.summary.isReadyForImport) {
    // All done! Show success
    showSuccess(`${result.summary.newRecordsCount} personnel imported`);
    return;
  }
  
  // Check for validation errors
  if (result.summary.errorCount > 0) {
    showErrors(
      `Found ${result.summary.errorCount} errors that must be fixed:`,
      result.errors
    );
  }
  
  // Check for duplicates
  if (result.summary.duplicateRecordsCount > 0) {
    showDuplicateConfirmation(
      `${result.summary.newRecordsCount} new personnel ready to save, ` +
      `${result.summary.duplicateRecordsCount} duplicates need your decision`,
      result.duplicates,
      onConfirm
    );
  }
}

function onConfirm(duplicates) {
  // User has set userAction on each duplicate
  result.duplicates = duplicates;  // Update with user choices
  
  // Send confirmation
  fetch('/api/personnels/upload/confirm-duplicates', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(result)
  })
  .then(r => r.json())
  .then(finalResult => {
    if (finalResult.success) {
      showSuccess(`${finalResult.data.data.length} records saved successfully!`);
    }
  });
}
```

---

## Summary Rules

| Scenario | isReadyForImport | Action |
|----------|------------------|--------|
| No errors, no duplicates | ✅ true | Auto-save, show success |
| No errors, has duplicates | ❌ false | Show confirmation dialog |
| Has errors, no duplicates | ❌ false | Show error list, user must fix |
| Has errors, has duplicates | ❌ false | Show both errors AND confirmation |

