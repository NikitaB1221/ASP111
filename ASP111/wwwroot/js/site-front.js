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
});

function loadSections() {
    const container = document.getElementById('sections');
    if(!container) throw "loadSections: Container 'sections' not found"
    fetch('/api/section', { method: 'GET' })
        .then(r => r.json())
        .then(j => {
            console.log(j);
            fetch('/tpl/forum-section-view.html')
                .then(r => r.text())
                .then(t => { 
                    let content = "";
                    for (let section of j) {
                        content += t.replaceAll('@Model.ImageUrl', section.imageUrl)
                            .replaceAll('@Model.Title', section.title)
                            .replaceAll('@Model.Description', section.description)
                            .replaceAll('@Model.CreateDt', section.createDt)
                            .replaceAll('@(Model.Likes)', section.likes)
                            .replaceAll('@(Model.Dislikes)', section.dislikes)
                            .replaceAll('@Model.Author.Name', section.author.name)
                            .replaceAll('@Model.Author.Email', section.author.email)
                            .replaceAll('@Model.Author.Avatar', section.author.avatar)
                    }
                    container.innerHTML = content;

                })
        });
}