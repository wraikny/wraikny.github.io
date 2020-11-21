// ナビバーの開閉を設定
for (const burder of document.getElementsByClassName('navbar-burger')) {
    const menuId = burder.dataset.target;
    const menu = document.getElementById(menuId);
    burder.addEventListener('click', e => {
        burder.classList.toggle('is-active');
        menu.classList.toggle('is-active');
    });
}