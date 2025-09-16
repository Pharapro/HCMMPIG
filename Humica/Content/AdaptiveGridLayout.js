var currentBreakpoint = "";
var previousDivWidth = 0;
var isContainerResizing = false;
ResizingDirection = {
    horizontal: "horizontal",
    vertical: "vertical",
    both: "both"
};
var ResizableHelper = ASPx.CreateClass(null, {
    constructor: function () {
    },
    MakeDivResizable: function (parentElementID, direction, sizeRestriction) {
        this.GetHelper(direction).MakeResizable(parentElementID, sizeRestriction);
    },
    GetHelper: function (direction) {
        switch (direction) {
            case ResizingDirection.both:
                return new ResizableBothHelper();
            case ResizingDirection.horizontal:
                return new ResizableHorizontalHelper();
            case ResizingDirection.vertical:
                return new ResizableVerticalHelper();
        }
    }
});
var ResizableBaseHelper = ASPx.CreateClass(null, {
    constructor: function () {
        this.state = {
            startX: -1,
            startY: -1,
            startWidth: -1,
            startHeight: -1,
            resizableContainer: null,
            minHeight: -1,
            maxHeight: -1,
            minWidth: -1,
            maxWidth: -1
        };
        this.resizerElement = null;
        this.doResizeFunction = null;
        this.stopResizeFunction = null;
    },
    MakeResizable: function (parentElementID, sizeRestriction) {
        var parentElement = document.getElementById(parentElementID);
        this.state.resizableContainer = parentElement;
        this.state.minHeight = sizeRestriction.minHeight;
        this.state.maxHeight = sizeRestriction.maxHeight;
        this.state.minWidth = sizeRestriction.minWidth;
        this.state.maxWidth = sizeRestriction.maxWidth;
        this.resizerElement = document.createElement('div');
        this.calculatePseudoElementOffset();
        this.applyResizingDivClasses();
        this.applyParentClasses();
        parentElement.appendChild(this.resizerElement);
        this.resizerElement.addEventListener(ASPx.TouchUIHelper.touchMouseDownEventName, this.startResize.aspxBind(this));
    },
    calculatePseudoElementOffset: function () { },
    applyResizingDivClasses: function () {
        this.resizerElement.classList.add("ResizingDivBase");
    },
    applyParentClasses: function () {
        this.state.resizableContainer.classList.add("ResizableContainerBase");
    },
    startResize: function (e) {
        e.preventDefault();
        var isTouchEvent = ASPx.TouchUIHelper.isTouchEvent(e);
        this.updateState(e, isTouchEvent);
        this.doResizeFunction = this.doResize.aspxBind(this);
        this.stopResizeFunction = this.stopResize.aspxBind(this);
        document.documentElement.addEventListener(ASPx.TouchUIHelper.touchMouseMoveEventName, this.doResizeFunction);
        document.documentElement.addEventListener(ASPx.TouchUIHelper.touchMouseUpEventName, this.stopResizeFunction);
        isContainerResizing = true;
    },
    updateState: function (e, isTouchEvent) {
        this.state.startX = isTouchEvent ? e.touches[0].clientX : e.clientX;
        this.state.startY = isTouchEvent ? e.touches[0].clientY : e.clientY;
        this.state.startWidth = this.getStartWidth();
        this.state.startHeight = this.getStartHeight();
    },
    getStartWidth: function () {
        var widthString = this.state.resizableContainer.style.width;
        var result = parseInt(widthString);
        if (ASPx.IsPercentageSize(widthString) !== false)
            result = this.state.resizableContainer.clientWidth;
        return result;
    },
    getStartHeight: function () {
        var heightString = this.state.resizableContainer.style.height;
        var result = parseInt(heightString);
        if (ASPx.IsPercentageSize(heightString) !== false)
            result = this.state.resizableContainer.clientHeight;
        return result;
    },
    doResize: function (e) { },
    doHorizontalResize: function (e, isTouchEvent) {
        var clientXOffset = isTouchEvent ? e.touches[0].clientX : e.clientX;
        var newWidth = this.state.startWidth + clientXOffset - this.state.startX;
        this.state.resizableContainer.style.width = this.getResizableContainerWidth(newWidth) + 'px';
        previousDivWidth = this.getResizableContainerWidth(newWidth);
        onResizing(this.getResizableContainerWidth(newWidth));
        this.calculatePseudoElementOffset();
    },
    getResizableContainerWidth: function (newWidth) {
        if (this.state.minWidth > newWidth)
            return this.state.minWidth;
        else if (newWidth > this.state.maxWidth)
            return this.state.maxWidth;
        else
            return newWidth;
    },
    doVerticalResize: function (e, isTouchEvent) {
        var clientYOffset = isTouchEvent ? e.touches[0].clientY : e.clientY;
        var newHeight = this.state.startHeight + clientYOffset - this.state.startY;
        this.state.resizableContainer.style.height = this.getResizableContainerHeight(newHeight) + 'px';

    },
    getResizableContainerHeight: function (newHeight) {
        if (this.state.minHeight > newHeight)
            return this.state.minHeight;
        else if (newHeight > this.state.maxHeight)
            return this.state.maxHeight;
        else
            return newHeight;
    },
    stopResize: function (e) {
        document.documentElement.removeEventListener(ASPx.TouchUIHelper.touchMouseMoveEventName, this.doResizeFunction);
        document.documentElement.removeEventListener(ASPx.TouchUIHelper.touchMouseUpEventName, this.stopResizeFunction);
        isContainerResizing = false;
    }
});
var ResizableBothHelper = ASPx.CreateClass(ResizableBaseHelper, {
    constructor: function () {
        this.constructor.prototype.constructor.call(this);
    },
    applyResizingDivClasses: function () {
        ResizableBaseHelper.prototype.applyResizingDivClasses.call(this);
        this.resizerElement.classList.add("ResizingDivBoth");
    },
    applyParentClasses: function () {
        ResizableBaseHelper.prototype.applyParentClasses.call(this);
        this.state.resizableContainer.classList.add("ResizableContainerBoth");
    },
    doResize: function (e) {
        var isTouchEvent = ASPx.TouchUIHelper.isTouchEvent(e);
        this.doHorizontalResize(e, isTouchEvent);
        this.doVerticalResize(e, isTouchEvent);
    }
});
var ResizableHorizontalHelper = ASPx.CreateClass(ResizableBaseHelper, {
    constructor: function () {
        this.constructor.prototype.constructor.call(this);
    },
    calculatePseudoElementOffset: function () {
        var dynamicCSSRule = [];
        dynamicCSSRule.push("#" + this.state.resizableContainer.id + "> .ResizingDivHorizontal::after {");
        var topOffset = this.getStartHeight() / 2 - 16;
        dynamicCSSRule.push("top: " + topOffset + "px;");
        dynamicCSSRule.push("}");
        styleSection.innerHTML = dynamicCSSRule.join("\n")
    },
    applyResizingDivClasses: function () {
        ResizableBaseHelper.prototype.applyResizingDivClasses.call(this);
        this.resizerElement.classList.add("ResizingDivHorizontal");
    },
    applyParentClasses: function () {
        ResizableBaseHelper.prototype.applyParentClasses.call(this);
        this.state.resizableContainer.classList.add("ResizableContainerHorizontal");
    },
    doResize: function (e) {
        this.doHorizontalResize(e, ASPx.TouchUIHelper.isTouchEvent(e));
    }
});
var ResizableVerticalHelper = ASPx.CreateClass(ResizableBaseHelper, {
    constructor: function () {
        this.constructor.prototype.constructor.call(this);
    },
    calculatePseudoElementOffset: function () {
        var dynamicCSSRule = [];
        dynamicCSSRule.push("#" + this.state.resizableContainer.id + "> .ResizingDivVertical::after {");
        var leftOffset = this.getStartWidth() / 2 - 16;
        dynamicCSSRule.push("left: " + leftOffset + "px;");
        dynamicCSSRule.push("}");
        styleSection.innerHTML = dynamicCSSRule.join("\n")
    },
    applyResizingDivClasses: function () {
        ResizableBaseHelper.prototype.applyResizingDivClasses.call(this);
        this.resizerElement.classList.add("ResizingDivVertical");
    },
    applyParentClasses: function () {
        ResizableBaseHelper.prototype.applyParentClasses.call(this);
        this.state.resizableContainer.classList.add("ResizableContainerVertical");
    },
    doResize: function (e) {
        this.doVerticalResize(e, ASPx.TouchUIHelper.isTouchEvent(e));
    }
});
window.helper = new ResizableHelper();
function OnControlsInitialized(s, e) {
    var sizeRestriction = { minWidth: 300, maxWidth: 896, minHeight: 0, maxHeight: 0 };
    helper.MakeDivResizable("ResizedDiv", ResizingDirection.horizontal, sizeRestriction);
    onResizing();
    ASPxClientUtils.AttachEventToElement(window, "resize", function (evt) {
        var resizedDiv = document.getElementById("ResizedDiv");
        var demoContainer = document.getElementById("DemoArea");
        if (resizedDiv.style.width != "100%") {
            if (resizedDiv.clientWidth >= demoContainer.clientWidth)
                resizedDiv.style.width = "100%";
        }
        else {
            if (previousDivWidth != 0)
                if (demoContainer.clientWidth >= previousDivWidth)
                    resizedDiv.style.width = previousDivWidth + "px";
        }
        onResizing();
    });
    ASPxClientHint.Register('[data-spanrules]', {
        onShowing: function (s, e) {
            if (isContainerResizing)
                e.cancel = true;
            var spanRules = JSON.parse(e.targetElement.getAttribute("data-spanrules"));
            var currentItemColumnSpan = 0;
            var currentItemRowSpan = 0;
            if (currentBreakpoint != "default") {
                currentItemColumnSpan = spanRules[currentBreakpoint][0];
                currentItemRowSpan = spanRules[currentBreakpoint][1];
            }
            else {
                currentItemColumnSpan = JSON.parse(e.targetElement.getAttribute("data-colspan")) ? JSON.parse(e.targetElement.getAttribute("data-colspan")) : 1;
                currentItemRowSpan = JSON.parse(e.targetElement.getAttribute("data-rowspan")) ? JSON.parse(e.targetElement.getAttribute("data-rowspan")) : 1;
            }
            if (e.targetElement.firstElementChild.firstElementChild.classList.contains("lastItem") &&
                currentBreakpoint == "default")
                currentItemColumnSpan = "Stretched";
            return "ColumnSpan: " + currentItemColumnSpan + "<br/> RowSpan: " + currentItemRowSpan;
        },
        position: 'bottom',
        triggerAction: 'hover',
        appearAfter: 800
    });
}
function onResizing(formLayoutWidth) {
    var formLayoutWidth = formLayoutWidth || FormLayout.GetWidth();
    if (formLayoutWidth <= 500) {
        CurrentWidthLabel.SetText(formLayoutWidth);
        CurrentBreakpointLabel.SetText("S");
        CurrentColCountLabel.SetText("1");
        currentBreakpoint = "s";
    }
    else if ((500 < formLayoutWidth) && (formLayoutWidth <= 800)) {
        CurrentWidthLabel.SetText(formLayoutWidth);
        CurrentBreakpointLabel.SetText("M");
        CurrentColCountLabel.SetText("2");
        currentBreakpoint = "m";
    }
    else {
        CurrentWidthLabel.SetText(formLayoutWidth);
        CurrentBreakpointLabel.SetText("Default");
        CurrentColCountLabel.SetText("3");
        currentBreakpoint = "default";
    }
    if (formLayoutWidth >= 660)
        CurrentWrapLabel.SetText("not wrapped");
    else
        CurrentWrapLabel.SetText("wrapped");
}

MVCxClientGlobalEvents.AddControlsInitializedEventHandler(OnControlsInitialized);