
function findGetParameter(parameterName) {
    var result = null,
        tmp = [];
    location.search
    .substr(1)
        .split("&")
        .forEach(function (item) {
            tmp = item.split("=");
            if (tmp[0] === parameterName) result = decodeURIComponent(tmp[1]);
        });
    return result;
}
function _Hide() {
    LeftPane.SetVisible(false);
   
    $("#_lefHide").hide();
    $("#_leftShow").show();

    $.ajax({
        type: 'post',
        url: _baseUrl + '/Ajax/menuSession',
        data: {

            'MenuEnable': 0,
        },
        success: function (data) {

        }
    });

}

function SaveHeigh(action) {
    
    var win = $(window).height();
    var title = $('#contentActionTitle').height();
    var form = $('.dxflFormLayout_DevEx').height();
    if (title == null || title == undefined)
    {
        title = 0;
    }

    if (form == null || form == undefined) {
        form = 0;
    }
    //alert(win + ":" + title + ":" + form);
    var contentHight = win - form - title;
    if (action == "Index")
    {
        contentHight = contentHight - 360;
    } 
    

    //alert(contentHight);

    $.ajax({
        type: 'post',
        url: _baseUrl + '/Ajax/SaveHeigh',
        data: { 'heigh': contentHight },
        success: function (data) {

            
        }

    });
}


function _Show() {
    LeftPane.SetVisible(true);
    $("#_lefHide").show();
    $("#_leftShow").hide();

    $.ajax({
        type: 'post',
        url: _baseUrl + '/Ajax/menuSession',
        data: {
            'MenuEnable': 1,
        },
        success: function (data) {


        }
    });
}

function UpdateNotification(s, e) {

    $.ajax({
        type: "POST",
        url: _baseUrl + '/Home/updateNotification',
        data: { notifyId: e.item.name },
        beforeSend: function () {
            //loadingPanel.Show();
        },
        success: function (response) {
           
        }
    });

}

function TabClick(s, e) {
    //var tab = GetTabByValue();
    //alert(e.tab.name);
    //$("#cbTabActive").prop("checked", e.tab.name == tab.name);
    //ShowTabContent(e.tab);
    $.ajax({
        type: "POST",
        url: _baseUrl + '/Home/CallbacksPartialMenu',
        data: { selectTap: e.item.name },
        beforeSend: function () {
            //loadingPanel.Show();
        },
        success: function (response) {
            $("#MenuTopR").html(response);
            //loadingPanel.Hide();

            $.ajax({
                type: "POST",
                url: _baseUrl + '/Home/CallbacksPartialMenuLeftAuto',
                data: { selectTap: e.item.name },
                beforeSend: function () {
                    //loadingPanel.Show();
                },
                success: function (response) {
                    $("#MenuLeftR").html(response);
                    //loadingPanel.Hide();
                }
            });
        }
    });

}


function TopMenuClick(s, e) {
    //var tab = GetTabByValue();
    //alert(e.tab.name);
    //$("#cbTabActive").prop("checked", e.tab.name == tab.name);
    //ShowTabContent(e.tab);    
    $.ajax({
        type: "POST",
        url: _baseUrl + '/Home/LeftPanelPartial',
        data: { selectTap: e.item.name },
        beforeSend: function () {
            //loadingPanel.Show();
        },
        success: function (response) {
            $("#menuLeftId").html(response);
            
        }
    });

}

function MenuLeftItemClick(s,e)
{
    //$.ajax({
    //    type: "GET",
    //    url: _baseUrl + '/Logistic/DP/NDeliveryPlan',
    //    data: { selectItem: e.item.name },
    //    beforeSend: function () {
    //        //loadingPanel.Show();
    //    },
    //    success: function (response) {
    //        $("#contentPane").html(response);
    //        //loadingPanel.Hide();
    //    },
    //    error: function (XMLHttpRequest, textStatus, errorThrown) {
    //        alert(XMLHttpRequest.responseText);
    //    }

    //});
    
}
function MenuTopItemClick(s, e) {
    $.ajax({
        type: "POST",
        url: _baseUrl + '/Home/CallbacksPartialMenuLeft',
        data: { selectItem: e.item.name },
        beforeSend: function () {
            //loadingPanel.Show();
        },
        success: function (response) {
            
            $("#MenuLeftR").html(response);
            //loadingPanel.Hide();            
            LeftPane.SetVisible(true); $('#_lefHide').show();
            $('#_leftShow').hide();
        }
    });
}
function _btnHideMsg(s,e) {
    $('#_messageAlert').remove();
}


