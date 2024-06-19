window.collapseSidebar = function (elementId) {
    $('#' + elementId).toggleClass('close');
}

window.toggleSidebar = function (elementId) {
    $('#' + elementId).toggleClass('h-100');
}

$(document).ready(function () {
    $(document).on('click', '.arrow', function () {
        let arrowParent = $(this).parent().parent();
        arrowParent.toggleClass("showMenu");
    });
    
    $(document).on('click', '#sidebar .nav-links li a', function () {
        window.toggleSidebar("sidebar");
    });
});
