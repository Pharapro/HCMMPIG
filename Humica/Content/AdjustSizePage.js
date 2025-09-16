function OnInit(s, e) {
    AdjustSize();
    ASPxClientUtils.AttachEventToElement(window, "resize", function (evt) {
        AdjustSize();
    });
}
function OnEndCallback(s, e) {
    AdjustSize();
}
function AdjustSize() {
    var height = document.documentElement.clientHeight;
    PartialTrainee.SetHeight(height);
}