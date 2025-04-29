const uri = '/auther';
let authers = [];

function getUsers() {
    fetch(uri)
        .then(response => response.json())
        .then(data => _displayUsers(data))
        .catch(error => console.error('Unable to get authers.', error));
}

function addUser() {
    const addNameTextbox = document.getElementById('add-name');
    const addAddressTextbox = document.getElementById('add-address');
    const addBirthdateTextbox = document.getElementById('add-birthdate');

    const auther = {
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
        body: JSON.stringify(auther)
    })
    .then(() => {
        getUsers();
        addNameTextbox.value = '';
        addAddressTextbox.value = '';
        addBirthdateTextbox.value = '';
    })
    .catch(error => console.error('Unable to add auther.', error));
}

function deleteUser(id) {
    fetch(`${uri}/${id}`, {
        method: 'DELETE'
    })
    .then(() => getUsers())
    .catch(error => console.error('Unable to delete auther.', error));
}

function displayEditForm(id) {
    const auther = authers.find(auther => auther.id === id);

    document.getElementById('edit-id').value = auther.id;
    document.getElementById('edit-name').value = auther.name;
    document.getElementById('edit-address').value = auther.address;
    document.getElementById('edit-birthdate').value = auther.birthDate;
    document.getElementById('editForm').style.display = 'block';
}

function updateUser() {
    const autherId = document.getElementById('edit-id').value;
    const auther = {
        id: parseInt(autherId, 10),
        name: document.getElementById('edit-name').value.trim(),
        address: document.getElementById('edit-address').value.trim(),
        birthDate: document.getElementById('edit-birthdate').value
    };

    fetch(`${uri}/${autherId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(auther)
    })
    .then(() => getUsers())
    .catch(error => console.error('Unable to update auther.', error));

    closeInput();
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

function _displayUsers(data) {
    const tBody = document.getElementById('authers');
    tBody.innerHTML = '';

    data.forEach(auther => {
        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        td1.appendChild(document.createTextNode(auther.name));

        let td2 = tr.insertCell(1);
        td2.appendChild(document.createTextNode(auther.address));

        let td3 = tr.insertCell(2);
        td3.appendChild(document.createTextNode(auther.birthDate));

        let td4 = tr.insertCell(3);
        let editButton = document.createElement('button');
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${auther.id})`);
        td4.appendChild(editButton);

        let td5 = tr.insertCell(4);
        let deleteButton = document.createElement('button');
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteUser(${auther.id})`);
        td5.appendChild(deleteButton);
    });

    authers = data;
}
