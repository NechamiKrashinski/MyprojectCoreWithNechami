const uri = '/user';
let users = [];

function getUsers() {
    fetch(uri)
        .then(response => response.json())
        .then(data => _displayUsers(data))
        .catch(error => console.error('Unable to get users.', error));
}

function addUser() {
    const addNameTextbox = document.getElementById('add-name');
    const addAddressTextbox = document.getElementById('add-address');
    const addBirthdateTextbox = document.getElementById('add-birthdate');

    const user = {
        name: addNameTextbox.value.trim(),
        address: addAddressTextbox.value.trim(),
        birthDate: addBirthdateTextbox.value
    };

    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(user)
    })
    .then(() => {
        getUsers();
        addNameTextbox.value = '';
        addAddressTextbox.value = '';
        addBirthdateTextbox.value = '';
    })
    .catch(error => console.error('Unable to add user.', error));
}

function deleteUser(id) {
    fetch(`${uri}/${id}`, {
        method: 'DELETE'
    })
    .then(() => getUsers())
    .catch(error => console.error('Unable to delete user.', error));
}

function displayEditForm(id) {
    const user = users.find(user => user.id === id);

    document.getElementById('edit-id').value = user.id;
    document.getElementById('edit-name').value = user.name;
    document.getElementById('edit-address').value = user.address;
    document.getElementById('edit-birthdate').value = user.birthDate;
    document.getElementById('editForm').style.display = 'block';
}

function updateUser() {
    const userId = document.getElementById('edit-id').value;
    const user = {
        id: parseInt(userId, 10),
        name: document.getElementById('edit-name').value.trim(),
        address: document.getElementById('edit-address').value.trim(),
        birthDate: document.getElementById('edit-birthdate').value
    };

    fetch(`${uri}/${userId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(user)
    })
    .then(() => getUsers())
    .catch(error => console.error('Unable to update user.', error));

    closeInput();
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

function _displayUsers(data) {
    const tBody = document.getElementById('users');
    tBody.innerHTML = '';

    data.forEach(user => {
        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        td1.appendChild(document.createTextNode(user.name));

        let td2 = tr.insertCell(1);
        td2.appendChild(document.createTextNode(user.address));

        let td3 = tr.insertCell(2);
        td3.appendChild(document.createTextNode(user.birthDate));

        let td4 = tr.insertCell(3);
        let editButton = document.createElement('button');
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${user.id})`);
        td4.appendChild(editButton);

        let td5 = tr.insertCell(4);
        let deleteButton = document.createElement('button');
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteUser(${user.id})`);
        td5.appendChild(deleteButton);
    });

    users = data;
}
