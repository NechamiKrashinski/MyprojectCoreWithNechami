let token = getCookieValue('AuthToken'); // משתנה גלובלי
let userRole = getUserRoleFromToken(token); 
let authorsList; // משתנה גלובלי לאחסון רשימת הסופרים
let currentAuthor;
let currentAuthorId;
let booksList; // משתנה גלובלי לאחסון רשימת הספרים

const getAuthorsAndBooksList = async () => {
    try {
        const [authorsResponse, booksResponse] = await Promise.all([
            axios.get(`/Author`, {
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            }),
            axios.get(`/Book`, {
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            })
        ]);

        authorsList = authorsResponse.data; // שמירה של רשימת הסופרים במשתנה הגלובלי
        booksList = booksResponse.data; // שמירה של רשימת הספרים במשתנה הגלובלי

        if (authorsList.length > 0) {
            currentAuthor = authorsList[0].name;
            currentAuthorId = authorsList[0].id;
            console.log("Current Author ID:", currentAuthorId);
        }

        console.log("currentAuthor", currentAuthor);
    } catch (error) {
        console.error("Error fetching data", error);
    }
};


function getCookieValue(cookieName) {
    let cookies = document.cookie;
    let cookieArray = cookies.split('; ');
    let cookie = cookieArray.find(c => c.startsWith(cookieName + '='));
    return cookie ? cookie.split('=')[1] : null; // אם הקוקי לא נמצא
}


function getUserRoleFromToken(token) {
    if (!token) return null;

    const payload = token.split('.')[1]; // החלק השני הוא ה-payload
    const decodedPayload = JSON.parse(atob(payload)); // פענוח ה-base64

    return decodedPayload.Role; // הנח שהשדה שמכיל את התפקיד נקרא "role"
}
function logoutUser() {
    userRole = null;
    authorsList = null;
    currentAuthor = null;
    currentAuthorId = null;
    booksList = null;

    // הפניה לדף הכניסה או לדף אחר
document.cookie = "AuthToken=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/";
    token = getCookieValue('AuthToken'); 

    // בדיקה אם ה-token לא null לפני קריאה ל-isTokenValid
    if (token) {
        isTokenValid(token);
    } else {
        console.log("Token is null after logout.");
    }

    window.location.href = 'http://localhost:5172/login.html';
}


function isTokenValid(token) {
    const payload = JSON.parse(atob(token.split('.')[1])); // פענוח הטוקן
    const isValid = payload.exp > Date.now() / 1000; // השוואת תאריך התפוגה עם הזמן הנוכחי
    console.log(`Token is valid: ${isValid}, Expiration: ${new Date(payload.exp * 1000)}`);
    return isValid;
}


const logoutButton = document.createElement('button');
logoutButton.innerText = 'Logout';
logoutButton.onclick = logoutUser;
document.body.appendChild(logoutButton);