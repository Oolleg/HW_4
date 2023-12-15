document.addEventListener('DOMContentLoaded', () => {
    const authButton = document.getElementById("auth-button");
    if (authButton) authButton.addEventListener('click', authButtonClick);

    const signoutButton = document.getElementById("signout-button");
    if (signoutButton) signoutButton.addEventListener('click', signoutButtonClick)
});

function signoutButtonClick() {
    fetch(`/api/auth`, { method: "DELETE" })
        .then(r => {
            
            if (r.status == 200) {
               
                window.location.assign(location.origin);
                console.log(r.status);
                r.json().then(j => { console.log(j.status) });
            }
            else {
                showAuthMessage("Server is unavailable");
            }
            console.log(r);
        });

}
function authButtonClick() {
    const loginInput = document.getElementById("auth-login");
    if (!loginInput) throw "Element #auth-login not found";
    const login = loginInput.value.trim();
    if (login.length == 0) {
        showAuthMessage("Login can't be empty!")
        return;
    }
    const PasswordInput = document.getElementById("auth-password");
    if (!PasswordInput) throw "Element #auth-password not found";
    const password = PasswordInput.value.trim();
    if (password.length == 0) {
        showAuthMessage("Password can't be empty!")
        return;
    }
    fetch(`/api/auth?login=${login}&password=${password}`)
        .then(r => {
            if (r.status == 200) {
               
                window.location.reload();
               
            }
            else {
                showAuthMessage("Access denied.");
            }
               
            
        });

        //    r.json())
        //.then(j => {
        //    showAuthMessage(j.status);
        //    console.log(j);
        //})
}

function showAuthMessage(message) {
    const authMessage = document.getElementById("auth-message");
    if (!authMessage) throw "Element #authMessage not found";

    authMessage.innerText = message;
    authMessage.classList.remove('visually-hidden')
}

