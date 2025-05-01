const loginUri = 'login';
function loginUser(event) {
    console.log('Login function called1');
    event.preventDefault(); // מונע את שליחת הטופס באופן ברירת מחדל
    console.log('Login attempt initiated');

    const loginIdTextbox = document.getElementById('login-id');
    const loginData = {
        id: parseInt(loginIdTextbox.value.trim())
    };
    
    console.log('Login data:', loginData);

    fetch(loginUri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(loginData)
    })
    .then(response => {
        console.log('Response received:', response);
        if (!response.ok) {
            throw new Error('Login failed');
        }
        return response.json();
    })
    .then(data => {
        console.log('Login successful', data);
        document.getElementById('message').innerText = 'Login successful!';
        
        window.location.href = '/author.html';
        // כאן תוכל לשמור את הטוקן או לעדכן את הממשק בהתאם
    })
    .catch(error => {
        console.error('Unable to login.', error);
        document.getElementById('message').innerText = 'Login failed: ' + error.message;
    });
}
