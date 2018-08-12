// @ts-check

$('#btn').on('click', function () {

    $.getJSON('http://localhost:16977/clients/1', function (data) {
        console.log(data);
    });

});