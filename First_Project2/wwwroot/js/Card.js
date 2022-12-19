const form = document.getElementById('form');
const input = document.getElementsByClassName('form-control');
const CardNumber = document.getElementById('CardNumber');
const Cvv = document.getElementById('Cvv');
const ExpiryDate = document.getElementById('ExpiryDate');
const NameOnCard = document.getElementById('NameOnCard');
const Balance = document.getElementById('Balance');
const errorElement = document.getElementsByClassName('error');


const setError = (element, message) => {
    const inputControl = element.parentElement;
    const errorDisplay = inputControl.querySelector('.error');

    errorDisplay.innerText = message;
    inputControl.classList.add('error');
    inputControl.classList.remove('success')
}

const setSuccess = element => {
    const inputControl = element.parentElement;
    const errorDisplay = inputControl.querySelector('.error');

    errorDisplay.innerText = '';
    inputControl.classList.add('success');
    inputControl.classList.remove('error');
};

const isValidCardNumber = CardNumber => {
    const CN = /^[0-9]{16}$/;
    return CN.test((CardNumber));
};

const isValidCvv = Cvv => {
    const CV = /^[0-9]{3}$/;
    return CV.test((Cvv));
};

form.addEventListener('submit', e => {

    let messages = []

    if (NameOnCard.value === '') {
        setError(NameOnCard, 'Username is required');
        messages.push('Username is required');
    } else {
        setSuccess(NameOnCard);
    }

    if (Balance.value === null) {
        setError(Balance, 'Balance is required');
        messages.push('Balance is required');
    } else if (Balance.value < 200) {
        setError(Balance, 'Balance must be at least 200 $ ');
        messages.push('Balance must be at least 200 $ ');
    } else {
        setSuccess(Balance);
    }

    if (ExpiryDate.value === '' || ExpiryDate.value == null) {
        setError(ExpiryDate, 'Expiry Date is required');
        messages.push('Expiry Date is required');
    } else {
        setSuccess(ExpiryDate);
    }

    if (CardNumber.value === '') {
        setError(CardNumber, 'Card Number is required');
        messages.push('Card Number is required');
    } else if (!isValidCardNumber(CardNumber.value)) {
        setError(CardNumber, 'Provide a valid Card Number Must be 16 digit');
        messages.push('Provide a valid Card Number Must be 16 digit');
    } else {
        setSuccess(CardNumber);
    }

    if (Cvv.value === '') {
        setError(Cvv, 'CVV is required');
        messages.push('CVV is required');
    } else if (!isValidCvv(Cvv.value)) {
        setError(Cvv, 'CVV must be 3 Number ');
        messages.push('CVV must be 3 Number ');
    } else {
        setSuccess(Cvv);
    }

    if (messages.length > 0) {
        e.preventDefault()
        errorElement.innerText = messages.join(', ')
    }
} );
