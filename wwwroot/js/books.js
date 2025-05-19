
// הוסף את הפונקציה הזו בקובץ books.js
const fillAuthorsDropdown = () => {
    const authorSelect = document.getElementById('add-author');
    const authorSelectEdit = document.getElementById('edit-author');
    authorSelect.innerHTML = ''; // ניקוי התוכן הקיים
    authorSelectEdit.innerHTML = '';

    authorsList.forEach(author => {
        const option = document.createElement('option');
        option.value = author.name; // הנח שהשדה שמכיל את שם הסופר נקרא "name"
        option.textContent = author.name;
        authorSelect.appendChild(option);
        authorSelectEdit.appendChild(option.cloneNode(true)); // הוספת אפשרות לעריכת סופר
    });
};

async function getBooks() {
    try {
        await getAuthorsAndBooksList(); // קריאה לפונקציה לקבלת רשימת הסופרים

        fillAuthorsDropdown(); // קריאה למילוי רשימת הסופרים

        const booksContainer = document.getElementById('books');
        booksContainer.innerHTML = ''; // ניקוי התוכן הקיים

        booksList.forEach(book => {
            const card = document.createElement('div');
            card.className = 'book-card';
            card.innerHTML = `
                <h4>${book.name}</h4>
                <p>Author: ${findNameAuthor(book.authorId)}</p>
                <p>Price: ${book.price}</p>
                <p>Publish Date: ${book.date}</p>
                <button onclick="editBook(${book.id}, '${book.name}', ${book.price}, '${book.date}')">Edit</button>
                <button onclick="deleteBook(${book.id})">Delete</button>
            `;
            booksContainer.appendChild(card);
            const userLinkContainer = document.getElementById('userLinkContainer');

            if (userRole === 'Admin') {
                userLinkContainer.innerHTML = '<a href="author.html">Go to Authors</a>';
            } else if (userRole === 'Author') {
                userLinkContainer.innerHTML = '<a href="author.html">Go to your details</a>'; // החלף את הקישור המתאים
            }
        });
    } catch (error) {
        console.error("Error fetching books:", error);
    }
}
const findNameAuthor = (id) => {
    console.log(id+"findNameAuthor");
    
    const author = authorsList.find(author => author.id === id); // Change to find by ID
    return author ? author.name : null; // Return the name of the author or null if not found
}
async function addBook() {
    const nameField = document.getElementById('add-name');
    const authorField = document.getElementById('add-author');
    const priceField = document.getElementById('add-price');
    const dateField = document.getElementById('add-date');
    
    const name = nameField.value;
    //const author = authorField.value;
    const price = parseFloat(priceField.value);
    const date = dateField.value;
    let bookAuthorId;
    if (userRole === 'Author') {
        bookAuthorId = currentAuthorId; // השתמש ב-ID של הסופר הנוכחי
    }
    else if (userRole === 'Admin') {
        bookAuthorId = authorsList.find(author => author.name === authorField.value).id; // מצא את ה-ID של הסופר לפי השם

    }
    const newBook = {
        id:0,
        name: name,
        authorId: bookAuthorId , // השתמש ב-ID של הסופר הנוכחי
        // author: author,
        price: price,
        date: date
    };

    try {
        await axios.post('/Book', newBook, {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });
        nameField.value = '';
        authorField.value = '';
        priceField.value = '';
        dateField.value = '';
        document.getElementById('addBookForm').style.display = 'none'; // הצג את טופס הוספת הספר

        getBooks(); // רענן את רשימת הספרים
    } catch (error) {
        console.error("Error adding book:", error);
    }
}

// function editBook(id, name, price, date) {
//    // document.getElementById('add-id').value = id; // אם יש לך שדה hidden או input אחר עבור ה-ID
//     document.getElementById('add-name').value = name;
//     document.getElementById('add-price').value = price;
//     document.getElementById('add-date').value = date;

//     // אם ה-userRole הוא Author, חסום את שדה הסופר
//     if (userRole === 'Author') {
//         document.getElementById('add-author').disabled = true; // Disable the author field
//     } else {
//         document.getElementById('add-author').disabled = false; // Enable the author field for admins
//     }

//     document.getElementById('addBookForm').style.display = 'block'; // הצג את טופס הוספת הספר
// }

// async function updateBook() {
//     const id = document.getElementById('add-id').value; // קח את ה-ID מהשדה הקיים
//     const name = document.getElementById('add-name').value;
//     const author = document.getElementById('add-author').value;
//     const price = parseFloat(document.getElementById('add-price').value);
//     const date = document.getElementById('add-date').value;

//     const updatedBook = {
//         id: id,
//         name: name,
//         author: author,
//         price: price,
//         date: date
//     };

//     try {
//         await axios.put(`/Book/${id}`, updatedBook, {
//             headers: {
//                 'Authorization': `Bearer ${token}`
//             }
//         });
//         closeInput(); // סגור את טופס ההוספה
//         getBooks(); // רענן את רשימת הספרים
//     } catch (error) {
//         console.error("Error updating book:", error);
//     }
// }

async function updateBook() {
    const id = document.getElementById('edit-id').value; // קח את ה-ID מהשדה hidden
    const name = document.getElementById('edit-name').value;
    const author = document.getElementById('edit-author').value; // קח את הסופר מהשדה
    const price = parseFloat(document.getElementById('edit-price').value);
    const date = document.getElementById('edit-date').value;

    const updatedBook = {
        id: id,
        name: name,
        author: author,
        price: price,
        date: date
    };

    try {
        await axios.put(`/Book/${id}`, updatedBook, {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });
        closeInput(); // סגור את טופס העריכה
        getBooks(); // רענן את רשימת הספרים
    } catch (error) {
        console.error("Error updating book:", error);
    }
}
function editBook(id, name, price, date) {
    document.getElementById('edit-id').value = id; // שים את ה-ID בשדה hidden
    document.getElementById('edit-name').value = name;
    document.getElementById('edit-price').value = price;
    document.getElementById('edit-date').value = date;

    // מלא את רשימת הסופרים בטופס העריכה
    fillAuthorsDropdown(); 

    document.getElementById('editForm').style.display = 'block'; // הצג את טופס העריכה
}

async function deleteBook(id) {
    try {
        await axios.delete(`/Book/${id}`, {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });
        getBooks(); // רענן את רשימת הספרים
    } catch (error) {
        console.error("Error deleting book:", error);
    }
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none'; // הסתר את טופס העריכה
}

// קריאה ל-API כדי לקבל את רשימת הספרים
getBooks();

function getCookieValue(cookieName) {
    let cookies = document.cookie;
    let cookieArray = cookies.split('; ');

    let cookie = cookieArray.find(c => c.startsWith(cookieName + '='));

    if (cookie) {
        return cookie.split('=')[1];
    }
    return null; // אם הקוקי לא נמצא
}



function getUserRoleFromToken(token) {
    if (!token) return null;

    // חילוץ החלקים של ה-JWT
    const payload = token.split('.')[1]; // החלק השני הוא ה-payload
    const decodedPayload = JSON.parse(atob(payload)); // פענוח ה-base64

    // החזר את תפקיד המשתמש
    return decodedPayload.Role; // הנח שהשדה שמכיל את התפקיד נקרא "role"
}
function showAddBookForm() {
    if (userRole === 'Author') {
        document.getElementById('add-author').disabled = true; // Disable the author field
        document.getElementById('add-author').value = currentAuthor; // Set the author field to the current author
    } else {
        document.getElementById('add-author').disabled = false; // Enable the author field for admins
    }
    document.getElementById('addBookForm').style.display = 'block'; // הצג את טופס הוספת הספר

}