// function loginUser() {
//     const loginIdTextbox = document.getElementById('login-id');

//     const loginData = {
//         id: parseInt(loginIdTextbox.value.trim())
//     };

//     fetch('/login', {
//         method: 'POST',
//         headers: {
//             'Accept': 'application/json',
//             'Content-Type': 'application/json'
//         },
        

//         body: JSON.stringify(loginData)
//     })
//     .then(response => {
//         if (!response.ok) {
//             throw new Error('Login failed');
//         }
//         return response.json();
//     })
//     .then(() => {
//         console.log('Login successful');
//         // כאן תוכל לעדכן את הממשק בהתאם
//     })
//     .catch(error => console.error('Unable to login.', error));
// }


function loginUser() {
    const loginIdTextbox = document.getElementById('login-id');
    console.log(loginIdTextbox.value + ".....................................");

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
        return response.json(); // מצפה שהשרת יחזיר { "token": "your-jwt-token" }
    })
    .then(data => {
        const token = data; // הוצא את הטוקן מהתגובה

        // אם אתה רוצה להשתמש ב-options בהמשך, תצטרך להגדיר אותו קודם
        const options = {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        };

        // הפנה לעמוד אחר או עדכן את הממשק
        window.location.href = 'book.html';
    })
    .catch(error => console.error('Unable to login.', error));
}


// Example usage of fetchWithToken
function getBooks() {
    fetchWithToken('/books')
        .then(data => {
            console.log('Books:', data);
        })
        .catch(error => console.error('Unable to fetch books.', error));
 }