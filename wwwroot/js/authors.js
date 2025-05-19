
const uri = '/author';
let authors = [];
let userRole;

function getCookie(name) {
    const value = `; ${document.cookie}`;
    console.log(document.cookie);
    const parts = value.split(`; ${name}=`);
    if (parts.length === 2) return parts.pop().split(';').shift();
    return null; // החזרת null אם הקוקי לא נמצא
}

function getUserRoleFromToken() {
    const token = getCookie('authToken'); // שליפת הטוקן מהקוקי
    if (!token) {
        return null; // או ערך ברירת מחדל אחר   
    }

    const payLoad = token.split('.')[1];
    const deCodedPayLoad = JSON.parse(atob(payLoad)); // פענוח ה-payload והמרה לאובייקט
    let roleValue = null;
    for (let key in deCodedPayLoad){
        if(key.includes("role")){
            roleValue = deCodedPayLoad[key];
            break;
        }
    }

    return  roleValue; // החזרת התפקיד מהטוקן
}

function showAddUserForm() {
    if (userRole === "Admin") {
        document.getElementById('addForm').style.display = 'block';
    }
    else {
        alert("You do not have permission to add users.");
    }
}
function hideAddUserForm() {
    if (userRole === "Admin") {
        document.getElementById("addForm").style.display = "none";
    }
}
function getUsers() {
    userRole =getUserRoleFromToken(); 
    fetch(uri, {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${getCookie('authToken')}`, // שליפת הטוקן מהקוקי
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        credentials: 'include' // מאפשר שליחה של קוקיז עם הבקשה
    })
        .then(response => {
            if (response.status === 401) {
                // הפנה לעמוד הלוגין
                window.location.href = '/login.html';
            }
            return response.json();
        })
        .then(data => _displayUsers(data))
        .catch(error => console.error('Unable to get authors.', error));
}

function addUser() {
    const addNameTextbox = document.getElementById('add-name');
    const addPasswordTextbox = document.getElementById('add-password');
    const addAddressTextbox = document.getElementById('add-address');
    const addBirthdateTextbox = document.getElementById('add-birthdate');
    const addRoleTextBox = document.getElementById("add-Role")
   

    const author = {
        Id: 0,
        Name: addNameTextbox.value.trim(),
        Password: addPasswordTextbox.value.trim(),
        role: addRoleTextBox.value === "Admin" ? 0 : 1,
        Address: addAddressTextbox.value.trim(),
        BirthDate: addBirthdateTextbox.value
    };

    fetch(uri, {
        method: 'POST',
        headers: {
            'Authorization': `Bearer ${getCookie('authToken')}`, // שליפת הטוקן מהקוקי
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        credentials: 'include', // מאפשר שליחה של קוקיז עם הבקשה
        body: JSON.stringify(author)
    })
        .then(response => {
            if (!response.ok) {
                console.log(response.status + response.text())
                console.log("Unable to add author.");

                throw new Error('Unable to add author.');
            }
            return response.json();
        })
        .then(() => {
            getUsers(); // עדכון הרשימה לאחר הוספה
            addNameTextbox.value = '';
            addAddressTextbox.value = '';
            addBirthdateTextbox.value = '';
            addRoleTextBox.value = "Select User Role"
        })
        .catch(error => console.error('Unable to add author.', error));
}

function deleteUser(id) {
    fetch(`${uri}/${id}`, {
        method: 'DELETE',
        headers: {
            'Authorization': `Bearer ${getCookie('authToken')}`, // שליפת הטוקן מהקוקי
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        credentials: 'include' // מאפשר שליחה של קוקיז עם הבקשה
    })
        .then(() => getUsers())
        .catch(error => console.error('Unable to delete author.', error));
}

function displayEditForm(id) {
    const author = authors.find(author => author.id === id);

    document.getElementById('edit-id').value = author.id;
    document.getElementById('edit-name').value = author.name;
    document.getElementById('edit-password').value = author.password;
    document.getElementById('edit-address').value = author.address;
    document.getElementById('edit-birthdate').value = author.birthDate;
    document.getElementById('editForm').style.display = 'block';
}

function updateUser() {
    const authorId = document.getElementById('edit-id').value;
    const author = {
        id: parseInt(authorId, 10),
        name: document.getElementById('edit-name').value.trim(),
        password :document.getElementById('edit-password').value.trim(),
        address: document.getElementById('edit-address').value.trim(),
        birthDate: document.getElementById('edit-birthdate').value
    };

    fetch(`${uri}/${authorId}`, {
        method: 'PUT',
        headers: {
            'Authorization': `Bearer ${getCookie('authToken')}`, // שליפת הטוקן מהקוקי
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        credentials: 'include', // מאפשר שליחה של קוקיז עם הבקשה
        body: JSON.stringify(author)
    })
        .then(() => getUsers())
        .catch(error => console.error('Unable to update author.', error));

    closeInput();
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

function _displayUsers(data) {
    const tBody = document.getElementById('authors');
    tBody.innerHTML = '';

    data.forEach(author => {
        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        td1.appendChild(document.createTextNode(author.name));

        let td5 = tr.insertCell(1);
        td5.appendChild(document.createTextNode(author.password));

        let td2 = tr.insertCell(2);
        td2.appendChild(document.createTextNode(author.address));

        let td3 = tr.insertCell(3);
        td3.appendChild(document.createTextNode(author.birthDate));

        let td4 = tr.insertCell(4);
        let editButton = document.createElement('button');
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${author.id})`);
        td4.appendChild(editButton);
        
        if(userRole === "Admin"){
            let td5 = tr.insertCell(5);
            let deleteButton = document.createElement('button');
            deleteButton.innerText = 'Delete';
            deleteButton.setAttribute('onclick', `deleteUser(${author.id})`);
            td5.appendChild(deleteButton);
        }
        
    });

    authors = data;
}
function filterAuthorsByName() {
    const filterValue = document.getElementById('filter-input').value.toLowerCase();
    const filteredAuthors = authors.filter(author => author.name.toLowerCase().includes(filterValue));
    _displayUsers(filteredAuthors);
}
function clearFilter() {
    document.getElementById('filter-input').value = ''; 
    getUsers();
}
// קריאה ל-getUsers כדי להציג את הסופרים
getUsers();


