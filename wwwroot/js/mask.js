window.addIMask = function (elementId, mask) {
    var element = document.getElementById(elementId);
    var maskObject = IMask(element, {
        mask: mask
    });
}