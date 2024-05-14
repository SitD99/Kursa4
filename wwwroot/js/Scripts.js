// Получите все элементы с классом card-link
var cardLinks = document.querySelectorAll('.card-link');

// Переберите все найденные элементы и добавьте обработчик событий для каждого из них
cardLinks.forEach(function (cardLink) {
    // Добавьте обработчик события щелчка для каждой карточки
    cardLink.addEventListener('click', function () {
        // Получите идентификатор карточки из атрибута data-card-id
        var cardId = cardLink.getAttribute('data-card-id');

        // Перенаправьте пользователя на страницу деталей выбранной карточки
        window.location.href = '/Card/Details/' + cardId;
    });
});