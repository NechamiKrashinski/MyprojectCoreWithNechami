let token = getCookieValue('AuthToken'); // משתנה גלובלי
let userRole = getUserRoleFromToken(token); 
let authorsList; // משתנה גלובלי לאחסון רשימת הסופרים
let currentAuthor;

const getAuthorsList = async () => {
    const response = await axios.get('/Author', {
        headers: {
            'Authorization': `Bearer ${token}`
        }
    });
    console.log("currentAuthor1", currentAuthor);
    
    authorsList = response.data; // שמירה של רשימת הסופרים במשתנה הגלובלי
    currentAuthor = authorsList[0].name;
    console.log("currentAuthor2", currentAuthor);

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
.