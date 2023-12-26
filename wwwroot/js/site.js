document.addEventListener('DOMContentLoaded', () => {
    const authButton = document.getElementById("auth-button");
    if (authButton) authButton.addEventListener('click', authButtonClick);

    const signoutButton = document.getElementById("signout-button");
    if (signoutButton) signoutButton.addEventListener('click', signoutButtonClick);

    const saveProfileButton = document.getElementById("profile-save-button");
    if (saveProfileButton) saveProfileButton.addEventListener('click', saveProfileButtonClick);

    const newLogoProfileButton = document.getElementById("logo-rofile-button");
    if (newLogoProfileButton) newLogoProfileButton.addEventListener('click', savaNewLogoProfileButtonClick);

    const deleteProfileButton = document.getElementById("delete-profile");
    if (deleteProfileButton) deleteProfileButton.addEventListener('click', deleteProfileButtonClick);

    const suspendProfileButton = document.getElementById("suspend-profile");
    if (suspendProfileButton) suspendProfileButton.addEventListener('click', suspendProfileButtonClick);

    const deleteModalWindowButton = document.getElementById("suspend-profile");
    if (deleteModalWindowButton) deleteModalWindowButton.addEventListener('click', deleteModalWindowButtonClick);


});

function deleteModalWindowButtonClick() {

    window.location.assign(location.origin);
}

function suspendProfileButtonClick() {
    fetch(`/user/SuspendProfile`, { method: 'DELETE' })
        .then(r => {
            if (r.status == 200) {
                showDivMessage("Profile has been deleted", "DeleteModalWindow");
               
            }
            else {
                showDivMessage("Authorization Required", "DeleteModalWindow");

            }
        }
        );
}

function deleteProfileButtonClick() {
    
   
   
    fetch(`/user/DeleteProfile`, { method: 'DELETE' })
        .then(r => {
            if (r.status == 200) {
                showDivMessage("Profile has been deleted", "DeleteModalWindow");
               /* window.location.assign(location.origin);*/
                /*window.location.assign(location.origin);*/
            }
            else {
                showDivMessage("Authorization Required", "DeleteModalWindow");
               
            }
        }
    );
}

function savaNewLogoProfileButtonClick() {
    //const avatarInputProfile = document.getElementById("new-avatar-input-profile");
    //if (!avatarInputProfile) throw 'Element id=new-avatar-input-profile not found';

    //console.dir(avatarInputProfile);

    var data = new FormData(formNewAvatar);
    /*data.append('logo', avatarInputProfile.files[0]);*/

    fetch(`/user/UpdateAvatarProfile`, { method: 'POST', body: data })
        .then(r => {
            if (r.status == 200) {
                window.location.reload();

            }
            else {
                showAuthMessage("Authorization Required");
            }
        });
}

function saveProfileButtonClick()
{
    const nameInput = document.getElementById("inputUsername");
    if (!nameInput) throw 'Element id=inputUsername not found';
    const emailInput = document.getElementById("inputEmailAddress");
    if (!emailInput) throw 'Element id=inputEmailAddress not found';
    const phoneInput = document.getElementById("inputPhoneProfile");
    if (!phoneInput) throw 'Element id=inputPhoneProfile not found';
    const loginInput = document.getElementById("inputLoginProfile");
    if (!loginInput) throw 'Element id=inputLoginProfile not found';

    console.dir(nameInput);
   
    fetch(`/user/UpdateProfile?newName=${nameInput.value}&newEmail=${emailInput.value}&newPhone=${phoneInput.value}&newLogin=${loginInput.value}`, { method: 'POST'})
        .then(r => {
            if (r.status == 401) {
                
                showDivMessage("Authorization Required", "edit-message");
                
            }

        });
}

function signoutButtonClick() {
    fetch(`/api/auth`, { method: "DELETE" })
        .then(r => {
            
            if (r.status == 200) {
               
                window.location.assign(location.origin);
               
            }
            else {
                showAuthMessage("Server is unavailable");
            }
           
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

        ////    r.json())
        ////.then(j => {
        ////    showAuthMessage(j.status);
        ////    console.log(j);
        ////})
}

function showAuthMessage(message) {
    const authMessage = document.getElementById("auth-message");
    if (!authMessage) throw "Element #authMessage not found";

    authMessage.innerText = message;
    authMessage.classList.remove('visually-hidden')
}

//function showDivMessage(message, divId) {
//    const authMessage = document.getElementById(divId);
//    if (!authMessage) throw "Element #authMessage not found";

//    authMessage.innerText = message;
//    authMessage.classList.remove('visually-hidden')
//}

function showDivMessage(message, divId) {
    const modalWindow = document.getElementById(divId);
    if (!modalWindow) throw "Element #modalWindow not found";

    const textWindow = modalWindow.firstElementChild.firstElementChild
        .firstElementChild.nextElementSibling;

    console.dir(textWindow);

    textWindow.innerText = message;
     new bootstrap.Modal(modalWindow).show();
   
}

