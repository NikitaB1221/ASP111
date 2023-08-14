document.addEventListener("DOMContentLoaded", () => {
    const authButton = document.getElementById("auth-button");
    if (authButton) {
        authButton.addEventListener("click", authButtonClick);
    }
    else {
        console.error("Element not found: auth-button");
    }
    for (let pencil of document.querySelectorAll("[data-edit]")) {
        pencil.addEventListener('click', editProfileClick);
    }
    for (let rate of document.querySelectorAll("[data-rate-item-id]")) {
        rate.addEventListener('click', rateClick);
    }
});

function editProfileClick(e) {
    const p = e.target.closest('p');
    const span = p.querySelector('span');
    span.setAttribute('contenteditable', 'true');
    span.onblur = editableBlur;
    span.onkeydown = editableKeydown;
    span.focus();
}

function editableBlur(e) {
    e.target.removeAttribute('contenteditable');
    console.log(e.target.innerText);
    fetch("/User/UpdateEmail?email=" + e.target.innerText, {
        method: "POST"
    })
        .then(r => r.json())
        .then(j => { console.log(j) });
}

function editableKeydown(e) {
    if (e.keyCode == 13) {
        e.preventDefault();
        e.target.blur();
    }
}

function authButtonClick() {
    const authLogin = document.getElementById("auth-login");
    if (!authLogin) throw "Element not found: auth-login"
    const authPassword = document.getElementById("auth-password");
    if (!authPassword) throw "Element not found: auth-password"
    if (authLogin.value.length === 0) {
        alert("Login can't be empty!");
        return;
    }
    if (authPassword.value.length === 0) {
        alert("Password can't be empty!");
        return;
    }

    window
        .fetch("/User/Auth", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                login: authLogin.value,
                password: authPassword.value
            })
        })
        .then(r => r.json())
        .then(j => {
            if (j.success == true) {
                location = location;
            }
        });
}

function rateClick(e) {
    const rateItemId = e.target.closest("[data-rate-item-id]");
    if (!rateItemId) {
        throw "Invalid structure - 'data-rate-item-id' not found";
    }
    const itemId = rateItemId.getAttribute('data-rate-item-id');
    const rateValue = rateItemId.getAttribute('data-rate-value');
    
    console.log('Rate Click' + ' ' + itemId + ' ' + rateValue);
    fetch(`/api/rate?itemId=${itemId}&rateValue=${rateValue}`, {
        method: "POST"
    }).then(r => {
        const contentType = r.headers.get("Content-Type");
        if (contentType.startsWith('text')) {
            return r.text().then(responseText => {
                // Отображаем текстовый ответ во всплывающем окне
                window.alert(responseText);
            });
        }
        else {
            return r.json().then(responseJson => {
                const rateSpan = document.querySelector('.rate');
                rateSpan.textContent = responseJson.rateNum;
                // Явно возвращаем значение responseJson.rateNum
                return responseJson.rateNum;
            });
        }
    }).then(console.log)
        .catch(error => {
        // Если произошла ошибка при выполнении fetch
            window.alert("Error while executing request: " + error.message);
    });
}