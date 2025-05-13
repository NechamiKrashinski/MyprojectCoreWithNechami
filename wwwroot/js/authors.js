async function getUsers() {
    try {
        await getAuthorsAndBooksList(); // קריאה לפונקציה לקבלת רשימת הסופרים

        const authorsContainer = document.getElementById('authors');
        authorsContainer.innerHTML = ''; // ניקוי התוכן הקיים
        authorsList.forEach(author => {
            const authorCard = document.createElement('div');
            authorCard.className = 'author-card'; // הוסף מחלקה עבור עיצוב
            authorCard.setAttribute('data-id', author.id); // הוסף את ה-ID כ-data attribute

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
    const authorsContainer = document.getElementById('authors');
    const authorCard = authorsContainer.querySelector(`.author-card[data-id='${id}']`);

    authorCard.innerHTML = `
        <input type="text" value="${name}" id="edit-name-${id}" required>
        <input type="text" value="${address}" id="edit-address-${id}" required>
        <input type="date" value="${birthDate}" id="edit-birthdate-${id}" required>
        <button onclick="saveUser(${id})">Save</button>
        <button onclick="cancelEdit(${id}, '${name}', '${address}', '${birthDate}')">Cancel</button>
    `;
}

async function saveUser(id) {
    const name = document.getElementById(`edit-name-${id}`).value;
    const address = document.getElementById(`edit-address-${id}`).value;
    const birthDate = document.getElementById(`edit-birthdate-${id}`).value;

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
    } catch (error) {
        console.error("Error updating author:", error);
    }
}

function cancelEdit(id, name, address, birthDate) {
    const authorsContainer = document.getElementById('authors');
    const authorCard = authorsContainer.querySelector(`.author-card[data-id='${id}']`);

    authorCard.innerHTML = `
        <h4>${name}</h4>
        <p>Address: ${address}</p>
        <p>Birth Date: ${birthDate}</p>
        <button onclick="editUser(${id}, '${name}', '${address}', '${birthDate}')">Edit</button>
        <button onclick="deleteUser(${id})">Delete</button>
    `;
}

// קריאה ל-API כדי לקבל את רשימת המחברים
getUsers();
