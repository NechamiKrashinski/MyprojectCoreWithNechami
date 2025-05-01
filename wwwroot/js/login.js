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
            throw new Error('Login failed');
        }
        return response.json();
    })
    .then(() => {
        console.log('Login successful');
        // כאן תוכל לעדכן את הממשק בהתאם
    })
    .catch(error => console.error('Unable to login.', error));
}
