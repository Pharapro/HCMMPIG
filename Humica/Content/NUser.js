var g_index = 0;

var qty = 0;
var total = parseInt(0);
var quantity = parseInt(0);
var serial = '';

var arr = [];


function onUploadControlFileUploadCompleteUser(s, e) {
    if (e.isValid) {
        txtImage.SetText(e.callbackData);
        $("#uploadedImage").attr("src", _baseUrl+"/Content/UploadFolder/" + e.callbackData);
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
