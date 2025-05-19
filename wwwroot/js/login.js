function loginUser() {
    const loginPasswordTextbox = document.getElementById('login-password');
    const loginNameTextbox = document.getElementById('login-name');
    const loginData = {
        password: loginPasswordTextbox.value.trim(),
        name: loginNameTextbox.value.trim()
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

function logoutUser() {
    fetch('/login', {
        method: 'DELETE',
        credentials: 'include' // לשלוח קוקיז עם הבקשה
    })
    .then(response => {
        if (!response.ok) {
            return response.text().then(text => {
                console.error('Logout failed:', text);
                throw new Error('Logout failed');
            });
        }
        console.log('Logout successful'); // הדפסת ההצלחה
        window.location.href = 'login.html'; // הפנה לעמוד הכניסה
    })
    .catch(error => console.error('Unable to logout.', error));
}