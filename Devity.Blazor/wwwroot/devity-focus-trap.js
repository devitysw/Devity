const focusableSelector = [
    'a[href]',
    'button:not([disabled])',
    'textarea:not([disabled])',
    'input:not([disabled])',
    'select:not([disabled])',
    '[tabindex]:not([tabindex="-1"])'
].join(',');

const inputSelector = [
    'input:not([disabled]):not([type="hidden"])',
    'textarea:not([disabled])',
    'select:not([disabled])'
].join(',');

export function activateFocusTrap(container, focusOnActivate) {
    if (!container) {
        return;
    }

    deactivateFocusTrap(container);

    const trap = event => {
        if (event.key !== 'Tab') {
            return;
        }

        const focusableElements = Array.from(container.querySelectorAll(focusableSelector))
            .filter(element => element.offsetParent !== null || element === document.activeElement);

        if (focusableElements.length === 0) {
            event.preventDefault();
            container.focus();
            return;
        }

        const firstElement = focusableElements[0];
        const lastElement = focusableElements[focusableElements.length - 1];

        if (event.shiftKey && (document.activeElement === firstElement || !container.contains(document.activeElement))) {
            event.preventDefault();
            lastElement.focus();
            return;
        }

        if (!event.shiftKey && document.activeElement === lastElement) {
            event.preventDefault();
            firstElement.focus();
        }
    };

    container.addEventListener('keydown', trap);
    container.devityFocusTrap = trap;

    if (focusOnActivate) {
        focusInitialElement(container);
    }
}

export function deactivateFocusTrap(container) {
    if (!container?.devityFocusTrap) {
        return;
    }

    container.removeEventListener('keydown', container.devityFocusTrap);
    delete container.devityFocusTrap;
}

function focusInitialElement(container) {
    const input = Array.from(container.querySelectorAll(inputSelector))
        .find(isVisible);

    if (input) {
        input.focus();
        return;
    }

    const focusableElement = Array.from(container.querySelectorAll(focusableSelector))
        .find(isVisible);

    if (focusableElement) {
        focusableElement.focus();
        return;
    }

    container.focus();
}

function isVisible(element) {
    return element.offsetParent !== null || element === document.activeElement;
}
