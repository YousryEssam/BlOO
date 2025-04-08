$(document).ready(function () {
    // Generate random sizes and positions for floating elements
    $('.floating-element').each(function (index) {
        // Random size between 30px and 80px
        const size = Math.floor(Math.random() * 50) + 30;

        // Random position
        const posX = Math.floor(Math.random() * 90) + 5;
        const posY = Math.floor(Math.random() * 90) + 5;

        // Apply styles
        $(this).css({
            'width': size + 'px',
            'height': size + 'px',
            'left': posX + '%',
            'top': posY + '%'
        });

        // Animation settings
        const speedX = (Math.random() - 0.5) * 0.1;
        const speedY = (Math.random() - 0.5) * 0.1;

        // Store speeds as data attributes
        $(this).data('speedX', speedX);
        $(this).data('speedY', speedY);
    });

    // Animate floating elements
    function animateElements() {
        $('.floating-element').each(function () {
            // Get current position and speed
            const speedX = $(this).data('speedX');
            const speedY = $(this).data('speedY');

            let posX = parseFloat($(this).css('left'));
            let posY = parseFloat($(this).css('top'));

            // Convert from px to percentage
            posX = (posX / $('.footer').width()) * 100;
            posY = (posY / $('.footer').height()) * 100;

            // Update position
            let newPosX = posX + speedX;
            let newPosY = posY + speedY;

            // Boundary check
            if (newPosX < 0 || newPosX > 95) {
                $(this).data('speedX', -speedX);
            }

            if (newPosY < 0 || newPosY > 95) {
                $(this).data('speedY', -speedY);
            }

            // Apply new position
            $(this).css({
                'left': newPosX + '%',
                'top': newPosY + '%'
            });
        });

        requestAnimationFrame(animateElements);
    }

    // Start the animation
    animateElements();

    // Form submission with jQuery
    $('.newsletter-form').submit(function (e) {
        e.preventDefault();
        const email = $(this).find('input[type="email"]').val();

        // Bootstrap alert for success - can be replaced with AJAX call
        const alertHtml = `
                    <div class="alert alert-success alert-dismissible fade show mt-3" role="alert">
                        Thank you for subscribing with: ${email}
                        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                    </div>
                `;

        $(this).after(alertHtml);
        $(this).find('input[type="email"]').val('');

        // Auto-dismiss after 3 seconds
        setTimeout(function () {
            $('.alert').alert('close');
        }, 3000);
    });

    // Add smooth hover effect for links using jQuery
    $('.footer-links a').hover(
        function () {
            $(this).stop().animate({ paddingLeft: '8px' }, 200);
            $(this).find('i').stop().animate({ marginRight: '10px' }, 200);
        },
        function () {
            $(this).stop().animate({ paddingLeft: '0' }, 200);
            $(this).find('i').stop().animate({ marginRight: '8px' }, 200);
        }
    );

    // Add tooltip to social icons using Bootstrap's tooltip
    $('.social-icon').tooltip();
});