// Big Lava CRM — лендинги. Без зависимостей: выбор тарифа в форме + AJAX-отправка заявки.
(function () {
    'use strict';

    var form = document.querySelector('[data-lead-form]');
    var planSelect = document.querySelector('[data-plan-select]');

    // Кнопки тарифов: подставляем тариф в форму и прокручиваем к ней (без перезагрузки).
    document.querySelectorAll('[data-plan]').forEach(function (btn) {
        btn.addEventListener('click', function (e) {
            var lead = document.getElementById('lead');
            if (!planSelect || !lead) return;
            e.preventDefault();
            planSelect.value = btn.getAttribute('data-plan');
            lead.scrollIntoView({ behavior: window.matchMedia('(prefers-reduced-motion: reduce)').matches ? 'auto' : 'smooth' });
            setTimeout(function () {
                var name = document.getElementById('lead-name');
                if (name) name.focus({ preventScroll: true });
            }, 450);
            if (window.ym) { try { window.ym(101658266, 'reachGoal', 'crm_plan_click', { plan: planSelect.value }); } catch (_) { } }
        });
    });

    if (!form || !window.fetch || !window.FormData) return;

    var alertBox = form.querySelector('[data-lead-alert]');
    var submit = form.querySelector('[data-lead-submit]');
    var success = document.querySelector('[data-lead-success]');
    var successText = document.querySelector('[data-lead-success-text]');

    function clearErrors() {
        form.querySelectorAll('[data-err]').forEach(function (el) { el.textContent = ''; });
        form.querySelectorAll('[aria-invalid]').forEach(function (el) { el.removeAttribute('aria-invalid'); });
        alertBox.hidden = true;
        alertBox.textContent = '';
    }

    function showErrors(errors) {
        var first = null;
        Object.keys(errors || {}).forEach(function (key) {
            var slot = form.querySelector('[data-err="' + key + '"]');
            var input = form.querySelector('[name="' + key + '"]');
            if (slot) slot.textContent = errors[key];
            if (input) { input.setAttribute('aria-invalid', 'true'); if (!first) first = input; }
        });
        if (first) first.focus();
    }

    function clientValidate() {
        var errors = {};
        var name = form.elements.Name.value.trim();
        var phoneDigits = form.elements.Phone.value.replace(/\D/g, '');
        var email = form.elements.Email.value.trim();
        if (name.length < 2) errors.Name = 'Укажите, как к вам обращаться';
        if (!phoneDigits) errors.Phone = 'Укажите телефон для связи';
        else if (phoneDigits.length < 10 || phoneDigits.length > 15) errors.Phone = 'Проверьте номер телефона';
        if (email && !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) errors.Email = 'Проверьте адрес почты';
        if (!form.elements.Consent.checked) errors.Consent = 'Нужно согласие на обработку персональных данных';
        return errors;
    }

    form.addEventListener('submit', function (e) {
        e.preventDefault();
        clearErrors();
        var errors = clientValidate();
        if (Object.keys(errors).length) { showErrors(errors); return; }

        submit.disabled = true;
        var label = submit.textContent;
        submit.textContent = 'Отправляем…';

        fetch(form.action, {
            method: 'POST',
            body: new FormData(form),
            headers: { 'X-Requested-With': 'XMLHttpRequest' },
            credentials: 'same-origin'
        })
            .then(function (res) { return res.json().catch(function () { return { success: false }; }); })
            .then(function (data) {
                if (data && data.success) {
                    if (successText && data.message) successText.textContent = data.message;
                    form.hidden = true;
                    success.hidden = false;
                    success.focus();
                    if (window.ym) { try { window.ym(101658266, 'reachGoal', 'crm_lead'); } catch (_) { } }
                    return;
                }
                if (data && data.errors) showErrors(data.errors);
                alertBox.textContent = (data && data.message) || 'Не удалось отправить заявку. Попробуйте ещё раз или позвоните: +7 (908) 120-90-23.';
                alertBox.hidden = false;
            })
            .catch(function () {
                alertBox.textContent = 'Нет связи с сервером. Проверьте интернет или позвоните: +7 (908) 120-90-23.';
                alertBox.hidden = false;
            })
            .then(function () {
                submit.disabled = false;
                submit.textContent = label;
            });
    });
})();
