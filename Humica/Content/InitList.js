function OnInitX(s, e) {
    AdjustSize();
    ASPxClientUtils.AttachEventToElement(window, "resize", function (evt) {
        AdjustSize();
    });
}
function OnEndCallbackx(s, e) {
    AdjustSize();
}
function AdjustSize() {
    const nodeList = document.documentElement.childNodes;
    var formHeight = nodeList[2].childNodes[1].clientHeight;
    var height = document.documentElement.clientHeight;
    var remainScreen = height - formHeight ;
    if ((remainScreen + formHeight) < height);
    GridViewList.SetHeight(GridViewList.GetHeight() + remainScreen);
   
    
}