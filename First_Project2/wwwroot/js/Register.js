//////Hide & Show Password////////////
$(document).ready(function () {
    $('#open').click(function () {
        $('#open').hide();
        $('#close').show();
        $('#password').attr("type", "text");
    });
    $('#close').click(function () {
        $('#open').show();
        $('#close').hide();
        $('#password').attr("type", "password");
    });

    $('#open1').click(function () {
        $('#open1').hide();
        $('#close1').show();
        $('#password1').attr("type", "text");

    });
    $('#close1').click(function () {
        $('#open1').show();
        $('#close1').hide();
        $('#password1').attr("type", "password");
    });

});

//////Form validation////////////

const form = document.getElementById('form');
const input = document.getElementsByClassName('form-control');
const fname = document.getElementById('fname');
const lname = document.getElementById('lname');
const phone = document.getElementById('phone');
const username = document.getElementById('username');
const email = document.getElementById('email');
const password = document.getElementById('password');
const password1 = document.getElementById('password1');
const errorElement = document.getElementsByClassName('error');



const setError = (element, messages) => {
    const inputControl = element.parentElement;
    const errorDisplay = inputControl.querySelector('.error');

    errorDisplay.innerText = messages;
    inputControl.classList.add('error');
    inputControl.classList.remove('success')
};

setError.onchange = function () {
    inputControl.classList.remove('error');
};

const setSuccess = element => {
    const inputControl = element.parentElement;
    const errorDisplay = inputControl.querySelector('.error');

    errorDisplay.innerText = '';
    inputControl.classList.add('success');
    inputControl.classList.remove('error');

};

const isValidEmail = email => {
    const re = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(String(email).toLowerCase());
};

const isValidPhone = Phone => {
    const ph = /^[0-9]{10}$/;
    return ph.test((Phone));
};


form.addEventListener('submit', e => {

    let messages = []

    if (fname.value === '' || fname.value == null ) {
        setError(fname, 'fname is required');
        messages.push('Name is required');
    } else {
        setSuccess(fname);
    }

    if (lname.value === '' || lname.value == null) {
        setError(lname, 'lname is required');
        messages.push('Name is required');
    } else {
        setSuccess(lname);
    }

    if (username.value === '' || username.value == null ) {
        setError(username, 'Username is required');
        messages.push('Username is required');
    } else {
        setSuccess(username);
    }

    if (email.value === '') {
        setError(email, 'Email is required');
        messages.push('Email is required');
    } else if (!isValidEmail(email.value)) {
        setError(email, 'Provide a valid email address');
        messages.push('Provide a valid email address');
    } else {
        setSuccess(email);
    }

    if (password.value === '') {
        setError(password, 'Password is required');
        messages.push('Password is required');
    } else if (password.value.length < 8) {
        setError(password, 'Password must be at least 8 character');
        messages.push('Password must be at least 8 character');
    } else {
        setSuccess(password);
    }

    if (password1.value === '' ) {
        setError(password1, 'Please confirm your password');
        messages.push('Please confirm your password');
    } else if (password1.value !== password.value) {
        setError(password1, "Passwords doesn't match");
        messages.push("Passwords doesn't match");
    } else {
        setSuccess(password1);
    }

    if (phone.value === '') {
        setError(phone, 'Phone Number is required');
        messages.push('Phone Number is required');
    } else if (!isValidPhone(phone.value)) {
        setError(phone, 'Provide a valid Phone Number');
        messages.push('Provide a valid Phone Number');
    } else {
        setSuccess(phone);
    }


    if (messages.length > 0) {
        e.preventDefault()
        errorElement.innerText = messages.join(', ')
    }
});