// range-slider.js
let filterInstance = null;

window.initFilterReference = function (instance) {
    filterInstance = instance;
}
function initializeRangeSlider(elementId, min, max, start, end, step, isToolTipEnabled) {
    $("#" + elementId).dxRangeSlider({
        min: min,
        max: max,
        start: start,
        end: end,
        step: step,
        tooltip: {
            enabled: isToolTipEnabled
        },
        valueChangeMode: 'onHandleRelease',
        onValueChanged: function (info) {
            filterInstance.invokeMethodAsync("UpdateSpinEditValue", info.start, info.end);
        }
    });
}
