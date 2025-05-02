import { getCookie } from './utils.js'; // Import the getCookie function
const uri = '/book';
let books = [];



function getItems() {
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
        if (!response.ok) {
            throw new Error('Unable to get items.');
        }
        return response.json();
    })
    .then(data => _displayItems(data))
    .catch(error => console.error('Unable to get items.', error));
}

function addItem() {
    const addNameTextbox = document.getElementById('add-name');
    const addAuthorTextbox = document.getElementById('add-author');
    const addPriceTextbox = document.getElementById('add-price');
    const addDateTextbox = document.getElementById('add-date');

    const item = {
        name: addNameTextbox.value.trim(),
        author: addAuthorTextbox.value.trim(),
        price: parseFloat(addPriceTextbox.value),
        date: addDateTextbox.value
    };

    fetch(uri, {
        method: 'POST',
        headers: {
            'Authorization': `Bearer ${getCookie('authToken')}`, // שליפת הטוקן מהקוקי
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        credentials: 'include', // מאפשר שליחה של קוקיז עם הבקשה
        body: JSON.stringify(item)
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('Unable to add item.');
        }
        return response.json();
    })
    .then(() => {
        getItems();
        addNameTextbox.value = '';
        addAuthorTextbox.value = '';
        addPriceTextbox.value = '';
        addDateTextbox.value = '';
    })
    .catch(error => console.error('Unable to add item.', error));
}

function deleteItem(id) {
    fetch(`${uri}/${id}`, {
        method: 'DELETE',
        headers: {
            'Authorization': `Bearer ${getCookie('authToken')}`, // שליפת הטוקן מהקוקי
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        credentials: 'include' // מאפשר שליחה של קוקיז עם הבקשה
    })
    .then(() => getItems())
    .catch(error => console.error('Unable to delete item.', error));
}

function displayEditForm(id) {
    const item = books.find(item => item.id === id);

    document.getElementById('edit-name').value = item.name;
    document.getElementById('edit-author').value = item.author;
    document.getElementById('edit-price').value = item.price;
    document.getElementById('edit-date').value = item.date;
    document.getElementById('edit-id').value = item.id;
    document.getElementById('editForm').style.display = 'block';
}

function updateItem() {
    const itemId = document.getElementById('edit-id').value;
    const item = {
        id: parseInt(itemId, 10),
        name: document.getElementById('edit-name').value.trim(),
        author: document.getElementById('edit-author').value.trim(),
        price: parseFloat(document.getElementById('edit-price').value),
        date: document.getElementById('edit-date').value
    };

    fetch(`${uri}/${itemId}`, {
        method: 'PUT',
        headers: {
            'Authorization': `Bearer ${getCookie('authToken')}`, // שליפת הטוקן מהקוקי
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        credentials: 'include', // מאפשר שליחה של קוקיז עם הבקשה
        body: JSON.stringify(item)
    })
    .then(() => getItems())
    .catch(error => console.error('Unable to update item.', error));

    closeInput();
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

function _displayCount(itemCount) {
    const name = (itemCount === 1) ? 'book' : 'books';
    document.getElementById('counter').innerText = `${itemCount} ${name}`;
}

function _displayItems(data) {
    const tBody = document.getElementById('books');
    tBody.innerHTML = '';

    _displayCount(data.length);

    data.forEach(item => {
        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        td1.appendChild(document.createTextNode(item.author));

        let td2 = tr.insertCell(1);
        td2.appendChild(document.createTextNode(item.name));

        let td3 = tr.insertCell(2);
        td3.appendChild(document.createTextNode(item.price));

        let td4 = tr.insertCell(3);
        td4.appendChild(document.createTextNode(item.date));

        let td5 = tr.insertCell(4);
        let editButton = document.createElement('button');
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${item.id})`);
        td5.appendChild(editButton);

        let td6 = tr.insertCell(5);
        let deleteButton = document.createElement('button');
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteItem(${item.id})`);
        td6.appendChild(deleteButton);
    });

    books = data;
}

// קריאה ל-getItems כדי להציג את הספרים
getItems();