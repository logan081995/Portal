let drawerContentInstance = null;

window.initDrawerContentReference = function (instance) {
    drawerContentInstance = instance;
}

window.initializeDrawer = function (elementId) {
    $('#' + elementId).dxDrawer({
        opened: false,
        position: 'right',
        shading: true,
        revealMode: 'slide',
        openedStateMode: 'shrink',
        closeOnOutsideClick: true,
        template: function (e) {
            var drawerContentElement = $('#drawer-content').removeClass("d-none");
            $("#" + elementId).append(drawerContentElement);

            return drawerContentElement;
        },
        onOptionChanged: function (e) {
            let duration = e.value === true ? 0 : 400;
            setTimeout(() => {
                $('#' + elementId).toggleClass("invisible");
            }, duration);
        }
    })
}

window.toggleDrawer = function (elementId) {
    var drawerInstance = $('#' + elementId).dxDrawer('instance');
    drawerInstance.toggle();
    drawerContentInstance.invokeMethodAsync("HandleLoadCart");
}