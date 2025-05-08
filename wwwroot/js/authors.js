async function getUsers() {
    try {
        await getAuthorsAndBooksList(); // קריאה לפונקציה לקבלת רשימת הסופרים

        const authorsContainer = document.getElementById('authors');
        authorsContainer.innerHTML = ''; // ניקוי התוכן הקיים
        authorsList.forEach(author => {
            const authorCard = document.createElement('div');
            authorCard.className = 'author-card'; // הוסף מחלקה עבור עיצוב

            authorCard.innerHTML = `
                <h4>${author.name}</h4>
                <p>Address: ${author.address}</p>
                <p>Birth Date: ${author.birthDate}</p>
                <button onclick="editUser(${author.id}, '${author.name}', '${author.address}', '${author.birthDate}')">Edit</button>
                <button onclick="deleteUser(${author.id})">Delete</button>
            `;
            authorsContainer.appendChild(authorCard);
        });

        // הוספת כפתור הוספת סופר אם ה-role הוא Admin
        if (userRole === 'Admin') {
            // בדוק אם הכפתור כבר קיים
            if (!document.getElementById('addAuthorButtonContainer').querySelector('button')) {
                const addButton = document.createElement('button');
                addButton.textContent = 'Add Author';
                addButton.onclick = openAddForm;
                document.getElementById('addAuthorButtonContainer').appendChild(addButton);
            }
        }
    } catch (error) {
        console.error("Error fetching authors:", error);
    }
}

function openAddForm() {
    document.getElementById('addForm').style.display = 'block'; // הצג את טופס ההוספה
}

function closeAddForm() {
    document.getElementById('addForm').style.display = 'none'; // הסתר את טופס ההוספה
}


// פונקציה להוספת מחבר חדש
async function addUser() {
    const name = document.getElementById('add-name').value;
    const address = document.getElementById('add-address').value;
    const birthDate = document.getElementById('add-birthdate').value;

    const newAuthor = {
        name: name,
        address: address,
        birthDate: birthDate
    };

    try {
        await axios.post('/Author', newAuthor, {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });
        getUsers(); // רענן את רשימת המחברים
        closeAddForm(); // סגור את טופס ההוספה
    } catch (error) {
        console.error("Error adding author:", error);
    }
}

// פונקציה לעדכון מחבר קיים
async function updateUser() {
    const id = document.getElementById('edit-id').value;
    const name = document.getElementById('edit-name').value;
    const address = document.getElementById('edit-address').value;
    const birthDate = document.getElementById('edit-birthdate').value;

    const updatedAuthor = {
        id: id,
        name: name,
        address: address,
        birthDate: birthDate
    };

    try {
        await axios.put(`/Author/${id}`, updatedAuthor, {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });
        getUsers(); // רענן את רשימת המחברים
        closeInput(); // סגור את טופס העריכה
    } catch (error) {
        console.error("Error updating author:", error);
    }
}

// פונקציה למחיקת מחבר
async function deleteUser(id) {
    try {
        await axios.delete(`/Author/${id}`, {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });
        getUsers(); // רענן את רשימת המחברים
    } catch (error) {
        console.error("Error deleting author:", error);
    }
}

// פונקציה לעדכון טופס העריכה
function editUser(id, name, address, birthDate) {
    document.getElementById('edit-id').value = id;
    document.getElementById('edit-name').value = name;
    document.getElementById('edit-address').value = address;
    document.getElementById('edit-birthdate').value = birthDate;
    document.getElementById('editForm').style.display = 'block'; // הצג את טופס העריכה
}

// פונקציה לסגירת טופס העריכה
function closeInput() {
    document.getElementById('editForm').style.display = 'none'; // הסתר את טופס העריכה
}

// קריאה ל-API כדי לקבל את רשימת המחברים
getUsers();

