var textSeparator = ",";
var _selectedIDsEmpCode = "";
var _selectedIDsCareerCode = "";
var _selectedIDsBranch = "";
var _selectedIDsDivision = "";
var _selectedIDsLocation = "";
var _selectedIDsDepartment = "";
var _selectedIDsSection = "";
var _selectedIDsPosition = "";
var _selectedIDsLevel = "";
var _selectedIDsLeaveType = "";
//-------------------Employee------------------------------------
function OnSelectionChangedEmpCode(s, e) {
    s.GetSelectedFieldValues("EmpCode", GetSelectedFieldValuesCallbackEmpCode);
}
function GetSelectedFieldValuesCallbackEmpCode(values) {
    _selectedIDsEmpCode = "";

    for (var index = 0; index < values.length; index++) {
        _selectedIDsEmpCode += values[index] + textSeparator;

    }
    if (_selectedIDsEmpCode.length > 0) {
        _selectedIDsEmpCode = _selectedIDsEmpCode.substring(0, _selectedIDsEmpCode.length - 1);
    }

    checkComboBoxEmpCode.SetText(_selectedIDsEmpCode);
}
function clearSelectedEmpCode(s, e) {
    checkComboBoxEmpCode.SetText("");
    EmpCodeGridListSelector.UnselectAllRowsOnPage();
    _selectedIDsEmpCode = "";

}
//-------------------Location------------------------------------
function OnSelectionChangedLocation(s, e) {
    s.GetSelectedFieldValues("Code", GetSelectedFieldValuesCallbackLocation);
}
function GetSelectedFieldValuesCallbackLocation(values) {
    _selectedIDsLocation = "";

    for (var index = 0; index < values.length; index++) {
        _selectedIDsLocation += values[index] + textSeparator;

    }
    if (_selectedIDsLocation.length > 0) {
        _selectedIDsLocation = _selectedIDsLocation.substring(0, _selectedIDsLocation.length - 1);
    }

    checkComboBoxLocation.SetText(_selectedIDsLocation);
}
function clearSelectedLocation(s, e) {
    checkComboBoxLocation.SetText("");
    LocationGridListSelector.UnselectAllRowsOnPage();
    _selectedIDsLocation = "";
}
//-------------------Career Code------------------------------------
function OnSelectionChangedCareerCode(s, e) {
    s.GetSelectedFieldValues("Code", GetSelectedFieldValuesCallbackCareerCode);
}
function GetSelectedFieldValuesCallbackCareerCode(values) {
    _selectedIDsCareerCode = "";

    for (var index = 0; index < values.length; index++) {
        _selectedIDsCareerCode += values[index] + textSeparator;

    }
    if (_selectedIDsCareerCode.length > 0) {
        _selectedIDsCareerCode = _selectedIDsCareerCode.substring(0, _selectedIDsCareerCode.length - 1);
    }

    checkComboBoxCareerCode.SetText(_selectedIDsCareerCode);
}
function clearSelectedCareerCode(s, e) {
    checkComboBoxCareerCode.SetText("");
    CareerCodeGridListSelector.UnselectAllRowsOnPage();
    _selectedIDsCareerCode = "";

}
//-------------------Branch Code------------------------------------
function OnSelectionChangedBranch(s, e) {
    s.GetSelectedFieldValues("Code", GetSelectedFieldValuesCallbackBranch);
}
function GetSelectedFieldValuesCallbackBranch(values) {
    _selectedIDsBranch = "";

    for (var index = 0; index < values.length; index++) {
        _selectedIDsBranch += values[index] + textSeparator;

    }
    if (_selectedIDsBranch.length > 0) {
        _selectedIDsBranch = _selectedIDsBranch.substring(0, _selectedIDsBranch.length - 1);
    }

    checkComboBoxBranch.SetText(_selectedIDsBranch);
}
function clearSelectedBranch(s, e) {
    checkComboBoxBranch.SetText("");
    BranchGridListSelector.UnselectAllRowsOnPage();
    _selectedIDsBranch = "";

}
//-------------------Division Code------------------------------------
function OnSelectionChangedDivision(s, e) {
    s.GetSelectedFieldValues("Code", GetSelectedFieldValuesCallbackDivision);
}
function GetSelectedFieldValuesCallbackDivision(values) {
    _selectedIDsDivision = "";

    for (var index = 0; index < values.length; index++) {
        _selectedIDsDivision += values[index] + textSeparator;

    }
    if (_selectedIDsDivision.length > 0) {
        _selectedIDsDivision = _selectedIDsDivision.substring(0, _selectedIDsDivision.length - 1);
    }

    checkComboBoxDivision.SetText(_selectedIDsDivision);
}
function clearSelectedDivision(s, e) {
    checkComboBoxDivision.SetText("");
    DivisionGridListSelector.UnselectAllRowsOnPage();
    _selectedIDsDivision = "";
}
//-------------------Department Code------------------------------------
function OnSelectionChangedDepartment(s, e) {
    s.GetSelectedFieldValues("Code", GetSelectedFieldValuesCallbackDepartment);
}
function GetSelectedFieldValuesCallbackDepartment(values) {
    _selectedIDsDepartment = "";

    for (var index = 0; index < values.length; index++) {
        _selectedIDsDepartment += values[index] + textSeparator;

    }
    if (_selectedIDsDepartment.length > 0) {
        _selectedIDsDepartment = _selectedIDsDepartment.substring(0, _selectedIDsDepartment.length - 1);
    }

    checkComboBoxDepartment.SetText(_selectedIDsDepartment);
}
function clearSelectedDepartment(s, e) {
    checkComboBoxDepartment.SetText("");
    DepartmentGridListSelector.UnselectAllRowsOnPage();
    _selectedIDsDepartment = "";
}
//-------------------Section Code------------------------------------
function OnSelectionChangedSection(s, e) {
    s.GetSelectedFieldValues("Code", GetSelectedFieldValuesCallbackSection);
}
function GetSelectedFieldValuesCallbackSection(values) {
    _selectedIDsSection = "";

    for (var index = 0; index < values.length; index++) {
        _selectedIDsSection += values[index] + textSeparator;
    }
    if (_selectedIDsSection.length > 0) {
        _selectedIDsSection = _selectedIDsSection.substring(0, _selectedIDsSection.length - 1);
    }
    checkComboBoxSection.SetText(_selectedIDsSection);
}
function clearSelectedSection(s, e) {
    checkComboBoxSection.SetText("");
    SectionGridListSelector.UnselectAllRowsOnPage();
    _selectedIDsSection = "";
}
//-------------------Position Code------------------------------------
function OnSelectionChangedPosition(s, e) {
    s.GetSelectedFieldValues("Code", GetSelectedFieldValuesCallbackPosition);
}
function GetSelectedFieldValuesCallbackPosition(values) {
    _selectedIDsPosition = "";

    for (var index = 0; index < values.length; index++) {
        _selectedIDsPosition += values[index] + textSeparator;
    }
    if (_selectedIDsPosition.length > 0) {
        _selectedIDsPosition = _selectedIDsPosition.substring(0, _selectedIDsPosition.length - 1);
    }
    checkComboBoxPosition.SetText(_selectedIDsPosition);
}
function clearSelectedPosition(s, e) {
    checkComboBoxPosition.SetText("");
    PositionGridListSelector.UnselectAllRowsOnPage();
    _selectedIDsPosition = "";
}
//-------------------Level Code------------------------------------
function OnSelectionChangedLevel(s, e) {
    s.GetSelectedFieldValues("Code", GetSelectedFieldValuesCallbackLevel);
}
function GetSelectedFieldValuesCallbackLevel(values) {
    _selectedIDsLevel = "";

    for (var index = 0; index < values.length; index++) {
        _selectedIDsLevel += values[index] + textSeparator;

    }
    if (_selectedIDsLevel.length > 0) {
        _selectedIDsLevel = _selectedIDsLevel.substring(0, _selectedIDsLevel.length - 1);
    }

    checkComboBoxLevel.SetText(_selectedIDsLevel);
}
function clearSelectedLevel(s, e) {
    checkComboBoxLevel.SetText("");
    LevelGridListSelector.UnselectAllRowsOnPage();
    _selectedIDsLevel = "";
}
//-------------------Leave Type------------------------------------
function OnSelectionChangedLeaveType(s, e) {
    s.GetSelectedFieldValues("Code", GetSelectedFieldValuesCallbackLeaveType);
}
function GetSelectedFieldValuesCallbackLeaveType(values) {
    _selectedIDsLeaveType = "";

    for (var index = 0; index < values.length; index++) {
        _selectedIDsLeaveType += values[index] + textSeparator;

    }
    if (_selectedIDsLeaveType.length > 0) {
        _selectedIDsLeaveType = _selectedIDsLeaveType.substring(0, _selectedIDsLeaveType.length - 1);
    }

    checkComboBoxLeaveType.SetText(_selectedIDsLeaveType);
}
function clearSelectedLeaveType(s, e) {
    checkComboBoxLeaveType.SetText("");
    LeaveTypeGridListSelector.UnselectAllRowsOnPage();
    _selectedIDsLeaveType = "";
}