    // Function to send user input and get response
    async function sendMessage() {
    const prompt = document.querySelector('.publisher-input').value.trim();
    if (prompt === '') return;

    // Display the user's message in the chat
    appendMessage(prompt, 'user');

    // Clear input field
    document.querySelector('.publisher-input').value = '';

    // Send the request to the server
    try {
    const response = await fetch('/AIChat/Ask', {
    method: 'POST',
    headers: {
    'Content-Type': 'application/json'
},
    body: JSON.stringify(prompt) // Send the prompt as JSON
});
    const responseData = await response.json();

    // Append the response message from the AI in the chat
    if (responseData) {
    appendMessage(responseData, 'ai');
} else {
    appendMessage('Sorry, something went wrong.', 'ai');
}
} catch (error) {
    console.error('Error:', error);
    appendMessage('An error occurred. Please try again.', 'ai');
}
}

    // Function to append messages in the chat
    function appendMessage(message, sender) {
    const chatContent = document.getElementById('chat-content');

    // Create a new media element for the message
    const mediaDiv = document.createElement('div');
    mediaDiv.classList.add('media');
    if (sender === 'ai') {
    mediaDiv.classList.add('media-chat');
} else {
    mediaDiv.classList.add('media-chat','media-chat-reverse');
}

    // Add avatar image
    const avatarImg = document.createElement('img');
    avatarImg.classList.add('avatar');
    avatarImg.src = sender === 'ai' ? 'https://img.icons8.com/color/36/000000/administrator-female.png':'https://img.icons8.com/color/36/000000/administrator-male.png'
    avatarImg.alt = '...';

    // Create message body
    const mediaBodyDiv = document.createElement('div');
    mediaBodyDiv.classList.add('media-body');
    const messageParagraph = document.createElement('p');
    messageParagraph.textContent = message;
    mediaBodyDiv.appendChild(messageParagraph);

    // Add timestamp
    const metaParagraph = document.createElement('p');
    metaParagraph.classList.add('meta');
    const time = document.createElement('time');
    time.setAttribute('datetime', new Date().toISOString());
    time.textContent = new Date().toLocaleTimeString();
    metaParagraph.appendChild(time);
    mediaBodyDiv.appendChild(metaParagraph);

    // Append avatar and message body to the media div
    mediaDiv.appendChild(avatarImg);
    mediaDiv.appendChild(mediaBodyDiv);

    // Append the new message to the chat content
    chatContent.appendChild(mediaDiv);

    // Scroll to the bottom of the chat container
    chatContent.scrollTop = chatContent.scrollHeight;
}

    // Event listener for the "send" button (paper plane)
    document.querySelector('.publisher-btn.text-info').addEventListener('click', (event) => {
    event.preventDefault();
    sendMessage();
});

    // Optional: Send message when pressing Enter key
    document.querySelector('.publisher-input').addEventListener('keypress', (event) => {
    if (event.key === 'Enter') {
    event.preventDefault();
    sendMessage();
}
});
