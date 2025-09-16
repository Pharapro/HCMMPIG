function Refreshvalue() {
    var total = 0;
    $.ajax({
        type: 'post',
        url: _baseUrl + '/HRM/EmployeeInfo/HRCareerHistory/Refreshvalue',
        data: { 'id': 'TOTAL', 'Increase': txtIncrease.GetValue() },
        success: function (data) {
            if (data.MS == "OK") {
                txtNewSalary.SetValue(data.NewSalary);
            }
            else {
                AlertMessage(data.MS);
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            AlertMessage(XMLHttpRequest.responseText);

        }
    });
}


function valueChange(s, e) {
    Refreshvalue();
}
var selectedIDs;
function ShowData() {
    var total = 0;
    $.ajax({
        type: 'post',
        url: _baseUrl + '/HRM/EmployeeInfo/HRCareerHistory/ShowData',
        data: { 'id': 'TOTAL', 'Tran': selectedIDs },
        success: function (data) {
            if (data.MS == "OK") {
                var date = data.EffectDate;
                var nowDate = new Date(parseInt(date.substr(6)));
                var To = data.ToDate;
                var ToDate = new Date(parseInt(To.substr(6)));
                txtEmpType.SetValue(data.EmpType);
                txtBranch.SetValue(data.Branch);
                txtDivision.SetValue(data.Division);
                txtDEPT.SetValue(data.DEPT);
                txtSECT.SetValue(data.SECT);
                txtJobCode.SetValue(data.JobCode);
                txtLevelCode.SetValue(data.LevelCode);
                txtCareerCode.SetValue(data.CareerCode);
                txtEffectDate.SetValue(nowDate);
                txtOldSalary.SetValue(data.OldSalary);
                txtIncrease.SetValue(data.Increase);
                txtNewSalary.SetValue(data.NewSalary);
                txtLastDate.SetValue(ToDate);
                txtRemark.SetValue(data.Remark);
            }
            else {
                AlertMessage(data.MS);
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            AlertMessage(XMLHttpRequest.responseText);

        }
    });
}
function OnSelectionChanged(s, e) {
    ss = 1;
    s.GetRowValues(s.GetFocusedRowIndex(), 'TranNo;', OnGetRowValues);
}


function OnGetRowValues(values) {
    selectedIDs = values[0];
    ShowData();
}
//function initDataLevel(s, e) {
//    $.ajax({
//        type: 'post',
//        url: _baseUrl + '/HRM/EmployeeInfo/HRCareerHistory/FitlerLevel',
//        data: { 'code': s.GetValue() },
//        success: function (data) {
//            cboJobGrade.PerformCallback();
//            cboJobGrade.SetValue(_L);
//        }
//    });

//}

//function selectLevel(s, e) {
//    $.ajax({
//        type: 'post',
//        url: _baseUrl + '/HRM/EmployeeInfo/HRCareerHistory/FitlerLevel',
//        data: { 'code': s.GetValue() },
//        success: function (data) {
//            cboJobGrade.PerformCallback();
//        }
//    });
//}
//function selectJobGrade(s, e) {
//    txtJobGrade.SetValue(s.GetValue());
//}

//New

function initDataBranch(s, e) {
    init_ShowDataJob(s.GetValue(), 'Branch');
}
function initDataDivision(s, e) {
    init_ShowDataJob(s.GetValue(), 'Division');
}
function initDataGroupDepartment(s, e) {
    init_ShowDataJob(s.GetValue(), 'GroupDepartment');
}
function initDataDepartment(s, e) {
    init_ShowDataJob(s.GetValue(), 'Department');
}
function initDataPosition(s, e) {
    init_ShowDataJob(s.GetValue(), 'Position');
}
function initDataSection(s, e) {
    init_ShowDataJob(s.GetValue(), 'Section');
}
function initDataLevel(s, e) {
    init_ShowDataJob(s.GetValue(), 'Level');
}
function initDataJobGrade(s, e) {
    init_ShowDataJob(s.GetValue(), 'JobGrade');
}

function selectBranch(s, e) {
    ShowDataJob(s.GetValue(), 'Branch');
}
function selectDivision(s, e) {
    ShowDataJob(s.GetValue(), 'Division');
}
function selectGroupDepartment(s, e) {
    ShowDataJob(s.GetValue(), 'GroupDepartment');
}
function selectDepartment(s, e) {
    ShowDataJob(s.GetValue(), 'Department');
}
function selectPosition(s, e) {
    ShowDataJob(s.GetValue(), 'Position');
}
function selectSection(s, e) {
    ShowDataJob(s.GetValue(), 'Section');
}
function selectLevel(s, e) {
    ShowDataJob(s.GetValue(), 'Level');
}
function selectJobGrade(s, e) {
    ShowDataJob(s.GetValue(), 'JobGrade');
}
function ShowDataJob(Code, Type) {
    $.ajax({
        type: 'post',
        url: _baseUrl + '/HRM/EmployeeInfo/HRCareerHistory/JobType',
        data: { 'code': Code, 'addType': Type },
        success: function (data) {
            if (data.MS == "Division")
                cboDision.PerformCallback();
            else if (data.MS == "GroupDepartment")
                cboGroupDepartment.PerformCallback();
            else if (data.MS == "Department")
                cboDepartment.PerformCallback();
            else if (data.MS == "Position")
                cboPosition.PerformCallback();
            else if (data.MS == "Section")
                cboSection.PerformCallback();
            else if (data.MS == "Level")
                cboLevel.PerformCallback();
            else if (data.MS == "JobGrade")
                cboJobGrade.PerformCallback();
        }
    });
}

function init_ShowDataJob(Code, Type) {
    $.ajax({
        type: 'post',
        url: _baseUrl + '/HRM/EmployeeInfo/HRCareerHistory/JobType',
        data: { 'code': Code, 'addType': Type },
        success: function (data) {
            if (data.MS == "Division") {
                cboDision.PerformCallback();
                cboDision.SetValue(_Div);               
            }
            else if (data.MS == "GroupDepartment") {
                cboGroupDepartment.PerformCallback();
                cboGroupDepartment.SetValue(_GDept);
            }
            else if (data.MS == "Department") {
                cboDepartment.PerformCallback();
                cboDepartment.SetValue(_Dept);
            }
            else if (data.MS == "Position") {
                cboPosition.PerformCallback();
                cboPosition.SetValue(_JC);
            }
            else if (data.MS == "Section") {
                cboSection.PerformCallback();
                cboSection.SetValue(_SECT);
            }
            else if (data.MS == "Level") {
                cboLevel.PerformCallback();
                cboLevel.SetValue(_LC);
            }
            else if (data.MS == "JobGrade") {
                cboJobGrade.PerformCallback();
                cboJobGrade.SetValue(_JG);
            }
        }
    });
}
