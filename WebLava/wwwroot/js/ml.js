// Big Lava — поведение новой дизайн-системы: шапка, мобильное меню, анимации появления.
(function () {
    var doc = document.documentElement;
    doc.classList.add('ml-js');

    // Шапка: фон после прокрутки, прячется при скролле вниз
    var header = document.querySelector('.ml-header');
    var lastY = window.scrollY;
    function onScroll() {
        var y = window.scrollY;
        if (header) {
            header.classList.toggle('is-scrolled', y > 20);
            header.classList.toggle('is-hidden', y > 400 && y > lastY && !doc.classList.contains('ml-menu-open'));
        }
        lastY = y;
    }
    window.addEventListener('scroll', onScroll, { passive: true });
    onScroll();

    // Мобильное меню
    var burger = document.querySelector('.ml-burger');
    if (burger) {
        burger.addEventListener('click', function () {
            var open = doc.classList.toggle('ml-menu-open');
            document.body.classList.toggle('ml-menu-open', open);
            burger.setAttribute('aria-expanded', open ? 'true' : 'false');
        });
        document.addEventListener('keydown', function (e) {
            if (e.key === 'Escape' && doc.classList.contains('ml-menu-open')) burger.click();
        });
    }

    // Появление блоков при прокрутке. data-reveal-stagger на родителе — задержка для детей.
    document.querySelectorAll('[data-reveal-stagger]').forEach(function (parent) {
        Array.prototype.forEach.call(parent.children, function (child, i) {
            child.setAttribute('data-reveal', '');
            child.style.setProperty('--ml-delay', (i * 0.08) + 's');
        });
    });
    var items = document.querySelectorAll('[data-reveal]');
    if (!('IntersectionObserver' in window)) {
        items.forEach(function (el) { el.classList.add('is-visible'); });
        return;
    }
    var io = new IntersectionObserver(function (entries) {
        entries.forEach(function (entry) {
            if (entry.isIntersecting) {
                entry.target.classList.add('is-visible');
                io.unobserve(entry.target);
            }
        });
    }, { rootMargin: '0px 0px -8% 0px', threshold: 0.08 });
    items.forEach(function (el) { io.observe(el); });
})();
