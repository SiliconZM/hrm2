# Payroll & Compensation Test Data

## Overview
The database is automatically seeded with Zambian market test data when running the application in development mode. This enables comprehensive testing of the entire payroll workflow.

## Seeded Data Structure

### Organization
- **Name**: ABC Corporation Zambia
- **Industry**: Manufacturing
- **Country**: ZM (Zambia)
- **City**: Lusaka

### Salary Structure
**Name**: Standard Zambian Salary Structure 2024
**Base Salary**: ZMW 5,000

### Salary Components (Zambian Context)

#### Earnings
1. **Basic Salary** - ZMW 5,000 (Fixed, Taxable)
2. **House Allowance** - ZMW 1,500 (Fixed, Taxable)
3. **Transport Allowance** - ZMW 500 (Fixed, Taxable)
4. **Meal Allowance** - ZMW 300 (Fixed, Non-taxable)

**Gross Salary Calculation**: 5,000 + 1,500 + 500 + 300 = **ZMW 7,800**

#### Deductions
1. **PAYE Tax** - 15% of gross (Zambian standard)
   - Amount: ZMW 1,170
2. **NAPSA Contribution** - 5% of gross (National Pension Scheme Authority)
   - Amount: ZMW 390
3. **Work Injury Benefits** - ZMW 50 (Fixed deduction)
   - Amount: ZMW 50

**Total Deductions**: 1,170 + 390 + 50 = **ZMW 1,610**

**Net Salary Calculation**: 7,800 - 1,610 = **ZMW 6,190**

### Employees
Five sample employees with Zambian names and details:

| Code | Name | Email | Location | Hire Date |
|------|------|-------|----------|-----------|
| EMP001 | John Banda | john.banda@abc.co.zm | Lusaka | 2022-01-15 |
| EMP002 | Grace Mwale | grace.mwale@abc.co.zm | Lusaka | 2022-03-20 |
| EMP003 | Mwila Chulu | mwila.chulu@abc.co.zm | Ndola | 2023-06-10 |
| EMP004 | Lungile Phiri | lungile.phiri@abc.co.zm | Kitwe | 2023-02-14 |
| EMP005 | Nathalie Kabonde | nathalie.kabonde@abc.co.zm | Livingstone | 2023-09-01 |

All employees are:
- Active
- Full-time
- Assigned to the Standard Zambian Salary Structure

### Payroll Run
- **Name**: November 2024 Payroll
- **Period**: November 1-30, 2024
- **Status**: Draft
- **Frequency**: Monthly
- **Employees**: 5
- **Total Gross**: ZMW 39,000 (7,800 × 5)
- **Total Deductions**: ZMW 8,050 (1,610 × 5)
- **Total Net**: ZMW 30,950 (6,190 × 5)

### Payroll Details
Created for all 5 employees with:
- Working Days: 22
- Days Worked: 22
- Leave Days: 0
- Status: Draft

### Salary Slips
Generated for the first 2 employees (John Banda and Grace Mwale):
- **Status**: Generated
- **Period**: November 2024
- Contains complete component breakdown

## Testing the Payroll Flow

### 1. View Salary Structures
**URL**: `/payroll/salary-structures`
- ✓ See "Standard Zambian Salary Structure 2024"
- ✓ View basic salary of ZMW 5,000
- ✓ See all 7 components listed

### 2. View Salary Components
From the salary structure page:
- ✓ 4 earning components with proper display order
- ✓ 3 deduction components
- ✓ Verify percentage-based calculations for PAYE (15%) and NAPSA (5%)

### 3. Assign Employee Salaries
**URL**: `/payroll/employee-salaries`
- ✓ See all 5 employees with their salary assignments
- ✓ View calculated gross salary (ZMW 7,800)
- ✓ View calculated net salary (ZMW 6,190)
- ✓ Check effective date and active status

### 4. Create/Edit Payroll Run
**URL**: `/payroll/payroll-runs`
- ✓ See existing November 2024 payroll in Draft status
- ✓ View total gross, deductions, and net amounts
- ✓ Edit payroll to test updates
- ✓ Create new payroll for different month

### 5. View Payroll Details
From payroll run page:
- ✓ See 5 employee details
- ✓ Verify gross salary ZMW 7,800 per employee
- ✓ Verify deductions ZMW 1,610 per employee
- ✓ See working days (22) and days worked (22)

