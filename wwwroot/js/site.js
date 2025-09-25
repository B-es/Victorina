const buttons = document.getElementsByClassName("answer-button");
const userChoiceInput = document.getElementsByName("userChoiceIndex").item(0);

function changeButtonColor(btn, _class) {
    const toRemove = btn.classList.item(btn.classList.length-1);
    btn.classList.remove(toRemove);
    btn.classList.add(_class);
}

function buttonsColorToDefault() {
    for (const btn of buttons) {
        changeButtonColor(btn, "btn-primary");
    }
}

function changeButtonState(e) {
    buttonsColorToDefault();
    const target = e.target;
    changeButtonColor(target, "btn-danger");
    userChoiceInput.value = target.id;
}


//Card

function onInsideCard(id) {
    console.log(id)
    const title = document.getElementById("title" + id);
    
    const start = document.getElementById("start" + id);
    title.style.display = 'none';
    start.style.display = 'block';
}

function onOutsideCard(id) {
    const title = document.getElementById("title" + id);
    const start = document.getElementById("start" + id);
    title.style.display = 'block';
    start.style.display = 'none';
}