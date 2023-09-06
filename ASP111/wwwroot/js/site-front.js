document.addEventListener('DOMContentLoaded', () => {
    fetch('/tpl/forum-index.html')
        .then(r => r.text())
        .then(t => {
            const pageBody = document.getElementById('pageBody')
            if (pageBody) {
                pageBody.innerHTML = t;
                onHashChanged();
            }
            else throw "DOMContentLoaded: pageBody element not found";
        })

    window.addEventListener("hashchange", onHashChanged);
    const authButton = document.getElementById("auth-button-front");
    if (authButton) {
        authButton.addEventListener('click', authButtonClick);
    }
    else {
        console.error("Element not found: auth-button-front");
    }

    let token = getAuthToken();
    let signIcon = document.getElementById("sign-icon");



    if (token) {
        signIcon.innerHTML = '<i class="bi bi-box-arrow-right f-large mx-3" role="button" onclick="logOut()">';
    }
    else {
        signIcon.innerHTML = '<i class="bi bi-person-down f-large mx-3" role="button" data-bs-toggle="modal" data-bs-target="#authModal"></i>';
    }
});

function onHashChanged(e) {
    console.log(window.location.hash);
    const path = window.location.hash.substring(1).split('/')
    switch (path[0].toLowerCase()) {
        case 'section': loadTopics(path[1]); break;
        case 'topic': loadThemes(path[1]); break;
        case 'theme': loadComments(path[1]); break;
        default: loadSections(); break;
    }
    //console.log(path);
}

function getPageContainer() {
    const container = document.getElementById('sections');
    if (!container) throw " getPageContainer(): Container 'section' not found";
    return container;
}

function loadThemes(topicId) {
    const container = getPageContainer();
    container.innerHTML = `<img src='/img/preloader.gif' alt="preloader">`;
    fillTemplatePar3('/tpl/forum-theme-view.html', '/api/theme?topicId=' + topicId, '/tpl/forum-theme-container.html')
        .then(content => container.innerHTML = content)
}

function loadComments(themeId) {
    const container = getPageContainer();
    container.innerHTML = `Comments ${themeId} coming soon!`;
}

function loadTopics(sectionId) {
    const container = getPageContainer();
    container.innerHTML = `<img src='/img/preloader.gif' alt="preloader">`;
    //fillTemplatePar('/tpl/forum-topic-view.html', '/api/topic?sectionId=' + sectionId)
    fillTemplatePar3('/tpl/forum-topic-view.html', '/api/topic?sectionId=' + sectionId, '/tpl/forum-topic-container.html')
        .then(content => {
            container.innerHTML = content
            const addTopicButton = document.getElementById('add-topic-button');
            if (!addTopicButton) throw "'add-topic-button' not found";
            addTopicButton.addEventListener('click', addTopicClick);
        });
}

function addSectionClick() {
    const sectionTitle = document.getElementById('section-title');
    if (!sectionTitle) throw "addSectionClick: 'section-title' not found!"
    const sectionDescription = document.getElementById('section-description');
    if (!sectionDescription) throw "addSectionClick: 'section-description' not found!"

    const title = sectionTitle.value;
    const description = sectionDescription.value;

    if (title.length < 1 || description.length < 1) {
        alert("Fill all data fields");
        return;
    }

    console.log(title + "\n" + description);
    return new Promise((resolve, reject) =>
        Promise.all([
            fetch('/api/section', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8',
                    // состояние авторизации передается в заголовке
                    'Authorization': `Bearer ${getAuthToken()}`
                },
                body: JSON.stringify(
                    {
                        'title': title,
                        'description': description
                    }
                )
            })
                .then(r => r.json())
                .then(data => {
                    console.log(data);
                    if (data.statusCode === "200") {
                        alert("Section added successfully!");
                        resolve();

                    }
                    else {
                        alert(data.message);
                        reject(data.message);
                    }
                })
        ])
            .then(() => loadSections())
    );
}

function loadSections() {
    console.log("lS: Start");
    const container = getPageContainer();
    let token = getAuthToken();
    let isAuth = isAuthenticated();
    container.innerHTML = `<img src='/img/preloader.gif' alt="preloader">`;
    fillTemplatePar3('/tpl/forum-section-view.html', '/api/section',
        isAuth
            ? '/tpl/forum-section-container-auth.html'
            : '/tpl/forum-section-container.html')
        .then(content => {
            container.innerHTML = content;
            if (isAuth) {
                const addSectionButton = document.getElementById('add-section-button');
                if (!addSectionButton) throw "'add-section-button' not found";
                addSectionButton.addEventListener('click', addSectionClick);
            }
        });
    console.log("lS: End");

}

