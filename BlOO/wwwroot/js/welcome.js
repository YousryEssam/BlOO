// Your existing JS code
function animateButton(button) {
    button.classList.add('clicked');

    // Remove the class after animation completes
    setTimeout(() => {
        button.classList.remove('clicked');
    }, 200);
}

// Add fade-in animation when page loads
document.addEventListener('DOMContentLoaded', () => {
    const card = document.querySelector('.card');
    card.style.opacity = '0';

    setTimeout(() => {
        card.style.opacity = '1';
    }, 100);
});

// Add hover effect for buttons
const buttons = document.querySelectorAll('.btn');
buttons.forEach(button => {
    button.addEventListener('mouseover', () => {
        button.style.transform = 'translateY(-2px)';
    });

    button.addEventListener('mouseout', () => {
        button.style.transform = 'translateY(0)';
    });
});
