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
    console.log(path);
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

    fetch('/api/section',
        {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json; charset=utf-8'
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
            if (data.message === "200") {
                alert("Section added successfully!");
            }
            else {
                alert(data.message);
                return;
            }
    
        })  //  .then(loadSections());  //  System.InvalidOperationException: "Cannot Open when State is Connecting."

     //  loadSections();  //  System.InvalidOperationException: "Cannot Open when State is Connecting."

}

function loadSections() {
    const container = getPageContainer();
    container.innerHTML = `<img src='/img/preloader.gif' alt="preloader">`;
    fillTemplatePar3('/tpl/forum-section-view.html', '/api/section', '/tpl/forum-section-container.html')
        .then(content => {
            container.innerHTML = content;
            const addSectionButton = document.getElementById('add-section-button');
            if (!addSectionButton) throw "'add-section-button' not found";
            addSectionButton.addEventListener('click', addSectionClick);
        });
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
            fetch(dataUrl, {
                method: 'GET'
            }).then(r => r.json()),

            fetch(templateUrl)
                .then(r => r.text()),

            fetch(containerUrl)
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
        }));
}
