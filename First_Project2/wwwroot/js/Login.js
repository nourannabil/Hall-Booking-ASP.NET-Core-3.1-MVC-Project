
let myele = document.querySelector(".error");
let myele1 = document.querySelector(".error1");
let errorMsg = document.querySelector(".error-msg");


myele.onclick = function () {
    errorMsg.style.display = "none";
}

myele1.onclick = function () {
    errorMsg.style.display = "none";
}


///////////////////////////////////////////////////////////////////////

$(document).ready(function () {
    $('#open').click(function () {
        $('#open').hide();
        $('#close').show();
        $('#password').attr("type", "text");
        $('#password1').attr("type", "text");

    });
    $('#close').click(function () {
        $('#open').show();
        $('#close').hide();
        $('#password').attr("type", "password");
        $('#password1').attr("type", "password");

    });
});

