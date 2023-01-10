

function showToastOk(mensaje){
    $.toast({
        heading: 'Success',
        text: mensaje,
        showHideTransition: 'slide',
        icon: 'success'
    })
}

function showToastError(mensaje) {
    $.toast({
        heading: 'Error',
        text: mensaje,
        showHideTransition: 'fade',
        icon: 'error'
    })
}

function showToastAlert(mensaje) {
    $.toast({
        heading: 'Warning',
        text: mensaje,
        showHideTransition: 'plain',
        icon: 'warning'
    })
}