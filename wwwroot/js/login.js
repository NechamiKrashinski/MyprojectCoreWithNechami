
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
    .then(data => {
        const token = data.token; // הנחה שהשרת מחזיר טוקן
        document.cookie = `authToken=${token}; path=/`; // שמור את הטוקן בקוקי
        window.location.href = 'book.html'; // הפנה לעמוד הספרים
    })
    .catch(error => console.error('Unable to login.', error));
}