function ActionItemClick(s, e) {
    _clickMenuAction(s, e);
}

function ReportItemClick(s, e) {
    _clickMenuReport(s, e);
}

function confirmQuestion(s, e)
{
    _clickConfirm(s, e);
}

function GoToWaitingList(s,e)
{
    _clickWatingList(s, e);
}

function ConfirmMessage(str)
{
    var img = "<img src='"+_baseUrl+"/Content/Images/question-mark.png'/>";
    var content = "<div class='msg_confirm'> " + img + "<div class='msg'>" + str + "</div>";
    $("#PopTheQuestion_PWC-1").html(content);
    PopTheQuestion.Show();        
}

function AlertMessage(str)
{
    var img = "<img src='" + _baseUrl + "/Content/Images/alert-mark.png'/>";
    var content = "<div class='msg_confirm'> " + img + "<div class='msg'>" + str + "</div>";
    $("#PopTheMessage_PWC-1").html(content);
    PopTheMessage.Show();
}

function AlertLoading() {
    var img = "<img src='" + _baseUrl + "/Content/img/waiting.gif'/>";
    var content = "<div class='msg_confirm'> " + img ;
    $("#PopTheLoading_PWC-1").html(content);
    PopTheLoading.Show();
}

function checkingFavorite(s,e)
{
    var mainElement = s.GetMainElement();    
    var screenid = $('#linkFavorite').attr("screenid");    
    $.ajax({
        type: "POST",
        url: _baseUrl + '/Home/checkingFavorite',
        data: { screenid: screenid },
        beforeSend: function () {
            //loadingPanel.Show();
        },
        success: function (response) {
            if(response=="ADD")
            {
                $('#linkFavorite').find("img").attr("src", _baseUrl + "/Content/images/star_f.png");
            }else if(response=="DEL")
            {
                $('#linkFavorite').find("img").attr("src", _baseUrl + "/Content/images/star_p.png");
            }
        }
    });
}
function renderMainIframe(s,e)
{
    //alert(e.node.title);
    var node = e.node.name;
    
    if (e.node.nodes.length === 0) { 
    $.ajax({
        type: "POST",
        url: _baseUrl + '/Home/loadingIframe',
        data: { id: node },
        beforeSend: function () {
            //loadingPanel.Show();
            PopTheLoading.Show();
        },
        success: function (data) {
            if(data.MS=="OK")
            {
                $('#_main_loading').attr('src', data.URL);
                window.history.pushState("", "", _baseUrl + "?Screen=" + data.SCREEN);
                document.title = data.TITLE;
                $.ajax({
                    type: "POST",
                    url: _baseUrl + '/Home/loadRight',
                    data: { id: node },
                    beforeSend: function () {
                        //PopTheLoading.Show();
                    },
                    success: function (response) {
                        $('#_loading_right').html(response);
                    }
                });
            } else if (data.MS == "R"){
                location.reload();
            }
        }
    });
    }
}

function getUrlVars() {
    var vars = {};
    var parts = window.location.href.replace(/[?&]+([^=&]+)=([^&]*)/gi, function (m, key, value) {
        vars[key] = value;
    });
    return vars;
}
function checkingFavoriteList(s,e)
{    
    var screenid = $('#linkFavoriteList').attr("screenid");
    
    var colCnt = GridViewList.GetColumnsCount();
    
    
    var columnData = GetColumnInfo(GridViewList);

    //do somethidg here		
    var list_attr = JSON.stringify(columnData, null)
    //alert(list_attr);
    $.ajax({
        type: "POST",
        url: _baseUrl + '/Home/checkingFavoriteList',
        data: { screenid: screenid,json:list_attr },
        beforeSend: function () {
            //loadingPanel.Show();
        },
        success: function (response) {
            if (response == "ADD") {
                $('#linkFavoriteList').find("img").attr("src", _baseUrl + "/Content/images/star_list_f.png");
            } else if (response == "DEL") {
                $('#linkFavoriteList').find("img").attr("src", _baseUrl + "/Content/images/star_list_p.png");
            }
        }
    });

}


