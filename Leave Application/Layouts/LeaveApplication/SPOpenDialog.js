function openDialog(url) {
    var options = SP.UI.$create_DialogOptions();
    options.url = url;
    options.height = 500;
    options.dialogReturnValueCallback = function () {
        window.location.href = window.location.href;
    };
    SP.UI.ModalDialog.showModalDialog(options);
}