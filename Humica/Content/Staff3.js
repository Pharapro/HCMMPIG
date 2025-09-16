var g_index = 0;

var qty = 0;
var total = parseInt(0);
var quantity = parseInt(0);
var serial = '';

var arr = [];


function onUploadControlFileUploadCompleteProfile(s, e) {
    if (e.isValid) {
        //alert(e.callbackData);
        txtImage.SetText(e.callbackData);
        $("#uploadedImage").attr("src",  e.callbackData);
        setElementVisible("uploadedImage", e.isValid);
    }
}
function onImageLoad() {
    var externalDropZone = $("#externalDropZone");
    var uploadedImage = $("#uploadedImage");
    uploadedImage.css({
        left: (externalDropZone.width() - uploadedImage.width()) / 2,
        top: (externalDropZone.height() - uploadedImage.height()) / 2
    });
    setElementVisible("dragZone", false);
}
function setElementVisible(elementId, visible) {
    var el = $("#" + elementId);
    if (visible)
        el.show();
    else
        el.hide();
}

function onUploadControlFileUploadCompleteSignature(s, e) {
    if (e.isValid) {
        //alert(e.callbackData);
        txtSignature.SetText(e.callbackData);
        $("#uploadedImage1").attr("src", e.callbackData);
        setElementVisible("uploadedImage1", e.isValid);
    }
}
function onImageLoad1() {
    var externalDropZone1 = $("#externalDropZone1");
    var uploadedImage1 = $("#uploadedImage1");
    uploadedImage1.css({
        left: (externalDropZone1.width() - uploadedImage1.width()) / 2,
        top: (externalDropZone1.height() - uploadedImage1.height()) / 2
    });
    setElementVisible("dragZone1", false);
}
function setElementVisible(elementId, visible) {
    var el = $("#" + elementId);
    if (visible)
        el.show();
    else
        el.hide();
}



var g_index = 0;


function AddNewRowEdu(s, e) {
    GridItemsEducational.AddNewRow();
}
function AddNewRowExp(s, e) {
    GridItemsExperience.AddNewRow();
}

function SetButtonsVisibility(s) {
    var statusBar = s.GetMainElement().getElementsByClassName("dxgvStatusBar_Moderno")[0].getElementsByTagName("td")[0];

    statusBar.style.visibility = "hidden";
}

function RemoveFile1()
{

    $.ajax({
        type: 'post',
        url: _baseUrl + '/HRM/EmployeeInfo/HRStaffProfile/DeleteFile',
        data: {
            'FileName': txtImage.GetValue()
        },
        success: function (data) {
            if (data == "OK") {                
                $("#uploadedImage").attr("src", "");
                txtImage.SetValue("");
                setElementVisible("uploadedImage",false);
            }
            else {
                AlertMessage(data);
            }
        }

    });
}

function RemoveFile2(s, e) {

    $.ajax({
        type: 'post',
        url: _baseUrl + '/HRM/EmployeeInfo/HRStaffProfile/DeleteFile',
        data: {
            'FileName': txtSignature.GetValue()
        },
        success: function (data) {
            if (data == "OK") {
                $("#uploadedImage1").attr("src", "");
                txtImage.SetValue("");
                setElementVisible("uploadedImage1", false);
            }
            else {
                AlertMessage(data);
            }
        }

    });
}
function initDataEdit(s, e) {
    //district filter
    $.ajax({
        type: 'post',
        url: _baseUrl + '/HRM/EmployeeInfo/HRStaffProfile/FitlerType',
        data: { 'code': cboProvince.GetValue(), 'addType': 1 },
        success: function (data) {
            cboDistrict.PerformCallback();
            cboDistrict.SetValue(_d);
            //commune filter
            $.ajax({
                type: 'post',
                url: _baseUrl + '/HRM/EmployeeInfo/HRStaffProfile/FitlerType',
                data: { 'code': cboDistrict.GetValue(), 'addType': 2 },
                success: function (data) {
                    cboCommune.PerformCallback();
                    cboCommune.SetValue(_c);
                    //village filter
                    $.ajax({
                        type: 'post',
                        url: _baseUrl + '/HRM/EmployeeInfo/HRStaffProfile/FitlerType',
                        data: { 'code': cboCommune.GetValue(), 'addType': 3 },
                        success: function (data) {
                            cboVillage.PerformCallback();
                            cboVillage.SetValue(_v);
                        }

                    });
                }

            });

        }

    });

}

function selectProvince(s, e) {
    $.ajax({
        type: 'post',
        url: _baseUrl + '/HRM/EmployeeInfo/HRStaffProfile/FitlerType',
        data: { 'code': cboProvince.GetValue(), 'addType': 1 },
        success: function (data) {
            cboDistrict.PerformCallback();
            cboCommune.PerformCallback();
            cboVillage.PerformCallback();
        }

    });
}
function selectDistrict(s, e) {
   // cboPostal.SetValue("");
    $.ajax({
        type: 'post',
        url: _baseUrl + '/HRM/EmployeeInfo/HRStaffProfile/FitlerType',
        data: { 'code': s.GetValue(), 'addType': 2 },
        success: function (data) {
            cboCommune.PerformCallback();
            cboVillage.PerformCallback();
        }

    });
}
function selectCommune(s, e) {
   // cboPostal.SetValue("");
    $.ajax({
        type: 'post',
        url: _baseUrl + '/HRM/EmployeeInfo/HRStaffProfile/FitlerType',
        data: { 'code': s.GetValue(), 'addType': 3 },
        success: function (data) {
            cboVillage.PerformCallback();
        }

    });
}
function selectVillage(s, e) {
    
    //txtVillage.SetValue(s.GetValue());
}

function initDataBranch(s, e) {
    ShowDataJob1(s.GetValue(), 'Branch');
}
function initDataDivision(s, e) {
    ShowDataJob1(s.GetValue(), 'Division');
    
}
function initDataGroupDepartment(s, e) {
    ShowDataJob1(s.GetValue(), 'GroupDepartment');
}
function initDataDepartment(s, e) {
    ShowDataJob1(s.GetValue(), 'Department');
}
function initDataPosition(s, e) {
    ShowDataJob1(s.GetValue(), 'Position');
}
function initDataSection(s, e) {
    ShowDataJob1(s.GetValue(), 'Section');
}
function initDataLevel(s, e) {
    ShowDataJob1(s.GetValue(), 'Level');
}
function initDataJobGrade(s, e) {
    ShowDataJob1(s.GetValue(), 'JobGrade');
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
    $.ajax({
        type: 'post',
        url: _baseUrl + '/HRM/EmployeeInfo/HRStaffProfile/FilterLevel',
        data: { 'Level': s.GetValue() },
        success: function (data) {
            txtSalary.SetEnabled(data.IsSalary);
            if (data.IsSalary == true) {
                txtSalary.SetValue(0);
            }
            else {
                txtSalary.SetValue("#####");
            }
        }
    });
}
function selectJobGrade(s, e) {
    ShowDataJob(s.GetValue(), 'JobGrade');
}

function ShowDataJob(Code, Type) {
    $.ajax({
        type: 'post',
        url: _baseUrl + '/HRM/EmployeeInfo/HRStaffProfile/JobType',
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

function ShowDataJob1(Code, Type) {
    $.ajax({
        type: 'post',
        url: _baseUrl + '/HRM/EmployeeInfo/HRStaffProfile/JobType',
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
