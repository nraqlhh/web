$(document).ready(function () {
    var registrationSuccess = $("#registrationSuccess").val();
    if (registrationSuccess === "true") {
        alert("Registration successful!");
    }
});


$(document).ready(function () {
    $('.password-eye').click(function () {
        $(this).toggleClass('fa-eye'); // Toggle between eye and eye-slash icons
        $(this).prev().attr('type', function (i, type) {
            return type === 'password' ? 'text' : 'password';
        });
    });
});