### 6. Process Payroll
(Manual action required):
- Click "Process Payroll" button
- ✓ Status changes from Draft to Processed
- ✓ ProcessedDate is recorded
- ✓ Totals are recalculated and verified

### 7. View Salary Slips
**URL**: `/payroll/salary-slips`
- ✓ Search by employee ID (e.g., Employee IDs 1-5)
- ✓ See generated slips for John Banda and Grace Mwale
- ✓ View period "November 2024"

### 8. Salary Slip Details
**URL**: `/payroll/salary-slips/view/{id}`
- ✓ See complete slip with employee name
- ✓ View gross salary breakdown:
  - Basic Salary: ZMW 5,000
  - House Allowance: ZMW 1,500
  - Transport Allowance: ZMW 500
  - Meal Allowance: ZMW 300
- ✓ View deductions:
  - PAYE Tax: ZMW 1,170
  - NAPSA: ZMW 390
  - WIB: ZMW 50
- ✓ View net payable: ZMW 6,190

### 9. Salary Slip Workflow
For generated salary slips:
- ✓ Approve slip (changes status to Approved)
- ✓ Send to employee (changes status to Sent)
- ✓ Mark as paid (records salary credit date)

## Zambian Payroll Features Demonstrated

### 1. PAYE (Pay As You Earn)
- Implemented as 15% deduction
- Applied to all taxable components
- Automatically calculated based on salary

### 2. NAPSA (National Pension Scheme Authority)
- Employee contribution: 5% of gross
- Separate from PAYE
- Mandatory deduction

### 3. Work Injury Benefits
- Fixed monthly contribution: ZMW 50
- Protects against workplace injuries

### 4. Allowances Structure
- House Allowance: Common in Zambian companies
- Transport Allowance: Standard benefit
- Meal Allowance: Non-taxable benefit

### 5. Salary Flexibility
- Override basic salary for individuals
- Support for different salary structures
- Component-based approach allows custom configurations

## Calculation Examples

### John Banda (Standard Rate)
```
Gross Salary Calculation:
  Basic Salary           =  5,000
  House Allowance        =  1,500
  Transport Allowance    =    500
  Meal Allowance         =    300
  ─────────────────────────────
  Gross Salary           =  7,800

Deductions Calculation:
  PAYE (15% × 7,800)     =  1,170
  NAPSA (5% × 7,800)     =    390
  Work Injury Benefits   =     50
  ─────────────────────────────
  Total Deductions       =  1,610

Net Salary:
  7,800 - 1,610 = 6,190
```

## Running the Payroll Cycle

### Step 1: Create Payroll Run
- Name: "December 2024 Payroll"
- Period: December 1-31, 2024
- Frequency: Monthly

### Step 2: Add Employees
- Add payroll details for each employee
- System auto-calculates salary breakdown
- Set working days and days worked

### Step 3: Process Payroll
- Click "Process Payroll"
- System calculates totals
- Status changes to Processed

### Step 4: Generate Salary Slips
- System generates slips for each employee
- Includes complete component breakdown
- Status is "Generated"

### Step 5: Approve & Send
- Approve each slip
- Send to employees
- Mark as paid when salary is credited

### Step 6: Payroll Completion
- All slips marked as paid
- Complete payroll record for audit trail

## Database Reset

To clear seeded data and re-seed:
```bash
# Delete the database file
rm src/HRManagement.Web/bin/Debug/net10.0/Data/hr.db

# Run the application
dotnet run

# Data will be automatically re-seeded
```

## Notes

- Seeding only runs in **Development** environment
- Data is idempotent (safe to run multiple times)
- All timestamps use UTC
- Salary calculations use 2 decimal places for currency
- All deductions and allowances configured for Zambian compliance

## Testing Checklist

- [ ] View salary structures
- [ ] View salary components
- [ ] Assign employee salaries
- [ ] Create payroll run
- [ ] View payroll details
- [ ] Process payroll
- [ ] Generate salary slips
- [ ] View salary slip details
- [ ] Test salary slip workflow (Approve → Send → Paid)
- [ ] Verify PAYE calculation (15%)
- [ ] Verify NAPSA calculation (5%)
- [ ] Verify net salary calculation
- [ ] Test override basic salary feature
- [ ] Test proration with different days worked
- [ ] Verify pagination on all list pages

---

**Last Updated**: November 2024
**For**: Payroll & Compensation Module Phase 2 Testing
**Market**: Zambian
