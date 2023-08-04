document.addEventListener("DOMContentLoaded", () => {
    const signInButton = document.getElementById("signin-button");
    if (signInButton) signInButton.addEventListener('click', signInButtonClick);


});
function signInButtonClick() {
    const userLoginInput = document.getElementById("signin-login");
    const userPasswordInput = document.getElementById("signin-password");
    if (!userLoginInput) throw "Елемент не знайдено: signin-login";
    if (!userPasswordInput) throw "Елемент не знайдено: signin-password";

    const userLogin = userLoginInput.value;
    const userPassword = userPasswordInput.value;

    const data = new FormData();
    data.append("login", userLogin);
    data.append("password", userPassword);
    
    const modalBody = document.getElementById("signinModal").querySelector(".section-input"); // Отримуємо елемент модального вікна

    const message = document.createElement("p"); // Створюємо елемент для відображення надпису
    message.textContent = "Зачекайте, триває авторизація..."; // Задаємо текст надпису
    modalBody.appendChild(message); // Додаємо надпис до модального вікна


    fetch(                      
        "/User/SignIn",          
        {                       
            method: "POST",     
            body: data          
        })                      
        .then(r => r.json())    
        .then(j => {            
            console.log(j);     

            modalBody.removeChild(message); // Видаляємо надпис після отримання відповіді
            if (typeof j.status != 'undefined') {
                if (j.status == 'OK') {
                    window.location.reload();   // оновлюємо сторінку як для автентифікованого користувача
                    alert("YES");
                }
                else {
                    const error = document.createElement("p"); // Створюємо елемент для відображення помилки
                    error.textContent = "Неправильний логін або пароль"; // Задаємо текст помилки
                    error.style.color = "red"; // Задаємо червоний колір тексту помилки
                    modalBody.appendChild(error); // Додаємо помилку до модального вікна
                }
            }
        });
}