function fillTemplate(templateUrl, dataUrl) {
    return new Promise((resolve, reject) =>
        fetch(dataUrl, {
            method: 'GET'
        })
            .then(r => r.json())
            .then(j => {
                // console.log(j);
                fetch(templateUrl)
                    .then(r => r.text())
                    .then(t => {
                        // j - данные, t - шаблон для заполнения данными
                        let content = "";
                        for (let section of j) {
                            let item = t;  // copy of template
                            for (let prop in section) {
                                // console.log(prop + " " + typeof section[prop]);
                                if ('object' === typeof section[prop]) {
                                    for (let subprop in section[prop]) {
                                        item = item.replaceAll(
                                            `{{${prop}.${subprop}}}`,
                                            section[prop][subprop]
                                        );
                                    }
                                }
                                else {
                                    item = item.replaceAll(`{{${prop}}}`, section[prop]);
                                }
                            }
                            content += item;
                        }
                        resolve(content);
                    })
            }));
}

function fillTemplatePar(templateUrl, dataUrl) {
    return new Promise((resolve, reject) =>
        Promise.all([
            fetch(dataUrl, {
                method: 'GET'
            }).then(r => r.json()),

            fetch(templateUrl)
                .then(r => r.text())
        ]).then(([j, t]) => {
            // console.log(j);                
            // j - данные, t - шаблон для заполнения данными
            let content = "";
            for (let section of j) {
                let item = t;  // copy of template
                for (let prop in section) {
                    // console.log(prop + " " + typeof section[prop]);
                    if ('object' === typeof section[prop]) {
                        for (let subprop in section[prop]) {
                            item = item.replaceAll(
                                `{{${prop}.${subprop}}}`,
                                section[prop][subprop]
                            );
                        }
                    }
                    else {
                        item = item.replaceAll(`{{${prop}}}`, section[prop]);
                    }
                }
                content += item;
            }
            resolve(content);
        }));
}

function fillTemplatePar3(templateUrl, dataUrl, containerUrl) {
    const container = getPageContainer();
    container.innerHTML = `<img src='/img/preloader.gif' alt='preloader' />`;
    return new Promise((resolve, reject) =>
        Promise.all([
            fetch(dataUrl + "?rand=" + Math.random(), {
                method: 'GET'
            }).then(r => r.json()),

            fetch(templateUrl)
                .then(r => r.text()),

            fetch(containerUrl + '?rnd=' + Math.random())
                .then(r => r.text()),
        ]).then(([j, t, c]) => {
            //console.log(j);                
            //console.log(t);                
            //console.log(c);                
            // j - данные, t - повторяющийся шаблон для заполнения данными, с - контейнер для заполненных шаблонов
            let content = "";
            for (let section of j) {
                let item = t;  // copy of template
                for (let prop in section) {
                    // console.log(prop + " " + typeof section[prop]);
                    if ('object' === typeof section[prop]) {
                        for (let subprop in section[prop]) {
                            item = item.replaceAll(
                                `{{${prop}.${subprop}}}`,
                                section[prop][subprop]
                            );
                        }
                    }
                    else {
                        item = item.replaceAll(`{{${prop}}}`, section[prop]);
                    }
                }
                content += item;
            }
            content = c.replaceAll('{{body}}', content);
            resolve(content);
        })
    );
}

function authButtonClick() {
    const authLogin = document.getElementById("auth-login");
    if (!authLogin) throw "Element not found: auth-login";
    const authPassword = document.getElementById("auth-password");
    if (!authPassword) throw "Element not found: auth-password";
    if (authLogin.value.length === 0) {
        alert("You must enter your login");
        return;
    }
    if (authPassword.value.length === 0) {
        alert("You must enter your password");
        return;
    }
    const body = JSON.stringify({
        login: authLogin.value,
        password: authPassword.value
    });
    console.log(body);
    // передаем данные на сервер асинхронно

    window
        .fetch(                // отсылка запроса
            "/api/auth", {    // URL - адрес запроса
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                login: authLogin.value,
                password: authPassword.value
            })
        }
        )
        .then(r => r.json())   // ответ => извлечение тела как JSON Объекта
        .then(j => {           // результат извлечения тела
            //console.log(j);
            if (j.id != "") {
                localStorage.setItem('token', j.id)
                window.location.reload();
            }
        });

}

function logOut() {
    localStorage.removeItem("token");
    window.location.reload();
}

function isAuthenticated() {
    let token = localStorage.getItem("token");
    return !!token;
}

function getAuthToken() {
    return localStorage.getItem("token");
}