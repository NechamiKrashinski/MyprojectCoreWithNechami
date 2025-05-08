let token = getCookieValue('AuthToken'); // משתנה גלובלי
let userRole = getUserRoleFromToken(token); 
let authorsList; // משתנה גלובלי לאחסון רשימת הסופרים
let currentAuthor;
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
