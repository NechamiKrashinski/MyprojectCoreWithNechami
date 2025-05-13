// login.js
function loginUser(event) {
    event.preventDefault(); // מונע את שליחת הטופס הרגילה

    const userId = document.getElementById('login-id').value;
    console.log("=====");
    
    axios.post('/Login', {
        id: userId
    })
    .then(response => {
        document.getElementById('message').innerText = 'Login successful!';
        console.log(response.data);
        window.location.href = 'http://localhost:5172/author.html';
    })
    .catch(error => {
        document.getElementById('message').innerText = error.response.data || 'Login failed!';
        console.error(error);
    });
}

// הוספת אירוע ה-submit
document.addEventListener('DOMContentLoaded', function() {
    const form = document.getElementById('loginForm');
    form.addEventListener('submit', loginUser);
});


// פונקציה לקבלת ערך קוקי לפי שם
function getCookieValue(cookieName) {
    // לקבל את כל הקוקיז
    let cookies = document.cookie;
    console.log("Cookies:", cookies);
    // להפריד את הקוקיז למערך
    let cookieArray = cookies.split('; ');

    // לחפש את הקוקי המבוקש
    let cookie = cookieArray.find(c => c.startsWith(cookieName + '='));

    if (cookie) {
        // לחלץ את הערך של הקוקי
        return cookie.split('=')[1];
    }
    return null; // אם הקוקי לא נמצא
}
