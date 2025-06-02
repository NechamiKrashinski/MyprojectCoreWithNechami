let token = getCookieValue('AuthToken'); // משתנה גלובלי
let userRole = getUserRoleFromToken(token); 
let authorsList; // משתנה גלובלי לאחסון רשימת הסופרים
let currentAuthor;
let currentAuthorId;
let booksList; // משתנה גלובלי לאחסון רשימת הספרים
const getBooksList = async () => {
    try {
        const booksResponse = await axios.get(`/Book`, {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });

        booksList = booksResponse.data; // שמירה של רשימת הספרים במשתנה הגלובלי

        console.log("Books List:", booksList);
    } catch (error) {
        console.error("Error fetching books data", error);
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


getBooksList();