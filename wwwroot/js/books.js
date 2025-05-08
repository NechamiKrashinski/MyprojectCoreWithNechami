
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
                <p>Author: ${book.author}</p>
                <p>Price: ${book.price}</p>
                <p>Publish Date: ${book.date}</p>
                <button onclick="editBook(${book.id}, '${book.name}', '${book.author}', ${book.price}, '${book.date}')">Edit</button>
                <button onclick="deleteBook(${book.id})">Delete</button>
            `;
            booksContainer.appendChild(card);
        });
    } catch (error) {
        console.error("Error fetching books:", error);
    }
}

async function addBook() {
    const nameField = document.getElementById('add-name');
    const authorField = document.getElementById('add-author');
    const priceField = document.getElementById('add-price');
    const dateField = document.getElementById('add-date');
    
    const name = nameField.value;
    const author = authorField.value;
    const price = parseFloat(priceField.value);
    const date = dateField.value;
    
    const newBook = {
        name: name,
        author: author,
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

function editBook(id, name, author, price, date) {
    document.getElementById('edit-id').value = id;
    document.getElementById('edit-name').value = name;
    document.getElementById('edit-author').value = author;
    document.getElementById('edit-price').value = price;
    document.getElementById('edit-date').value = date;
    if (userRole === 'Author') {
        document.getElementById('edit-author').disabled = true; // Disable the author field
    } else {
        document.getElementById('edit-author').disabled = false; // Enable the author field for admins
    }
    document.getElementById('editForm').style.display = 'block'; // הצג את טופס העריכה
}

async function updateBook() {
    const id = document.getElementById('edit-id').value;
    const name = document.getElementById('edit-name').value;
    const author = document.getElementById('edit-author').value;
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