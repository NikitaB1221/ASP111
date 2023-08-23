document.addEventListener('DOMContentLoaded', () => {
    fetch('/tpl/forum-index.html')
        .then(r => r.text())
        .then(t => {
            const pageBody = document.getElementById('pageBody')
            if (pageBody) {
                pageBody.innerHTML = t;
                loadSections();
            }
            else throw "pageBody element not found";
        })
    window.addEventListener("hashchange", onHashChanged)
});

function onHashChanged(e) {
    console.log(window.location.hash);
    const path = window.location.hash.substring(1).split('/')
    switch (path[0].toLowerCase()) {
        case 'section': loadTopics(path[1]); break;
    }
    console.log(path);
}

function getPageContainer() {
    const container = document.getElementById('sections');
    if (!container) throw " loadSection(): Container 'section' not found";
    return container;
}

function loadTopics(sectionId) {
    const container = getPageContainer();
    container.innerHTML = `${sectionId} will coming soon`;
}

function loadSections() {
    const container = getPageContainer();
    fetch('/api/section', { method: 'GET' })
        .then(r => r.json())
        .then(j => {
            console.log(j);
            fetch('/tpl/forum-section-view.html')
                .then(r => r.text())
                .then(t => {
                    let content = "";
                    for (let section of j) {
                        let item = t;  //  Template copy
                        for (let prop in section) {
                            if ('object' === typeof section[prop]) {
                                for (let subprop in section[prop]) {
                                    item = item.replaceAll(`{{${prop}.${subprop}}}`, section[prop][subprop]);
                                }
                            }
                            else {
                                item = item.replaceAll(`{{${prop}}}`, section[prop])
                            }
                        }
                        content += item;
                    }
                    container.innerHTML = content;

                })
        }
        );
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