function saveFavoriteList(screenid, GridViewList,url) {
    

    var colCnt = GridViewList.GetColumnsCount();


    var columnData = GetColumnInfo(GridViewList);

    //do somethidg here		
    var list_attr = JSON.stringify(columnData, null)
    //alert(list_attr);
    $.ajax({
        type: "POST",
        url: _baseUrl + '/Home/saveFavoriteListExport',
        data: { screenid: screenid, json: list_attr },
        beforeSend: function () {
            //loadingPanel.Show();
        },
        success: function (response) {
            window.location = url;
        }
    });

}

function InitPopupMenu(s, e) {
    $(".dxtv-subnd").bind("contextmenu", OnGridContextMenu);
}
function OnGridContextMenu(evt) {
    PopupMenu.ShowAtPos(evt.pageX, evt.pageY);
    evt.preventDefault();
}
function OnPopupMenuItemClick(s, e) {
    window.open(_baseUrl, '_blank');
}

function GetColumnInfo(grid) {
    var columnData = new Array();
    var prop = grid.cpColumnsProp;
    for (var name in prop) {
        var column = grid.GetColumnByField(name);
        columnData.push({
            fieldName: prop[name].fieldName,
            caption: prop[name].caption,
            sotrIndex: prop[name].sortIndex,
            sortOrder: prop[name].sortOrder,
            groupIndex: prop[name].groupIndex,
            width: prop[name].width,
            visibleIndex: prop[name].visibleIndex,
            visible: prop[name].visible,
            fitler: prop[name].filter,
        });
    }
    return columnData;
}

function completedUploadFile(s,e)
{
    GridViewListUpload.Refresh();
    gridItemEmpInfor.Refresh();
}

function completedUploadFileItem(s,e)
{
    if (e.isValid) {        
        $("#file_uploaded").attr("href", e.callbackData);
        file_uploaded.SetVisible(true);
    }
}
function completedUploadFileItem1(s, e)
{
    if (e.isValid) {
        $("#file_uploaded1").attr("href", e.callbackData);
        file_uploaded1.SetVisible(true);
    }
}
function deleteFileUploated(s, e)
{

    var c = confirm("Are you sure to delete this file?");
    if (c)
    {
        var mainElement = s.GetMainElement();
        var id = mainElement.title;
        $.ajax({
            type: "POST",
            url: _baseUrl + '/LoadingList/deleteFileUploaded',
            data: { id: id },
            beforeSend: function () {

            },
            success: function (response) {
                GridViewListUpload.Refresh();
                gridItemEmpInfor.Refresh();
            }
        });
    }
   
}



function tabClickCommand(s, e) {
    var index = s.GetActiveTab().index;
    if (index == 0) {
        $("#pageToolbar li").show();

    } else {
        $("#pageToolbar li").hide();
    }
    
}


var _unsaved = false;


function unloadPage() {
    if (_unsaved) {
        return "You have unsaved changes on this page. Do you want to leave this page and discard your changes or stay on this page?";
    }
}

function colorChange(s,e)
{
    var color = s.GetValue().replace('#', '');    
    window.location = _baseUrl + '/home/changeColor?code=' + color + '&StandingUrl=current';
}
function CheckUnSavedChanges(form) {
    for (var i = 0; i < form.elements.length; i++) {
        var element = form.elements[i];
        var type = element.type;
        if (type == "checkbox" || type == "radio") {
            if (element.checked != element.defaultChecked) {
                return true;
            }
        }
        else if (type == "text" || type == "textarea") {
            if (element.value != element.defaultValue) {
                return true;
            }
        }
        else if (type == "select-one" || type == "select-multiple") {
            for (var j = 0; j < element.options.length; j++) {
                if (element.options[j].selected != element.options[j].defaultSelected) {
                    return true;
                }
            }
        }
    }
    return false;
} 
function formatDate(date) {
    var d = new Date(date),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();

    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;

    return [year, month, day].join('-');
}