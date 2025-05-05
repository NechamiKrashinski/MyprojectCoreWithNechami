function loginUser() {
    const loginIdTextbox = document.getElementById('login-id');

    const loginData = {
        id: parseInt(loginIdTextbox.value.trim())
    };

    fetch('/login', {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(loginData)
    })
    .then(response => {
        if (!response.ok) {
            return response.text().then(text => {
                console.error('Login failed:', text);
                throw new Error('Login failed');
            });
        }
        return response.json();
    })
    .then(data => {
        console.log('Login successful:', data); // הדפסת ההצלחה
        window.location.href = 'book.html'; // הפנה לעמוד הספרים
    })
    .catch(error => console.error('Unable to login.', error));
}