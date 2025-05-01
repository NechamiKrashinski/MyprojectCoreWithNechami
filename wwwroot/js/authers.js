<<<<<<< HEAD
const loginUri = '/login';

function loginUser() {
    const loginIdTextbox = document.getElementById('login-id');

    const loginData = {
        id: parseInt(loginIdTextbox.value.trim())
=======
const uri = '/author';
let authors = [];

function getUsers() {
    fetch(uri)
        .then(response => response.json())
        .then(data => _displayUsers(data))
        .catch(error => console.error('Unable to get authors.', error));
}

function addUser() {
    const addNameTextbox = document.getElementById('add-name');
    const addAddressTextbox = document.getElementById('add-address');
    const addBirthdateTextbox = document.getElementById('add-birthdate');

    const author = {
        name: addNameTextbox.value.trim(),
        address: addAddressTextbox.value.trim(),
        birthDate: addBirthdateTextbox.value
>>>>>>> e5f0c2f45f3159d29c8be38a0b4d2eeb1432a9fa
    };

    fetch(loginUri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
<<<<<<< HEAD
        body: JSON.stringify(loginData)
=======
        body: JSON.stringify(author)
>>>>>>> e5f0c2f45f3159d29c8be38a0b4d2eeb1432a9fa
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('Login failed');
        }
        return response.json();
    })
<<<<<<< HEAD
    .then(data => {
        console.log('Login successful', data);
        // כאן תוכל לשמור את הטוקן או לעדכן את הממשק בהתאם
    })
    .catch(error => console.error('Unable to login.', error));
=======
    .catch(error => console.error('Unable to add author.', error));
}

function deleteUser(id) {
    fetch(`${uri}/${id}`, {
        method: 'DELETE'
    })
    .then(() => getUsers())
    .catch(error => console.error('Unable to delete author.', error));
}

function displayEditForm(id) {
    const author = authors.find(author => author.id === id);

    document.getElementById('edit-id').value = author.id;
    document.getElementById('edit-name').value = author.name;
    document.getElementById('edit-address').value = author.address;
    document.getElementById('edit-birthdate').value = author.birthDate;
    document.getElementById('editForm').style.display = 'block';
}

function updateUser() {
    const authorId = document.getElementById('edit-id').value;
    const author = {
        id: parseInt(authorId, 10),
        name: document.getElementById('edit-name').value.trim(),
        address: document.getElementById('edit-address').value.trim(),
        birthDate: document.getElementById('edit-birthdate').value
    };

    fetch(`${uri}/${authorId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
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

        let td2 = tr.insertCell(1);
        td2.appendChild(document.createTextNode(author.address));

        let td3 = tr.insertCell(2);
        td3.appendChild(document.createTextNode(author.birthDate));

        let td4 = tr.insertCell(3);
        let editButton = document.createElement('button');
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${author.id})`);
        td4.appendChild(editButton);

        let td5 = tr.insertCell(4);
        let deleteButton = document.createElement('button');
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteUser(${author.id})`);
        td5.appendChild(deleteButton);
    });

    authors = data;
>>>>>>> e5f0c2f45f3159d29c8be38a0b4d2eeb1432a9fa
}
