//function selectAssetClass(s, e) {

//    $.ajax({
//        type: 'post',
//        url: _baseUrl + '/Asset/HRAssetRegister/FitlerType',
//        data: { 'code': s.GetValue() },
//        success: function (data) {
//            txtAssetCode.SetValue("");
//            if (data.ASSET_NUMBER == "INTERNAL") {
//                txtAssetCode.SetEnabled(false);
//                txtAssetCode.SetValue(data.ASSET_NUMBER);
//            } else {
//                txtAssetCode.SetEnabled(true);
//                txtAssetCode.SetFocus();
//            }
//        }

//    });
//}


//function CalculateEndDate(s, e) {

//    $.ajax({
//        type: 'post',
//        url: _baseUrl + "/Asset/HRAssetRegister/CalculateEndDate",
//        data: { 'DeprDate': dtpDepreFromDate.GetValue().toDateString(), 'Year': txtUsefulLifeYear.GetValue(), 'Action': 'Create' },
//        success: function (data) {
//            if (data == "OK") {
//                gridItemsFABookBalance.Refresh();
//                //gridItemsHistory.Refresh();
//            }
//            else {
//                AlertMessage(data);
//            }

//        },
//        error: function (XMLHttpRequest, textStatus, errorThrown) {
//            AlertMessage(XMLHttpRequest.responseText);
//        }
//    });


//}

//function ChangeCost(s, e) {


//    $.ajax({
//        type: 'post',
//        url: _baseUrl + "/Asset/HRAssetRegister/ChangeCost",
//        data: { 'Cost': s.GetValue(), 'Action': 'Create' },
//        success: function (data) {
//            if (data == "OK") {
//                gridItemsFABookBalance.Refresh();
//                //gridItemsHistory.Refresh();
//            }
//            else {
//                AlertMessage(data);
//            }

//        },
//        error: function (XMLHttpRequest, textStatus, errorThrown) {
//            AlertMessage(XMLHttpRequest.responseText);
//        }
//    });


//}

//function SelectChangeCbo(s, e) {

//    if (s.GetSelectedItem() != null) {

//        chkPosting.SetChecked(s.GetSelectedItem().GetColumnText(2));
//        txtMidPeriodType.SetValue(s.GetSelectedItem().GetColumnText(3));
//        txtMidPeriodDay.SetValue(s.GetSelectedItem().GetColumnText(4));
//    }

//}

//function SelectChangeCboMethod(s, e) {

//    if (s.GetSelectedItem() != null) {

//        cboAverageConvertion.SetValue(s.GetSelectedItem().GetColumnText(2));
//    }

//}


//function cboBookHisChange(s, e) {

//    $.ajax({
//        type: 'post',
//        url: _baseUrl + "/Asset/HRAssetRegister/getAssetDeprHistory",
//        data: { 'Branch': cboBranchID.GetValue(), 'AssetCode': txtAssetCode.GetValue(), 'Book': cboBookHis.GetValue(), 'Year': cboYearHis.GetValue(), 'Action': 'Edit' },
//        success: function (data) {

//            if (data == "OK") {
//                GridItemsHistory.Refresh();
//                //gridItemsHistory.Refresh();
//            }
//            else {
//                AlertMessage(data);
//            }

//        },
//        error: function (XMLHttpRequest, textStatus, errorThrown) {
//            AlertMessage(XMLHttpRequest.responseText);
//        }
//    });


//}
